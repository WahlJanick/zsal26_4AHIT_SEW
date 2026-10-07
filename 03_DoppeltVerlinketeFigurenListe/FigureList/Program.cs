using System;

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
		public Figure Previous { get; set; }

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
		public Figure Last { get; set; }
		public int Count { get; set; }

		public Figure GetAt(int index)
		{
			if (index < 0 || index >= Count)
				return null;

			Figure current;

			if (index <= Count / 2)
			{
				current = First;

				for (int i = 0; i < index; i++)
				{
					current = current.Next;
				}
			}
			else
			{
				current = Last;

				for (int i = Count - 1; i > index; i--)
				{
					current = current.Previous;
				}
			}

			return current;
		}

		public void Add(Figure fig)
		{
			if (First == null)
			{
				First = fig;
				Last = fig;

				fig.Previous = null;
				fig.Next = null;
			}
			else
			{
				fig.Previous = Last;
				fig.Next = null;

				Last.Next = fig;
				Last = fig;
			}

			Count++;
		}

		public void PrintAllFigures()
		{
			Figure current = First;

			while (current != null)
			{
				Console.WriteLine(
					$"{current.Description} - {current.CalculateSurface()} - {current.CalculateVolume()}"
				);

				current = current.Next;
			}
		}

		public void PrintAllFiguresReversed()
		{
			Figure current = Last;

			while (current != null)
			{
				Console.WriteLine(
					$"{current.Description} - {current.CalculateSurface()} - {current.CalculateVolume()}"
				);

				current = current.Previous;
			}
		}

		public void Push(Figure fig)
		{
			if (First == null)
			{
				First = fig;
				Last = fig;

				fig.Previous = null;
				fig.Next = null;
			}
			else
			{
				fig.Next = First;
				fig.Previous = null;

				First.Previous = fig;
				First = fig;
			}

			Count++;
		}

		public Figure Pop()
		{
			if (First == null)
				return null;

			Figure f = First;

			First = First.Next;

			if (First != null)
			{
				First.Previous = null;
			}
			else
			{
				Last = null;
			}

			f.Next = null;
			f.Previous = null;

			Count--;

			return f;
		}

		public void InsertAt(int index, Figure fig)
		{
			if (index <= 0)
			{
				Push(fig);
				return;
			}

			if (index >= Count)
			{
				Add(fig);
				return;
			}

			Figure current;

			if (index <= Count / 2)
			{
				current = First;

				for (int i = 0; i < index; i++)
				{
					current = current.Next;
				}
			}
			else
			{
				current = Last;

				for (int i = Count - 1; i > index; i--)
				{
					current = current.Previous;
				}
			}

			fig.Next = current;
			fig.Previous = current.Previous;

			current.Previous.Next = fig;
			current.Previous = fig;

			Count++;
		}

		public void Remove(int index)
		{
			if (index < 0 || index >= Count)
				return;

			if (index == 0)
			{
				Pop();
				return;
			}

			if (index == Count - 1)
			{
				Figure oldLast = Last;

				Last = Last.Previous;
				Last.Next = null;

				oldLast.Previous = null;

				Count--;
				return;
			}

			Figure current;

			if (index <= Count / 2)
			{
				current = First;

				for (int i = 0; i < index; i++)
				{
					current = current.Next;
				}
			}
			else
			{
				current = Last;

				for (int i = Count - 1; i > index; i--)
				{
					current = current.Previous;
				}
			}

			current.Next.Previous = current.Previous;
			current.Previous.Next = current.Next;

			current.Next = null;
			current.Previous = null;

			Count--;
		}

		public void Reverse()
		{
			Figure current = First;

			while (current != null)
			{
				Figure temp = current.Next;

				current.Next = current.Previous;
				current.Previous = temp;

				current = temp;
			}

			Figure tempFirst = First;
			First = Last;
			Last = tempFirst;
		}

		public void SwapNeighbors()
		{
			Figure a = First;

			while (a != null && a.Next != null)
			{
				Figure b = a.Next;
				Figure previous = a.Previous;
				Figure next = b.Next;

				if (previous != null)
				{
					previous.Next = b;
				}
				else
				{
					First = b;
				}

				b.Previous = previous;

				b.Next = a;
				a.Previous = b;

				a.Next = next;

				if (next != null)
				{
					next.Previous = a;
				}
				else
				{
					Last = a;
				}

				a = next;
			}
		}

		public void Attach(LinkedFigureList other)
		{
			if (other == null || other.First == null)
				return;

			if (First == null)
			{
				First = other.First;
				Last = other.Last;
				Count = other.Count;
				return;
			}

			Last.Next = other.First;
			other.First.Previous = Last;

			Last = other.Last;
			Count += other.Count;
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

			Console.WriteLine("\nRückwärts:");
			list.PrintAllFiguresReversed();

			Console.WriteLine($"\nCount: {list.Count}");
		}
	}
}