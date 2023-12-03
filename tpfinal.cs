using System;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;

namespace practica5
{
	class Program
	{
		public static void Main(string[] args)
		{
			Pila pila = new Pila();
			Aula aula = new Aula();

			pila.SetOrdenInicio(new OrdenInicio(aula));
			pila.SetOrdenLlegaAlumno(new OrdenLlegaAlumno(aula));
			pila.SetOrdenAulaLlena(new OrdenAulaLlena(aula));

			Llenar(pila, 5);
			Llenar(pila, 6);

			Console.ReadKey();
		}

		public static void Llenar(IColeccionable c, int opcion)
		{
			for (int i = 1; i <= 20; i++)
			{
				c.Agregar(FabricaDeComparables.CrearAleatorio(opcion));
			}
		}

		public static void LlenarPersonas(IColeccionable coleccionable)
		{
			string[] nombres = { "Ana", "Juan", "Maria", "Pedro", "Laura", "Carlos", "Sofia", "Luis", "Elena", "Miguel" };
			Random rand = new Random();

			for (int i = 0; i < 20; i++)
			{
				string nombreAleatorio = nombres[rand.Next(nombres.Length)];
				int dniAleatorio = rand.Next(10000000, 99999999); // DNI aleatorio de 8 digitos

				IComparable personaAleatoria = new Persona(nombreAleatorio, dniAleatorio);
				coleccionable.Agregar(personaAleatoria);
			}
		}
		public static void DictadoDeClases(Profesor profe)
		{
			for (int i = 0; i <= 5; i++)
			{
				profe.HablarALaClase();
				profe.EscribirEnElPizarron();
			}
		}

		public static void LlenarAlumnos(IColeccionable c)
		{
			Random num = new Random();

			// Lista de nombres
			string[] nombres = { "Ana", "Juan", "Maria", "Pedro", "Laura", "Carlos", "Sofia", "Luis", "Elena", "Miguel" };

			Console.WriteLine("MENU DE OPCIONES PARA ALUMNOS:");
			Console.WriteLine("---------------------------------");
			Console.WriteLine("A | COMPARAR POR NOMBRE");
			Console.WriteLine("B | COMPARAR POR DNI");
			Console.WriteLine("C | COMPARAR POR LEGAJO");
			Console.WriteLine("D | COMPARAR POR PROMEDIO");
			Console.Write("ELIJE UNA OPCION: ");
			string opcion = Console.ReadLine().ToLower();

			IComparacionAlumnos estrategia = null;
			switch (opcion)
			{
				case "a":
					estrategia = new ComparadorPorNombre();
					break;
				case "b":
					estrategia = new ComparadorPorDni();
					break;
				case "c":
					estrategia = new ComparadorPorLegajo();
					break;
				case "d":
					estrategia = new ComparadorPorPromedio();
					break;
			}

			for (int i = 1; i <= 5; i++)
			{
				int dni = num.Next(10000000, 99999999);
				int legajo = num.Next(0, 99999);
				int promedio = num.Next(0, 10);
				int index = num.Next(nombres.Length);
				string nombre = nombres[index];
				double calificacion = num.Next(0, 10);

				IComparable p = new Alumno(nombre, dni, legajo, promedio, estrategia, calificacion);
				((Alumno)p).SetEstrategia(estrategia); // Asigna la estrategia a cada alumno
				c.Agregar(p);
				Console.Clear();
			}
		}

		public static void Informar(IColeccionable coleccionable, int opcion)
		{
			Console.WriteLine("INFORME");
			Console.WriteLine("----------------------------------");
			int cant = coleccionable.Cuantos();
			Console.WriteLine("Cantidad de datos: " + cant);
			Console.WriteLine("Dato minimo: " + coleccionable.Minimo().ToString());
			Console.WriteLine("Dato maximo: " + coleccionable.Maximo().ToString());
			IComparable a = FabricaDeComparables.CrearPorTeclado(opcion);
			if (coleccionable.Contiene(a))
				Console.WriteLine("El elemento leido esta en la coleccion");
			else
				Console.WriteLine("El elemento leido no esta en la coleccion");
		}

		public static void ImprimirElementos(IColeccionable c)
		{
			ITerador i = ((ITerable)c).CrearIterador();
			while (!(i.Fin()))
			{
				Console.WriteLine(i.Actual());
				i.Siguiente();
			}
		}

		public static void CambiarEstrategia(IColeccionable c, IComparacionAlumnos a)
		{
			ITerador i = ((ITerable)c).CrearIterador();
			while (!(i.Fin()))
			{
				Alumno alum = (Alumno)i.Actual();
				alum.SetEstrategia(a);
				i.Siguiente();
			}
		}

	}

	public interface IComparable
	{
		bool SosIgual(IComparable p);
		bool SosMenor(IComparable p);
		bool SosMayor(IComparable p);
	}

	public class Numero : IComparable
	{
		private int valor;

		public Numero(int v)
		{
			this.valor = v;
		}

		public int GetValor()
		{
			return valor;
		}

		public bool SosIgual(IComparable otro)
		{
			Numero n = (Numero)otro;
			if (this.valor == n.GetValor())
			{ return true; }
			else
			{ return false; }
		}

		public bool SosMenor(IComparable otro)
		{
			Numero g = (Numero)otro;
			if (this.valor < g.GetValor())
			{ return true; }
			else
			{ return false; }
		}

		public bool SosMayor(IComparable otro)
		{
			Numero f = (Numero)otro;
			if (this.valor > f.GetValor())
			{ return true; }
			else
			{ return false; }
		}
		public override string ToString()
		{
			return valor.ToString();
		}
		public int VALOR
		{
			set { this.valor = value; }
			get { return this.valor; }
		}
	}

	public interface IColeccionable
	{
		int Cuantos();
		IComparable Minimo();
		IComparable Maximo();
		void Agregar(IComparable comparable);
		bool Contiene(IComparable comparable);
	}

	public class Pila : IColeccionable, ITerable, IOrdenable
	{
		private List<IComparable> elementos;
		private IOrdenEnAula1 OrdenInicio, OrdenFin;
		private IOrdenEnAula2 OrdenLlega;

		public void SetOrdenInicio(IOrdenEnAula1 iniciador)
		{
			this.OrdenInicio = iniciador;
		}
		public void SetOrdenLlegaAlumno(IOrdenEnAula2 llega)
		{
			this.OrdenLlega = llega;

		}
		public void SetOrdenAulaLlena(IOrdenEnAula1 finalizador)
		{
			this.OrdenFin = finalizador;
		}

		public Pila()
		{
			elementos = new List<IComparable>();
		}

		public List<IComparable> Getlist() { return elementos; }

		public int Cuantos()
		{
			return elementos.Count;
		}

