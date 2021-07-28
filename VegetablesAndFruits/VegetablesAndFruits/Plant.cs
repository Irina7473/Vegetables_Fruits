using System;

namespace VegetablesAndFruits
{
    public class Plant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public uint Calories { get; set; }

        public Plant() { }
        public Plant(int id, string name, string type, string color, uint calories)
        {
            Id = id;
            Name = name;
            Type = type;
            Color = color;
            Calories = calories;
        }
    }
}