using System;

namespace NeuroTest01
{
	class NeuroNet
	{
		public NeuroLayer[] L;
		public float[] Sensor;
		public float[] Response;

		public NeuroNet() // Конструктор
		{
			L = new NeuroLayer[0]; // В начале - слоев нет, сеть пустая.
			Sensor = new float[0]; // Пустой набор входящих данных,
			Response = new float[0]; // ... и пустой набор выходных данных.
		}

		public bool AddLayer()
		{
			int NewSize = L.Length + 1;
			Array.Resize<NeuroLayer>(ref L, NewSize);
			L[NewSize-1] = new NeuroLayer();
			L[NewSize-1].MyNeuroNet = this;
			return true;
		}

		static void Grow()
		{
			/* Процедура роста сети.
			Варианты роста:
			1)	Анализируется количество входов в нейроны. Если где-то входов больше 10, в слой добавляется ещё один
				нейрон, после чего половина "А" переводится на новый нейрон. Выход нового нейрона должен быть 
			*/
		}

		public void NetDo()
		{
			for (int i=0;i<L.Length;i++)
			{
				L[i].LayerDo();
			}
		}
	}

	class NeuroLayer
	{
		public Guid id;
		public Neuron2[] N; // Массив нейронов.
		public NeuroNet MyNeuroNet;

		public NeuroLayer() // Конструктор
		{
			id = new Guid();
			N = new Neuron2[0]; // Начальный массив - нулевой.
		}

		public bool AddNeuron()
		{
			int NewSize = N.Length + 1;
			Array.Resize<Neuron2>(ref N, NewSize);
			N[NewSize - 1] = new Neuron2();
			N[NewSize - 1].MyLayer = this;
			return true;
		}

		public void LayerDo()
		{
			for (int i=0;i<N.Length;i++) N[i].Do();
		}
	}

	class Neuron
	{
		// Array.Resize(ref A, 15); // Использовать для увеличения количества массива.

		Guid id; // Уникальный ID нейрона.
		ulong LearnCycleCount; // Количество запусков ChW().
		ulong UseCycleCount; // Количество запусков Do().
		ulong ErrorCount; // Количество ошибок при проверке.
		double[] A; // Входные сигналы. Записываются сюда перед тем, как выполнить Do().
		double[] W; // Веса.
		double Bias; // Порог срабатывания.

		public Neuron() // Конструктор.
		{
			id = Guid.NewGuid();
			LearnCycleCount = 0;
			UseCycleCount = 0;
			ErrorCount = 0;
			Bias = 0.5; // Порог срабатывания. Пересмотреть на 0..1.
			A = new double[15]; // Тут надо переписать под массивы динамического размера.
			W = new double[15];
			for (int i = 0; i < 15; i++) { A[i] = 0; W[i] = 0; } // Заполняем входы и веса нулями.
		}

		public bool Do(string st)
		{
			for (int i = 0; i < 15; i++) A[i] = Convert.ToDouble(st.Substring(i,1));
			double result=0;
			for (int i = 0; i < 15; i++) result = result + A[i] * W[i];
			//Console.WriteLine("result=" + result);
			UseCycleCount++;
			result = 1.0f / (1.0f + (float)Math.Exp(-result));
			return (result >= Bias);
		}

		public void ChW(bool IncFlag) // Усиление или ослабление весов в соответствии с наличием возбуждающего сигнала.
		{
			//Console.WriteLine("ChW " + IncFlag.ToString());
			double d = -1;
			if (IncFlag) d = 1;
			for (int i=1;i<15;i++)
			{
				if (A[i] == 1) W[i] = W[i] + d; // W[i] / 10 * d;
				//Console.WriteLine("A[i]=" + A[i] + ", W[i]=" + W[i] + ", d=" + d);
			}
			LearnCycleCount++;
		}

		public void Print() // Вывод состояния нейрона на консоль.
		{
			Console.WriteLine();
			Console.WriteLine("Start Neuron ID = " + id);
			for (int i=0;i<15;i++) Console.Write(" | w" + i + "=" + W[i]); Console.WriteLine();
			Console.WriteLine("Learn\\Use Cycles Count = " + LearnCycleCount + " \\ " + UseCycleCount);
			if (ErrorCount==0) Console.BackgroundColor = ConsoleColor.DarkGreen; else Console.BackgroundColor = ConsoleColor.Red;
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("Errors = " + ErrorCount);
			Console.ResetColor();
			Console.WriteLine("End Neuron ID = " + id);
			Console.WriteLine();
		}

		public void AddError()
		{
			ErrorCount++;
		}
	}

	class Neuron2
	{
		// Array.Resize(ref A, 15); // Использовать для увеличения количества массива.

		Guid id; // Уникальный ID нейрона.
		ulong LearnCycleCount; // Количество запусков ChW().
		ulong UseCycleCount; // Количество запусков Do().
		ulong ErrorCount; // Количество ошибок при проверке.
		double[] A; // Входные сигналы. Записываются сюда перед тем, как выполнить Do().
		double[] W; // Веса.
		double Bias; // Порог срабатывания.
		public NeuroLayer MyLayer;
		int NextL; // Номер следующего слоя.
		int NextN; // Номер следующего нейрона.
		int NextA; // Номер следующего A.