		public IComparable Minimo()
		{
			if (elementos.Count == 0)
			{
				throw new InvalidOperationException("La coleccion esta vacia");
			}

			IComparable min = elementos[0];
			foreach (IComparable elem in elementos)
			{
				if (elem.SosMenor(min))
				{
					min = elem;
				}
			}
			return min;
		}

		public IComparable Maximo()
		{
			if (elementos.Count == 0)
			{
				throw new InvalidOperationException("La coleccion esta vacia");
			}

			IComparable max = elementos[0];
			foreach (IComparable elem in elementos)
			{
				if (elem.SosMayor(max))
				{
					max = elem;
				}
			}
			return max;
		}

		public void Agregar(IComparable comparable)
		{
			elementos.Add(comparable); // Agrega al final de la lista para simular una pila
			if (Cuantos() == 1)
			{
				Console.WriteLine("orden inicio...");
				OrdenInicio.Ejecutar();
			}

			Console.WriteLine("orden llega alumno...");
			OrdenLlega.Ejecutar(comparable);

			if (Cuantos() == 40)
			{
				Console.WriteLine("orden aula llena...");
				OrdenFin.Ejecutar();
			}
			Console.WriteLine("Elemento agregado a la Pila: " + comparable);
		}

		public bool Contiene(IComparable h)
		{
			foreach (IComparable elem in elementos)
			{
				if (elem.SosIgual(h))
				{
					return true;
				}
			}
			return false;
		}

		public ITerador CrearIterador()
		{
			return new IteradorPila(this);
		}
	}

	public class Cola : IColeccionable, ITerable, IOrdenable
	{
		private List<IComparable> elementos;
		private IOrdenEnAula1 OrdenInicio, OrdenFin;
		private IOrdenEnAula2 OrdenLlega;

		public void SetOrdenInicio(IOrdenEnAula1 iniciador)
		{
			this.OrdenInicio = iniciador;
		}
		public void SetOrdenLlegaAlumno(IOrdenEnAula2 llega)
		{
			this.OrdenLlega = llega;

		}
		public void SetOrdenAulaLlena(IOrdenEnAula1 finalizador)
		{
			this.OrdenFin = finalizador;
		}

		public Cola()
		{
			elementos = new List<IComparable>();
		}

		public int Cuantos()
		{
			return elementos.Count;
		}

		public IComparable Minimo()
		{
			if (elementos.Count == 0)
			{
				throw new InvalidOperationException("La cola esta vacia");
			}

			IComparable min = elementos[0];
			foreach (IComparable elem in elementos)
			{
				if (elem.SosMenor(min))
				{
					min = elem;
				}
			}
			return min;
		}

		public IComparable Maximo()
		{
			if (elementos.Count == 0)
			{
				throw new InvalidOperationException("La cola esta vacia");
			}

			IComparable max = elementos[0];
			foreach (IComparable elem in elementos)
			{
				if (elem.SosMayor(max))
				{
					max = elem;
				}
			}
			return max;
		}

		public void Agregar(IComparable comparable)
		{
			elementos.Add(comparable); // Agrega al final de la lista para simular una pila
			if (Cuantos() == 1)
			{
				Console.WriteLine("orden inicio...");
				OrdenInicio.Ejecutar();
			}

			Console.WriteLine("orden llega alumno...");
			OrdenLlega.Ejecutar(comparable);

			if (Cuantos() == 40)
			{
				Console.WriteLine("orden aula llena...");
				OrdenFin.Ejecutar();
			}
			Console.WriteLine("Elemento agregado a la Cola: " + comparable);
		}

		public bool Contiene(IComparable h)
		{
			foreach (IComparable elem in elementos)
			{
				if (elem.SosIgual(h))
				{
					return true;
				}
			}
			return false;
		}
		public List<IComparable> getlist() { return elementos; }

		public override string ToString()
		{
			return "Pila";
		}

		public ITerador CrearIterador()
		{
			return new IteradorCola(this);
		}
	}

	public class MiColeccion : IColeccionable
	{
		private List<IComparable> elementos;

		public MiColeccion()
		{
			elementos = new List<IComparable>();
		}

		public int Cuantos()
		{
			return elementos.Count;
		}

		public IComparable Minimo()
		{
			if (elementos.Count == 0)
			{
				throw new InvalidOperationException("La coleccion esta vacia");
			}

			IComparable min = null;

			foreach (IComparable elem in elementos)
			{
				if (min == null || elem.SosMenor(min))
				{
					min = elem;
				}
			}

			if (min == null)
			{
				throw new InvalidOperationException("No se encontraron objetos comparables en la coleccion");
			}

			return min;
		}

		public IComparable Maximo()
		{
			if (elementos.Count == 0)
			{
				throw new InvalidOperationException("La coleccion esta vacia");
			}

			IComparable max = null;

			foreach (IComparable elem in elementos)
			{
				if (max == null || elem.SosMayor(max))
				{
					max = elem;
				}
			}

			if (max == null)
			{
				throw new InvalidOperationException("No se encontraron objetos comparables en la coleccion");
			}

			return max;
		}


		public void Agregar(IComparable comp)
		{
			elementos.Add(comp);
		}

		public bool Contiene(IComparable comparable)
		{
			foreach (IComparable elem in elementos)
			{
				if (elem.SosIgual(comparable))
				{
					return true;
				}
			}
			return false;
		}
	}

	public class ColeccionMultiple : IColeccionable
	{
		private Pila p;
		private Cola c;

		public ColeccionMultiple(Pila p, Cola c)
		{
			this.p = p;
			this.c = c;
		}

		public int Cuantos()
		{
			return p.Cuantos() + c.Cuantos();
		}

		public IComparable Minimo()
		{
			IComparable minimoP = p.Minimo();
			IComparable minimoC = c.Minimo();

			if (minimoP.SosMenor(minimoC))
			{
				return minimoP;
			}
			else
			{
				return minimoC;
			}
		}

		public IComparable Maximo()
		{
			IComparable maximoP = p.Maximo();
			IComparable maximoC = c.Maximo();

			if (maximoP.SosMayor(maximoC))
			{
				return maximoP;
			}
			else
			{
				return maximoC;
			}
		}

		public void Agregar(IComparable comparable)
		{
			// No hace nada, porque no se puede agregar directamente a una coleccion multiple.
		}

		public bool Contiene(IComparable comparable)
		{
			return p.Contiene(comparable) || c.Contiene(comparable);
		}
	}

	public class Persona : IComparable
	{
		private string nombre;
		private int dni;

		public Persona(string n, int d)
		{
			this.nombre = n;
			this.dni = d;
		}

		public string GetNombre()
		{
			return nombre;
		}

		public int GetDNI()
		{
			return dni;
		}

		public bool SosIgual(IComparable otro)
		{
			Persona n = (Persona)otro;
			if (this.dni == n.GetDNI())
			{ return true; }
			else
			{ return false; }
		}

