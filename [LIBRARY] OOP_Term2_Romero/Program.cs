﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _LIBRARY__OOP_Term2_Romero
{
    internal class Program
    {
        static string _uInput = "";
        static string _dateToday = "";
        static string _name = "";

        static void Main(string[] args)
        {
            bool repeat = true;

            while (repeat)
            {
                Console.Clear();

                DateTime now = DateTime.Now;
                _dateToday = now.ToString("MM/dd/yyyy");

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Date Today: {0}", _dateToday);
                Console.ResetColor();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("Do you want to run the library system? [Y/N]: ");
                Console.ResetColor();
                _uInput = Console.ReadLine().ToUpper();

                if (_uInput != "Y" && _uInput != "N")
                    continue;

                repeat = RepeatProcess(repeat);

                while (repeat)
                {
                    Console.Clear();
                    DateToday();

                    while (repeat)
                    {
                        ProgramStart();

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Date Today: {0}", _dateToday);
                        Console.ResetColor();
                        Console.WriteLine();
                        Console.Write("Are you a new user? [Y/N] : ");
                        _uInput = Console.ReadLine().ToUpper();

                        repeat = !RepeatProcess(repeat);
                    }
                    repeat = true;
                    break;
                }

                if (!repeat)
                {
                    Console.WriteLine();
                    Console.WriteLine("Press any key to exit.");
                    Console.ReadKey();
                    break;
                }               
            }            
        }

        static void DateToday()
        {
            //Console.ForegroundColor = ConsoleColor.Blue;
            //Console.Write("What is the date today? [MM/DD/YYYY] : ");
            //Console.ResetColor();
            //_dateToday = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Date Today: {0}", _dateToday);
            Console.ResetColor();
            Console.WriteLine();
            Console.Write("What is your name? ");
            _name = Console.ReadLine();
            Console.WriteLine();
        }

        public static void ProgramStart()
        {
            TasksClass tC = new TasksClass();

            tC.BooksAvailable_Fill();

            Console.Clear();
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor= ConsoleColor.Cyan;
                Console.WriteLine("Date Today: {0}",_dateToday);
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("----------------------------------------");
                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("|        Welcome to the library        |");
                Console.ResetColor();
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("| A. Return Book                       |");
                Console.WriteLine("| B. Borrow Book                       |");
                Console.WriteLine("| C. Create an Account                 |");
                Console.WriteLine("| D. View your Library Card            |");
                Console.WriteLine("| E. Exit                              |");
                Console.WriteLine("----------------------------------------");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("Please choose what you want to do: ");
                Console.ResetColor();
                _uInput = Console.ReadLine().ToUpper();
                Console.WriteLine();

                if (_uInput == "A")
                {
                    tC.ReturnBook(_dateToday, _name);
                    break;
                }
                else if (_uInput == "B")
                {
                    tC.BorrowBook(_dateToday, _name);
                    break;
                }
                else if (_uInput == "C")
                {
                    tC.CreateAccount(_name);
                    break;
                }
                else if (_uInput == "D")
                {
                    tC.ViewCard(_name);
                    break;
                }
                else if (_uInput == "E")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Press any key to exit.");
                    Console.ResetColor();
                    Console.ReadKey();
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }
            }
        }

        static bool RepeatProcess(bool repeat)
        {
            while (true)
            {
                if (_uInput == "Y")
                {
                    repeat = true;
                    return repeat;
                }
                else if (_uInput.ToUpper() == "N")
                {
                    repeat = false;
                    return repeat;
                }
                else
                {
                    Console.WriteLine("Invalid input. Press any key to continue.");
                    Console.ReadKey();
                }
            }
        }
    }
}
