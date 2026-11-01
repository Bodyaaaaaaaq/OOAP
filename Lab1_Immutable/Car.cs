using System;

namespace Lab1_Immutable
{
    // Автомобіль у журналі реєстрації.
    // Шаблон Immutable: у класі немає жодного методу, який міняє стан об'єкта.
    // Значення задаються один раз у конструкторі, далі їх можна тільки читати.
    // Щоб "передати" автомобіль іншому користувачу, створюється новий об'єкт Car
    // (метод WithNewUser), а старий залишається без змін - саме він і стає записом історії.
    class Car
    {
        private readonly string brand;
        private readonly string model;
        private readonly string number;
        private readonly Person user;      // автомобіль має лише одного користувача
        private readonly DateTime since;   // з якої дати цей користувач ним володіє

        public Car(string brand, string model, string number, Person user, DateTime since)
        {
            this.brand = brand;
            this.model = model;
            this.number = number;
            this.user = user;
            this.since = since;
        }

        public string Brand { get { return brand; } }
        public string Model { get { return model; } }
        public string Number { get { return number; } }
        public Person User { get { return user; } }
        public DateTime Since { get { return since; } }

        // Повертає НОВИЙ автомобіль з іншим користувачем.
        // Поточний об'єкт при цьому не змінюється.
        public Car WithNewUser(Person newUser, DateTime date)
        {
            return new Car(brand, model, number, newUser, date);
        }

        public override string ToString()
        {
            return number + "  " + brand + " " + model +
                   "  -  " + user + ", з " + since.ToString("dd.MM.yyyy");
        }
    }
}