		public bool SosMenor(IComparable otro)
		{
			Persona n = (Persona)otro;
			if (this.dni < n.GetDNI())
			{ return true; }
			else
			{ return false; }
		}

		public bool SosMayor(IComparable otro)
		{
			Persona n = (Persona)otro;
			if (this.dni > n.GetDNI())
			{ return true; }
			else
			{ return false; }
		}
	}

	public class Alumno : Persona, IComparable, ITerado, IObservador, IAlumno
	{
		private int legajo;
		private double promedio;
		private IComparacionAlumnos comp;
		private double calificacion;
		private Random random;
		public Alumno(string n, int d, int l, double p, IComparacionAlumnos estrategia, double calificacion) : base(n, d)
		{
			this.legajo = l;
			this.promedio = p;
			this.comp = estrategia;
			this.calificacion = calificacion;
			this.random = new Random();
		}

		public double GetCalificacion()
		{
			return calificacion;
		}

		public void SetCalificacion(double c)
		{
			calificacion = c;
		}

		public int GetLegajo()
		{
			return legajo;
		}

		public double GetPromedio()
		{
			return promedio;
		}

		public virtual int ResponderPregunta(int pregunta)
		{
			return random.Next(1, 4);
		}

		public string MostrarCalificacion()
		{
			return string.Format("Nombre: {0}, legajo: {1}, Calificacion: {2} ", this.GetNombre(), this.legajo, this.calificacion);
		}

		public bool SosIgual(IComparable otro)
		{
			return comp.SosIgual(this, (IAlumno)otro);
		}

		public bool SosMenor(IComparable otro)
		{
			return comp.SosMenor(this, (IAlumno)otro);
		}

		public bool SosMayor(IComparable otro)
		{
			return comp.SosMayor(this, (IAlumno)otro);
		}
		public void PrestarAtencion()
		{
			Console.WriteLine("Alumno " + GetNombre() + " está prestando atención.");
		}

		public void Distraerse()
		{
			List<string> frasesDistraerse = new List<string>
		{
		"Alumno " + GetNombre() + " está mirando el celular",
		"Alumno " + GetNombre() + " está dibujando en el margen de la carpeta",
		"Alumno " + GetNombre() + " está tirando aviones de papel"
		};

			Random random = new Random();
			int indiceFrase = random.Next(frasesDistraerse.Count);
			string fraseElegida = frasesDistraerse[indiceFrase];

			Console.WriteLine(fraseElegida);
		}

		public void Actualizar(IObservado o)
		{
			Profesor profesor = o as Profesor;
			if (profesor != null)
			{
				if (profesor.EstaHablando())
				{
					PrestarAtencion();
				}
				else
				{
					Distraerse();
				}
			}
		}

		public void SetEstrategia(IComparacionAlumnos a)
		{
			comp = a;
		}
		public override string ToString()
		{
			return "Alumno: " + GetNombre() + " DNI: " + GetDNI() + " legajo: " + this.legajo + " Promedio: " + this.promedio;
		}
	}

	public class AlumnoMuyEstudioso : Alumno
	{
		public AlumnoMuyEstudioso(string nombre, int dni, int legajo, double promedio, IComparacionAlumnos estrategia, double calificacion) : base(nombre, dni, legajo, promedio, estrategia, calificacion)
		{

		}
		public override int ResponderPregunta(int pregunta)
		{
			// Devuelve la respuesta correcta a la pregunta
			return pregunta % 3;
		}
	}

	public interface IStudent
	{
		string GetName();
		int YourAnswerIs(int question);
		void SetScore(int score);
		string ShowResult();
		bool Equals(IStudent student);
		bool LessThan(IStudent student);
		bool GreaterThan(IStudent student);
	}

	public interface ICollection
	{
		IIteratorOfStudent GetIterator();
		void AddStudent(IStudent student);
		void Sort();
	}

	public interface IIteratorOfStudent
	{
		void Begin();
		bool End();
		IStudent Current();
		void Next();
	}

	public class ListOfStudent : ICollection
	{
		private List<IStudent> list = new List<IStudent>();

		public IIteratorOfStudent GetIterator()
		{
			return new ListOfStudentIterator(list);
		}

		public void AddStudent(IStudent student)
		{
			list.Add(student);
		}

		public void Sort()
		{
			list.Sort(new StudentComparer());
		}
	}

	public class ListOfStudentIterator : IIteratorOfStudent
	{
		private List<IStudent> list;
		private int index;

		public ListOfStudentIterator(List<IStudent> _list)
		{
			list = _list;
		}

		public void Begin()
		{
			index = 0;
		}

		public bool End()
		{
			return index >= list.Count;
		}

		public IStudent Current()
		{
			return list[index];
		}

		public void Next()
		{
			index++;
		}
	}

	public class StudentComparer : IComparer<IStudent>
	{
		public int Compare(IStudent s1, IStudent s2)
		{
			// Compara los nombres de los estudiantes para ordenar alfabeticamente
			return s1.GetName().CompareTo(s2.GetName());
		}
	}

	public class Teacher
	{
		private ICollection students;

		public Teacher()
		{
			students = new ListOfStudent();
		}

		public void SetStudents(ICollection _students)
		{
			students = _students;
		}

		public void GoToClass(IStudent student)
		{
			students.AddStudent(student);
		}

		public void TeachingAClass()
		{
			IStudent student;
			IIteratorOfStudent iterator = students.GetIterator();

			// Taking attendance
			Console.WriteLine("Good morning\n\n");
			Console.WriteLine("I'm going to take attendance");
			iterator.Begin();
			while (!iterator.End())
			{
				student = iterator.Current();
				Console.WriteLine("\t" + student.GetName() + " is present");
				iterator.Next();
			}
			Console.WriteLine("\n\n");

			// Taking an exam
			Console.WriteLine("I'm going to take an exam");
			iterator.Begin();
			while (!iterator.End())
			{
				student = iterator.Current();
				TakeAnExam(student);
				iterator.Next();
			}
			Console.WriteLine("\n\n");

			// Displaying results
			Console.WriteLine("Exam results");
			students.Sort();
			iterator.Begin();
			while (!iterator.End())
			{
				student = iterator.Current();
				Console.WriteLine(student.ShowResult() + "\n");
				iterator.Next();
			}
		}

		private void TakeAnExam(IStudent student)
		{
			int hits = 0;
			for (int i = 0; i < 10; i++)
			{
				int answer = student.YourAnswerIs(i);
				if (answer == (i % 3))
					hits++;
			}
			student.SetScore(hits);

			Console.WriteLine("\t" + student.GetName() + "'s score is " + hits.ToString());
		}
	}

	public class AdaptadorAlumno : IStudent
	{
		IAlumno alu;

		public IAlumno GetAlumno()
		{
			return alu;
		}
		public AdaptadorAlumno(IAlumno alumno)
		{
			this.alu = alumno;
		}

