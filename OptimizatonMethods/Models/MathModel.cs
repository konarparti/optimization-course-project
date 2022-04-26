using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizatonMethods.Models
{
    public class MathModel
    {
        private readonly Task _task;

        public MathModel(Task task)
        {
            _task = task;
        }
        public int CalculationCount { get; private set; } = 0;
        private double Function(double t1, double t2)
        {
            double alpha = (double)_task.Alpha;
            double beta = (double)_task.Beta;
            double mu = (double)_task.Mu;
            double delta = (double)_task.Delta;
            double G = (double)_task.G;
            double A = (double)_task.A;
            double N = (double)_task.N;

            return alpha * G * (Math.Pow(t2 - beta * A, N) + mu * Math.Pow(Math.E, Math.Pow(t1 + t2, N)) + delta * (t2 - t1));
        }

        private bool Conditions(double t1, double t2)
        {
            return t1 >= _task.T1min && t1 <= _task.T1max && t2 >= _task.T2min && t2 <= _task.T2max && t2 - t1 >= _task.DifferenceTemp;
        }

        public void Calculate(out List<Point> points, out List<Point3D> points3D)
        {
            double funcMin = double.MaxValue;
            double funcMax = double.MinValue;
            double param1min = -1, param2min = -1, param1max = -1, param2max = -1;

            points = new List<Point>();
            points3D = new List<Point3D>();

            for (double Param2Coord = (double)_task.T2min; Param2Coord <= (double)_task.T2max; Param2Coord += 0.1)
                for (double Param1Coord = (double)_task.T1min; Param1Coord <= (double)_task.T1max; Param1Coord += 0.1)
                {
                    if (!Conditions(Param1Coord, Param2Coord))
                        continue;
                    double val = Function(Param1Coord, Param2Coord);
                    CalculationCount++;
                    if (val < 10)
                    {

                        if (funcMin - val > 0.001)
                        {
                            param1min = Param1Coord;
                            param2min = Param2Coord;
                            funcMin = val;
                        }

                        if (val - funcMax > 0.001)
                        {
                            param1max = Param1Coord;
                            param2max = Param2Coord;
                            funcMax = val;
                        }

                        points3D.Add(new Point3D(Math.Round(Param1Coord, 1), Math.Round(Param2Coord, 1), Math.Round(val, 1)));
                    }
                }
            points.Add(new Point(param1min, param2min));
        }
    }
}
