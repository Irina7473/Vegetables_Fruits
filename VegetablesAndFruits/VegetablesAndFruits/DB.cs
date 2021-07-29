using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace VegetablesAndFruits
{
    public class DB
    {
        private string _connectionString;
        private SqliteConnection _connection;
        private SqliteCommand _query;               

        public DB(string patch)
        {
            _connectionString = $"Data Source={patch};Mode=ReadWrite;";
            try {
                _connection = new SqliteConnection(_connectionString);
                _query = new SqliteCommand { Connection = _connection };
            }
            catch (Exception)
            {
                throw new Exception("Путь к базе данных не найден");
            }
        }

        public void Open()
        {
            try
            {
                _connection.Open();
                Console.WriteLine("Успешное подключение к базе данных");
            }
            catch (InvalidOperationException)
            {
                throw new Exception("Ошибка открытия базы данных");
            }
            catch (SqliteException)
            {
                throw new Exception("Подключаемся к уже открытой базе данных");
            }
            catch (Exception)
            {
                throw new Exception("Путь к базе данных не найден");
            }
        }

        public void Close()
        {
            _connection.Close();
        }

        public void ShowAll()
        {
            Open();
            _query.CommandText = "SELECT * FROM table_vegetables_fruits;";
            var result = _query.ExecuteReader();

            if (!result.HasRows)
            {
                Console.WriteLine("Нет данных");
                return;
            }

            var plants = new List<Plant>();
            do 
            {
                while (result.Read())
                {
                    var plant = new Plant
                    {
                        Id = result.GetInt32(0),
                        Name = result.GetString(1),
                        Type = result.GetString(2),
                        Color = result.GetString(3),
                        Calories = (uint)result.GetInt32(4),
                    };

                    plants.Add(plant);
                } 
            } while (result.NextResult());

            Console.WriteLine(" --------------------------------------------");
            Console.WriteLine(" № | Название |  Тип  |  Цвет  | Калорийность");
            Console.WriteLine(" --------------------------------------------");
            foreach (var plant in plants)
                Console.WriteLine($" {plant.Id} | {plant.Name} | {plant.Type} | {plant.Color} | {plant.Calories}");
            Console.WriteLine(" --------------------------------------------");

            if (result != null) result.Close();
            Close();
        }

        public void ShowAllNames()
        {
            _connection.Open();
            _query.CommandText = "SELECT id, name FROM table_vegetables_fruits;";
            var result = _query.ExecuteReader();

            if (!result.HasRows)
            {
                Console.WriteLine("Нет данных");
                return;
            }

            Console.WriteLine("В базе данных есть следующие овощи и фрукты");
            do
            {
                while (result.Read())
                {
                    var id = result.GetInt32(0);
                    var name = result.GetString(1);
                    Console.WriteLine($" {id} - {name}");
                }
            } while (result.NextResult());
            Console.WriteLine(" --------------------------------------------");

            if (result != null) result.Close();
            _connection.Close();
        }

        public void NumberPlantType()
        {
            var numberVegetables = 0;
            var numberFruits = 0;
            var numberBerries = 0;

            _connection.Open();
            _query.CommandText = "SELECT type FROM table_vegetables_fruits;";
            var result = _query.ExecuteReader();

            if (!result.HasRows)
            {
                Console.WriteLine("Нет данных");
                return;
            }

            var types = new List<string>();
            do
            {
                while (result.Read())
                {
                    var type = result.GetString(0);
                    types.Add(type);
                }
            } while (result.NextResult());

            if (result != null) result.Close();
            _connection.Close();

            foreach (var type in types)
            {
                if (type == "овощ") numberVegetables++;
                if (type == "фрукт") numberFruits++;
                if (type == "ягода") numberBerries++;
            }

            Console.WriteLine($"В базе данных {numberVegetables} овощей, {numberFruits} фруктов, {numberBerries} ягод");
            Console.WriteLine(" --------------------------------------------");
        }

        public List<string> AllColors()
        {
            _connection.Open();
            _query.CommandText = "SELECT color FROM table_vegetables_fruits;";
            var result = _query.ExecuteReader();

            if (!result.HasRows)
            {
                Console.WriteLine("Нет данных");
                return null;
            }

            Console.WriteLine("В базе данных есть следующие цвета");
            var colors = new List<string>();
            do
            {
                while (result.Read())
                {
                    bool selection = true;
                    var color = result.GetString(0);
                    if (colors != null)
                        foreach (var col in colors)
                        {
                            if (col == color) selection = false;
                            if (!selection) break;
                        }
                    if (selection) colors.Add(color);
                }
            } while (result.NextResult());

            if (result != null) result.Close();
            _connection.Close();
            return colors;
        }

        public void ShowAllColors()
        {
            /*_connection.Open();
            _query.CommandText = "SELECT color FROM table_vegetables_fruits;";            
            var result = _query.ExecuteReader();

            if (!result.HasRows)
            {
                Console.WriteLine("Нет данных");
                return;
            }

            Console.WriteLine("В базе данных есть следующие цвета");
            var colors = new List<string>();
            do 
            {                
                while (result.Read())
                {
                    bool selection = true;
                    var color = result.GetString(0);                    
                    if (colors != null)
                        foreach (var col in colors)
                        {
                            if (col == color) selection = false;
                            if (!selection) break;
                        }
                    if(selection)colors.Add(color);                
                }
            }  while (result.NextResult());*/

            var colors = this.AllColors();
            foreach (var color in colors) Console.WriteLine($" {color}");
            Console.WriteLine(" --------------------------------------------");



        }

        public void NumberPlantColor()
        {
            var colors = this.AllColors();
            _connection.Open();
            _query.CommandText = "SELECT color FROM table_vegetables_fruits;";
            var result = _query.ExecuteReader();

            if (!result.HasRows)
            {
                Console.WriteLine("Нет данных");
                return;
            }

            var allcolors = new List<string>();
            do
            {
                while (result.Read())
                {
                    var allcolor = result.GetString(0);
                    allcolors.Add(allcolor);
                }
            } while (result.NextResult());

            if (result != null) result.Close();
            _connection.Close();

            Console.WriteLine($"В базе данных плодов каждого цвета :");
            for (var i = 0; i < colors.Count(); i++)
            {
                var count = 0;
                foreach (var allcolor in allcolors)
                {
                    if (colors[i] == allcolor) count++;
                }
                Console.WriteLine($"{colors[i]} - {count}");
            }

            Console.WriteLine(" --------------------------------------------");
        }

        public void NumberPlantColor(string speccolor)
        {
            var count = 0;
            _connection.Open();
            _query.CommandText = "SELECT color FROM table_vegetables_fruits WHERE color={speccolor};";
            var result = _query.ExecuteReader();

            if (!result.HasRows)
            {
                Console.WriteLine("Нет данных");
                return;
            }

            do
            {
                while (result.Read())
                {
                    count++;
                }
            } while (result.NextResult());

            if (result != null) result.Close();
            _connection.Close();

            Console.WriteLine($"В базе данных плодов цвета {speccolor} - {count}");
            Console.WriteLine(" --------------------------------------------");
        }

        private List<string> ListCalories()
        {
            _connection.Open();
            _query.CommandText = "SELECT calories FROM table_vegetables_fruits;";
            var result = _query.ExecuteReader();

            if (!result.HasRows)
            {
                Console.WriteLine("Нет данных");
                return null;
            }

            var calories = new List<string>();
            do
            {
                while (result.Read())
                {
                    var cal = result.GetString(0);
                    calories.Add(cal);
                }
            } while (result.NextResult());

            if (result != null) result.Close();
            _connection.Close();
            return calories;
        }

        public void ShowMaxCalories()
        {
            uint max = 0;
            var calories = ListCalories();
            foreach (var cal in calories)
            {
                var c = (uint)Convert.ToInt32(cal);
                if (c > max) max = c;
            }

            Console.WriteLine($" Максимальная калорийность плодов - {max} калорий");
            _connection.Open();
            _query.CommandText = $"SELECT name FROM table_vegetables_fruits WHERE calories={max};";
            var result = _query.ExecuteReader();

            Console.Write(" Плоды с максимальной калорийностью в базе данных : ");
            do
            {
                while (result.Read())
                {
                    var name = result.GetString(0);
                    Console.Write($"{name}  ");
                }
            } while (result.NextResult());
            Console.WriteLine();
            Console.WriteLine(" --------------------------------------------");

            if (result != null) result.Close();
            _connection.Close();
        }

        public void ShowMinCalories()
        {
            uint min = 10000;
            var calories = ListCalories();
            foreach (var cal in calories)
            {
                var c = (uint)Convert.ToInt32(cal);
                if (c < min) min = c;
            }

            Console.WriteLine($" Минимальная калорийность плодов - {min} калорий");
            _connection.Open();
            _query.CommandText = $"SELECT name FROM table_vegetables_fruits WHERE calories={min};";
            var result = _query.ExecuteReader();

            Console.Write(" Плоды с минимальной калорийностью в базе данных : ");
            do
            {
                while (result.Read())
                {
                    var name = result.GetString(0);
                    Console.Write($"{name}  ");
                }
            } while (result.NextResult());
            Console.WriteLine();
            Console.WriteLine(" --------------------------------------------");

            if (result != null) result.Close();
            _connection.Close();
        }

        public void ShowMiddleCalories()
        {
            double middle = 0;
            int count = 0;
            var calories = ListCalories();
            foreach (var cal in calories)
            {
                var c = (uint)Convert.ToInt32(cal);
                middle += c;
                count++;
            }
            middle /= count;
            middle = Math.Round(middle, 0);
            Console.WriteLine($" Средняя арифметическая калорийность плодов - {middle} калорий");
            Console.WriteLine(" --------------------------------------------");
        }

        public void ShowRangCalories(int lower, int upper)
        {            
            _connection.Open();
            _query.CommandText = "SELECT name, calories FROM table_vegetables_fruits WHERE calories>{lower} AND calories<{upper};";
            var result = _query.ExecuteReader();

            if (!result.HasRows)
            {
                Console.WriteLine("Нет данных");
                return;
            }

            Console.WriteLine($"В базе данных плоды с калорийностью в диапазоне {lower} - {upper} :");
            do
            {
                while (result.Read())
                {
                    var name = result.GetString(0);
                    var calories = result.GetString(1);
                    Console.WriteLine($" {name} - {calories}");
                }
            } while (result.NextResult());

            if (result != null) result.Close();
            _connection.Close();

            Console.WriteLine(" --------------------------------------------");
        }
    }
}