		public string GetName()
		{
			return alu.GetNombre();
		}
		public int YourAnswerIs(int question)
		{
			return alu.ResponderPregunta(question);
		}
		public void SetScore(int score)
		{
			alu.SetCalificacion(score);
		}
		public string ShowResult()
		{
			return alu.MostrarCalificacion();
		}
		public bool LessThan(IStudent student)
		{
			IComparable temp = (IComparable)((AdaptadorAlumno)student).GetAlumno();
			return alu.SosMenor(temp);
		}

		public bool GreaterThan(IStudent student)
		{
			IComparable temp = (IComparable)((AdaptadorAlumno)student).GetAlumno();
			return alu.SosMayor(temp);
		}

		public bool Equals(IStudent student)
		{
			IComparable temp = (IComparable)((AdaptadorAlumno)student).GetAlumno();
			return alu.SosIgual(temp);
		}
	}


	public interface IComparacionAlumnos
	{
		bool SosIgual(IAlumno a, IAlumno b);
		bool SosMenor(IAlumno a, IAlumno b);
		bool SosMayor(IAlumno a, IAlumno b);
	}
	public class ComparadorPorNombre : IComparacionAlumnos
	{
		public bool SosIgual(IAlumno a, IAlumno b)
		{
			return (a.GetNombre()).Equals(b.GetNombre());
		}
		public bool SosMenor(IAlumno a, IAlumno b)
		{
			return a.GetNombre().Length < b.GetNombre().Length;
		}
		public bool SosMayor(IAlumno a, IAlumno b)
		{
			return a.GetNombre().Length > b.GetNombre().Length;
		}
	}

	public class ComparadorPorDni : IComparacionAlumnos
	{
		public bool SosIgual(IAlumno a, IAlumno b)
		{
			return a.GetDNI() == b.GetDNI();
		}
		public bool SosMenor(IAlumno a, IAlumno b)
		{
			return a.GetDNI() < b.GetDNI();
		}
		public bool SosMayor(IAlumno a, IAlumno b)
		{
			return a.GetDNI() > b.GetDNI();
		}
	}

	public class ComparadorPorLegajo : IComparacionAlumnos
	{
		public bool SosIgual(IAlumno a, IAlumno b)
		{
			return a.GetLegajo() == b.GetLegajo();
		}
		public bool SosMenor(IAlumno a, IAlumno b)
		{
			return a.GetLegajo() < b.GetLegajo();
		}
		public bool SosMayor(IAlumno a, IAlumno b)
		{
			return a.GetLegajo() > b.GetLegajo();
		}
	}

	public class ComparadorPorPromedio : IComparacionAlumnos
	{
		public bool SosIgual(IAlumno a, IAlumno b)
		{
			return a.GetPromedio() == b.GetPromedio();
		}
		public bool SosMenor(IAlumno a, IAlumno b)
		{
			return a.GetPromedio() < b.GetPromedio();
		}
		public bool SosMayor(IAlumno a, IAlumno b)
		{
			return a.GetPromedio() > b.GetPromedio();
		}
	}

	public class Conjunto : IColeccionable, ITerable, IOrdenable
	{
		private List<IComparable> elementos;
		private IOrdenEnAula1 OrdenInicio, OrdenFin;
		private IOrdenEnAula2 OrdenLlega;

		public void SetOrdenInicio(IOrdenEnAula1 iniciador)
		{
			this.OrdenInicio = iniciador;
		}
		public void SetOrdenLlegaAlumno(IOrdenEnAula2 llega)
		{
			this.OrdenLlega = llega;

		}
		public void SetOrdenAulaLlena(IOrdenEnAula1 finalizador)
		{
			this.OrdenFin = finalizador;
		}

		public Conjunto() { elementos = new List<IComparable>(); }

		public List<IComparable> Getlist() { return elementos; }
		public void Agregar(IComparable c)
		{
			if (Pertenece(c))
				elementos.Add(c);
			if (Cuantos() == 1)
			{
				Console.WriteLine("orden inicio...");
				OrdenInicio.Ejecutar();
			}

			Console.WriteLine("orden llega alumno...");
			OrdenLlega.Ejecutar(c);

			if (Cuantos() == 40)
			{
				Console.WriteLine("orden aula llena...");
				OrdenFin.Ejecutar();
			}
			else
				Console.WriteLine("Comparable ya existe");
		}

		public bool Pertenece(IComparable c)
		{
			if (elementos.Count == 0) { return false; }
			foreach (IComparable b in elementos)
			{
				IComparable comp = b;
				if (!(comp.SosIgual(c)))
				{
					return true;
				}
			}
			return false;
		}
		public int Cuantos()
		{
			return elementos.Count;
		}

		public IComparable Minimo()
		{
			IComparable menor = elementos[0];
			foreach (IComparable b in elementos)
			{
				if (b.SosMenor(menor))
				{
					menor = b;
				}
			}
			return menor;
		}
		public IComparable Maximo()
		{
			IComparable menor = elementos[0];
			foreach (IComparable b in elementos)
			{
				if (b.SosMenor(menor))
				{
					menor = b;
				}
			}
			return menor;
		}
		public bool Contiene(IComparable c)
		{
			foreach (IComparable b in elementos)
			{
				IComparable comp = (IComparable)b;
				if (comp.SosIgual(c))
				{
					return true;
				}
			}
			return false;
		}

		public ITerador CrearIterador()
		{
			return new IteradorConjunto(this);
		}
	}

	public class Diccionario : IColeccionable, ITerable, IOrdenable
	{
		private List<ClaveValor> elementos;
		public Diccionario() { elementos = new List<ClaveValor>(); }

		private IOrdenEnAula1 OrdenInicio, OrdenFin;
		private IOrdenEnAula2 OrdenLlega;

		public void SetOrdenInicio(IOrdenEnAula1 iniciador)
		{
			this.OrdenInicio = iniciador;
		}
		public void SetOrdenLlegaAlumno(IOrdenEnAula2 llega)
		{
			this.OrdenLlega = llega;

		}
		public void SetOrdenAulaLlena(IOrdenEnAula1 finalizador)
		{
			this.OrdenFin = finalizador;
		}

		public List<ClaveValor> Getlist() { return elementos; }
		public void Agregar(IComparable clave, IComparable valor)
		{
			IComparable a = ValorDe(clave);
			if (a != null)
			{
				foreach (ClaveValor c in elementos)
				{
					if (c.Getclave().SosIgual(clave))
					{
						c.Setvalor(valor); return;
					}
				}
			}
			else
			{
				ClaveValor nuevo = new ClaveValor(clave, valor);
				elementos.Add(nuevo);
				if (Cuantos() == 1)
				{
					Console.WriteLine("orden inicio...");
					OrdenInicio.Ejecutar();
				}

				Console.WriteLine("orden llega alumno...");
				OrdenLlega.Ejecutar(nuevo);

				if (Cuantos() == 40)
				{
					Console.WriteLine("orden aula llena...");
					OrdenFin.Ejecutar();
				}
			}
		}

