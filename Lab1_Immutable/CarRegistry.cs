using System;
using System.Collections.Generic;

namespace Lab1_Immutable
{
    // Журнал реєстрації автомобілів на підприємстві.
    // Усередині дві колекції: поточний стан і архів попередніх записів.
    // Сам журнал змінний - незмінними є записи в ньому (об'єкти Car).
    class CarRegistry
    {
        private List<Car> cars = new List<Car>();     // поточні записи
        private List<Car> archive = new List<Car>();  // попередні користувачі

        public void Add(Car car)
        {
            if (Find(car.Number) != null)
            {
                Console.WriteLine("Автомобіль " + car.Number + " вже є в журналі.");
                return;
            }
            cars.Add(car);
            Console.WriteLine("Додано: " + car);
        }

        public Car Find(string number)
        {
            foreach (Car c in cars)
            {
                if (c.Number == number)
                    return c;
            }
            return null;
        }

        public void Remove(string number)
        {
            Car car = Find(number);
            if (car == null)
            {
                Console.WriteLine("Автомобіль " + number + " не знайдено.");
                return;
            }
            cars.Remove(car);
            archive.Add(car);   // запис не зникає, а переходить в архів
            Console.WriteLine("Знято з обліку: " + car);
        }

        // Передача автомобіля іншому користувачу.
        // Старий об'єкт Car не змінюється - він іде в архів,
        // а в журнал стає новий об'єкт з новим користувачем.
        public void Transfer(string number, Person newUser, DateTime date)
        {
            Car oldCar = Find(number);
            if (oldCar == null)
            {
                Console.WriteLine("Автомобіль " + number + " не знайдено.");
                return;
            }

            Car newCar = oldCar.WithNewUser(newUser, date);

            archive.Add(oldCar);
            cars[cars.IndexOf(oldCar)] = newCar;

            Console.WriteLine("Передано " + number + ": " +
                              oldCar.User.Name + " -> " + newUser.Name +
                              " (" + date.ToString("dd.MM.yyyy") + ")");
        }

        public void PrintAll()
        {
            Console.WriteLine("--- Журнал реєстрації ---");
            if (cars.Count == 0)
            {
                Console.WriteLine("(порожньо)");
            }
            foreach (Car c in cars)
            {
                Console.WriteLine("  " + c);
            }
        }

        // Уся історія користування конкретним автомобілем.
        public void PrintHistory(string number)
        {
            Console.WriteLine("--- Історія автомобіля " + number + " ---");

            foreach (Car c in archive)
            {
                if (c.Number == number)
                    Console.WriteLine("  було:  " + c.User + ", з " + c.Since.ToString("dd.MM.yyyy"));
            }

            Car current = Find(number);
            if (current != null)
                Console.WriteLine("  зараз: " + current.User + ", з " + current.Since.ToString("dd.MM.yyyy"));
            else
                Console.WriteLine("  зараз: автомобіль знято з обліку");
        }
    }
}
