﻿using System;

namespace TodoListApp
{
   public class Program
    {
        //Глобальные переменные
        static string userFirstName = "";
        static string userLastName = "";
        static string birthYearInput = "";
        //Константы
        const int InitialArraySize = 2;
        const int GrowthMultiplier = 2;
        const char SpaceSeparator = ' ';

        public static void Main()
        {
            Console.WriteLine("Работу выполнили Нечитайлов и Шишкина\n");

            Console.Write("Введите ваше имя: ");
            userFirstName = Console.ReadLine();

            Console.Write("Введите вашу фамилию: ");
            userLastName = Console.ReadLine();

            Console.Write("Введите ваш год рождения: ");
            birthYearInput = Console.ReadLine();


            if (int.TryParse(birthYearInput, out int birthYear))
            {
                int currentYear = DateTime.Now.Year;
                int userAge = currentYear - birthYear;

                Console.WriteLine($"Добавлен пользователь {userFirstName} {userLastName}, возраст - {userAge}" );
            }

            else
            {
                Console.WriteLine("Ошибка: год рождения должен быть числом!");
            }
            
            Console.WriteLine("\nДля списка команд введите 'help'\n");

            string[] todos = new string[InitialArraySize];
            bool[] statuses = new bool[InitialArraySize];
            DateTime[] dates = new DateTime[InitialArraySize];

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) continue;

                string[] parts = input.Split(new char[] { SpaceSeparator }, 2, StringSplitOptions.RemoveEmptyEntries);
                string command = parts[0].ToLower();
                string arguments = parts.Length > 1 ? parts[1]: "";

                if (command == "help")
                {
                    ShowHelp();
                }

                else if (command == "profile")
                {
                    ShowProfile();
                }
        
                else if (command == "add")
                {
                    string taskText = "";

                    if (arguments.Contains("--multiline") || arguments.Contains("-m"))
                    {
                        taskText = ReadMultilineTask();
                    }
                    else
                    {
                        if (parts.Length > 1)
                        {
                            taskText = parts[1].Trim('"'); 
                        }
                    }

                    if (!string.IsNullOrEmpty(taskText))
                    {
                        AddTask(taskText, ref todos, ref statuses, ref dates);
                    }
                    
                }

                else if (command == "view")
                {
                    ViewTasks(arguments,todos, statuses, dates);
                }

                else if (command == "done")
                {
                    if (parts.Length > 1 && int.TryParse(parts[1], out int index))
                        MarkTaskDone(index, statuses, dates);
                }

                else if (command == "delete")
                {
                    if (parts.Length > 1 && int.TryParse(parts[1], out int index))
                        DeleteTask(index, todos, statuses, dates);
                }

                else if (command == "update")
                {
                    if (parts.Length > 1)
                    {
                        string[] updateParts = parts[1].Split(new char[] { SpaceSeparator }, 2);
                        if (updateParts.Length > 1 && int.TryParse(updateParts[0], out int index))
                        {
                            string newText = updateParts[1].Trim('"');
                            UpdateTask(index, newText, todos, dates);
                        }
                    }
                }

