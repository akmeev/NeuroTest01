using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTest01
{
	class Neuron
	{
		Guid id;
		ulong LearnCycleCount;
		ulong UseCycleCount;
		double[] A;
		double[] W;
		double Bias;

		public Neuron()
		{
			id = Guid.NewGuid();
			LearnCycleCount = 0;
			UseCycleCount = 0;
			Bias = 7;
			A = new double[15];
			W = new double[15];
			for (int i = 0; i < 15; i++) { A[i] = 0; W[i] = 0; }
		}

		public bool Do(string st)
		{
			for (int i = 0; i < 15; i++) A[i] = Convert.ToDouble(st.Substring(i,1));
			double result=0;
			for (int i = 0; i < 15; i++) result = result + A[i] * W[i];
			//Console.WriteLine("result=" + result);
			UseCycleCount++;
			return (result >= Bias);
		}

		public void ChW(bool IncFlag)
		{
			//Console.WriteLine("ChW " + IncFlag.ToString());
			double d = -1;
			if (IncFlag) d = 1;
			for (int i=1;i<15;i++)
			{
				if (A[i] == 1) W[i] = W[i] + d; //W[i] / 2 * d;
				//Console.WriteLine("A[i]=" + A[i] + ", W[i]=" + W[i] + ", d=" + d);
			}
			LearnCycleCount++;
		}

		public void Print()
		{
			Console.WriteLine("Start Neuron ID = " + id);
			for (int i=0;i<15;i++)
			{
				Console.Write("  W(" + i + ") = " + W[i]);
			}
			Console.WriteLine();
			Console.WriteLine("Learn\\Use Cycles Count = " + LearnCycleCount + " \\ " + UseCycleCount);

			Console.WriteLine("End Neuron ID = " + id);
		}
	}

	class Program
	{
		static string[] GoodData = {
			"111101101101111", // 0
			"001001001001001", // 1
			"111001111100111", // 2
			"111001111001111", // 3
			"101101111001001", // 4
			"111100111001111", // 5
			"111100111101111", // 6
			"111001001001001", // 7
			"111101111101111", // 8
			"111101111001111"  // 9
		}; // Правильные цифры формата 3*5.

		static string[] WrongData = {
			"111100111000111",
			"111100010001111",
			"111100011001111",
			"110100111001111",
			"110100111001011",
			"111100101001111"
		}; // Несколько вариантов немного кривоватых цифр "5".

		static void TrainNeuronForDigit(Neuron N, int Cycles, int Value)
		{
			Random random = new Random();
			for (int i = 0; i < Cycles; i++)
			{
				Console.Write("\r" + i); // /r это backspace - чтобы счетчик отрабатывал в одной строке.
				int j = random.Next(0, 10);
				//Console.WriteLine("j=" + j);
				bool r = N.Do(GoodData[j]);
				if (!r && j == Value) N.ChW(true); // Результат (-) Вход (+) Up.
				if (r && j != Value) N.ChW(false); // Результат (+) Вход (-) Down.
			}
			Console.WriteLine();
		}

		static void CheckNeuronForDigit(Neuron N)
		{
			for (int i = 0; i < 10; i++)
			{
				bool r = N.Do(GoodData[i]);
				Console.WriteLine("i=" + i + " -> r=" + r.ToString());
			}

			for (int i = 0; i < 6; i++)
			{
				bool r = N.Do(WrongData[i]);
				Console.WriteLine("i=" + i + " -> r=" + r.ToString());
			}
		}

		static void Main(string[] args)
		{
			for (int i = 0; i < 10; i++) {
				Neuron N = new Neuron();
				N.Print();
				TrainNeuronForDigit(N, 100000, i);
				N.Print();
				CheckNeuronForDigit(N);
			}

			Console.ReadLine();
		}
	}
}
