using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace _LIBRARY__OOP_Term2_Romero
{
    internal class TasksClass
    {      
        //public List<List<string>> _details = new List<List<string>>();
        public List<string> _BooksAvailable = new List<string>();
        public List<string> _BooksToBorrow = new List<string>();
        public List<string> _BooksUnavailable = new List<string>();
        public List<string> _columnDetails = new List<string>();
        public string[] _temp = new string[] { };
        public string _FileName = "";
        public string _uInput = "";
        public string _line = "";
        public string _status = "";
        public string _date = "";
        public int _month = 0;
        public int _day = 0;
        public int _year = 0;
        

        public List<string> _BookLibrary = new List<string>() { "Alice in Wonderland", "Green Eggs and Ham", "How the World Really Works", "The Wild Life of Animals", 
            "The Picture of Dorian Gray", "How to Stop Time", "The Secret History", "The Storyteller",
            "Crime and Punishment", "Nothing More to Tell", "Pride and Prejudice", "Red, White & Royal Blue" };

        //csv file columns
        // [0] Date Borrowed , [1] Book Title , [2] Scheduled Return , [3] Date Returned , [4] Status

        public void BooksAvailable_Fill()
        {
            for (int x = 0; x < _BookLibrary.Count; x++)
            {
                _BooksAvailable.Add(_BookLibrary[x]);
            }

            using (StreamReader sr = new StreamReader("NotAvailable.txt")) //List of unavailable books
            {
                while ((_line = sr.ReadLine()) != null)
                {
                    if (_line.Length > 0)
                    {
                        _BooksUnavailable.Add(_line);
                    }
                }
            }

            for (int y = 0; y < _BooksUnavailable.Count; y++) //updating the list of available books
            {
                if (_BooksAvailable.Contains(_BooksUnavailable[y]))
                {
                    _BooksAvailable.Remove(_BooksUnavailable[y]);
                }
            }
        }

        public void ReturnBook(string dateToday, string name)
        {
            List<string> columnDetails = new List<string>();
            string[] dToday = new string[] { };
            string[] sched = new string[] { };
            string dTodayAsNUM = "";
            string schedAsNUM = "";
            int rows = 0;

            Console.Clear();
            _FileName = name + ".csv";
            if (File.Exists(_FileName))
            {
                using (StreamReader sr = new StreamReader(_FileName))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Date Today: {0}", dateToday);
                    Console.ResetColor();
                    Console.WriteLine();
                    Console.WriteLine("Checking the books........");
                    Console.WriteLine();
                    while ((_line = sr.ReadLine()) != null)
                    {
                        rows++;
                        if (rows > 3) //skips the headers
                        {
                            if (_line.Length > 0)
                            {
                                _temp = _line.Split(',');
                                _date = _temp[2];

                                UpdateLists();

                                dToday = dateToday.Split('/');
                                dTodayAsNUM = dToday[2] + dToday[0] + dToday[1]; //rebuild as a number but based on largest measurement

                                sched = _date.Split('/');
                                schedAsNUM = sched[2] + sched[0] + sched[1]; //rebuild as a number but based on largest measurement

                                //Comparing dates
                                if (int.Parse(dTodayAsNUM) > int.Parse(schedAsNUM)) //LATE
                                {
                                    _status = "Late";
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("You returned {0} late", _temp[1]);
                                    Console.ResetColor();
                                    Console.WriteLine("Press any key to continue.......");
                                    Console.WriteLine();
                                    Console.ReadKey();
                                }
                                else
                                {
                                    _status = "On time";
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("You returned {0} on time", _temp[1]);
                                    Console.ResetColor();
                                    Console.WriteLine("Press any key to continue.......");                                   
                                    Console.WriteLine();
                                    Console.ReadKey();
                                }

                                //columnDetails.Clear();

                                for (int i = 0; i < _temp.Length; i++) //put the columns to a list
                                {
                                    _columnDetails.Add(_temp[i]);
                                }
                                _columnDetails.Add(dateToday); //add date returned
                                _columnDetails.Add(_status); //add status
                                //_details.Add(columnDetails); //store them into a list
                            }
                        }                       
                    }                
                }
            }

            else
            {
                Console.WriteLine("Create an account first! Press any key to continue.");
                Console.ReadKey();
                CreateAccount(name);
            }
            UpdateCard(name, dateToday, _date);
        }

        public void UpdateLists()
        {
            //update NotAvailable.txt REMOVE THE RETURNED BOOKS
            _BooksAvailable.Add(_temp[1]);
            _BooksUnavailable.Remove(_temp[1]);

            using (StreamWriter sw = new StreamWriter("NotAvailable.txt"))
            {
                for (int x = 0; x < _BooksUnavailable.Count; x++)
                {
                    sw.WriteLine(_BooksUnavailable[x]);
                }
            }
        }

        public void BorrowBook(string dateToday, string name)
        {           
            _FileName = name + ".csv";

            if (File.Exists(_FileName)) //account exists
            {
                while (true) //book library
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Date Today: {0}", dateToday);
                    Console.ResetColor();
                    Console.WriteLine();
                    BookLibrary();

                    int libraryIDX = 0;
                    bool isNum = false;

                    while (!isNum) //checking if user typed an index
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write("What book do you want to borrow?: ");
                        Console.ResetColor();
                        _uInput = Console.ReadLine();
                        Console.WriteLine();
                        isNum = int.TryParse(_uInput, out libraryIDX);
                    }                   

                    if (_BooksUnavailable.Contains(_BookLibrary[libraryIDX-1])) //this book has been borrowed
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Schedule Conflict! Please Choose another book. Press any key to continue.");
                        Console.ReadKey();
                        Console.ResetColor();                        
                        continue;
                    }

                    else //the book is available
                    {
                        //updating the lists
                        _BooksToBorrow.Add(_BookLibrary[libraryIDX-1]);
                        _BooksUnavailable.Add(_BookLibrary[libraryIDX-1]);
                        _BooksAvailable.Remove(_BookLibrary[libraryIDX-1]);
                        using (StreamWriter sw = File.AppendText("NotAvailable.txt"))
                        {
                            sw.WriteLine(_BookLibrary[libraryIDX-1]);
                        }
                        Console.WriteLine("You have successfully borrowed the book {0}.", _BookLibrary[libraryIDX - 1]);
                        Console.WriteLine();

                        _temp = dateToday.Split('/');
                        ReturnDate();

                        Console.WriteLine("The return date is {0}", _date);
                        Console.WriteLine();
                    }

                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("Do you want to borrow more books? [Y/N] : ");
                    Console.ResetColor();
                    _uInput = Console.ReadLine().ToUpper();
                    Console.WriteLine();
                    if (_uInput == "N")
                    {
                        break;
                    }
                    else if (_uInput != "Y" && _uInput != "N")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid input. Press any key to continue.");
                        Console.ResetColor();
                        Console.ReadKey();
                    }
                }
                UpdateCard(name, dateToday, _date);
                
            }
            else // no account
            {
                Console.WriteLine("Create an account first! Press any key to continue.");
                Console.ReadKey();
                CreateAccount(name);
            }
        }

        public void BookLibrary()
        {
            Console.WriteLine("--------------------------------- \t ---------------------------------");
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Write("|     Children's Literature     |");
            Console.ResetColor();
            Console.Write(" \t ");
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.Write("|          Educational          |");
            Console.ResetColor();
            Console.WriteLine("\n--------------------------------- \t ---------------------------------");
            Console.WriteLine("| 1. Alice in Wonderland        | \t | 3. How the World Really Works |");
            Console.WriteLine("| 2. Green Eggs and Ham         | \t | 4. The Wild Life of Animals   |");
            Console.WriteLine("--------------------------------- \t ---------------------------------");
            Console.WriteLine();

            Console.WriteLine("--------------------------------- \t ---------------------------------");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.Write("|            Fiction            |");
            Console.ResetColor();
            Console.Write(" \t ");
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write("|           Historical          |");
            Console.ResetColor();
            Console.WriteLine("\n--------------------------------- \t ---------------------------------");
            Console.WriteLine("| 5. The Picture of Dorian Gray | \t | 7. How to Stop Time           |");
            Console.WriteLine("| 6. The Secret History         | \t | 8. The Storyteller            |");
            Console.WriteLine("--------------------------------- \t ---------------------------------");
            Console.WriteLine();

            Console.WriteLine("--------------------------------- \t ---------------------------------");
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("|             Mystery           |");
            Console.ResetColor();
            Console.Write(" \t ");
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Write("|             Romance           |");
            Console.ResetColor();
            Console.WriteLine("\n--------------------------------- \t ---------------------------------");
            Console.WriteLine("| 9.  Crime and Punishment      | \t | 11. Pride and Prejudice       |");
            Console.WriteLine("| 10. Nothing More to Tell      | \t | 12. Red, White & Royal Blue   |");
            Console.WriteLine("--------------------------------- \t ---------------------------------");
            Console.WriteLine();
        }

        public void ReturnDate()
        {
            string temp = "";

            if (int.Parse(_temp[1]) <= 21) //js add to the day
            {
                _day = int.Parse(_temp[1]) + 7;
                if (_day < 10) //adding the 0 before the first digit
                {
                    temp = "0" + _day.ToString();
                    _date = _temp[0] + "/" + temp + "/" + _temp[2];
                }
                else
                    _date = _temp[0] + "/" + _day.ToString() + "/" + _temp[2];
            }
            else if (int.Parse(_temp[1]) > 21) //new month or new year
            {
                _day = 28 - int.Parse(_temp[1]);
                if (_temp[0] != "12") //retain year
                {
                    _month = int.Parse(_temp[0]) + 1;
                    _date = _month.ToString() + "/" + _day.ToString() + "/" + _temp[2];
                }
                else
                {
                    temp = "01";
                    _year = int.Parse(_temp[2]) + 1;
                    _date = temp + "/" + _day.ToString() + "/" + _year.ToString();
                }
            }
        }

        public void CreateAccount(string name)
        {
            Console.Clear();
            _FileName = name + ".csv";
            if (File.Exists(_FileName))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("This account already exists. Press any key to continue");
                Console.ResetColor();
                Console.ReadKey();
                Program.ProgramStart();
            }
            else
            {
                New_Or_ReWrite(_FileName, name);
                Console.WriteLine("Account has been created. Press any key to continue.");
                Console.ReadKey();
            }
        }

        public void New_Or_ReWrite(string _FileName, string name)
        {
            using (StreamWriter sw = new StreamWriter(_FileName))
            {
                sw.WriteLine("NAME: " + "," + name);
                sw.WriteLine();
                sw.WriteLine("Date Borrowed" + "," + "Book Title" + "," + "Scheduled Return" + "," + "Date Returned" + "," + "Status");
            }          
        }

        public void UpdateCard(string name, string dateToday, string _date)
        {
            Console.Clear();
            _FileName = name + ".csv";
            if (File.Exists(_FileName))
            {
                if (_BooksToBorrow.Count > 0) //BookBorrow
                {
                    using (StreamWriter sw = File.AppendText(_FileName))
                    {
                        for (int x = 0; x < _BooksToBorrow.Count; x++)
                        {
                            sw.WriteLine(dateToday + "," + _BooksToBorrow[x] + "," + _date);
                        }
                    }
                }

                else //BookReturn
                {
                    New_Or_ReWrite(_FileName, name);
                    using (StreamWriter sw = File.AppendText(_FileName))
                    {
                        for (int x = 0; x < _columnDetails.Count; x++)
                        {
                            if (x > 0)
                            {
                                if (x % 5 == 0)
                                {
                                    sw.WriteLine();
                                }
                            }                          
                            sw.Write(_columnDetails[x]);
                            sw.Write(",");
                        }
                    }
                }
            }
        }

        public void ViewCard(string name)
        {
            int rows = 0;
            int chars = 0;

            Console.Clear();
            _FileName = name + ".csv";
            if (File.Exists(_FileName))
            {
                using (StreamReader sr = new StreamReader(_FileName))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("Name: ");
                    Console.ResetColor();
                    Console.Write(name);
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();

                    while ((_line = sr.ReadLine()) != null)
                    {
                        rows++;
                        if (rows > 2) //skips the headers
                        {
                            if (_line.Length > 0)
                            {
                                _temp = _line.Split(',');
                                for (int i = 0; i < _temp.Length; i++)
                                {
                                    chars = 0;
                                    Console.Write("| {0}", _temp[i]);
                                    chars += _temp[i].Length;

                                    for (int j = chars; j < 27; j++)
                                    {
                                        Console.Write(" ");
                                    }
                                }
                            }
                            Console.Write("|");
                            Console.WriteLine();
                        }                       
                    }
                }
            }
            else
            {
                Console.WriteLine("Create an account first! Press any key to continue.");
                Console.ReadKey();
                CreateAccount(name);
            }
            Console.ReadKey();
        }
    }
}