                else if (command == "exit")
                {
                    break;
                }
                
            }

        }
        //Методы
        static void ShowHelp()
        {
            Console.WriteLine("Доступные команды:");
            Console.WriteLine(" help    - Список команд");
            Console.WriteLine(" profile - Данные пользователя");
            Console.WriteLine(" add     - Добавить задачу (add \"текст\")");
            Console.WriteLine(" view    - Показать задачи");
            Console.WriteLine(" done <idx>  - Отметить задачу выполненной");
            Console.WriteLine(" delete <idx>  - Удалить задачу");
            Console.WriteLine(" update <idx> \"текст\" - Обновить текст задачи");
            Console.WriteLine(" exit    - Выход");
        }

        static void ShowProfile()
        {
            Console.WriteLine($"{userFirstName} {userLastName}, {birthYearInput}");
        }
        
        static void AddTask(string taskText, ref string[] todos, ref bool[] statuses, ref DateTime[] dates) //ref чтобы можно было изменять массивы
        {
            int freeIndex = FindFreeIndex(todos);

            if (freeIndex == -1)
            {
                Console.WriteLine("Массив заполнен, расширяем...");
                todos = ExpandStringArray(todos);
                statuses = ExpandBoolArray(statuses);
                dates = ExpandDateArray(dates);
                freeIndex = todos.Length / GrowthMultiplier;
            }

            todos[freeIndex] = taskText;
            statuses[freeIndex] = false; 
            dates[freeIndex] = DateTime.Now; 
            Console.WriteLine($"Задача \"{taskText}\" добавлена.");
        }

        static void ViewTasks(string arguments, string[] todos, bool[] statuses, DateTime[] dates)
        {
            bool showIndex = arguments.Contains("--index") || arguments.Contains("-i");
            bool showStatus = arguments.Contains("--status") || arguments.Contains("-s");
            bool showDate = arguments.Contains("--update-date") || arguments.Contains("-d");
            bool showAll = arguments.Contains("--all") || arguments.Contains("-a");

            if (showAll)
            {
                showIndex = true;
                showStatus = true;
                showDate = true;
            }

            Console.WriteLine("Ваши задачи:");

            if (showIndex) Console.Write($"{ "Idx",-4} | ");
            Console.Write($"{ "Задача",-32} | ");
            if (showStatus) Console.Write($"{ "Статус",-12} | ");
            if (showDate) Console.Write($"{ "Дата",-10} | ");
            Console.WriteLine();
            Console.WriteLine(new string('-', 70));
            

            bool isEmpty = true;
            for (int i = 0; i < todos.Length; i++)
            {
                if (!string.IsNullOrEmpty(todos[i]))
                {
                    isEmpty = false;

                    string taskText = todos[i];
                    taskText = taskText.Replace("\n", " ");
                    if (taskText.Length > 30)
                    {
                        taskText = taskText.Substring(0, 27) + "...";
                    }

                    if (showIndex) Console.Write($"{i,-4} | ");
                    Console.Write($"{taskText,-32} | ");
                    if (showStatus) 
                    {
                        string statusText = statuses[i] ? "сделано" : "не сделано";
                        Console.Write($"{statusText,-12} | ");
                    }
                    if (showDate) 
                    {
                        string formattedDate = dates[i].ToString("dd.MM.yyyy");
                        Console.Write($"{formattedDate,-10} | ");
                    }
                    Console.WriteLine();
                }
            }

            if (isEmpty) Console.WriteLine("  (Список пуст)");
        }

        static void MarkTaskDone(int index, bool[] statuses, DateTime[] dates)
        {
            if (index >= 0 && index < statuses.Length && !string.IsNullOrEmpty(dates[index].ToString()))
            {
                statuses[index] = true;
                dates[index] = DateTime.Now; 
                Console.WriteLine($"Задача {index} выполнена.");
            }
            else
            {
                Console.WriteLine("Ошибка: неверный индекс.");
            }
        }


        static void DeleteTask(int index, string[] todos, bool[] statuses, DateTime[] dates)
        {
            if (index >= 0 && index < todos.Length && !string.IsNullOrEmpty(todos[index]))
            {
                for (int i = index; i < todos.Length - 1; i++)
                {
                    todos[i] = todos[i + 1];
                    statuses[i] = statuses[i + 1];
                    dates[i] = dates[i + 1];
                }

                todos[todos.Length - 1] = null;
                statuses[statuses.Length - 1] = false;
                dates[dates.Length - 1] = default(DateTime);
                Console.WriteLine($"Задача {index} удалена.");
            }
            else
            {
                Console.WriteLine("Ошибка: неверный индекс.");
            }
        }


        static void UpdateTask(int index, string newText, string[] todos, DateTime[] dates)
        {
            if (index >= 0 && index < todos.Length && !string.IsNullOrEmpty(todos[index]))
            {
                todos[index] = newText;
                dates[index] = DateTime.Now;
                Console.WriteLine($"Задача {index} обновлена.");
            }
            else
            {
                Console.WriteLine("Ошибка: неверный индекс.");
            }
        }
       static string ReadMultilineTask()
        {
            Console.WriteLine("Введите строки задачи (для завершения введите !end):");
            string result = "";
            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if (line == null || line.Trim() == "!end") break;
                
                if (result != "") result += "\n";
                result += line;
            }
            return result;
        }

        //Доп. методы
         static string[] ExpandStringArray(string[] oldArray)
        {
            string[] newArray = new string[oldArray.Length * GrowthMultiplier];
            for (int i = 0; i < oldArray.Length; i++) newArray[i] = oldArray[i];
            return newArray;
        }

        static bool[] ExpandBoolArray(bool[] oldArray)
        {
            bool[] newArray = new bool[oldArray.Length * GrowthMultiplier];
            for (int i = 0; i < oldArray.Length; i++) newArray[i] = oldArray[i];
            return newArray;
        }

        static DateTime[] ExpandDateArray(DateTime[] oldArray)
        {
            DateTime[] newArray = new DateTime[oldArray.Length * GrowthMultiplier];
            for (int i = 0; i < oldArray.Length; i++) newArray[i] = oldArray[i];
            return newArray;
        }

        static int FindFreeIndex(string[] todos)
        {
            for (int i = 0; i < todos.Length; i++)
            {
                if (string.IsNullOrEmpty(todos[i])) return i;
            }
            return -1;
        } 
    } 
}