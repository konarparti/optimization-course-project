using System;
using System.Collections.Generic;
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
            return t1 >= t1min && t1 <= t1max && t2 >= t2min && t2 <= t2max && (t2 - t1) >= tempDiff;
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
    }
}
