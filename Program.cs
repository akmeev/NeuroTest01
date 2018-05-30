using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroTest01
{

	class Neuron
	{
		double[] A;
		double[] W;
		double Bias;

		public Neuron()
		{
			Bias = 7;
			A = new double[15];
			W = new double[15];
			for (int i = 0; i < 15; i++)
			{
				A[i] = 0;
				W[i] = 0;
			}
		}

		public bool Do(string st)
		{
			for (int i = 0; i < 15; i++) A[i] = Convert.ToDouble(st.Substring(i,1));
			double result=0;
			for (int i = 0; i < 15; i++)
			{
				result = result + A[i] * W[i];
			}
			//Console.WriteLine("result=" + result);
			return (result >= Bias);
		}

		public void ChW(bool IncFlag)
		{
			//Console.WriteLine("ChW " + IncFlag.ToString());
			double d = -1;
			if (IncFlag) d = 1;
			for (int i=1;i<15;i++)
			{
				if (A[i] == 1) W[i] = W[i] + d; // W[i] * 0.1 * d;
				//Console.WriteLine("A[i]=" + A[i] + ", W[i]=" + W[i] + ", d=" + d);
			}
		}

		public void Print()
		{
			Console.WriteLine("Start");
			for (int i=0;i<15;i++)
			{
				Console.WriteLine("W(" + i + ") = " + W[i]);
			}
			Console.WriteLine("End");
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			string[] GoodData = {
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
			};

			string[] WrongData = {
			"111100111000111",
			"111100010001111",
			"111100011001111",
			"110100111001111",
			"110100111001011",
			"111100101001111"
			};

			Neuron N = new Neuron();
			N.Print();
			Random random = new Random();
			for (int i=0;i<50000;i++)
			{
				// learinig circle for good data
				Console.Write("%d%%    \r"+i);
				int j = random.Next(0, 9);
				//Console.WriteLine("j=" + j);
				bool r = N.Do(GoodData[j]);
				if (!r && j==5) N.ChW(true); // Результат (-) Вход (+) Up.
				if (r && j!=5) N.ChW(false); // Результат (+) Вход (-) Down.
			}
			N.Print();

			// Checking ...
			for (int i=0;i<10;i++)
			{
				bool r = N.Do(GoodData[i]);
				Console.WriteLine("i="+i+" r="+r.ToString());
			}

			for (int i = 0; i < 6; i++)
			{
				bool r = N.Do(WrongData[i]);
				Console.WriteLine("i=" + i + " r=" + r.ToString());
			}
			Console.ReadLine();
		}
	}
}
