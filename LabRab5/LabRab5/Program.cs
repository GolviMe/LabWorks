using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LabRab5
{
    class ExceptionHandler
    {
        public virtual void HandleException(Exception ex)
        {
            Console.WriteLine($"Исключение: {ex.GetType().Name}");
            Console.WriteLine($"Сообщение: {ex.Message}");
        }
    }

    class CustomExceptionHandler : ExceptionHandler
    {
        public override void HandleException(Exception ex)
        {
            Console.WriteLine("\nОбработка исключения:");

            if (ex is ArrayTypeMismatchException)
            {
                Console.WriteLine("Несоответствие типов в массиве!");
            }
            else if (ex is DivideByZeroException)
            {
                Console.WriteLine("Деление на ноль!");
            }
            else if (ex is IndexOutOfRangeException)
            {
                Console.WriteLine("Выход за границы массива!");
            }
            else if (ex is InvalidCastException)
            {
                Console.WriteLine("Неверное приведение типов!");
            }
            else if (ex is OutOfMemoryException)
            {
                Console.WriteLine("Недостаточно памяти!");
            }
            else if (ex is OverflowException)
            {
                Console.WriteLine("Арифметическое переполнение!");
            }
            else
            {
                base.HandleException(ex);
            }

            Console.WriteLine("Конец обратоки\n");
        }
    }

    

    static class ExceptionGenerator
    {
        public static void GenerateArrayTypeMismatch()
        {
            object[] array = new string[10];
            array[0] = 123; // Несоответствие типов
        }

        public static void GenerateDivideByZero()
        {
            int a = 10, b = 0;
            int result = a / b; // Деление на 0
        }

        public static void GenerateIndexOutOfRange()
        {
            int[] array = new int[5];
            int value = array[10]; // Выход за границы
        }

        public static void GenerateInvalidCast()
        {
            object obj = "Hell";
            int number = (int)obj; // Неверное приведение
        }

        public static void GenerateOutOfMemory()
        {
            // Попытка выделить слишком много памяти
            byte[] hugeArray = new byte[int.MaxValue];
        }

        public static void GenerateOverflow()
        {
            checked
            {
                int max = int.MaxValue;
                int overflow = max + 1; // Арифметическое переполнение
            }
        }
    }

    interface IPrintable
    {
        void PrintInfo();
    }

    interface IScorable
    {
        void CheckScore();
    }

    delegate void StudentHandler(Student st);

    class Institute
    {
        public string nameOfInstitute { get; set; }
        public List<Student> listOfStudents { get; set; } = new List<Student>();

        public Institute(string name)
        {
            nameOfInstitute = name;
        }

        public void AddStudent(Student st)
        {
            listOfStudents.Add(st);
        }

        public void ForEachStudent(StudentHandler handler)
        {
            foreach (var s in listOfStudents)
                handler(s);
        }
    }

    delegate void PrintAllAboutStudent(Student stud);

    class Student : IPrintable, IScorable
    {
        public string firstNameOfStudent;
        public string lastNameOfStudent;
        public int numberOfCourse;
        public int lowestMark;

        public Student(string first, string last, int course, int mark)
        {
            firstNameOfStudent = first;
            lastNameOfStudent = last;
            numberOfCourse = course;
            lowestMark = mark;
        }

        public string ToTXT(string instituteName)
        {
            return $"{firstNameOfStudent} {lastNameOfStudent} {instituteName} {numberOfCourse} {lowestMark}";
        }

        public string ToTXT_Report()
        {
            return $"{firstNameOfStudent} {lastNameOfStudent}";
        }

        public static Student FromTXT(string text, out string instituteName)
        {
            string[] parts = text.Split();
            instituteName = parts[2];
            return new Student(parts[0], parts[1], int.Parse(parts[3]), int.Parse(parts[4]));
        }

        public void PrintInfo()
        {
            Console.WriteLine($"{firstNameOfStudent} {lastNameOfStudent}, {numberOfCourse} курс, мин. оценка {lowestMark}");
        }

        public void CheckScore()
        {
            if (lowestMark == 5) Console.WriteLine("Данный студент - Отличник!");
            else if (lowestMark == 4) Console.WriteLine("Данный студент - Хорошист");
            else Console.WriteLine("У данного студента проблемы с учёбой");
        }
    }

    internal class Program
    {
        static string filePath = @"C:\Учебные материалы\2 курс\КАиСД\Гит\LabWorks\LabRab5\LabRab5\StudentsInfo.txt";
        static CustomExceptionHandler exceptionHandler = new CustomExceptionHandler(); 

        static void TestExceptions()
        {
            Console.WriteLine("\nТестирование обработки исключений:");
            Console.WriteLine("1 - ArrayTypeMismatchException");
            Console.WriteLine("2 - DivideByZeroException");
            Console.WriteLine("3 - IndexOutOfRangeException");
            Console.WriteLine("4 - InvalidCastException");
            Console.WriteLine("5 - OutOfMemoryException");
            Console.WriteLine("6 - OverflowException");
            Console.Write("Выберите исключение для теста: ");

            try
            {
                int testChoice = int.Parse(Console.ReadLine());

                switch (testChoice)
                {
                    case 1:
                        ExceptionGenerator.GenerateArrayTypeMismatch();
                        break;
                    case 2:
                        ExceptionGenerator.GenerateDivideByZero();
                        break;
                    case 3:
                        ExceptionGenerator.GenerateIndexOutOfRange();
                        break;
                    case 4:
                        ExceptionGenerator.GenerateInvalidCast();
                        break;
                    case 5:
                        ExceptionGenerator.GenerateOutOfMemory();
                        break;
                    case 6:
                        ExceptionGenerator.GenerateOverflow();
                        break;
                    default:
                        Console.WriteLine("Неверный выбор");
                        break;
                }
            }
            catch (Exception ex)
            {
                exceptionHandler.HandleException(ex);
            }
        }

        static void Main(string[] args)
        {
            Institute FKTiPM = new Institute("FKTiPM");
            Institute FMiKN = new Institute("FMiKN");
            Institute FTF = new Institute("FTF");

            List<Institute> institutes = new List<Institute> { FKTiPM, FMiKN, FTF };
            ImportStudentsFromFile(institutes);

            PrintAllAboutStudent multiDelegate = null;
            multiDelegate += (s) => s.PrintInfo();
            multiDelegate += (s) => s.CheckScore();

            int choice = 0;
            Console.WriteLine("Добро пожаловать в систему!");
            while (choice != 6)
            {
                Console.WriteLine("\nМеню:\n1 - Список студентов\n2 - Добавить студента\n3 - Лидер по отличникам (1 курс)\n4 - Посмотреть успеваемость случайного студента\n5 - Обработка ошибок\n6 - Выход");
                try { choice = int.Parse(Console.ReadLine()); }
                catch { Console.WriteLine("Ошибка ввода."); continue; }

                if (choice == 1)
                {
                    foreach (var inst in institutes)
                    {
                        Console.WriteLine($"\nСтуденты института {inst.nameOfInstitute}:");
                        inst.ForEachStudent((s) => s.PrintInfo());
                    }
                }
                else if (choice == 2)
                {
                    Console.Write("Имя: ");
                    string fn = Console.ReadLine();

                    Console.Write("Фамилия: ");
                    string ln = Console.ReadLine();

                    Console.Write("Институт (FKTiPM/FMiKN/FTF): ");
                    string instName = Console.ReadLine();
                    while (!(instName == "FKTiPM" || instName == "FMiKN" || instName == "FTF"))
                    {
                        Console.Write("Вы ввели несуществующий институт, попробуйте ещё раз.\n");
                        instName = Console.ReadLine();
                    }

                    Console.Write("Курс: ");
                    int course = int.Parse(Console.ReadLine());

                    Console.Write("Мин. оценка: ");
                    int mark = int.Parse(Console.ReadLine());

                    Student st = new Student(fn, ln, course, mark);
                    var inst = institutes.FirstOrDefault(i => i.nameOfInstitute == instName);

                    if (inst != null)
                    {
                        inst.AddStudent(st);
                        ExportStudentsToFile(institutes);
                        Console.WriteLine("Студент добавлен!");
                    }
                }
                else if (choice == 3)
                {
                    string bestInst = "";
                    int maxCount = 0;
                    foreach (var inst in institutes)
                    {
                        int count = inst.listOfStudents.Count(s => s.lowestMark == 5 && s.numberOfCourse == 1);
                        if (count > maxCount)
                        {
                            maxCount = count;
                            bestInst = inst.nameOfInstitute;
                        }
                    }
                    if (maxCount > 0)
                    {
                        Console.WriteLine($"Лучше всего отличников на 1 курсе в институте: {bestInst}");
                        Console.WriteLine("Вот список этих студентов: ");

                        string filePathForReview = @"C:\Учебные материалы\2 курс\КАиСД\Гит\LabWorks\LabRab4\LabRab4\ReportAboutBestInstitute.txt";
                        var tempLines = new List<string>();

                        tempLines.Add($"Список лучших первокурсников института {bestInst}:");
                        foreach (var inst in institutes)
                        {
                            if (inst.nameOfInstitute == bestInst)
                            {
                                foreach (var st in inst.listOfStudents)
                                {
                                    if (st.lowestMark == 5 && st.numberOfCourse == 1)
                                    {
                                        Console.WriteLine($"{st.firstNameOfStudent} {st.lastNameOfStudent}");
                                        tempLines.Add(st.ToTXT_Report());
                                    }
                                }
                            }
                        }
                        File.WriteAllLines(filePathForReview, tempLines, Encoding.UTF8);
                    }
                    else
                        Console.WriteLine("Отличников на 1 курсе нет.");
                }
                else if (choice == 4)
                {
                    Random rnd = new Random();
                    int rndInst = rnd.Next(0, 3);

                    if (institutes[rndInst].listOfStudents.Count > 0)
                    {
                        int rndStud = rnd.Next(0, institutes[rndInst].listOfStudents.Count);
                        Console.WriteLine(institutes[rndInst].nameOfInstitute);
                        multiDelegate(institutes[rndInst].listOfStudents[rndStud]);
                    }
                    else
                    {
                        Console.WriteLine("В выбранном институте нет студентов");
                    }
                }
                else if (choice == 5)
                {
                    TestExceptions();
                }
                else if (choice == 6)
                {
                    Console.WriteLine("Пока!");
                    Console.Read();
                }
                else Console.WriteLine("Вы ввели неверное значение, попробуйте ещё раз\n");
            }
        }

        static void ImportStudentsFromFile(List<Institute> institutes)
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        try
                        {
                            string instName;
                            Student st = Student.FromTXT(line, out instName);
                            var inst = institutes.FirstOrDefault(i => i.nameOfInstitute == instName);
                            inst?.AddStudent(st);
                        }
                        catch (Exception ex)
                        {
                            exceptionHandler.HandleException(ex);
                        }
                    }
                }
            }
        }

        static void ExportStudentsToFile(List<Institute> institutes)
        {
            try
            {
                var lines = new List<string>();
                foreach (var inst in institutes)
                {
                    foreach (var st in inst.listOfStudents)
                        lines.Add(st.ToTXT(inst.nameOfInstitute));
                }
                File.WriteAllLines(filePath, lines, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                exceptionHandler.HandleException(ex);
            }
        }
    }
}