using System;
using System.Collections.Generic;

namespace OptimizatonMethods
{
    public partial class Task
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public double? Alpha { get; set; }
        public double? Beta { get; set; }
        public double? Delta { get; set; }
        public double? Mu { get; set; }
        public double? G { get; set; }
        public double? A { get; set; }
        public double? N { get; set; }
        public double? T1min { get; set; }
        public double? T1max { get; set; }
        public double? T2min { get; set; }
        public double? T2max { get; set; }
        public double? Price { get; set; }
        public double? Step { get; set; }
    }
}
