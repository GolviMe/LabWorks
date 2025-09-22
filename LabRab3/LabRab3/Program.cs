using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabRab3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int choice = 0;

            List<Student> listOfStudents = ImportStudentsFromFile();

            Console.WriteLine("Добро пожаловать в систему!");
            while (choice != 4)
            {
                Console.WriteLine("Выберите, что требуется:\n1 - Посмотреть список студентов\n2 - Добавить студента\n3 - Вывести институт, на котором на первом курсе наибольшее количество отличников\n4 - Выход из системы\n");

                try
                {
                    choice = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Вы ввели неверные данные, попробуйте ещё раз\n");
                }

                if (choice == 1)
                {
                    //Показать список студентов

                    Console.WriteLine("\nВсе студенты:");
                    foreach (var s in listOfStudents)
                    {
                        Console.WriteLine($"{s.firstNameOfStudent} {s.lastNameOfStudent}, обучается на {s.numberOfCourse} курсе факультета {s.nameOfInstitute}, минимальная оценка: {s.lowestMark}");
                    }

                }
                else if (choice == 2)
                {
                    //Добавить студента
                    Console.WriteLine("Введите имя: ");
                    string firstName = Console.ReadLine();

                    Console.WriteLine("Введите фамилию: ");
                    string lastName = Console.ReadLine();

                    Console.WriteLine("Введите институт: ");
                    string institute = Console.ReadLine();

                    Console.WriteLine("Введите номер курса: ");
                    int course = int.Parse(Console.ReadLine());

                    Console.WriteLine("Введите минимальную оценку: ");
                    int mark = int.Parse(Console.ReadLine());

                    Student newStudent = new Student(firstName, lastName, institute, course, mark);
                    listOfStudents.Add(newStudent);

                    ExportStudentsToFile(listOfStudents);

                    Console.WriteLine("Студент добавлен!\n");
                }
                else if (choice == 3)
                {
                    //Вывести институт, на котором на первом курсе наибольшее количество отличников

                    int maxValueOfMarks = 0;
                    string nameOfBestInstitute = "";
                    
                    Dictionary<string, int> bestInstitute = new Dictionary<string, int>();
                    foreach (var s in listOfStudents)
                    {
                        
                        if (s.lowestMark == 5 && s.numberOfCourse == 1)
                        {
                            if (bestInstitute.ContainsKey(s.nameOfInstitute))
                            {
                                bestInstitute[s.nameOfInstitute] += 1;
                            }
                            else bestInstitute[s.nameOfInstitute] = 1;
                            if (maxValueOfMarks < bestInstitute[s.nameOfInstitute])
                            {
                                maxValueOfMarks = bestInstitute[s.nameOfInstitute];
                                nameOfBestInstitute = s.nameOfInstitute;
                            }
                        }
                    }
                    if (maxValueOfMarks != 0)
                    {
                        Console.WriteLine("Название института, на котором на первом курсе больше всего отличников: {0}.", nameOfBestInstitute);
                    }
                    else
                    {
                        Console.WriteLine("Такого института пока что не существует.");
                    }
                }
                else if (choice == 4)
                {
                    Console.WriteLine("Пока");
                    Console.Read();
                }
                else
                {
                    Console.WriteLine("Вы ввели неправильное значение, попробуйте ещё раз");
                }
            }
        }

        static string filePath = @"C:\Учебные материалы\2 курс\КАиСД\Гит\LabWorks\LabRab3\LabRab3\StudentsInfo.txt";

        static List<Student> ImportStudentsFromFile()
        {
            var result = new List<Student>();
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        result.Add(Student.FromTXT(line));
                }
            }
            return result;
        }

        static void ExportStudentsToFile(List<Student> students)
        {
            string[] lines = new string[students.Count];
            for (int i = 0; i < students.Count; i++)
            {
                lines[i] = students[i].ToTXT();
            }
            File.WriteAllLines(filePath, lines, Encoding.UTF8);
        }

    }
    class Student
    {
        public string firstNameOfStudent;
        public string lastNameOfStudent;
        public string nameOfInstitute;

        public int numberOfCourse;
        public int lowestMark;

        public Student(string firstNameOfStudent, string lastNameOfStudent, string nameOfInstitute, int numberOfCourse, int lowestMark)
        {
            this.firstNameOfStudent = firstNameOfStudent;
            this.lastNameOfStudent = lastNameOfStudent;
            this.nameOfInstitute = nameOfInstitute;
            this.numberOfCourse = numberOfCourse;
            this.lowestMark = lowestMark;
        }
        public string ToTXT()
        {
            return $"{firstNameOfStudent} {lastNameOfStudent} {nameOfInstitute} {numberOfCourse} {lowestMark}";
        }
        public static Student FromTXT(string text)
        {
            string[] parts = text.Split();
            return new Student(parts[0], parts[1], parts[2], Convert.ToInt32(parts[3]), Convert.ToInt32(parts[4]));
        }
    }
}
