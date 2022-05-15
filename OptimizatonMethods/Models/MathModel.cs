using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace OptimizatonMethods.Models
{
    public class MathModel
    {
        private readonly Task _task;
        private double _step;
        private double _k = 10;
        private double _r = 2;
        private double _n = 2;
        private double _epsilon = 0.01;
        private double alpha;
        private double beta;
        private double mu;
        private double delta;
        private double G;
        private double A;
        private double N;
        private double t1min;
        private double t1max;
        private double t2min;
        private double t2max;
        private double tempDiff;
        List<Point3D> values = new List<Point3D>();

        public MathModel(Task task)
        {
            _task = task;
            alpha = (double)_task.Alpha;
            beta = (double)_task.Beta;
            mu = (double)_task.Mu;
            delta = (double)_task.Delta;
            G = (double)_task.G;
            A = (double)_task.A;
            N = (double)_task.N;
            t1min = (double)_task.T1min;
            t1max = (double)_task.T1max;
            t2min = (double)_task.T2min;
            t2max = (double)_task.T2max;
            tempDiff = (double)_task.DifferenceTemp;
        }
        public int CalculationCount { get; private set; } = 0;
        public double Function(double t1, double t2)
        {
            return (double)(_task.Price * alpha * G * (Math.Pow(t2 - beta * A, N) + mu * Math.Pow(Math.Exp(t1 + t2), N) + delta * (t2 - t1)));
        }

        private bool Conditions(double t1, double t2)
        {
            return (t2 - t1) >= tempDiff;
        }

        public void Calculate(out List<Point3D> points3D)
        {
            double funcMin = double.MaxValue;
            _step = Math.Pow(_k, _r) * _epsilon;
            points3D = new List<Point3D>();
            var p3D = new List<Point3D>();
            List<double> values;
            Point newMin;

            newMin = SearchMinOnGrid(out p3D, out values);
            t1min = newMin.X - _step;
            t2min = newMin.Y - _step;

            t1max = newMin.X + _step;
            t2max = newMin.Y + _step;

            _step /= _k;
            points3D.AddRange(p3D);

            while (funcMin > values.Min())
            {
                newMin = SearchMinOnGrid(out p3D, out values);

                t1min = newMin.X - _step;
                t2min = newMin.Y - _step;

                t1max = newMin.X + _step;
                t2max = newMin.Y + _step;

                _step /= _k;
                funcMin = values.Min();
                points3D.AddRange(p3D);
            }
        }

        private Point SearchMinOnGrid(out List<Point3D> points3D, out List<double> values)
        {
            points3D = new List<Point3D>();

            for (var t1 = t1min; t1 <= t1max; t1 += _step)
                for (var t2 = t2min; t2 <= t2max; t2 += _step)
                {
                    if (!Conditions(t1, t2))
                        continue;
                    CalculationCount++;
                    var value = Function(t1, t2);
                    if (value < 0)
                    {
                        MessageBox.Show($"t1 {t1} t2 {t2} Z {value}");
                    }
                    points3D.Add(new Point3D(Math.Round(t1, 2), Math.Round(t2, 2), Math.Round(value, 2)));

                }

            var valuesListTemp = points3D.Select(item => item.Z).ToList();
            values = valuesListTemp;
            return new Point(points3D.Find(x => x.Z == valuesListTemp.Min()).X, points3D.Find(x => x.Z == valuesListTemp.Min()).Y);
        }

        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: OptimizatonMethods.Models.Point3D")]
        [SuppressMessage("ReSharper.DPA", "DPA0003: Excessive memory allocations in LOH", MessageId = "type: OptimizatonMethods.Models.Point3D[]")]
        [SuppressMessage("ReSharper.DPA", "DPA0003: Excessive memory allocations in LOH", MessageId = "type: OptimizatonMethods.Models.Point[]")]
        public void GeneticAlg(int count)
        {
            var rand = new Random();
            var population = new List<Point>();
            int countOfIndividuals = 0;

            //создание начальной популяции, равномерно распределенной по все области поиска

            for (var t1 = (double)_task.T1min; t1 <= (double)_task.T1max; t1 += 1)
            {
                for (var t2 = (double)_task.T2min; t2 <= (double)_task.T2max; t2 += 1)
                {
                    var individual = new Point(t1, t2);
                    population.Add(individual);
                    countOfIndividuals++;
                }
            }

            for (int k = 0; k < count; k++) // кол-во поколений
            {
                //отбор для скрещивания
                var percent =
                    (int)Math.Round(population.Count * 0.1,
                        0); // процент особей, которые будут скрещены от кол-ва всех (10%)

                var valuesOfFunction = new List<Point3D>();
                foreach (var value in population)
                {
                    var v = Function(value.X, value.Y);
                    valuesOfFunction.Add(new Point3D(value.X, value.Y, v));
                }

                valuesOfFunction.Sort(delegate (Point3D p1, Point3D p2)
                {
                    if (p1.Z > p2.Z)
                        return 1;
                    if (p1.Z < p2.Z)
                        return -1;
                    return 0;
                }); // сортировка по возрастанию

                var bestOfAll = new List<Point>();
                for (int i = 0; i < percent; i++)
                {
                    bestOfAll.Add(new Point(valuesOfFunction[i].X,
                        valuesOfFunction[i]
                            .Y)); // отбор 10% особей, у которых самые лучшие значение целевой функции из общего списка
                }

                if (bestOfAll.Count % 2 != 0)
                    bestOfAll.RemoveAt(bestOfAll.Count - 1);

                // Процесс скрещивания пар, получаем лист потомков
                var children = new List<Point>();
                for (int i = 0; i < bestOfAll.Count - 1; i += 2)
                {
                    var newIndividual = new Point((bestOfAll[i].X + bestOfAll[i + 1].X) / 2,
                        (bestOfAll[i].Y + bestOfAll[i + 1].Y) / 2);
                    children.Add(newIndividual);
                }

                //Процесс мутации.
                //TODO: Вероятность мутации надо бы настраивать, но пока она 50%
                foreach (var value in children)
                {
                    if (rand.Next(1, 101) <= 50)
                    {
                        var r = rand.Next(0, 4);
                        if (r == 0)
                            value.X += 1;
                        else if (r == 1)
                            value.X -= 1;
                        else if (r == 2)
                            value.Y += 1;
                        else if (r == 3)
                            value.Y -= 1;
                    }
                }

                population.AddRange(children);

                var temp = new List<Point>();
                temp.AddRange(population);
                //Очистка популяции. 
                foreach (var individual in temp)
                {
                    //if (population.Count > countOfIndividuals)
                    {
                        //1 этап. Проверка условий
                        if (!Conditions(individual.X, individual.Y))
                        {
                            population.Remove(individual);
                        }
                    }
                }

                //2 этап. Проверка значений ЦФ
                if (population.Count > countOfIndividuals)
                {
                    var vf = new List<Point3D>();
                    foreach (var value in population)
                    {
                        var v = Function(value.X, value.Y);
                        vf.Add(new Point3D(value.X, value.Y, v));
                    }

                    vf.Sort(delegate (Point3D p1, Point3D p2)
                    {
                        if (p1.Z > p2.Z)
                            return 1;
                        if (p1.Z < p2.Z)
                            return -1;
                        return 0;
                    }); // сортировка по возрастанию

                    var diff = countOfIndividuals - population.Count;
                    if (diff > 0) // это не обязательно проверять
                    {
                        for (int i = 0; i < diff; i++)
                        {
                            population.Remove(new Point(vf[i].X, vf[i].Y));
                        }
                    }
                }

                
                foreach (var value in population)
                {
                    var v = Function(value.X, value.Y);
                    values.Add(new Point3D(value.X, value.Y, v));
                }

                values.Sort(delegate (Point3D p1, Point3D p2)
                {
                    if (p1.Z > p2.Z)
                        return 1;
                    if (p1.Z < p2.Z)
                        return -1;
                    return 0;
                }); // сортировка по возрастанию
                

            }
            MessageBox.Show($" Температура в змеевике Т1, С: {Math.Round(values[0].X,2)}\n " +
                            $"Температура в диффузоре Т2, С: {Math.Round(values[0].Y,2)} \n " +
                            $"Минимальная себестоимость, у.е.:{Math.Round(values[0].Z, 2)}");
        }
    }
}
