using System;
using System.Collections.Generic;

namespace Lab2_Builder
{
    // Директор. Знає послідовність кроків складання, але не знає,
    // які саме компоненти ставляться - це справа будівельника.
    // Тут же зберігається список доступних комплектацій, щоб покупець
    // міг обрати потрібну за назвою.
    class CarShop
    {
        private CarBuilder carBuilder;
        private List<CarBuilder> trims = new List<CarBuilder>();

        public void AddTrim(CarBuilder builder)
        {
            trims.Add(builder);
        }

        public void SetCarBuilder(CarBuilder builder)
        {
            carBuilder = builder;
        }

        // Змушує будівельника пройти всі кроки складання
        public void ConstructCar()
        {
            carBuilder.CreateNewCar();
            carBuilder.SetEngine();
            carBuilder.SetSafety();
            carBuilder.SetComfort();
            carBuilder.SetInterior();
            carBuilder.SetPrice();
        }

        public Car GetCar()
        {
            return carBuilder.GetMyCar();
        }

        // Покупець називає комплектацію - отримує зібраний автомобіль.
        // Якщо такої назви немає, повертається null.
        public Car BuildByName(string trimName)
        {
            foreach (CarBuilder b in trims)
            {
                if (string.Equals(b.TrimName, trimName, StringComparison.OrdinalIgnoreCase))
                {
                    SetCarBuilder(b);
                    ConstructCar();
                    return GetCar();
                }
            }
            return null;
        }

        public List<string> GetTrimNames()
        {
            List<string> names = new List<string>();
            foreach (CarBuilder b in trims)
            {
                names.Add(b.TrimName);
            }
            return names;
        }
    }
}