		public Neuron2() // Конструктор.
		{
			id = Guid.NewGuid();
			LearnCycleCount = 0;
			UseCycleCount = 0;
			ErrorCount = 0;
			Bias = 0.5; // Порог срабатывания.
			A = new double[0];
			W = new double[0];
			for (int i = 0; i < A.Length; i++) { A[i] = 0; W[i] = 0; } // Заполняем входы и веса нулями. Dummy.
		}

		public bool Load(int i, float Value)
		{
			// Загрузка значений из предыдущего слоя.
			A[i] = Value;
			return true;
		}

		public int AddA()
		{
			// Добавление A в нейрон.
			Array.Resize(ref A, A.Length + 1);
			return A.Length;
		}

		public bool Do()
		{
			double result = 0;
			for (int i = 0; i < A.Length; i++) result = result + A[i] * W[i];
			//Console.WriteLine("result=" + result);
			UseCycleCount++;
			result = 1.0f / (1.0f + (float)Math.Exp(-result));
			// Теперь надо записать выходные значения во входы для нейронов следующего слоя.
			if (NextL==0)
			{
				// Следующего слоя нет - выходная матрица.

			}
			else
			{
				// Следующий слой существует.
				
			}
			return (result >= Bias);
		}

		public void ChW(bool IncFlag) // Усиление или ослабление весов в соответствии с наличием возбуждающего сигнала.
		{
			//Console.WriteLine("ChW " + IncFlag.ToString());
			double d = -1;
			if (IncFlag) d = 1;
			for (int i = 1; i < 15; i++)
			{
				if (A[i] == 1) W[i] = W[i] + d; // W[i] / 10 * d;
				//Console.WriteLine("A[i]=" + A[i] + ", W[i]=" + W[i] + ", d=" + d);
			}
			LearnCycleCount++;
		}

		public void Print() // Вывод состояния нейрона на консоль.
		{
			Console.WriteLine();
			Console.WriteLine("Start Neuron ID = " + id);
			for (int i = 0; i < 15; i++) Console.Write(" | w" + i + "=" + W[i]); Console.WriteLine();
			Console.WriteLine("Learn\\Use Cycles Count = " + LearnCycleCount + " \\ " + UseCycleCount);
			Console.BackgroundColor = ConsoleColor.Red;
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("Errors = " + ErrorCount);
			// Console.ResetColor();
			Console.WriteLine("End Neuron ID = " + id);
			Console.WriteLine();
		}

		public void AddError()
		{
			ErrorCount++;
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
		}; // Правильные цифры формата 3*5. На них происходит обучение.

		static string[] WrongData = {
			"111100111000111",
			"111100010001111",
			"111100011001111",
			"110100111001111",
			"110100111001011",
			"111100101001111"
		}; // Несколько вариантов немного кривоватых цифр "5". Для проверки.

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

		static void CheckNeuronForDigit(Neuron N, int CheckedValue)
		{
            Console.WriteLine("Проверяем по цифрам:");
			for (int i = 0; i < 10; i++)
			{
				bool r = N.Do(GoodData[i]);
				Console.WriteLine("i=" + i + " -> r=" + r.ToString());
				if ((r && i!=CheckedValue) || (!r && i==CheckedValue)) N.AddError();
			}

            // Далее - хардкод: мы проверяем именно цифру 5 и ничего другого.
            Console.WriteLine("Проверяем кривые пятерки:");
            for (int i = 0; i < 6; i++)
			{
				bool r = N.Do(WrongData[i]);
				Console.WriteLine("i=" + i + " -> r=" + r.ToString());
				if ((r && CheckedValue!=5) || (!r && CheckedValue==5)) N.AddError();
			}
		}

		static void LearnForFive()
		{
			// Тестовая функция на обучение нейрона цифре 5 (в матрице 3x5).
			for (int i = 0; i < 10; i++)
			{
                Console.WriteLine("Обучаем нейрон цифре " + i);
				Neuron N = new Neuron(); // Новая цифра - новый нейрон. Старые значения нам не нужны.
				N.Print(); // Печатаем состояние нейрона до обучения.
				TrainNeuronForDigit(N, 300000, i);
				CheckNeuronForDigit(N, i); // Проверяем - хорошо ли обучена зверушка.
				N.Print(); // Печатаем состояние нейрона после обучения.
                Console.WriteLine("Press Enter for продолжить, однако.");
                Console.ReadLine();
            }
		}

		static void NewEmptyNet()
		{
			NeuroNet NN = new NeuroNet();
			NN.AddLayer();
			NN.L[0].AddNeuron();
			NN.L[0].N[0].AddA();
			// В этой точке готова сеть с одним слоем, одним нейроном и одним А.
			// Такая сеть может распознавать только ноль или единицу.
			// Более сложные распознавания требуют алгоритма усложнения сети.
		}

		static void Main(string[] args)
		{
			LearnForFive(); // Это тест на класс Neuron. основное потом - на Neuron2 (+Layer +Net).
			//NewEmptyNet();
			Console.ReadLine();
		}
	}
}
