# Инициализация весов сети
weights = []
for i in range(15):
weights.append(0)	
# Порог функции активации
32.	bias = 7
33.	
34.	# Является ли данное число 5
35.	def proceed(number):
36.	# Рассчитываем взвешенную сумму
37.	net = 0
38.	for i in range(15):
39.	net += int(number[i])*weights[i]
40.	
41.	# Превышен ли порог? (Да - сеть думает, что это 5. Нет - сеть думает, что это другая цифра)
42.	return net >= bias
43.	
44.	# Уменьшение значений весов, если сеть ошиблась и выдала 1
45.	def decrease(number):
46.	for i in range(15):
47.	# Возбужденный ли вход
48.	if int(number[i]) == 1:
49.	# Уменьшаем связанный с ним вес на единицу
50.	weights[i] -= 1
51.	
52.	# Увеличение значений весов, если сеть ошиблась и выдала 0
53.	def increase(number):
54.	for i in range(15):
55.	# Возбужденный ли вход
56.	if int(number[i]) == 1:
57.	# Увеличиваем связанный с ним вес на единицу
58.	weights[i] += 1
59.	
60.	# Тренировка сети
61.	for i in range(10000):
62.	# Генерируем случайное число от 0 до 9
63.	option = random.randint(0, 9)
64.	
65.	# Если получилось НЕ число 5
66.	if option != 5:
67.	# Если сеть выдала True/Да/1, то наказываем ее
68.	if proceed(nums[option]):
69.	decrease(nums[option])
70.	# Если получилось число 5
71.	else:
72.	# Если сеть выдала False/Нет/0, то показываем, что эта цифра - то, что нам нужно
73.	if not proceed(num5):
74.	increase(num5)
75.	
76.	# Вывод значений весов
77.	print(weights)
78.	
79.	# Прогон по обучающей выборке
80.	print("0 это 5? ", proceed(num0))
81.	print("1 это 5? ", proceed(num1))
82.	print("2 это 5? ", proceed(num2))
83.	print("3 это 5? ", proceed(num3))
84.	print("4 это 5? ", proceed(num4))
85.	print("6 это 5? ", proceed(num6))
86.	print("7 это 5? ", proceed(num7))
87.	print("8 это 5? ", proceed(num8))
88.	print("9 это 5? ", proceed(num9), '\n')
89.	
90.	# Прогон по тестовой выборке
91.	print("Узнал 5? ", proceed(num5))
92.	print("Узнал 5 - 1? ", proceed(num51))
93.	print("Узнал 5 - 2? ", proceed(num52))
94.	print("Узнал 5 - 3? ", proceed(num53))
95.	print("Узнал 5 - 4? ", proceed(num54))
96.	print("Узнал 5 - 5? ", proceed(num55))
97.	print("Узнал 5 - 6? ", proceed(num56))