using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_Delegates
{
    public class Student
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public int Grade { get; set; }
    }
    internal class Program
    {
        public delegate void StudentAction(Student s);
        public delegate bool StudentFilter(Student s);
        static List<Student> Filter(List<Student> l, StudentFilter s) => l.Where(student => s(student)).ToList();
        static bool IsPositive(Student s) => s.Grade < 5;
        static bool IsNegative(Student s) => !IsPositive(s);
        static bool Is4AHIT(Student s) => s.Class is "4AHIT";
        static bool IsVeryGood(Student s) => s.Grade is 1 || s.Grade is 2;
        static void PrintAll(List<Student> l) => l.ForEach(s => PrintLong(s));
        static void PrintGrade(Student s)
        {
            string grade;

            switch (s.Grade)
            {
                case 1:
                    grade = "Sehr gut";
                    break;
                case 2:
                    grade = "Gut";
                    break;
                case 3:
                    grade = "Befriedigend";
                    break;
                case 4:
                    grade = "Genügend";
                    break;
                case 5:
                    grade = "Nicht genügend";
                    break;
                default:
                    grade = "Invalid";
                    break;
            }

            Console.WriteLine($"{s.Name}: {grade}");
        }
        static void PrintShort(Student s) => Console.WriteLine($"Name: {s.Name}");

        static void PrintLong(Student s) => Console.WriteLine($"Name: {s.Name} | Class: {s.Class} | Grade: {s.Grade}");

        static void ForeachStudent(List<Student> list, StudentAction act) => list.ForEach(s => act(s));

        static void Main(string[] args) 
        {
            List<Student> list = new List<Student>
            { 
                new Student{Name = "Janick", Class = "4AHIT", Grade = 3},
                new Student{Name = "Christoph", Class = "4AHIT", Grade = 4},
                new Student{Name = "Michael", Class = "4AHIT", Grade = 4},
                new Student{Name = "Thomas", Class = "3AHIT", Grade = 1},
                new Student{Name = "Alex", Class = "4AHIT", Grade = 2},
                new Student{Name = "Peter", Class = "1AHIT", Grade = 5},
                new Student{Name = "Brezeljunge", Class = "5AHIT", Grade = 5},
                new Student{Name = "Spring", Class = "3AHMBA", Grade = 2 }
            };
            /*
            StudentAction action = PrintShort;
            action(list[0]);
            action = PrintLong;
            action(list[0]);
            ForeachStudent(list, PrintShort);
            ForeachStudent(list, PrintLong);
            ForeachStudent(list, PrintGrade);
            Filter(list, Is4AHIT);
            Filter(list, IsNegative);
            Filter(list, IsPositive);
            Filter(list, IsVeryGood);
            */
            StudentFilter filter;

            Console.WriteLine("Choose a filter:");
            Console.WriteLine("1 - All students");
            Console.WriteLine("2 - Positive students");
            Console.WriteLine("3 - Negative students");
            Console.WriteLine("4 - 4AHIT students");
            Console.WriteLine("5 - Very good students");

            switch (Console.ReadLine())
            {
                case "1":
                    filter = s => true;
                    break;
                case "2":
                    filter = IsPositive;
                    break;
                case "3":
                    filter = IsNegative;
                    break;
                case "4":
                    filter = Is4AHIT;
                    break;
                case "5":
                    filter = IsVeryGood;
                    break;
                default:
                    Console.WriteLine("Invalid filter!");
                    return;
            }

            Console.WriteLine("\nChoose an output format:");
            Console.WriteLine("1 - Short");
            Console.WriteLine("2 - Long");

            StudentAction action;

            switch (Console.ReadLine())
            {
                case "1":
                    action = PrintShort;
                    break;
                case "2":
                    action = PrintLong;
                    break;
                default:
                    Console.WriteLine("Invalid output format!");
                    return;
            }

            Console.WriteLine("\nResults:");
            ForeachStudent(Filter(list, filter), action);

        }
    }
}
