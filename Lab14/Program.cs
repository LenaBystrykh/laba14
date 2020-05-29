using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Lab10;
using Lab11;

namespace Lab14
{
    public class Program
    {
            public static Stack<Trial> StackTrial;
            public static Stack<string> StackString;
            public static Dictionary<Trial, Trial> DictTrial;
            public static Dictionary<string, Trial> DictString;
            public static TestCollections col;
        static void Main(string[] args)
        {
            StackTrial = new Stack<Trial>();
            StackString = new Stack<string>();
            DictTrial = new Dictionary<Trial, Trial>();
            DictString = new Dictionary<string, Trial>();
            col = new TestCollections(10);

            ShowCollection(col);

            Sample(4, col);      // выборка испытаний, сданных на оценку 4+
            Count(17, col);      // подсчёт испытаний, написанных позднее 17 числа любого месяца
            Variety(10, 6, col); // разность множеств (испытания, написанные позднее 10 числа любого месяца, и испытания, написанные раньше 6 месяца)
            Grouping(7, col);    // группировка по оценкам: >=7 и <7
            Aggregation(col);    // сумма всех оценок
        }

        public static void Sample(int sMark, TestCollections col)
        {
            Console.WriteLine($"Выборка всех испытаний, сданных на оценку > {sMark}");

            // LINQ
            var linq = from trial in col.StackTrial where trial.Mark > sMark select trial;
            foreach (Trial trial in linq)
            {
                trial.Show();
            }

            // расширяющие методы
            var methods = col.StackTrial.Where(trial => trial.Mark > sMark).Select(trial => trial);

            // совпадение результатов
            if (linq.Count() == methods.Count())
                Console.WriteLine("Результаты LINQ запроса совпадают с результатами запроса с использованием методов расширения");
            Console.WriteLine();
        }

        public static void Count(int cDate, TestCollections col)
        {
            var linq = (from trial in col.StackTrial where trial.Day > cDate select trial).Count<Trial>();

            var methods = (col.StackTrial.Where(trial => trial.Day > cDate).Select(trial => trial))
                .Count<Trial>();

            Console.WriteLine($"Кол-во испытаний, написанных позднее {cDate} числа месяца: {linq}");
            if (linq == methods)
                Console.WriteLine("Результаты LINQ запроса совпадают с результатами запроса с использованием методов расширения");
            Console.WriteLine();
        }

        public static void Variety(int vDay, int vMon, TestCollections col)
        {
            Console.WriteLine($"Испытания, сданные позднее {vDay} числа месяца, но ранее {vMon} месяца");

            var linq = (from trial in col.StackTrial where trial.Day > vDay select trial)
                .Except(from trial in col.StackTrial where trial.Month > vMon select trial);

            var methods =
                (col.StackTrial.Where(trial => trial.Day > vDay).Select(trial => trial)).Except(
                    col.StackTrial.Where(trial => trial.Month > vMon).Select(trial => trial));

            foreach (Trial trial in linq)
            {
                trial.Show();
            }

            if (linq.Count() == methods.Count())
                Console.WriteLine("Результаты LINQ запроса совпадают с результатами запроса с использованием методов расширения");
            Console.WriteLine();
        }

        public static void Aggregation(TestCollections col)
        {
            Trial SubAggregate(Trial a, Trial b)
            {
                b.Mark += a.Mark;
                return b;
            }


            var linq = (from trial in col.StackTrial select trial.Mark).Sum();

            var methods = (col.StackTrial.Aggregate(SubAggregate)).Mark;

            Console.WriteLine("Сумма всех полученных оценок: " + linq);

            if (linq == methods)
                Console.WriteLine("Результаты LINQ запроса совпадают с результатами запроса с использованием методов расширения");

            Console.WriteLine();
        }

        public static void Grouping(int num, TestCollections col)
        {
            Console.WriteLine($"Полученная за испытание оценка > {num}");

            var linq = from trial in col.StackTrial
                       group trial by trial.Mark > num;

            var methods = col.StackTrial.GroupBy(trial => trial.Mark > num);

            foreach (IGrouping<bool, Trial> g in linq)
            {
                Console.WriteLine(g.Key ? $"Оценка > {num}" : $"Оценка <= {num}");
                foreach (var t in g)
                    t.Show();
                Console.WriteLine();
            }
            if (linq.Count() == methods.Count())
                Console.WriteLine("Результаты LINQ запроса совпадают с результатами запроса с использованием методов расширения");
            Console.WriteLine();
        }

        public static void ShowCollection(TestCollections col)
        {
            Console.WriteLine("Коллекция: ");
            foreach (Trial item in col.StackTrial)
            {
                item.Show();
            }

            Console.WriteLine();
        }
    }

    public class TestCollections
    {
        public Stack<Trial> StackTrial;
        public Stack<string> StackString;
        public Dictionary<Trial, Trial> DictTrial;
        public Dictionary<string, Trial> DictString;

        public int Length;

        public TestCollections(int length)
        {
            StackTrial = new Stack<Trial>();
            StackString = new Stack<string>();
            DictTrial = new Dictionary<Trial, Trial>();
            DictString = new Dictionary<string, Trial>();

            Length = length;

            Random r = new Random();
            for (int i = 0; i < Length; i++)
            {
                int d = r.Next(1, 30);
                int mo = r.Next(1, 12);
                int ma = r.Next(1, 10);
                Trial t = new Trial(d, mo, ma);
                //Console.WriteLine(t.value);

                string tmpString = i.ToString();

                StackString.Push(tmpString);
                StackTrial.Push(t);

                int d1 = r.Next(1, 30);
                int mo1 = r.Next(1, 12);
                int ma1 = r.Next(1, 10);
                Trial tmp = new Trial(d1, mo1, ma1);
                //Console.WriteLine(tmp.value);

                DictString.Add(tmpString, tmp);
                DictTrial.Add(t, tmp);
            }
        }

        public void AddElement(string key, Test tKey, Test tValue)
        {
            if (!StackString.Contains(key) && !StackTrial.Contains(tKey))
            {
                Length += 1;

                StackString.Push(key);
                StackTrial.Push(tKey);

                DictTrial.Add(tKey, tValue);
                DictString.Add(key, tValue);
            }
            else
            {
                throw new Exception("Ключ должен быть уникальным!");
            }
        }
    }
}
