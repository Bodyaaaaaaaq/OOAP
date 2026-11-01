using System;

namespace Lab2_Builder
{
    // Чотири конкретні будівельники - чотири стандартні комплектації.
    // Кожен виконує ті самі кроки, але ставить різні компоненти.

    class BaseBuilder : CarBuilder
    {
        public override string TrimName { get { return "Base"; } }

        public override void SetEngine()
        {
            Car.EngineType = "бензиновий";
            Car.EngineVolume = 1.4;
        }

        public override void SetSafety()
        {
            Car.Abs = true;
            Car.Esp = false;
            Car.Airbags = 2;
        }

        public override void SetComfort()
        {
            Car.OnBoardComputer = false;
            Car.Climate = "кондиціонер";
        }

        public override void SetInterior()
        {
            Car.Upholstery = "тканина";
        }

        public override void SetPrice()
        {
            Car.Price = 17500;
        }
    }

    class ComfortBuilder : CarBuilder
    {
        public override string TrimName { get { return "Comfort"; } }

        public override void SetEngine()
        {
            Car.EngineType = "бензиновий";
            Car.EngineVolume = 1.6;
        }

        public override void SetSafety()
        {
            Car.Abs = true;
            Car.Esp = true;
            Car.Airbags = 4;
        }

        public override void SetComfort()
        {
            Car.OnBoardComputer = true;
            Car.Climate = "кондиціонер";
        }

        public override void SetInterior()
        {
            Car.Upholstery = "тканина покращена";
        }

        public override void SetPrice()
        {
            Car.Price = 21300;
        }
    }

    class EleganceBuilder : CarBuilder
    {
        public override string TrimName { get { return "Elegance"; } }

        public override void SetEngine()
        {
            Car.EngineType = "дизельний";
            Car.EngineVolume = 1.9;
        }

        public override void SetSafety()
        {
            Car.Abs = true;
            Car.Esp = true;
            Car.Airbags = 6;
        }

        public override void SetComfort()
        {
            Car.OnBoardComputer = true;
            Car.Climate = "клімат-контроль";
        }

        public override void SetInterior()
        {
            Car.Upholstery = "велюр";
        }

        public override void SetPrice()
        {
            Car.Price = 25900;
        }
    }

    class PremiumBuilder : CarBuilder
    {
        public override string TrimName { get { return "Premium"; } }

        public override void SetEngine()
        {
            Car.EngineType = "дизельний";
            Car.EngineVolume = 2.2;
        }

        public override void SetSafety()
        {
            Car.Abs = true;
            Car.Esp = true;
            Car.Airbags = 8;
        }

        public override void SetComfort()
        {
            Car.OnBoardComputer = true;
            Car.Climate = "двозонний клімат-контроль";
        }

        public override void SetInterior()
        {
            Car.Upholstery = "шкіра";
        }

        public override void SetPrice()
        {
            Car.Price = 31400;
        }
    }
}
