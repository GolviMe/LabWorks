using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabRab2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Engineer Ehrmantraut = new Engineer("Ehrmantraut", "Mike", "Los Pollos Hermanos", 200000, 2, 33);
            Ehrmantraut.Info();
            Ehrmantraut.ShowWorkExperience();
            Officer Schrader = new Officer("Schrader", "Hank", "Government", 7500, "Drug Enforcement Administration", 19);
            Schrader.Info();
            Schrader.ShowWorkExperience();
            Worker Skyler = new Worker("Skyler", "White", "Car wash Albouquerque", 4500, 13);
            Skyler.Info();
            Skyler.ShowWorkExperience();

            Console.WriteLine();
        }
    }

    abstract class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Person(string lName, string fName)
        {
            FirstName = fName;
            LastName = lName;
        }

        public abstract void Info();
    }

    interface WorkExperience
    {
        void ShowWorkExperience();
        int WorkExp { get; set; }
    }

    class Worker : Person, WorkExperience
    {
        public string Company { get; set; }
        public int Salary { get; set; }
        public int WorkExp { get; set; }

        public Worker(string lName, string fName, string company, int salary, int workExp) : base(lName, fName) 
        {
            Company = company;
            Salary = salary;
            WorkExp = workExp;
        }
        public override void Info()
        {
            Console.WriteLine("{0} {1}, работает. Работодатель: {2}, зарплата: {3}$.\n", LastName, FirstName, Company, Salary);
        }
        public virtual void ShowWorkExperience()
        {
            Console.WriteLine("{0} {1} имеет стаж работы, равный {2} лет. Настоящая деятельность: неизвестно.\n", LastName, FirstName, WorkExp);
        }
    }

    class Officer : Worker, WorkExperience 
    {
        public string NameOfDivision { get; set; }
        public Officer(string lName, string fName, string company, int salary, string nameOfDivision, int workExp) : base(lName, fName, company, salary, workExp)
        {
            NameOfDivision = nameOfDivision;
        }
        public override void Info()
        {
            Console.WriteLine("{0} {1}, работает. Работодатель: {2}, зарплата: {3}$. Профессия: офицер, наименование дивизии: {4}.\n", LastName, FirstName, Company, Salary, NameOfDivision);
        }

        public override void ShowWorkExperience()
        {
            Console.WriteLine("{0} {1} имеет стаж работы, равный {2} лет. Настоящая деятельность: офицер\n", LastName, FirstName, WorkExp);
        }
    }

    class Engineer : Worker, WorkExperience
    {
        private int rank = 1;
        public int Rank
        {
            get
            {
                return rank;
            }
            set
            {
                if (value <= 3 && value > 0)
                {
                    this.rank = value;
                }
            }
        }
        public Engineer(string lName, string fName, string company, int salary, int rank, int workExp) : base(lName, fName, company, salary, workExp)
        {
            Rank = rank;
        }
        public override void Info()
        {
            Console.WriteLine("{0} {1}, работает. Работодатель: {2}, зарплата: {3}$. Профессия: инженер, уровень квалификации: {4}/3.\n", LastName, FirstName, Company, Salary, Rank);
        }

        public override void ShowWorkExperience()
        {
            Console.WriteLine("{0} {1} имеет стаж работы, равный {2} лет. Настоящая деятельность: инженер.\n", LastName, FirstName, WorkExp);
        }
    }
}