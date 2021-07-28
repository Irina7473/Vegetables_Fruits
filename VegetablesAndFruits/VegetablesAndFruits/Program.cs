﻿using System;

namespace VegetablesAndFruits
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("  *** База данных овощей, фруктов, ягод ***");
            Console.WriteLine();
            var VF=new DB(@"G:\STEP\Vegetables_Fruits\vegetables_fruits_db.sqlite");
            VF.Open();            
            Console.WriteLine();
            VF.ShowAll();
            Console.WriteLine();
            VF.ShowAllNames();
            Console.WriteLine();
            //VF.ShowAllColors();
            Console.WriteLine();

        }
    }
}