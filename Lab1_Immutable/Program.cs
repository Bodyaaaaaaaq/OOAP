using System;
using System.Text;

namespace Lab1_Immutable
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Person ivanov = new Person("Іванов І. І.", "водій", 101);
            Person petrenko = new Person("Петренко П. П.", "водій", 102);
            Person kovalchuk = new Person("Ковальчук С. М.", "інженер", 205);
            Person shevchuk = new Person("Шевчук О. В.", "начальник відділу", 301);

            CarRegistry registry = new CarRegistry();

            registry.Add(new Car("Skoda", "Octavia", "BC1234AK", ivanov, new DateTime(2023, 3, 10)));
            registry.Add(new Car("Renault", "Duster", "BC5678AM", petrenko, new DateTime(2023, 5, 22)));
            registry.Add(new Car("Ford", "Transit", "BC9012AO", kovalchuk, new DateTime(2024, 1, 15)));

            Console.WriteLine();
            registry.PrintAll();

            // Кілька передач одного автомобіля
            Console.WriteLine();
            registry.Transfer("BC1234AK", kovalchuk, new DateTime(2024, 6, 1));
            registry.Transfer("BC1234AK", shevchuk, new DateTime(2025, 2, 14));
            registry.Transfer("BC5678AM", ivanov, new DateTime(2025, 4, 3));

            Console.WriteLine();
            registry.PrintAll();

            Console.WriteLine();
            registry.PrintHistory("BC1234AK");

            // Пошук за номером
            Console.WriteLine();
            Car found = registry.Find("BC9012AO");
            Console.WriteLine("Пошук BC9012AO: " + found);

            // Зняття з обліку
            Console.WriteLine();
            registry.Remove("BC9012AO");
            registry.PrintHistory("BC9012AO");

            // Перевірка незмінності: старий об'єкт лишився таким, яким був
            Console.WriteLine();
            Console.WriteLine("--- Перевірка незмінності ---");
            Car car = new Car("Toyota", "Corolla", "BC3333AB", ivanov, new DateTime(2025, 1, 1));
            Car sameCarNewUser = car.WithNewUser(petrenko, new DateTime(2025, 9, 1));
            Console.WriteLine("  старий об'єкт: " + car);
            Console.WriteLine("  новий об'єкт:  " + sameCarNewUser);
            Console.WriteLine("  це різні об'єкти: " + !ReferenceEquals(car, sameCarNewUser));

            Console.WriteLine();
            Console.WriteLine("Натисніть Enter для виходу...");
            Console.ReadLine();
        }
    }
}
