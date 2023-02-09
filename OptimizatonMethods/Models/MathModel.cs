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
        public readonly Task _task;
        public double _step;
        public double _k = 10;
        public double _r = 2;
        public double _n = 2;
        public double _epsilon = 0.01;
        public double alpha;
        public double beta;
        public double mu;
        public double delta;
        public double G;
        public double A;
        public double N;
        public double t1min;
        public double t1max;
        public double t2min;
        public double t2max;
        public double tempDiff;
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
            return (double)(_task.Price * alpha * G *
                            (Math.Pow(t2 - beta * A, N) + mu * Math.Pow(Math.Exp(t1 + t2), N) + delta * (t2 - t1)));
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
                        //MessageBox.Show($"t1 {t1} t2 {t2} Z {value}");
                    }

                    points3D.Add(new Point3D(Math.Round(t1, 2), Math.Round(t2, 2), Math.Round(value, 2)));

                }

            var valuesListTemp = points3D.Select(item => item.Z).ToList();
            values = valuesListTemp;
            return new Point(points3D.Find(x => x.Z == valuesListTemp.Min()).X,
                points3D.Find(x => x.Z == valuesListTemp.Min()).Y);
        }

        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
            MessageId = "type: OptimizatonMethods.Models.Point3D")]
        [SuppressMessage("ReSharper.DPA", "DPA0003: Excessive memory allocations in LOH",
            MessageId = "type: OptimizatonMethods.Models.Point3D[]")]
        [SuppressMessage("ReSharper.DPA", "DPA0003: Excessive memory allocations in LOH",
            MessageId = "type: OptimizatonMethods.Models.Point[]")]
        public void GeneticAlg(int count, out List<Point3D> points)
        {
            points = new List<Point3D>();
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

                var temp2 = values
                    .Select(value => new Point3D(Math.Round(value.X, 2), Math.Round(value.Y, 2), Math.Round(value.Z, 2))).ToList();

                points.AddRange(temp2);

                values.Sort(delegate (Point3D p1, Point3D p2)
                {
                    if (p1.Z > p2.Z)
                        return 1;
                    if (p1.Z < p2.Z)
                        return -1;
                    return 0;
                }); // сортировка по возрастанию

            }
        }

        #region Box

        /// <summary>
        /// Хранение значений комплекса для вывода на экран
        /// </summary>
        public struct Complex
        {
            /// <summary>
            /// номер комплекса
            /// </summary>
            public int NumberComplex;

            /// <summary>
            /// точки в комплекса 
            /// </summary>
            public double PointX;

            public double PointY;

            /// <summary>
            /// Значения точек
            /// </summary>
            public double Func;
        }

        public List<Complex> Complices;

        /// <summary>
        /// тип данных для хранения наилучшей и наихуйдшей вершины
        /// </summary>
        struct ExtrPoint
        {
            /// <summary>
            /// // 0 - если плохая вершина, 1 - если вершина хорошая
            /// </summary>
            public int Flag;

            /// <summary>
            /// индекс в массиве точек комплекса
            /// </summary>
            public int Index;

            /// <summary>
            /// значение функции в этой точке
            /// </summary>
            public double ValueFunc;

            /// <summary>
            /// координаты точки
            /// </summary>
            public Point ValuePoint;
        }

        /// <summary>
        /// массив для хранения исходных вершин
        /// </summary>
        private static Point[] _StartPoints { get; set; }

        /// <summary>
        /// массив для хранения активного комплекса
        /// </summary>
        private static Point[] _ComplexPoints { get; set; }

        /// <summary>
        /// массив для хранения точек, не выполняющих условия
        /// </summary>
        private Point[] _ErrorPoints { get; set; }

        private double[] _ValuesFunc { get; set; }

        private void SearchPoints(ref bool flag, ref int countErrorPoints, ref int countComplexPoints, int countPoint)
        {
            countComplexPoints = 0;
            countErrorPoints = 0;
            Random random = new Random();
            Complices = new List<Complex>();

            // находим начальные точки
            for (int i = 0; i < countPoint; i++)
            {
                _StartPoints[i] = new Point(
                    t1min + random.NextDouble() * (t1max - t1min),
                    t2min + random.NextDouble() * (t2max - t2min));
            }

            flag = true; // false если хотя бы одна вершина удовлетворяет условиям

            for (int i = 0; i < countPoint; i++)
            {

                // проверяем что найденная вершина удовлетворяет ограничениям второго рода
                if (_StartPoints[i].X * 0.5 + _StartPoints[i].Y <= tempDiff)
                {
                    _ComplexPoints[countComplexPoints] = new Point(_StartPoints[i].X, _StartPoints[i].Y);

                    flag = false;
                    countComplexPoints++;
                }
                else
                {
                    _ErrorPoints[countErrorPoints] = new Point(_StartPoints[i].X, _StartPoints[i].Y);

                    countErrorPoints++;
                }
            }
        }

        public void Calc(out List<Point3D> points)
        {
            points = new List<Point3D>();

            // определяем количество вершин комплекса
            var countPoint = 0;
            if (N <= 5)
                countPoint = (int)N * 2;
            else
                countPoint = (int)N + 1;

            _StartPoints = new Point[countPoint]; // массив исходных точек
            _ComplexPoints = new Point[countPoint];
            _ErrorPoints = new Point[countPoint];
            _ValuesFunc = new double[countPoint];



            bool flag = true; // false - если хотя бы одна вершина удовлетворяет условиям
            int countErrorPoints = 0;
            int countComplexPoints = 0;

            while (flag)
            {
                SearchPoints(ref flag, ref countErrorPoints, ref countComplexPoints, countPoint);

            }

            double sumComplexPointsX = 0;
            double sumComplexPointsY = 0;

            //считаем сумму значений по каждой координате
            for (int i = 0; i < countComplexPoints; i++)
            {
                sumComplexPointsX += _ComplexPoints[i].X;
                sumComplexPointsY += _ComplexPoints[i].Y;
            }

            // исправление вершин, которые не выполняют ограничения
            for (int i = 0; i < countErrorPoints; i++)
            {
                _ErrorPoints[i].X = 0.5 * (_ErrorPoints[i].X + (1 / (countComplexPoints)) * sumComplexPointsX);
                _ErrorPoints[i].Y = 0.5 * (_ErrorPoints[i].Y + (1 / (countComplexPoints)) * sumComplexPointsY);

                if (_ErrorPoints[i].X * 0.5 + _ErrorPoints[i].Y <=
                    tempDiff) // проверяем что в найденной вершине выполняются ограничения второго рода
                {
                    _ComplexPoints[countComplexPoints] = new Point(_ErrorPoints[i].X, _ErrorPoints[i].Y);

                    countComplexPoints++;
                }

                else
                {
                    i -= 1;
                }
            }

            // вычисление значений функции в вершинах комплекса
            for (int i = 0; i < _ComplexPoints.Length; i++)
            {
                _ValuesFunc[i] = Function(_ComplexPoints[i].X, _ComplexPoints[i].Y);
            }

            int number = 0;

            while (true)
            {
                for (int i = 0; i < _ComplexPoints.Length; i++)
                {
                    Complices.Add(new Complex
                    {
                        NumberComplex = number,
                        PointX = _ComplexPoints[i].X,
                        PointY = _ComplexPoints[i].Y,
                        Func = _ValuesFunc[i]
                    });
                }

                number++;

                double[] sortValuesFunc = new double[_ValuesFunc.Length];

                for (int i = 0; i < _ValuesFunc.Length; i++)
                {
                    sortValuesFunc[i] = _ValuesFunc[i];
                }

                Array.Sort(sortValuesFunc);

                var extrPoint = new ExtrPoint[2]; // массив для хранения самой "хорошей" и самой "плохой" вершины

                // запоминаем точки самой "хорошей" и самой "плохой" вершины
                for (int i = 0; i < _ValuesFunc.Length; i++)
                {
                    if (_ValuesFunc[i] == sortValuesFunc[0])
                    {
                        extrPoint[0].Flag = 1;
                        extrPoint[0].Index = i;
                        extrPoint[0].ValueFunc = _ValuesFunc[i];
                        extrPoint[0].ValuePoint = _ComplexPoints[i];
                    }

                    else if (_ValuesFunc[i] == sortValuesFunc[sortValuesFunc.Length - 1])
                    {
                        extrPoint[extrPoint.Length - 1].Flag = 0;
                        extrPoint[extrPoint.Length - 1].Index = i;
                        extrPoint[extrPoint.Length - 1].ValueFunc = _ValuesFunc[i];
                        extrPoint[extrPoint.Length - 1].ValuePoint = _ComplexPoints[i];
                    }
                }

                var centerPoint = new Point(); // координата центра комплекса

                double
                    sumMinusExtrValX =
                        0; // хранение суммы значений вершин по Х минус худшая вершина для промежуточных вычислений
                double
                    sumMinusExtrValY =
                        0; // хранение суммы значений вершин по Х минус худшая вершина для промежуточных вычислений


                // вычисление промежуточной суммы для координаты центра комплекса
                for (int i = 0; i < _ComplexPoints.Length; i++)
                {
                    sumMinusExtrValX += _ComplexPoints[i].X;
                    sumMinusExtrValY += _ComplexPoints[i].Y;
                }

                // координаты центра комплекса
                centerPoint.X = 1.0 / (countPoint - 1) *
                                (sumMinusExtrValX - extrPoint.Last(x => x.Flag == 0).ValuePoint.X);
                centerPoint.Y = 1.0 / (countPoint - 1) *
                                (sumMinusExtrValY - extrPoint.Last(x => x.Flag == 0).ValuePoint.Y);

                double sumB = 0; // хранение суммы для проверки окончания поиска

                sumB += Math.Abs((centerPoint.X - extrPoint.Last(x => x.Flag == 0).ValuePoint.X)) +
                        Math.Abs((centerPoint.X - extrPoint.Last(x => x.Flag == 1).ValuePoint.X));
                sumB += Math.Abs((centerPoint.Y - extrPoint.Last(x => x.Flag == 0).ValuePoint.Y)) +
                        Math.Abs((centerPoint.Y - extrPoint.Last(x => x.Flag == 1).ValuePoint.Y));

                double B = 1.0 / (2 * N) * sumB;



                if (B < 0.1)
                {
                    var point = new Point3D
                    {
                        X = Math.Round(centerPoint.X, 2),
                        Y = Math.Round(centerPoint.Y, 2),
                        Z = Math.Round(Function(centerPoint.X, centerPoint.Y), 2),
                    };
                    points.Add(point);
                    return;
                }

                else
                {
                    var newPoint = new Point
                    {
                        X = 2.3 * centerPoint.X - 1.3 * extrPoint.Last(x => x.Flag == 0).ValuePoint.X,
                        Y = 2.3 * centerPoint.Y - 1.3 * extrPoint.Last(x => x.Flag == 0).ValuePoint.Y
                    }; // новая координата взамен наихудшей

                    // проверям ограничений первого рода                
                    if (t1min > newPoint.X)
                    {
                        newPoint.X = t1min + 0.1;
                    }
                    else if (newPoint.X > t2max)
                    {
                        newPoint.X = t1min - 0.1;
                    }

                    if (t1max > newPoint.Y)
                    {
                        newPoint.Y = t2max + 0.1;
                    }
                    else if (newPoint.Y > t2max)
                    {
                        newPoint.Y = t2max - 0.1;
                    }

                    // проверка ограничений второго рода
                    // пока ограничение не выполняется смещаем координату к центру
                    while ((newPoint.X * 0.5 + newPoint.Y) > tempDiff)
                    {
                        newPoint.X = 0.5 * (newPoint.X + centerPoint.X);
                        newPoint.Y = 0.5 * (newPoint.Y + centerPoint.Y);
                    }

                    // вычисляем значение функции в новой точке
                    double newPointF = Function(newPoint.X, newPoint.Y);

                    while (newPointF > extrPoint.Last(x => x.Flag == 0).ValueFunc)
                    {
                        newPoint.X = 0.5 * (newPoint.X + extrPoint.Last(x => x.Flag == 1).ValuePoint.X);
                        newPoint.Y = 0.5 * (newPoint.Y + extrPoint.Last(x => x.Flag == 1).ValuePoint.Y);
                        newPointF = Function(newPoint.X, newPoint.Y);
                    }

                    // записываем значения новой точке в массив вершин Комплекса
                    _ComplexPoints[extrPoint.Last(x => x.Flag == 0).Index] = newPoint;
                    _ValuesFunc[extrPoint.Last(x => x.Flag == 0).Index] = newPointF;
                    points.Add(new Point3D(Math.Round(newPoint.X, 2), Math.Round(newPoint.Y, 2), Math.Round(newPointF, 2)));
                }
            }
        }

        #endregion
    }
}
