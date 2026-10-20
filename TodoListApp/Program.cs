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
                    Console.WriteLine("Доступные команды:");
                    Console.WriteLine("  help    - Список команд");
                    Console.WriteLine("  profile - Данные пользователя");
                    Console.WriteLine("  add     - Добавить задачу (add \"текст\")");
                    Console.WriteLine("  view    - Показать задачи");
                    Console.WriteLine("  exit    - Выход");
                }
                else if (command == "exit")
                {
                    break;
                }
                else if (command == "profile")
                {
                    Console.WriteLine($"{userFirstName} {userLastName}, {birthYearInput}");
                }
                else if (command == "view")
                {
                    Console.WriteLine("Ваши задачи:");
                    bool isEmpty = true;
                    
                    for (int i = 0; i < todos.Length; i++)
                    {
                        if (todos[i] != null && todos[i] != "")
                        {
                            Console.WriteLine($"  {i + 1}. {todos[i]}");
                            isEmpty = false;
                        }
                    }
                    if (isEmpty) Console.WriteLine("  (Список пуст)");
                }
                else if (command == "add")
                {
                    if (parts.Length > 1)
                    {
                        string taskText = parts[1].Trim('"'); 

                        int freeIndex = -1;
                        for (int i = 0; i < todos.Length; i++)
                        {
                            if (todos[i] == null || todos[i] == "")
                            {
                                freeIndex = i;
                                break;
                            }
                        }

                        if (freeIndex == -1)
                        {
                            Console.WriteLine(" Массив заполнен, расширяем...");
                            
                            string[] newTodos = new string[todos.Length * 2];
                        
                            for (int i = 0; i < todos.Length; i++)
                            {
                                newTodos[i] = todos[i];
                            }
                            
                            todos = newTodos; 
                            
                            freeIndex = todos.Length / 2; 
                        }

                        todos[freeIndex] = taskText;
                        Console.WriteLine($" Задача \"{taskText}\" добавлена.");
                    }
                    else
                    {
                        Console.WriteLine("Введите текст. Пример: add \"Купить хлеб\"");
                    }
                }
            }

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