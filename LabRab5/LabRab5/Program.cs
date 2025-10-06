using System;
using System.Collections.Generic;
using System.IO;                                                                               //ПЕРЕПИСАТЬ
using System.Linq;                                                                             //ПЕРЕПИСАТЬ
using System.Text;                                                                             //ПЕРЕПИСАТЬ
                                                                                               //ПЕРЕПИСАТЬ
namespace LabRab5                                                                              //ПЕРЕПИСАТЬ
{                                                                                              //ПЕРЕПИСАТЬ
    // === Класс аргументов события для ошибок ===                                             //ПЕРЕПИСАТЬ
    public class ErrorEventArgs : EventArgs                                                    //ПЕРЕПИСАТЬ
    {                                                                                          //ПЕРЕПИСАТЬ
        public string Message { get; }                                                         //ПЕРЕПИСАТЬ
        public ErrorEventArgs(string msg) { Message = msg; }                                   //ПЕРЕПИСАТЬ
    }                                                                                          //ПЕРЕПИСАТЬ
                                                                                               //ПЕРЕПИСАТЬ
    // === Делегат события ===                                                                 //ПЕРЕПИСАТЬ
    public delegate void ErrorHandler(object sender, ErrorEventArgs e);                        //ПЕРЕПИСАТЬ
                                                                                               //ПЕРЕПИСАТЬ
    // === Обработчик ошибок ===                                                               //ПЕРЕПИСАТЬ
    class SafeExecutor                                                                         //ПЕРЕПИСАТЬ
    {                                                                                          //ПЕРЕПИСАТЬ
        public event ErrorHandler OnError; // событие                                          //ПЕРЕПИСАТЬ
                                                                                               //ПЕРЕПИСАТЬ
        public void Execute(Action action)                                                     //ПЕРЕПИСАТЬ
        {                                                                                      //ПЕРЕПИСАТЬ
            try
            {
                action();
            }
            catch (DivideByZeroException)
            {
                OnError?.Invoke(this, new ErrorEventArgs("Ошибка: деление на ноль (DivideByZeroException)"));
            }
            catch (ArrayTypeMismatchException)
            {
                OnError?.Invoke(this, new ErrorEventArgs("Ошибка: типы массива не совпадают (ArrayTypeMismatchException)"));
            }
            catch (IndexOutOfRangeException)
            {
                OnError?.Invoke(this, new ErrorEventArgs("Ошибка: выход за границы массива/списка (IndexOutOfRangeException)"));
            }
            catch (InvalidCastException)
            {
                OnError?.Invoke(this, new ErrorEventArgs("Ошибка: неверное приведение типа (InvalidCastException)"));
            }
            catch (OutOfMemoryException)
            {
                OnError?.Invoke(this, new ErrorEventArgs("Ошибка: нехватка памяти (OutOfMemoryException)"));
            }
            catch (OverflowException)
            {
                OnError?.Invoke(this, new ErrorEventArgs("Ошибка: переполнение числа (OverflowException)"));
            }
            catch (StackOverflowException)
            {
                OnError?.Invoke(this, new ErrorEventArgs("StackOverflowException — этот тип исключения не обрабатывается в runtime, завершает процесс. Бросаем вручную только для демонстрации!"));
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new ErrorEventArgs("Другая ошибка: " + ex.Message));
            }
        }
    }

    // === Интерфейсы ===                                                                            //ПЕРЕПИСАТЬ
    interface IPrintable { void PrintInfo(); }                                                       //ПЕРЕПИСАТЬ
    interface IScorable { void CheckScore(); }                                                       //ПЕРЕПИСАТЬ
                                                                                                     //ПЕРЕПИСАТЬ
    delegate void StudentHandler(Student st);                                                        //ПЕРЕПИСАТЬ
                                                                                                     //ПЕРЕПИСАТЬ
    class Institute                                                                                  //ПЕРЕПИСАТЬ
    {                                                                                                //ПЕРЕПИСАТЬ
        public string nameOfInstitute { get; set; }                                                  //ПЕРЕПИСАТЬ
        public List<Student> listOfStudents { get; set; } = new List<Student>();                     //ПЕРЕПИСАТЬ
                                                                                                     //ПЕРЕПИСАТЬ
        public Institute(string name) { nameOfInstitute = name; }                                    //ПЕРЕПИСАТЬ
        public void AddStudent(Student st) { listOfStudents.Add(st); }                               //ПЕРЕПИСАТЬ
                                                                                                     //ПЕРЕПИСАТЬ
        public void ForEachStudent(StudentHandler handler)                                           //ПЕРЕПИСАТЬ
        {                                                                                            //ПЕРЕПИСАТЬ
            foreach (var s in listOfStudents)                                                        //ПЕРЕПИСАТЬ
                handler(s);                                                                          //ПЕРЕПИСАТЬ
        }                                                                                            //ПЕРЕПИСАТЬ
    }                                                                                                //ПЕРЕПИСАТЬ
                                                                                                     //ПЕРЕПИСАТЬ
    class Student : IPrintable, IScorable                                                            //ПЕРЕПИСАТЬ
    {                                                                                                //ПЕРЕПИСАТЬ
        public string firstNameOfStudent;                                                            //ПЕРЕПИСАТЬ
        public string lastNameOfStudent;                                                             //ПЕРЕПИСАТЬ
        public int numberOfCourse;                                                                   //ПЕРЕПИСАТЬ
        public int lowestMark;                                                                       //ПЕРЕПИСАТЬ
                                                                                                     //ПЕРЕПИСАТЬ
        public Student(string first, string last, int course, int mark)                              //ПЕРЕПИСАТЬ
        {                                                                                            //ПЕРЕПИСАТЬ
            firstNameOfStudent = first;                                                              //ПЕРЕПИСАТЬ
            lastNameOfStudent = last;
            numberOfCourse = course;
            lowestMark = mark;
        }

        public string ToTXT(string instituteName)
        {
            return $"{firstNameOfStudent} {lastNameOfStudent} {instituteName} {numberOfCourse} {lowestMark}";
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
        static string filePath = @"C:\Учебные материалы\2 курс\КАиСД\Гит\LabWorks\LabRab3\LabRab3\StudentsInfo.txt";

        static void Main(string[] args)
        {
            // Создаём безопасный исполнитель и подписываемся на событие
            SafeExecutor exec = new SafeExecutor();
            exec.OnError += (s, e) => Console.WriteLine($"[Обработано событием] {e.Message}");

            Institute FKTiPM = new Institute("FKTiPM");
            Institute FMiKN = new Institute("FMiKN");
            Institute FTF = new Institute("FTF");
            List<Institute> institutes = new List<Institute> { FKTiPM, FMiKN, FTF };
            ImportStudentsFromFile(institutes);

            int choice = 0;
            Console.WriteLine("Добро пожаловать в систему!");
            while (choice != 6)
            {
                Console.WriteLine("\nМеню:\n1 - Список студентов\n2 - Добавить студента\n3 - Лидер по отличникам (1 курс)\n4 - Случайный студент\n5 - Проверить ошибки\n6 - Выход");
                exec.Execute(() =>
                {
                    try { choice = int.Parse(Console.ReadLine()); }
                    catch { Console.WriteLine("Ошибка ввода."); choice = 0; }
                });
                                                                                                                         //ПЕРЕПИСАТЬ
                if (choice == 1)                                                                                         //ПЕРЕПИСАТЬ
                {                                                                                                        //ПЕРЕПИСАТЬ
                    foreach (var inst in institutes)                                                                     //ПЕРЕПИСАТЬ
                    {                                                                                                    //ПЕРЕПИСАТЬ
                        Console.WriteLine($"\nСтуденты института {inst.nameOfInstitute}:");                              //ПЕРЕПИСАТЬ
                        inst.ForEachStudent((s) => s.PrintInfo());                                                       //ПЕРЕПИСАТЬ
                    }                                                                                                    //ПЕРЕПИСАТЬ
                }                                                                                                        //ПЕРЕПИСАТЬ
                else if (choice == 2)                                                                                    //ПЕРЕПИСАТЬ
                {                                                                                                        //ПЕРЕПИСАТЬ
                    Console.Write("Имя: "); string fn = Console.ReadLine();                                              //ПЕРЕПИСАТЬ
                    Console.Write("Фамилия: "); string ln = Console.ReadLine();                                          //ПЕРЕПИСАТЬ
                    Console.Write("Институт (FKTiPM/FMiKN/FTF): "); string instName = Console.ReadLine();                //ПЕРЕПИСАТЬ
                    while (!(instName == "FKTiPM" || instName == "FMiKN" || instName == "FTF"))                          //ПЕРЕПИСАТЬ
                    {                                                                                                    //ПЕРЕПИСАТЬ
                        Console.Write("Вы ввели несуществующий институт, попробуйте ещё раз.\n");                        //ПЕРЕПИСАТЬ
                        instName = Console.ReadLine();                                                                   //ПЕРЕПИСАТЬ
                    }                                                                                                    //ПЕРЕПИСАТЬ
                                                                                                                         //ПЕРЕПИСАТЬ
                    Console.Write("Курс: "); int course = int.Parse(Console.ReadLine());                                 //ПЕРЕПИСАТЬ
                    Console.Write("Мин. оценка: "); int mark = int.Parse(Console.ReadLine());                            //ПЕРЕПИСАТЬ
                                                                                                                         //ПЕРЕПИСАТЬ
                    Student st = new Student(fn, ln, course, mark);                                                      //ПЕРЕПИСАТЬ
                    var inst = institutes.FirstOrDefault(i => i.nameOfInstitute == instName);                            //ПЕРЕПИСАТЬ
                    inst?.AddStudent(st);                                                                                //ПЕРЕПИСАТЬ
                    ExportStudentsToFile(institutes);                                                                    //ПЕРЕПИСАТЬ
                    Console.WriteLine("Студент добавлен!");                                                              //ПЕРЕПИСАТЬ
                }                                                                                                        //ПЕРЕПИСАТЬ
                else if (choice == 3)                                                                                    //ПЕРЕПИСАТЬ
                {                                                                                                        //ПЕРЕПИСАТЬ
                    string bestInst = ""; int maxCount = 0;                                                              //ПЕРЕПИСАТЬ
                    foreach (var inst in institutes)                                                                     //ПЕРЕПИСАТЬ
                    {                                                                                                    //ПЕРЕПИСАТЬ
                        int count = inst.listOfStudents.Count(s => s.lowestMark == 5 && s.numberOfCourse == 1);          //ПЕРЕПИСАТЬ
                        if (count > maxCount) { maxCount = count; bestInst = inst.nameOfInstitute; }                     //ПЕРЕПИСАТЬ
                    }                                                                                                    //ПЕРЕПИСАТЬ
                    if (maxCount > 0)                                                                                    //ПЕРЕПИСАТЬ
                        Console.WriteLine($"Лучше всего отличников на 1 курсе в институте: {bestInst}");                 //ПЕРЕПИСАТЬ
                    else                                                                                                 //ПЕРЕПИСАТЬ
                        Console.WriteLine("Отличников на 1 курсе нет.");                                                 //ПЕРЕПИСАТЬ
                }                                                                                                        //ПЕРЕПИСАТЬ
                else if (choice == 4)                                                                                    //ПЕРЕПИСАТЬ
                {                                                                                                        //ПЕРЕПИСАТЬ
                    Random rnd = new Random();                                                                           //ПЕРЕПИСАТЬ
                    int rndInst = rnd.Next(0, 3);                                                                        //ПЕРЕПИСАТЬ
                    if (institutes[rndInst].listOfStudents.Count > 0)                                                    //ПЕРЕПИСАТЬ
                    {
                        int rndStud = rnd.Next(0, institutes[rndInst].listOfStudents.Count);
                        institutes[rndInst].listOfStudents[rndStud].PrintInfo();
                        institutes[rndInst].listOfStudents[rndStud].CheckScore();
                    }
                    else Console.WriteLine("В этом институте пока нет студентов.");
                }
                else if (choice == 5)
                {
                    // Демонстрация всех ошибок
                    exec.Execute(() => { double x = 1 / 0.0; }); // DivideByZero
                    exec.Execute(() => { int[] arr = { 1, 2 }; Console.WriteLine(arr[5]); }); // IndexOutOfRange
                    exec.Execute(() => { object str = "abc"; int n = (int)str; }); // InvalidCast
                    exec.Execute(() => { Array arr = Array.CreateInstance(typeof(string), 1); arr.SetValue(42, 0); }); // ArrayTypeMismatch
                    exec.Execute(() => { throw new OutOfMemoryException(); }); // OutOfMemory
                    exec.Execute(() => { checked { int i = int.MaxValue; i++; } }); // Overflow
                    exec.Execute(() => { throw new StackOverflowException(); }); // StackOverflow (симуляция)
                }
                else if (choice == 6)
                {
                    Console.WriteLine("Пока!");
                    Console.Read();
                }                                                                                                      //ПЕРЕПИСАТЬ
            }                                                                                                          //ПЕРЕПИСАТЬ
        }                                                                                                              //ПЕРЕПИСАТЬ
                                                                                                                       //ПЕРЕПИСАТЬ
        static void ImportStudentsFromFile(List<Institute> institutes)                                                 //ПЕРЕПИСАТЬ
        {                                                                                                              //ПЕРЕПИСАТЬ
            if (File.Exists(filePath))                                                                                 //ПЕРЕПИСАТЬ
            {                                                                                                          //ПЕРЕПИСАТЬ
                string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);                                           //ПЕРЕПИСАТЬ
                foreach (string line in lines)                                                                         //ПЕРЕПИСАТЬ
                {                                                                                                      //ПЕРЕПИСАТЬ
                    if (!string.IsNullOrWhiteSpace(line))                                                              //ПЕРЕПИСАТЬ
                    {                                                                                                  //ПЕРЕПИСАТЬ
                        string instName;                                                                               //ПЕРЕПИСАТЬ
                        Student st = Student.FromTXT(line, out instName);                                              //ПЕРЕПИСАТЬ
                        var inst = institutes.FirstOrDefault(i => i.nameOfInstitute == instName);                      //ПЕРЕПИСАТЬ
                        inst?.AddStudent(st);                                                                          //ПЕРЕПИСАТЬ
                    }                                                                                                  //ПЕРЕПИСАТЬ
                }                                                                                                      //ПЕРЕПИСАТЬ
            }                                                                                                          //ПЕРЕПИСАТЬ
        }                                                                                                              //ПЕРЕПИСАТЬ
                                                                                                                       //ПЕРЕПИСАТЬ
        static void ExportStudentsToFile(List<Institute> institutes)                                                   //ПЕРЕПИСАТЬ
        {                                                                                                              //ПЕРЕПИСАТЬ
            var lines = new List<string>();                                                                            //ПЕРЕПИСАТЬ
            foreach (var inst in institutes)                                                                           //ПЕРЕПИСАТЬ
                foreach (var st in inst.listOfStudents)                                                                //ПЕРЕПИСАТЬ
                    lines.Add(st.ToTXT(inst.nameOfInstitute));                                                         //ПЕРЕПИСАТЬ
                                                                                                                       //ПЕРЕПИСАТЬ
            File.WriteAllLines(filePath, lines, Encoding.UTF8);                                                        //ПЕРЕПИСАТЬ
        }                                                                                                              //ПЕРЕПИСАТЬ
    }                                                                                                                  //ПЕРЕПИСАТЬ
}                                                                                                                      //ПЕРЕПИСАТЬ