		public IComparable ValorDe(IComparable clave)
		{
			IComparable e = null;
			foreach (ClaveValor c in elementos)
			{
				IComparable comp = c.Getclave();
				if (comp.SosIgual(clave))
					return c.Getvalor();
			}
			return e;
		}

		public int Cuantos()
		{
			return elementos.Count;
		}

		public IComparable Minimo()
		{
			ClaveValor menor = elementos[0];
			IComparable min = menor.Getvalor();
			foreach (ClaveValor b in elementos)
			{
				IComparable comp = b.Getvalor();
				if (comp.SosMenor(min))
				{
					min = comp;
				}
			}
			return min;
		}
		public IComparable Maximo()
		{
			ClaveValor maximo = elementos[0];
			IComparable max = maximo.Getvalor();
			foreach (ClaveValor b in elementos)
			{
				IComparable comp = b.Getvalor();
				if (comp.SosMayor(max))
				{
					max = comp;
				}
			}
			return max;
		}
		public void Agregar(IComparable c)
		{
			Random num = new Random();
			IComparable numero = new Numero(num.Next(1, 100));
			ClaveValor a = new ClaveValor(numero, c);
			elementos.Add(a);
		}
		public bool Contiene(IComparable c)
		{
			foreach (IComparable b in elementos)
			{
				IComparable comp = (IComparable)b;
				if (comp.SosIgual(c))
				{
					return true;
				}
			}
			return false;
		}

		public ITerador CrearIterador()
		{
			return new IteradorDiccionario(this);
		}
	}

	public class ClaveValor : ITerado, IComparable
	{
		private IComparable clave;
		private IComparable valor;

		public ClaveValor(IComparable clave, IComparable valor)
		{
			this.clave = clave;
			this.valor = valor;
		}

		public IComparable Getclave()
		{
			return this.clave;
		}
		public IComparable Getvalor()
		{
			return this.valor;
		}

		public void Setvalor(IComparable valor)
		{
			this.valor = valor;
		}

		public bool SosIgual(IComparable c)
		{
			ClaveValor a = (ClaveValor)c;
			return this.clave == a.Getclave();
		}

		public bool SosMayor(IComparable c)
		{
			ClaveValor a = (ClaveValor)c;
			return this.clave.SosMayor(a);
		}

		public bool SosMenor(IComparable c)
		{
			ClaveValor a = (ClaveValor)c;
			return this.clave.SosMenor(a);
		}
	}

	public class IteradorCola : ITerador
	{
		int indice;
		Cola cola;

		public IteradorCola(Cola cola)
		{
			this.cola = cola;
			Primero();
		}
		public ITerado Actual()
		{
			List<IComparable> lista = this.cola.getlist();
			return (ITerado)lista[indice];
		}

		public bool Fin()
		{
			return this.indice >= this.cola.getlist().Count;
		}

		public void Primero()
		{
			this.indice = 0;
		}

		public void Siguiente()
		{
			indice++;
		}
	}

	public class IteradorPila : ITerador
	{
		Pila pila;
		int indice;

		public IteradorPila(Pila pila)
		{
			this.pila = pila;
			Primero();
		}
		public ITerado Actual()
		{
			List<IComparable> lista = this.pila.Getlist();
			return (ITerado)lista[indice];
		}

		public bool Fin() { return this.indice == 0; }

		public void Primero() { this.indice = pila.Getlist().Count - 1; }

		public void Siguiente() { indice--; }
	}

	public class IteradorDiccionario : ITerador
	{
		int indice;
		Diccionario diccionario;
		List<ClaveValor> elementos;

		public IteradorDiccionario(Diccionario diccionario)
		{
			elementos = diccionario.Getlist();
			Primero();
		}
		public ITerado Actual()
		{
			return (ITerado)elementos[indice].Getvalor();
		}

		public bool Fin() { return this.indice >= this.elementos.Count; }

		public void Primero() { this.indice = 0; }

		public void Siguiente() { indice++; }
	}

	public class IteradorConjunto : ITerador
	{
		int indice;
		Conjunto conjunto;

		public IteradorConjunto(Conjunto conjunto)
		{
			this.conjunto = conjunto;
			Primero();
		}
		public ITerado Actual()
		{
			List<IComparable> lista = this.conjunto.Getlist();
			return (ITerado)lista[indice];
		}

		public bool Fin() { return this.indice >= this.conjunto.Getlist().Count; }

		public void Primero() { this.indice = 0; }

		public void Siguiente() { indice++; }
	}

	public interface ITerador
	{
		void Primero();
		void Siguiente();
		bool Fin();
		ITerado Actual();
	}

	public interface ITerable
	{
		ITerador CrearIterador();
	}

	public interface ITerado
	{

	}

	public class GeneradorDeDatosAleatorios
	{
		static Random rand = new Random();

		public static int NumeroAleatorio(int max)
		{
			return rand.Next(max + 1);
		}

		public static string StringAleatorio(int cant)
		{
			string letras = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
			char[] chars = new char[cant];
			for (int i = 0; i < cant; i++)
			{
				int index = rand.Next(letras.Length);
				chars[i] = letras[index];
			}
			return new string(chars);
		}
	}

	public class LectorDeDatos
	{
		public static int NumeroPorTeclado()
		{
			return int.Parse(Console.ReadLine());
		}

		public static string StringPorTeclado()
		{
			return Console.ReadLine();
		}
	}

	public abstract class FabricaDeComparables
	{
		public static IComparable CrearAleatorio(int opcion)
		{
			FabricaDeComparables fabrica = new FabricaDeNumeros();
			switch (opcion)
			{
				case 1:
					fabrica = new FabricaDeNumeros();
					break;
				case 2:
					fabrica = new FabricaDeAlumnos();
					break;
				case 3:
					fabrica = new FabricaDeProfesores();
					break;
				case 4:
					fabrica = new FabricaDeAlumnoMuyEstudiosos();
					break;
				case 5:
					fabrica = new FabricaDeAlumnosProxy();
					break;
				case 6:
					fabrica = new FabricaDeAlumnosMuyEstudiosoProxy();
					break;
			}
			return fabrica.CrearAleatorio();
		}

		public static IComparable CrearPorTeclado(int opcion)
		{
			FabricaDeComparables fabrica = new FabricaDeNumeros();
			switch (opcion)
			{
				case 1:
					fabrica = new FabricaDeNumeros();
					break;
				case 2:
					fabrica = new FabricaDeAlumnos();
					break;
				case 3:
					fabrica = new FabricaDeProfesores();
					break;
			}
			return fabrica.CrearPorTeclado();
		}
		public abstract IComparable CrearAleatorio();
		public abstract IComparable CrearPorTeclado();
	}

