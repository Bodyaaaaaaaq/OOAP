using System;
using System.Text;

namespace Lab2_Builder
{
    // Складний об'єкт, який збирає Будівельник.
    // Сам клас нічого не знає про те, хто і в якому порядку його заповнює.
    class Car
    {
        public string Model { get; set; }
        public string Trim { get; set; }            // назва комплектації

        public string EngineType { get; set; }      // бензиновий / дизельний
        public double EngineVolume { get; set; }    // об'єм двигуна, л

        public bool Abs { get; set; }               // антиблокувальна система
        public bool Esp { get; set; }               // система стабілізації
        public int Airbags { get; set; }            // кількість подушок безпеки

        public bool OnBoardComputer { get; set; }   // бортовий комп'ютер
        public string Climate { get; set; }         // кондиціонер / клімат-контроль
        public string Upholstery { get; set; }      // обшивка салону

        public decimal Price { get; set; }          // вартість, євро

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine(Model + " - комплектація \"" + Trim + "\"");
            s.AppendLine(Line("двигун", EngineType + ", " + EngineVolume.ToString("0.0") + " л"));
            s.AppendLine(Line("ABS", YesNo(Abs)));
            s.AppendLine(Line("ESP", YesNo(Esp)));
            s.AppendLine(Line("подушки безпеки", Airbags.ToString()));
            s.AppendLine(Line("бортовий комп'ютер", YesNo(OnBoardComputer)));
            s.AppendLine(Line("клімат", Climate));
            s.AppendLine(Line("обшивка салону", Upholstery));
            s.AppendLine(Line("вартість", Price.ToString("### ### ###").Trim() + " EUR"));
            return s.ToString();
        }

        private string Line(string label, string value)
        {
            return "  " + (label + ":").PadRight(21) + value;
        }

        private string YesNo(bool value)
        {
            if (value)
                return "є";
            return "немає";
        }
    }
}
