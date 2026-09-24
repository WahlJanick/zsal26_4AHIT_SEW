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

        }
        public void SwapNeighbors()
        {

        }
        public void Attach(LinkedFigureList other)
        {

        }
    }
    class Program
    {
        static void Main()
        {
            
        }
    }
}