	public class FabricaDeAlumnos : FabricaDeComparables
	{
		public override IComparable CrearAleatorio()
		{
			string nombre = GeneradorDeDatosAleatorios.StringAleatorio(10);
			int dni = GeneradorDeDatosAleatorios.NumeroAleatorio(99999999);
			int legajo = GeneradorDeDatosAleatorios.NumeroAleatorio(9999);
			int promedio = GeneradorDeDatosAleatorios.NumeroAleatorio(10);
			double calificacion = GeneradorDeDatosAleatorios.NumeroAleatorio(10);
			IComparacionAlumnos estrategia = null;

			// Genera un numero aleatorio entre 1 y 4 para seleccionar una estrategia
			Random random = new Random();
			int numeroAleatorio = random.Next(1, 5);

			switch (numeroAleatorio)
			{
				case 1:
					estrategia = new ComparadorPorNombre();
					break;
				case 2:
					estrategia = new ComparadorPorDni();
					break;
				case 3:
					estrategia = new ComparadorPorLegajo();
					break;
				case 4:
					estrategia = new ComparadorPorPromedio();
					break;
			}

			Alumno alumno = new Alumno(nombre, dni, legajo, promedio, estrategia, calificacion);
			return alumno;
		}

		public override IComparable CrearPorTeclado()
		{
			Console.Write("Ingrese nombre: ");
			string nombre = LectorDeDatos.StringPorTeclado();
			Console.Write("Ingrese DNI: ");
			int dni = LectorDeDatos.NumeroPorTeclado();
			Console.Write("Ingrese legajo: ");
			int legajo = LectorDeDatos.NumeroPorTeclado();
			Console.Write("Ingrese promedio: ");
			int promedio = LectorDeDatos.NumeroPorTeclado();
			Console.Write("Ingrese calificacion: ");
			double calificacion = LectorDeDatos.NumeroPorTeclado();

			IComparacionAlumnos estrategia = SeleccionarEstrategiaPorTeclado();

			Alumno alumno = new Alumno(nombre, dni, legajo, promedio, estrategia, calificacion);

			return alumno;
		}

		public IComparacionAlumnos SeleccionarEstrategiaPorTeclado()
		{
			Console.WriteLine("Selecciona una estrategia (1 para Comparar por nombre, 2 para DNI, 3 para legajo, 4 para promedio): ");
			int opcionEstrategia = int.Parse(Console.ReadLine());

			switch (opcionEstrategia)
			{
				case 1:
					return new ComparadorPorNombre();
				case 2:
					return new ComparadorPorDni();
				case 3:
					return new ComparadorPorLegajo();
				case 4:
					return new ComparadorPorPromedio();
				default:
					Console.WriteLine("Opcion no valida. Seleccionando ComparadorPorNombre por defecto.");
					return new ComparadorPorNombre();
			}
		}
	}

	public class FabricaDeAlumnoMuyEstudiosos : FabricaDeComparables
	{
		private static int cont = 0;
		public override IComparable CrearAleatorio()
		{

			string nombre = GeneradorDeDatosAleatorios.StringAleatorio(10);
			int dni = GeneradorDeDatosAleatorios.NumeroAleatorio(99999999);
			int legajo = cont++;
			int promedio = 10;
			double calificacion = GeneradorDeDatosAleatorios.NumeroAleatorio(10);
			IComparacionAlumnos estrategia = new ComparadorPorNombre(); // Reemplaza con la estrategia que desees
			return new AlumnoMuyEstudioso(nombre, dni, legajo, promedio, estrategia, calificacion);
		}

		public override IComparable CrearPorTeclado()
		{
			Console.Write("Ingrese nombre: ");
			string nombre = LectorDeDatos.StringPorTeclado();
			Console.Write("Ingrese dni: ");
			int dni = LectorDeDatos.NumeroPorTeclado();
			Console.Write("Ingrese legajo: ");
			int legajo = LectorDeDatos.NumeroPorTeclado();
			Console.Write("Ingrese promedio: ");
			int promedio = LectorDeDatos.NumeroPorTeclado();
			Console.Write("Ingrese calificacion: ");
			double calificacion = LectorDeDatos.NumeroPorTeclado();
			IComparacionAlumnos estrategia = new ComparadorPorNombre(); // Reemplaza con la estrategia que desees

			return new AlumnoMuyEstudioso(nombre, dni, legajo, promedio, estrategia, calificacion);
		}
	}

	public class FabricaDeNumeros : FabricaDeComparables
	{
		public override IComparable CrearAleatorio()
		{
			return new Numero(GeneradorDeDatosAleatorios.NumeroAleatorio(5));
		}

		public override IComparable CrearPorTeclado()
		{
			return new Numero(LectorDeDatos.NumeroPorTeclado());
		}
	}

	public class FabricaDeProfesores : FabricaDeComparables
	{
		public override IComparable CrearAleatorio()
		{
			string nombre = GeneradorDeDatosAleatorios.StringAleatorio(10);
			int dni = GeneradorDeDatosAleatorios.NumeroAleatorio(99999999);
			int antiguedad = GeneradorDeDatosAleatorios.NumeroAleatorio(40);

			Profesor profesor = new Profesor(nombre, dni, antiguedad);
			return profesor;
		}

		public override IComparable CrearPorTeclado()
		{
			Console.Write("Ingrese nombre del profesor: ");
			string nombre = LectorDeDatos.StringPorTeclado();
			Console.Write("Ingrese DNI del profesor: ");
			int dni = LectorDeDatos.NumeroPorTeclado();
			Console.Write("Ingrese antiguedad del profesor: ");
			int antiguedad = LectorDeDatos.NumeroPorTeclado();
			Profesor profesor = new Profesor(nombre, dni, antiguedad);
			return profesor;
		}
	}

	public class FabricaDeAlumnosProxy : FabricaDeComparables
	{
		private static int cont = 0;
		public override IComparable CrearAleatorio()
		{

			string nombre = GeneradorDeDatosAleatorios.StringAleatorio(10);
			int dni = GeneradorDeDatosAleatorios.NumeroAleatorio(99999999);
			int legajo = cont++;
			int promedio = GeneradorDeDatosAleatorios.NumeroAleatorio(10);
			double calificacion = GeneradorDeDatosAleatorios.NumeroAleatorio(10);
			return new AlumnoProxy(nombre, dni, legajo, promedio, calificacion);
		}

