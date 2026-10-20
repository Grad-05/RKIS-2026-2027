﻿using System;
using System.Threading.Channels;

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

                string[] parts = input.Split(new char[] { SpaceSeparator }, 2);
                string command = parts[0].ToLower();

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
                    if (parts.Length > 1)
                    {
                        string taskText = parts[1].Trim('"'); 
                        AddTask(taskText, ref todos, ref statuses, ref dates); //ref чтобы можно было изменять массивы
                    }
                    else
                    {
                        Console.WriteLine("Введите текст. Пример: add \"Купить хлеб\"");
                    }
                }

                else if (command == "view")
                {
                    ViewTasks(todos, statuses, dates);
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
        
        static void ViewTasks(string[] todos, bool[] statuses, DateTime[] dates)
        {
            Console.WriteLine("Ваши задачи:");
            bool isEmpty = true;
            for (int i = 0; i < todos.Length; i++)
            {
                if (!string.IsNullOrEmpty(todos[i]))
                {
                    string statusText = statuses[i] ? "сделано" : "не сделано";
                    Console.WriteLine($"  {i} {todos[i]} [{statusText}] {dates[i]:dd.MM.yyyy}");
                    isEmpty = false;
                }
            }
            if (isEmpty) Console.WriteLine("  (Список пуст)");
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