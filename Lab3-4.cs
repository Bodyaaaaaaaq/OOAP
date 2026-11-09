using System;
using System.IO;
using System.Text;

namespace Lab3_4
{

    // Singleton

    public enum ProductGroup { Food, Medicine, Clothing }

    public sealed class SalesCounter
    {
        private static readonly SalesCounter instance = new SalesCounter();

        private readonly Random random = new Random();
        private readonly int[] counts = new int[3];
        private readonly decimal[] markup = { 0.05m, 0.10m, 0.15m };
        private readonly string[] names = { "Продукти харчування", "Ліки", "Одяг" };

        public decimal TotalCost { get; private set; }

        private SalesCounter() { }
        public static SalesCounter Instance() => instance;

        public void Sell(ProductGroup group)
        {
            int i = (int)group;
            decimal price = Math.Round(random.Next(10, 1000) * (1 + markup[i]), 2);

            counts[i]++;
            TotalCost += price;

            File.AppendAllText("sales.txt", DateTime.Now + " | " + names[i] + "\n", Encoding.UTF8);
        }

        public int Count(ProductGroup g) => counts[(int)g];
    }

    // Bridge

    interface IPayment
    {
        decimal Calculate(decimal rate, decimal amount);
    }

    class Hourly : IPayment
    {
        public decimal Calculate(decimal rate, decimal hours) => rate * hours;
    }

    class Piecework : IPayment
    {
        public decimal Calculate(decimal rate, decimal units) => rate * units;
    }

    abstract class Employee
    {
        public string Name { get; }
        public decimal Rate { get; }
        protected IPayment payment;

        protected Employee(string name, decimal rate, IPayment payment)
        {
            Name = name;
            Rate = rate;
            this.payment = payment;
        }

        public void SetPayment(IPayment p) => payment = p;
        public decimal Salary(decimal amount) => payment.Calculate(Rate, amount);
        public abstract string Info();
    }

    class Programmer : Employee
    {
        private string language;
        public Programmer(string n, decimal r, IPayment p, string lang) : base(n, r, p) { language = lang; }
        public override string Info() => Name + " (програміст, " + language + ")";
    }

    class Driver : Employee
    {
        private string category;
        public Driver(string n, decimal r, IPayment p, string cat) : base(n, r, p) { category = cat; }
        public override string Info() => Name + " (водій, категорія " + category + ")";
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("=== Лаб 3: Singleton ===");
            var counter = SalesCounter.Instance();
            counter.Sell(ProductGroup.Food);
            counter.Sell(ProductGroup.Medicine);
            counter.Sell(ProductGroup.Clothing);
            counter.Sell(ProductGroup.Food);

            Console.WriteLine("Продукти: " + counter.Count(ProductGroup.Food));
            Console.WriteLine("Ліки:     " + counter.Count(ProductGroup.Medicine));
            Console.WriteLine("Одяг:     " + counter.Count(ProductGroup.Clothing));
            Console.WriteLine("Сума:     " + counter.TotalCost + " грн");
            Console.WriteLine("Екземпляр єдиний: " +
                ReferenceEquals(SalesCounter.Instance(), SalesCounter.Instance()));

            Console.WriteLine();
            Console.WriteLine("=== Лаб 4: Bridge ===");
            Employee a = new Programmer("Коваленко", 250, new Hourly(), "C#");
            Employee b = new Driver("Мельник", 35, new Piecework(), "B, C");

            Console.WriteLine(a.Info() + " -> " + a.Salary(160) + " грн (160 год)");
            Console.WriteLine(b.Info() + " -> " + b.Salary(40) + " грн (40 од.)");

            a.SetPayment(new Piecework());
            Console.WriteLine(a.Info() + " -> " + a.Salary(20) + " грн (20 од., відрядна)");

            Console.WriteLine();
            Console.WriteLine("Натисніть Enter для виходу...");
            Console.ReadLine();
        }
    }
}