		public override IComparable CrearPorTeclado()
		{
			Console.Write("ingrese nombre: ");
			string nombre = LectorDeDatos.StringPorTeclado();
			Console.Write("ingrese dni: ");
			int dni = LectorDeDatos.NumeroPorTeclado();
			Console.Write("ingrese legajo: ");
			int legajo = LectorDeDatos.NumeroPorTeclado();
			Console.Write("ingrese promedio: ");
			int promedio = LectorDeDatos.NumeroPorTeclado();
			Console.Write("ingrese la calificacion: ");
			double calificacion = LectorDeDatos.NumeroPorTeclado();
			return new AlumnoProxy(nombre, dni, legajo, promedio, calificacion);
		}
	}
	public class FabricaDeAlumnosMuyEstudiosoProxy : FabricaDeComparables
	{
		private static int cont = 0;
		public override IComparable CrearAleatorio()
		{

			string nombre = GeneradorDeDatosAleatorios.StringAleatorio(10);
			int dni = GeneradorDeDatosAleatorios.NumeroAleatorio(99999999);
			int legajo = cont++;
			int promedio = 10;
			double calificacion = GeneradorDeDatosAleatorios.NumeroAleatorio(10);
			return new AlumnoMuyEstudiosoProxy(nombre, dni, legajo, promedio, calificacion);
		}

		public override IComparable CrearPorTeclado()
		{
			Console.Write("ingrese nombre: ");
			string nombre = LectorDeDatos.StringPorTeclado();
			Console.Write("ingrese dni: ");
			int dni = LectorDeDatos.NumeroPorTeclado();
			Console.Write("ingrese legajo: ");
			int legajo = LectorDeDatos.NumeroPorTeclado();
			Console.Write("ingrese promedio: ");
			int promedio = LectorDeDatos.NumeroPorTeclado();
			Console.Write("ingrese la calificacion: ");
			double calificacion = LectorDeDatos.NumeroPorTeclado();
			IComparacionAlumnos estrategia = SeleccionarEstrategiaPorTeclado();
			return new Alumno(nombre, dni, legajo, promedio, estrategia, calificacion);
		}
		public IComparacionAlumnos SeleccionarEstrategiaPorTeclado()
		{
			Console.WriteLine("Selecciona una estrategia (1 para Comparar por nombre, 2 para DNI, 3 para legajo, 4 para promedio): ");
			int opcionEstrategia = int.Parse(Console.ReadLine());

			switch (opcionEstrategia)
			{
				case 1:
					return new ComparadorPorNombre();
				case 2:
					return new ComparadorPorDni();
				case 3:
					return new ComparadorPorLegajo();
				case 4:
					return new ComparadorPorPromedio();
				default:
					Console.WriteLine("Opcion no valida. Seleccionando ComparadorPorNombre por defecto.");
					return new ComparadorPorNombre();
			}
		}
	}

	public class Profesor : Persona, IComparable, IObservado
	{
		private int antiguedad;
		private List<IObservador> observadores = new List<IObservador>();
		private bool hablando;

		public Profesor(string nombre, int dni, int antiguedad) : base(nombre, dni)
		{
			this.antiguedad = antiguedad;
			hablando = false;
		}

		public void AgregarObservador(IObservador observador)
		{
			observadores.Add(observador);
		}

		public void QuitarObservador(IObservador observador)
		{
			observadores.Remove(observador);
		}

		public void Notificar()
		{
			foreach (IObservador observador in observadores)
			{
				observador.Actualizar(this);
			}
		}

		public void HablarALaClase()
		{
			Console.WriteLine("Profesor " + GetNombre() + " está hablando de algún tema");
			hablando = true;
			Notificar(); // Invoca el método "Notificar" que a su vez notifica a los observadores
		}

		public void EscribirEnElPizarron()
		{
			Console.WriteLine("Profesor " + GetNombre() + " está escribiendo en el pizarrón");
			hablando = false;
			Notificar();
		}

		public int GetAntiguedad()
		{
			return antiguedad;
		}

		public bool SosIgual(IComparable otro)
		{
			Profesor p = (Profesor)otro;
			return this.antiguedad == p.GetAntiguedad();
		}

		public bool SosMenor(IComparable otro)
		{
			Profesor p = (Profesor)otro;
			return this.antiguedad < p.GetAntiguedad();
		}

		public bool SosMayor(IComparable otro)
		{
			Profesor p = (Profesor)otro;
			return this.antiguedad > p.GetAntiguedad();
		}
		public bool EstaHablando()
		{
			return hablando;
		}
	}

	public interface IObservado
	{
		void AgregarObservador(IObservador o);
		void QuitarObservador(IObservador o);
		void Notificar();
	}
	public interface IObservador
	{
		void Actualizar(IObservado o);
	}

	public abstract class DecoradoresAlumnos : IAlumno, IComparable
	{
		private IAlumno alu;

		public DecoradoresAlumnos(IAlumno a)
		{
			this.alu = a;
		}

		public double GetCalificacion()
		{
			return alu.GetCalificacion();
		}
		public int GetLegajo()
		{
			return alu.GetLegajo();
		}
		public string GetNombre()
		{
			return alu.GetNombre();
		}
		public double GetPromedio()
		{
			return alu.GetPromedio();
		}
		public int GetDNI()
		{
			return alu.GetDNI();
		}
		public virtual string MostrarCalificacion()
		{
			return alu.MostrarCalificacion();
		}
		public int ResponderPregunta(int b)
		{
			return alu.ResponderPregunta(b);
		}
		public void SetCalificacion(double a)
		{
			alu.SetCalificacion(a);
		}
		public bool SosIgual(IComparable c)
		{
			return alu.SosIgual(c);
		}
		public bool SosMayor(IComparable c)
		{
			return alu.SosMayor(c);
		}
		public bool SosMenor(IComparable c)
		{
			return alu.SosMenor(c);
		}
	}

	public class DecoradorLegajo : DecoradoresAlumnos
	{
		public DecoradorLegajo(IAlumno a) : base(a)
		{

		}

		public override string MostrarCalificacion()
		{
			string cadena = base.MostrarCalificacion();
			return cadena + GetLegajo();
		}
	}

	public class DecoradorNota : DecoradoresAlumnos
	{
		public DecoradorNota(IAlumno a) : base(a)
		{

		}

		public override string MostrarCalificacion()
		{
			string notaString = "";
			switch ((int)GetCalificacion())
			{
				case 0:
					notaString = "(CERO)";
					break;
				case 1:
					notaString = "(UNO)";
					break;
				case 2:
					notaString = "(DOS)";
					break;
				case 3:
					notaString = "(TRES)";
					break;
				case 4:
					notaString = "(CUATRO)";
					break;
				case 5:
					notaString = "(CINCO)";
					break;
				case 6:
					notaString = "(SEIS)";
					break;
				case 7:
					notaString = "(SIETE)";
					break;
				case 8:
					notaString = "(OCHO)";
					break;
				case 9:
					notaString = "(NUEVE)";
					break;
				case 10:
					notaString = "(DIEZ)";
					break;
			}
			return base.MostrarCalificacion() + notaString;
		}
	}

