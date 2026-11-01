using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2_Builder
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            CarShop shop = new CarShop();
            shop.AddTrim(new BaseBuilder());
            shop.AddTrim(new ComfortBuilder());
            shop.AddTrim(new EleganceBuilder());
            shop.AddTrim(new PremiumBuilder());

            Console.WriteLine("Доступні комплектації моделі AV-5:");
            foreach (string name in shop.GetTrimNames())
            {
                Console.WriteLine("  - " + name);
            }
            Console.WriteLine();

            // Покупець називає комплектацію і бачить її характеристики
            Console.WriteLine("=== Покупець обрав комплектацію \"Comfort\" ===");
            Car comfort = shop.BuildByName("Comfort");
            Console.WriteLine(comfort);

            Console.WriteLine("=== Покупець обрав комплектацію \"Premium\" ===");
            Car premium = shop.BuildByName("Premium");
            Console.WriteLine(premium);

            // Назви немає в каталозі
            Console.WriteLine("=== Покупець обрав комплектацію \"Sport\" ===");
            Car sport = shop.BuildByName("Sport");
            if (sport == null)
                Console.WriteLine("Такої комплектації немає в каталозі.");
            Console.WriteLine();

            // Повний каталог - усі комплектації підряд
            Console.WriteLine("=== Повний каталог ===");
            foreach (string name in shop.GetTrimNames())
            {
                Console.WriteLine(shop.BuildByName(name));
            }

            // Порівняння вартості
            Console.WriteLine("=== Вартість комплектацій ===");
            foreach (string name in shop.GetTrimNames())
            {
                Car car = shop.BuildByName(name);
                Console.WriteLine("  " + name.PadRight(10) + car.Price.ToString("### ### ###").Trim() + " EUR");
            }

            Console.WriteLine();
            Console.WriteLine("Натисніть Enter для виходу...");
            Console.ReadLine();
        }
    }
}
