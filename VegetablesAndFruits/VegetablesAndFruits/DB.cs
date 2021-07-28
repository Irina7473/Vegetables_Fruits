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
            _connection.Open();
            _query.CommandText = "SELECT * FROM table_vegetables_fruits;";
            var result = _query.ExecuteReader();

            if (!result.HasRows)
            {
                Console.WriteLine("Нет данных");
                return;
            }

            var plants = new List<Plant>();
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

            Console.WriteLine(" --------------------------------------------");
            Console.WriteLine(" № | Название |  Тип  |  Цвет  | Калорийность");
            Console.WriteLine(" --------------------------------------------");
            foreach (var plant in plants)
                Console.WriteLine($" {plant.Id} | {plant.Name} | {plant.Type} | {plant.Color} | {plant.Calories}");
            Console.WriteLine(" --------------------------------------------");

            _connection.Close();
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
            while (result.Read())
            {
                var id = result.GetInt32(0);
                var name = result.GetString(1);
                Console.WriteLine($" {id} - {name}");
            }
            Console.WriteLine(" --------------------------------------------");

            _connection.Close();
        }
        /*
        public void ShowAllColors()
        {
            _connection.Open();
            _query.CommandText = "SELECT color FROM table_vegetables_fruits;";
            
            var result = _query.ExecuteReader();

            if (!result.HasRows)
            {
                Console.WriteLine("Нет данных");
                return;
            }

            Console.WriteLine("В базе данных есть следующие цвета");
            var colors = new List<string>();
            while (result.Read())
            {                
                var color = result.GetString(3);
                foreach(var col in colors) if (color == col) return;
                colors.Add(color);                 
            }
            foreach (var color in colors) Console.WriteLine($" {color}");
            Console.WriteLine(" --------------------------------------------");

            _connection.Close();
        }*/
    }
}