	public class DecoradorResultado : DecoradoresAlumnos
	{
		public DecoradorResultado(IAlumno a) : base(a)
		{

		}

		public override string MostrarCalificacion()
		{
			string notaString = "";
			if (GetCalificacion() < 4)
				notaString = "(DESAPROBADO)";
			else if (GetCalificacion() < 7)
				notaString = "(APROBADO)";
			else
				notaString = "(PROMOCION)";

			return base.MostrarCalificacion() + notaString;
		}
	}

	public class DecoradorAstericos : DecoradoresAlumnos
	{
		public DecoradorAstericos(IAlumno a) : base(a)
		{
		}

		public override string MostrarCalificacion()
		{
			string resultado = base.MostrarCalificacion();
			string recuadro = "***********************************************************************\n";
			recuadro += "* " + resultado + " *\n";
			recuadro += "***********************************************************************\n";
			return recuadro;
		}
	}

	public interface IAlumno
	{
		double GetCalificacion();
		int GetLegajo();
		string GetNombre();
		double GetPromedio();
		int GetDNI();
		int ResponderPregunta(int pregunta);
		void SetCalificacion(double calificacion);
		bool SosIgual(IComparable otro);
		bool SosMayor(IComparable otro);
		bool SosMenor(IComparable otro);
		string MostrarCalificacion();
	}

	public class AlumnoProxy : IAlumno, IComparable
	{
		private IAlumno alu = null;
		private string nombre;
		private int dni;
		private int legajo;
		private double promedio;
		private double calificacion;
		IComparacionAlumnos estra;

		public AlumnoProxy(string nombre, int dni, int legajo, double promedio, double calificacion)
		{
			this.nombre = nombre;
			this.dni = dni;
			this.legajo = legajo;
			this.promedio = promedio;
			this.calificacion = calificacion;
		}

		public string GetNombre() { return nombre; }
		public int GetDNI() { return dni; }
		public int GetLegajo() { return legajo; }
		public double GetPromedio() { return promedio; }
		public double GetCalificacion() { return calificacion; }
		public void SetCalificacion(double n) { this.calificacion = n; }
		public int ResponderPregunta(int pregunta)
		{
			return GetAlumnoReal().ResponderPregunta(pregunta);
		}
		public string MostrarCalificacion()
		{
			return string.Format("Nombre: {0}, legajo: {1}, Calificacion: {2} ", this.GetNombre(), this.legajo, calificacion);
		}
		public string MostrarResultado()
		{
			return string.Format("Nombre: {0}, legajo: {1}, Calificacion: {2} ", this.GetNombre(), this.legajo, calificacion);
		}
		public bool SosIgual(IComparable c)
		{
			return GetAlumnoReal().SosIgual(c);
		}
		public bool SosMayor(IComparable c)
		{
			return GetAlumnoReal().SosMayor(c);
		}
		public bool SosMenor(IComparable c)
		{
			return GetAlumnoReal().SosMenor(c);
		}

		private IAlumno GetAlumnoReal()
		{
			if (alu == null)
			{
				Console.WriteLine("Creando alumno real...");
				alu = new Alumno(nombre, dni, legajo, promedio, estra, calificacion);
			}
			return alu;
		}
	}

	public class AlumnoMuyEstudiosoProxy : IAlumno, IComparable
	{
		private IAlumno aluestudioso = null;
		private string nombre;
		private int dni;
		private int legajo;
		private double promedio;
		private double calificacion;
		private IComparacionAlumnos comp;

		public AlumnoMuyEstudiosoProxy(string nombre, int dni, int legajo, double promedio, double calificacion)
		{
			this.nombre = nombre;
			this.dni = dni;
			this.legajo = legajo;
			this.promedio = promedio;
			this.calificacion = calificacion;
		}

		public string GetNombre() { return nombre; }
		public int GetDNI() { return dni; }
		public int GetLegajo() { return legajo; }
		public double GetPromedio() { return promedio; }
		public double GetCalificacion() { return calificacion; }
		public void SetCalificacion(double n) { this.calificacion = n; }
		public int ResponderPregunta(int pregunta)
		{
			return GetAlumnoReal().ResponderPregunta(pregunta);
		}
		public string MostrarCalificacion()
		{
			return string.Format("Nombre: {0}, legajo: {1}, Calificacion: {2} ", this.GetNombre(), this.legajo, calificacion);
		}
		public string MostrarResultado()
		{
			return string.Format("Nombre: {0}, legajo: {1}, Calificacion: {2} ", this.GetNombre(), this.legajo, calificacion);
		}
		public bool SosIgual(IComparable c)
		{
			return GetAlumnoReal().SosIgual(c);
		}
		public bool SosMayor(IComparable c)
		{
			return GetAlumnoReal().SosMayor(c);
		}
		public bool SosMenor(IComparable c)
		{
			return GetAlumnoReal().SosMenor(c);
		}

		private IAlumno GetAlumnoReal()
		{
			if (aluestudioso == null)
			{
				Console.WriteLine("Creando alumno estudioso...");
				aluestudioso = new AlumnoMuyEstudioso(nombre, dni, legajo, promedio, comp, calificacion);
			}
			return aluestudioso;
		}
	}

	public class Aula
	{
		private Teacher teach;

		public void Comenzar()
		{
			Console.WriteLine("Comenzando la clase...");
			teach = new Teacher();
		}

		public void NuevoAlumno(IAlumno alumno)
		{
			teach.GoToClass(new AdaptadorAlumno(alumno));
		}

		public void ClaseLista()
		{
			teach.TeachingAClass();
		}
	}

	public interface IOrdenEnAula1
	{
		void Ejecutar();
	}

	public interface IOrdenEnAula2
	{
		void Ejecutar(IComparable comparable);
	}
	public interface IOrdenable
	{
		void SetOrdenInicio(IOrdenEnAula1 orden);
		void SetOrdenLlegaAlumno(IOrdenEnAula2 orden);
		void SetOrdenAulaLlena(IOrdenEnAula1 orden);
	}

	public class OrdenInicio : IOrdenEnAula1
	{
		private Aula aula;

		public OrdenInicio(Aula a)
		{
			this.aula = a;
		}

		public void Ejecutar()
		{
			aula.Comenzar();
		}
	}
	public class OrdenAulaLlena : IOrdenEnAula1
	{
		private Aula aula;

		public OrdenAulaLlena(Aula aula)
		{
			this.aula = aula;
		}

		public void Ejecutar()
		{
			aula.ClaseLista();
		}
	}
	public class OrdenLlegaAlumno : IOrdenEnAula2
	{
		private Aula aula;

		public OrdenLlegaAlumno(Aula aula)
		{
			this.aula = aula;
		}

		public void Ejecutar(IComparable c)
		{
			aula.NuevoAlumno((IAlumno)c);
		}
	}

	wwwww
}