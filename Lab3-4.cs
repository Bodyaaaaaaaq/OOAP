using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_4_Singleton_Bridge
{

    public enum ProductGroup { Food, Medicine, Clothing }

    public sealed class SalesCounter
    {
        private static readonly SalesCounter _instance = new SalesCounter();

        private const string LogFile = "sales_log.txt";

        private readonly object _lock = new object();
        private readonly Random _random = new Random();
        private readonly Dictionary<ProductGroup, int> _counts = new Dictionary<ProductGroup, int>();

        public int TotalCount { get; private set; }
        public decimal TotalCost { get; private set; }

        private SalesCounter()
        {
            foreach (ProductGroup g in Enum.GetValues(typeof(ProductGroup)))
                _counts[g] = 0;
        }

        public static SalesCounter Instance() => _instance;

        public static string GroupName(ProductGroup group) => group switch
        {
            ProductGroup.Food => "Продукти харчування",
            ProductGroup.Medicine => "Ліки",
            _ => "Одяг"
        };

        public static decimal Markup(ProductGroup group) => group switch
        {
            ProductGroup.Food => 0.05m,
            ProductGroup.Medicine => 0.10m,
            _ => 0.15m
        };

        public (decimal basePrice, decimal finalPrice) Sell(ProductGroup group)
        {
            lock (_lock)  
            {
                decimal basePrice = Math.Round((decimal)(_random.NextDouble() * 990 + 10), 2);
                decimal finalPrice = Math.Round(basePrice * (1 + Markup(group)), 2);

                _counts[group]++;
                TotalCount++;
                TotalCost += finalPrice;

                File.AppendAllText(LogFile,
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {GroupName(group)}{Environment.NewLine}",
                    Encoding.UTF8);

                return (basePrice, finalPrice);
            }
        }

        public int CountOf(ProductGroup group)
        {
            lock (_lock) return _counts[group];
        }

        public string[] ReadLog() => File.Exists(LogFile) ? File.ReadAllLines(LogFile, Encoding.UTF8) : Array.Empty<string>();
    }


    public sealed record PayRates(decimal PerHour, decimal PerUnit);

    public interface IPaymentMethod
    {
        string Title { get; }
        string WorkUnit { get; }  
        decimal Calculate(PayRates rates, decimal workAmount);
    }

    public class HourlyPayment : IPaymentMethod
    {
        private const decimal NormHours = 160m;     
        private const decimal OvertimeFactor = 1.5m; 

        public string Title => "Погодинна (за відпрацьовані години)";
        public string WorkUnit => "годин";

        public decimal Calculate(PayRates rates, decimal hours)
        {
            decimal overtime = Math.Max(0, hours - NormHours);
            decimal regular = hours - overtime;
            return Math.Round(regular * rates.PerHour + overtime * rates.PerHour * OvertimeFactor, 2);
        }
    }

    public class PieceworkPayment : IPaymentMethod
    {
        public string Title => "Відрядна (за виконану роботу)";
        public string WorkUnit => "одиниць роботи";

        public decimal Calculate(PayRates rates, decimal units) => Math.Round(units * rates.PerUnit, 2);
    }

    public abstract class Employee
    {
        public string Name { get; }
        public string Position { get; }
        public PayRates Rates { get; }


        public IPaymentMethod PaymentMethod { get; private set; }

        protected Employee(string name, string position, PayRates rates, IPaymentMethod paymentMethod)
        {
            Name = name;
            Position = position;
            Rates = rates;
            PaymentMethod = paymentMethod;
        }

        public void SetPaymentMethod(IPaymentMethod method) => PaymentMethod = method;

        public decimal CalculateSalary(decimal workAmount) => PaymentMethod.Calculate(Rates, workAmount);

        public abstract string GetInfo();
    }

    public class Programmer : Employee
    {
        public string Language { get; }

        public Programmer(string name, PayRates rates, IPaymentMethod payment, string language)
            : base(name, "Програміст", rates, payment) => Language = language;

        public override string GetInfo() => $"основна мова: {Language}";
    }

    public class Driver : Employee
    {
        public string LicenseCategory { get; }

        public Driver(string name, PayRates rates, IPaymentMethod payment, string licenseCategory)
            : base(name, "Водій", rates, payment) => LicenseCategory = licenseCategory;

        public override string GetInfo() => $"категорія прав: {LicenseCategory}";
    }

    public class Accountant : Employee
    {
        public string Department { get; }

        public Accountant(string name, PayRates rates, IPaymentMethod payment, string department)
            : base(name, "Бухгалтер", rates, payment) => Department = department;

        public override string GetInfo() => $"відділ: {Department}";
    }

    public class Mason : Employee
    {
        public int Grade { get; }

        public Mason(string name, PayRates rates, IPaymentMethod payment, int grade)
            : base(name, "Муляр", rates, payment) => Grade = grade;

        public override string GetInfo() => $"розряд: {Grade}";
    }


    internal static class Program
    {
        private static readonly HourlyPayment Hourly = new HourlyPayment();
        private static readonly PieceworkPayment Piecework = new PieceworkPayment();
        private static readonly List<Employee> Employees = new List<Employee>();

        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            SeedEmployees();

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("===== Singleton + Bridge =====");
                Console.WriteLine("1. Лічильник продажів (Singleton)");
                Console.WriteLine("2. Бухгалтерія (Bridge)");
                Console.WriteLine("0. Вихід");
                Console.Write("Ваш вибір: ");

                switch (Console.ReadLine()?.Trim())
                {
                    case "1": SalesMenu(); break;
                    case "2": AccountingMenu(); break;
                    case "0": return;
                    default: Console.WriteLine("Невірний пункт меню."); break;
                }
            }
        }


        private static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int v) && v >= min && v <= max) return v;
                Console.WriteLine($"Введіть ціле число від {min} до {max}.");
            }
        }

        private static decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string s = (Console.ReadLine() ?? "").Trim();
                if ((decimal.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, out decimal v) ||
                     decimal.TryParse(s.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out v))
                    && v >= 0)
                    return v;
                Console.WriteLine("Введіть невід'ємне число.");
            }
        }


        private static void SalesMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("--- Лічильник продажів ---");
                Console.WriteLine("1. Продати товар");
                Console.WriteLine("2. Імітація 30 продажів у кількох потоках");
                Console.WriteLine("3. Статистика");
                Console.WriteLine("4. Переглянути файл журналу продажів");
                Console.WriteLine("5. Перевірити, що екземпляр єдиний");
                Console.WriteLine("0. Назад");

                switch (ReadInt("Ваш вибір: ", 0, 5))
                {
                    case 1: SellOne(); break;
                    case 2: SimulateParallel(); break;
                    case 3: ShowStats(); break;
                    case 4: ShowLogFile(); break;
                    case 5: CheckSingleton(); break;
                    case 0: return;
                }
            }
        }

        private static ProductGroup ChooseGroup()
        {
            Console.WriteLine("Група товару: 1 — Продукти харчування (+5%), 2 — Ліки (+10%), 3 — Одяг (+15%)");
            return (ProductGroup)(ReadInt("Оберіть групу: ", 1, 3) - 1);
        }

        private static void SellOne()
        {
            var group = ChooseGroup();
            var (basePrice, finalPrice) = SalesCounter.Instance().Sell(group);

            Console.WriteLine($"Продано: {SalesCounter.GroupName(group)}. " +
                              $"Ціна {basePrice:N2} + надбавка {SalesCounter.Markup(group):P0} = {finalPrice:N2} грн");
        }

        private static void SimulateParallel()
        {
            var groups = (ProductGroup[])Enum.GetValues(typeof(ProductGroup));

            Parallel.For(0, 30, i =>
            {
                SalesCounter.Instance().Sell(groups[i % groups.Length]);
            });

            Console.WriteLine("Виконано 30 продажів у кількох потоках.");
        }

        private static void ShowStats()
        {
            var counter = SalesCounter.Instance();

            Console.WriteLine();
            foreach (ProductGroup g in Enum.GetValues(typeof(ProductGroup)))
                Console.WriteLine($"  {SalesCounter.GroupName(g),-22}: {counter.CountOf(g)} шт.");

            Console.WriteLine($"  {"Усього продано",-22}: {counter.TotalCount} шт.");
            Console.WriteLine($"  {"Сумарна вартість",-22}: {counter.TotalCost:N2} грн");
        }

        private static void ShowLogFile()
        {
            var lines = SalesCounter.Instance().ReadLog();
            if (lines.Length == 0)
            {
                Console.WriteLine("Файл журналу порожній або ще не створений.");
                return;
            }

            Console.WriteLine();
            foreach (var line in lines) Console.WriteLine("  " + line);
        }

        private static void CheckSingleton()
        {
            var a = SalesCounter.Instance();
            var b = SalesCounter.Instance();
            Console.WriteLine($"ReferenceEquals(instance1, instance2) = {ReferenceEquals(a, b)}");
        }


        private static void SeedEmployees()
        {
            Employees.Add(new Programmer("Коваленко Андрій", new PayRates(250m, 900m), Hourly, "C#"));
            Employees.Add(new Driver("Мельник Василь", new PayRates(140m, 35m), Piecework, "B, C"));
            Employees.Add(new Accountant("Бондаренко Ірина", new PayRates(180m, 60m), Hourly, "розрахунок заробітної плати"));
            Employees.Add(new Mason("Ткачук Олег", new PayRates(160m, 450m), Piecework, 4));
        }

        private static void AccountingMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("--- Бухгалтерія ---");
                Console.WriteLine("1. Список працівників");
                Console.WriteLine("2. Нарахувати зарплату працівнику");
                Console.WriteLine("3. Порівняння способів оплати для всіх працівників");
                Console.WriteLine("0. Назад");

                switch (ReadInt("Ваш вибір: ", 0, 3))
                {
                    case 1: ShowEmployees(); break;
                    case 2: PayEmployee(); break;
                    case 3: CompareMethods(); break;
                    case 0: return;
                }
            }
        }

        private static void ShowEmployees()
        {
            Console.WriteLine();
            for (int i = 0; i < Employees.Count; i++)
            {
                var e = Employees[i];
                Console.WriteLine($"  {i + 1}. {e.Name} — {e.Position} ({e.GetInfo()})");
                Console.WriteLine($"     ставка: {e.Rates.PerHour:N2} грн/год, {e.Rates.PerUnit:N2} грн/од.; " +
                                  $"оплата зараз: {e.PaymentMethod.Title}");
            }
        }

        private static void PayEmployee()
        {
            ShowEmployees();
            var emp = Employees[ReadInt("Номер працівника: ", 1, Employees.Count) - 1];

            Console.WriteLine("Спосіб оплати: 1 — погодинна, 2 — відрядна, 0 — залишити поточний");
            switch (ReadInt("Ваш вибір: ", 0, 2))
            {
                case 1: emp.SetPaymentMethod(Hourly); break;
                case 2: emp.SetPaymentMethod(Piecework); break;
            }

            decimal amount = ReadDecimal($"Кількість ({emp.PaymentMethod.WorkUnit}): ");
            decimal salary = emp.CalculateSalary(amount);

            Console.WriteLine();
            Console.WriteLine($"{emp.Name}, {emp.Position}");
            Console.WriteLine($"Спосіб оплати: {emp.PaymentMethod.Title}");
            Console.WriteLine($"Нараховано: {salary:N2} грн");
        }

        private static void CompareMethods()
        {
            const decimal hours = 168m;
            const decimal units = 40m;

            Console.WriteLine();
            Console.WriteLine($"Умови: {hours} год або {units} од. роботи");
            Console.WriteLine($"{"Працівник",-20}{"Посада",-12}{"Погодинна",14}{"Відрядна",14}");

            foreach (var e in Employees)
            {
                var original = e.PaymentMethod;

                e.SetPaymentMethod(Hourly);
                decimal byHours = e.CalculateSalary(hours);

                e.SetPaymentMethod(Piecework);
                decimal byUnits = e.CalculateSalary(units);

                e.SetPaymentMethod(original);  

                Console.WriteLine($"{e.Name,-20}{e.Position,-12}{byHours,14:N2}{byUnits,14:N2}");
            }
        }
    }
}
