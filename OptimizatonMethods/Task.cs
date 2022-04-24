using System.ComponentModel;

namespace OptimizatonMethods
{
    public partial class Task
    {
        [DisplayName("Идентификатор")]
        public long Id { get; set; }
        [DisplayName("Вариант №")]
        public string? Name { get; set; }
        [DisplayName("α")]
        public double? Alpha { get; set; }
        [DisplayName("β")]
        public double? Beta { get; set; }
        [DisplayName("Δ")]
        public double? Delta { get; set; }
        [DisplayName("μ")]
        public double? Mu { get; set; }
        [DisplayName("Расход реакционной массы, кг/ч")]
        public double? G { get; set; }
        [DisplayName("Давление в реакторе, Кпа")]
        public double? A { get; set; }
        [DisplayName("Количество теплообменных устройств, шт")]
        public double? N { get; set; }
        [DisplayName("Мин. температура Т1, ℃")]
        public double? T1min { get; set; }
        [DisplayName("Макс. температура Т1, ℃")]
        public double? T1max { get; set; }
        [DisplayName("Мин. температура Т2, ℃")]
        public double? T2min { get; set; }
        [DisplayName("Макс. температура Т2, ℃")]
        public double? T2max { get; set; }
        [DisplayName("Разница температур, ℃")]
        public double? DifferenceTemp { get; set; }
        [DisplayName("Себестоимость 1 кг. компонента, у.е.")]
        public double? Price { get; set; }
        [DisplayName("Точность решения, у.е.")]
        public double? Step { get; set; }
    }
}
