using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LabRab3
{

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
            {
                handler(s);
            }
                
        }
    }

    class Student
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
    }

    internal class Program
    {
        static string filePath = @"C:\Учебные материалы\2 курс\КАиСД\Гит\LabWorks\LabRab3\LabRab3\StudentsInfo.txt";

        static void Main(string[] args)
        {
            Institute FKTiPM = new Institute("FKTiPM");
            Institute FMiKN = new Institute("FMiKN");
            Institute FTF = new Institute("FTF");

            List<Institute> institutes = new List<Institute> { FKTiPM, FMiKN, FTF };

            ImportStudentsFromFile(institutes);

            int choice = 0;
            Console.WriteLine("Добро пожаловать в систему!");
            while (choice != 4)
            {
                Console.WriteLine("\nМеню:\n1 - Список студентов\n2 - Добавить студента\n3 - Лидер по отличникам (1 курс)\n4 - Выход");
                try { choice = int.Parse(Console.ReadLine()); }
                catch { Console.WriteLine("Ошибка ввода."); continue; }

                if (choice == 1)
                {
                    foreach (var inst in institutes)
                    {
                        Console.WriteLine($"\nСтуденты института {inst.nameOfInstitute}:");
                        inst.ForEachStudent(PrintStudentInfo);
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
                    else
                        Console.WriteLine("Такого института нет.");
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

                        string filePathForReview = @"C:\Учебные материалы\2 курс\КАиСД\Гит\LabWorks\LabRab3\LabRab3\ReportAboutBestInstitute.txt";
                        var tempLines = new List<string>();

                        tempLines.Add($"Список лучших первокурсников института {bestInst}:");
                        foreach (var inst in institutes)
                        {
                            if(inst.nameOfInstitute == bestInst)
                            {
                                foreach(var st in inst.listOfStudents)
                                {
                                    if(st.lowestMark == 5)
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
                    Console.WriteLine("Пока!");
                    Console.Read();
                }
            }
        }

        static void PrintStudentInfo(Student s)
        {
            Console.WriteLine($"{s.firstNameOfStudent} {s.lastNameOfStudent}, {s.numberOfCourse} курс, мин. оценка {s.lowestMark}");
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
                        string instName;
                        Student st = Student.FromTXT(line, out instName);
                        var inst = institutes.FirstOrDefault(i => i.nameOfInstitute == instName);
                        inst?.AddStudent(st);
                    }
                }
            }
        }

        static void ExportStudentsToFile(List<Institute> institutes)
        {
            var lines = new List<string>();
            foreach (var inst in institutes)
            {
                foreach (var st in inst.listOfStudents)
                    lines.Add(st.ToTXT(inst.nameOfInstitute));
            }
            File.WriteAllLines(filePath, lines, Encoding.UTF8);
        }
    }
}