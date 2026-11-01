using System;

namespace Lab2_Builder
{
    // Абстрактний будівельник.
    // Описує кроки, з яких складається будь-яка комплектація автомобіля.
    // Що саме ставиться на кожному кроці - вирішує конкретний будівельник.
    abstract class CarBuilder
    {
        protected Car Car { get; private set; }

        // Назва комплектації, за якою покупець її обирає
        public abstract string TrimName { get; }

        public void CreateNewCar()
        {
            Car = new Car();
            Car.Model = "Автовиробник AV-5";
            Car.Trim = TrimName;
        }

        // Повертає готовий автомобіль назовні
        public Car GetMyCar()
        {
            return Car;
        }

        // Кроки, потрібні щоб зібрати автомобіль
        public abstract void SetEngine();
        public abstract void SetSafety();
        public abstract void SetComfort();
        public abstract void SetInterior();
        public abstract void SetPrice();
    }
}
