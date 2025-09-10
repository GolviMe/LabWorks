using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabRab1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Chemist Heisenberg = new Chemist("Walter", "White", 50, "Male", "Public School of Albuquerque", 6500, 7);
            Heisenberg.Info();
            Engineer Ehrmantraut = new Engineer("Mike", "Ehrmantraut", 69, "Male", "Los Pollos Hermanos", 200000, 1);
            Ehrmantraut.Info();
            Officer Schrader = new Officer("Hank", "Schrader", 43, "Male", "Government", 7500, "Drug Enforcement Administration", "El Paso Division");
            Schrader.Info();
            Person Skyler = new Person("Skyler", "White", 42, "Female");
            Skyler.Info();

            Console.WriteLine();
        }
    }
    public class Person
    {
        private string name;
        private string secondName;
        private int age;
        private string gender;

        public Person(string name, string secondName, int age, string gender)
        {
            this.name = name;
            this.secondName = secondName;
            this.age = age;
            this.gender = gender;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public string SecondName
        {
            get
            {
                return this.secondName;
            }
            set
            {
                this.secondName = value;
            }
        }

        public int Age
        {
            get
            {
                return this.age;
            }
            set
            {
                if (value > 0)
                {
                    this.age = value;
                }
            }
        }

        public string Gender
        {
            get
            {
                return this.gender;
            }
            set
            {
                this.gender = value;
            }
        }

        public virtual void Info()
        {
            Console.Write("{0} {1}, {2} y.o., {3}. ", name, secondName, age, gender);
        }
    }

    public class Worker : Person
    {
        private string company;
        private int salary;

        public string Company
        {
            get
            {
                return this.company;
            }
            set
            {
                this.company = value;
            }
        }

        public int Salary
        {
            get
            {
                return salary;
            }
            set
            {
                if (value > 0)
                {
                    this.salary = value;
                }
            }
        }

        public Worker(string name, string secondName, int age, string gender, string company, int salary) : base(name, secondName, age, gender)
        {
            this.salary = salary;
            this.company = company;
        }

        public override void Info()
        {
            base.Info();
            Console.Write("Компания: {0}, з/п: {1}$. ", company, salary);
        }
    }

    public class Officer : Worker
    {
        private string department;
        private string nameOfDivision;

        public string Department
        {
            get
            {
                return this.department;
            }
            set
            {
                this.department = value;
            }
        }


        public string NameDivision
        {
            get
            {
                return nameOfDivision;
            }
            set
            {
                this.nameOfDivision = value;
            }
        }

        public Officer(string name, string secondName, int age, string gender, string company, int salary, string department, string nameOfDivision) : base(name, secondName, age, gender, company, salary)
        {
            this.department = department;
            this.nameOfDivision = nameOfDivision;
        }

        public override void Info()
        {
            base.Info();
            Console.WriteLine("Должность: офицер. Департамент: {0}, имя подразделения: {1}. ", department, nameOfDivision);
        }
    }

    public class Engineer : Worker
    {
        private int classOfQualification;

        public int ClassOfQualification
        {
            get
            {
                return this.classOfQualification;
            }
            set
            {
                if (value >= 0 && value <= 3)
                {
                    this.classOfQualification = value;
                }
            }
        }

        public Engineer(string name, string secondName, int age, string gender, string company, int salary, int classOfQualification) : base(name, secondName, age, gender, company, salary)
        {
            this.classOfQualification = classOfQualification;
        }

        public override void Info()
        {
            base.Info();
            Console.WriteLine("Должность: Инженер. Класс квалификации: {0} из 3. ", classOfQualification);
        }
    }

    public class Chemist : Worker
    {
        private int classOfQualification;

        public int ClassOfQualification
        {
            get
            {
                return this.classOfQualification;
            }
            set
            {
                if (value >= 0 && value <= 7)
                {
                    this.classOfQualification = value;
                }
            }
        }

        public Chemist(string name, string secondName, int age, string gender, string company, int salary, int classOfQualification) : base(name, secondName, age, gender, company, salary)
        {
            this.classOfQualification = classOfQualification;
        }

        public override void Info()
        {
            base.Info();
            Console.WriteLine("Должность: Химик. Класс квалификации: {0} из 7. ", classOfQualification);
        }
    }
}
