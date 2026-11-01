using System;

namespace Lab1_Immutable
{
    // Працівник підприємства - користувач автомобіля.
    // Клас незмінний (Immutable): поля readonly, є тільки конструктор і властивості
    // для читання. Змінити працівника після створення неможливо.
    class Person
    {
        private readonly string name;
        private readonly string position;
        private readonly int tabNumber;

        public Person(string name, string position, int tabNumber)
        {
            this.name = name;
            this.position = position;
            this.tabNumber = tabNumber;
        }

        public string Name { get { return name; } }
        public string Position { get { return position; } }
        public int TabNumber { get { return tabNumber; } }

        public override string ToString()
        {
            return name + " (" + position + ", таб. № " + tabNumber + ")";
        }
    }
}
