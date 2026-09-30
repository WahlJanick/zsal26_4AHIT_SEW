using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
namespace VerlinkteListen
{
    interface IFigure
    {
        double CalculateSurface();
        double CalculateVolume();
    }

    abstract class Figure : IFigure
    {
        public Figure(string beschreibung)
        {
            Description = beschreibung;
        }

        public string Description { get; }

        public Figure Next { get; set; }

        public abstract double CalculateSurface();
        public abstract double CalculateVolume();

        public override string ToString()
        {
            return $"{Description} (Oberflaeche={CalculateSurface():F2}, Volumen={CalculateVolume():F2})";
        }
    }

    class Sphere : Figure
    {
        public double Radius { get; }

        public Sphere(string beschreibung, double radius) : base(beschreibung)
        {
            Radius = radius;
        }

        public override double CalculateSurface()
        {
            return 4.0 * Math.PI * Math.Pow(Radius, 2);
        }

        public override double CalculateVolume()
        {
            return (4.0 * Math.PI * Math.Pow(Radius, 3)) / 3.0;
        }
    }

    class Cube : Figure
    {
        public double Sidelength { get; }

        public Cube(string beschreibung, double a) : base(beschreibung)
        {
            Sidelength = a;
        }

        public override double CalculateSurface()
        {
            return 6.0 * Math.Pow(Sidelength, 2);
        }

        public override double CalculateVolume()
        {
            return Math.Pow(Sidelength, 3);
        }
    }

    class LinkedFigureList
    {
        public Figure First { get; set; }
        public int Count { get; set; }
        public Figure GetAt(int index)
        {
            Figure current = First;
            for(int i = 0; i < index; i++)
            {
                current = current.Next;
            }
            return current;
        }
        public void Add(Figure fig)
        {
            Figure current = First;
            while (current.Next != null)
            {
                current = current.Next;
            }
            current.Next = fig;
            Count++;
        }
        public void PrintAllFigures()
        {
            Figure current = First;

            while (current != null)
            {
                Console.WriteLine($"{current.Description} {current.CalculateSurface()} {current.CalculateVolume()}");
                current = current.Next;
            }
        }
        public void Push(Figure fig)
        {
            fig.Next = First;
            First = fig;
            Count++;
        }
        public Figure Pop()
        {
            Figure res = First;
            First = First.Next;
            res.Next = null;
            Count--;
            return res;
        }
        public void InsertAt(int index, Figure fig)
        {
            Figure prev = GetAt(index - 1);
            fig.Next = prev.Next;
            prev.Next = fig;
            Count++;
        }
        public void Remove(int index)
        {
            Figure prev = GetAt(index - 1);
            prev.Next = prev.Next.Next;
            Count--;
        }
        public void Reverse()
        {
            Figure next, prev = null, current = First;

            while (current != null)
            {
                next = current.Next;
                current.Next = prev;
                prev = current;
                current = next;
            }
            First = prev;
        }
        public void SwapNeighbors()
        {
            if (First == null || First.Next == null)
                return;

            Figure previous = null;
            Figure current = First;

            First = First.Next;

            while (current != null && current.Next != null)
            {
                Figure second = current.Next;
                Figure nextPair = second.Next;

                second.Next = current;
                current.Next = nextPair;

                if (previous != null)
                {
                    previous.Next = second;
                }

                previous = current;
                current = nextPair;
            }
        }

        public void Attach(LinkedFigureList other)
        {
            if (other == null || other.First == null)
                return;

            if (First == null)
            {
                First = other.First;
            }
            else
            {
                Figure current = First;

                while (current.Next != null)
                {
                    current = current.Next;
                }

                current.Next = other.First;
            }

            Count += other.Count;

            other.First = null;
            other.Count = 0;
        }
    }
    class Program
    {
        static void Main()
        {
            LinkedFigureList list = new LinkedFigureList();

            list.Add(new Sphere("Kugel 1", 2));
            list.Add(new Cube("Würfel 1", 3));
            list.Add(new Sphere("Kugel 2", 4));

            Console.WriteLine("Normale Liste:");
            list.PrintAllFigures();

            Console.WriteLine("\nGetAt(1):");
            Console.WriteLine(list.GetAt(1));

            Console.WriteLine("\nPush:");
            list.Push(new Cube("Würfel Push", 2));
            list.PrintAllFigures();

            Console.WriteLine("\nPop:");
            Console.WriteLine(list.Pop());
            list.PrintAllFigures();

            Console.WriteLine("\nInsertAt:");
            list.InsertAt(1, new Sphere("Kugel Insert", 3));
            list.PrintAllFigures();

            Console.WriteLine("\nRemove:");
            list.Remove(1);
            list.PrintAllFigures();

            Console.WriteLine("\nReverse:");
            list.Reverse();
            list.PrintAllFigures();

            Console.WriteLine("\nSwapNeighbors:");
            list.SwapNeighbors();
            list.PrintAllFigures();

            LinkedFigureList list2 = new LinkedFigureList();
            list2.Add(new Cube("Würfel 2", 5));
            list2.Add(new Sphere("Kugel 3", 1));

            Console.WriteLine("\nAttach:");
            list.Attach(list2);
            list.PrintAllFigures();
        }
    }
}