using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace HolidayMaker05;

public class Book
{
    private readonly NpgsqlDataSource _db;
    public Book(NpgsqlDataSource db)
    {
        _db = db;
    }
    public async Task Open()
    {
        bool exit = false;
        while (exit)
        {
            Console.Clear();
            Console.WriteLine("What would you like to book?");

            Console.WriteLine("1 - Make a full reservation");
            Console.WriteLine("2 - Unknown");
            Console.WriteLine("3 - Unknown");

            Console.WriteLine("0 - Exit");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("");
                    Reservation();
                    break;
                case "2":
                    Console.WriteLine("");
                    break;
                case "3":
                    Console.WriteLine();
                    break;
                case "4":
                    exit = true;
                    break;
            }

        }

    }
    public async Task Reservation() {
        Console.WriteLine("What is the customers information?");
        Console.WriteLine("Which days do you want to reserve?");
        // Visa alla rum som är tillgängliga
        Console.WriteLine("Which hotel would you like to stay at?");
        //Print all hotels
        //customer väljer ett hotell
        Console.WriteLine("How many beds would you like?");
        //show all rooms with amounts of bed
         
        Console.WriteLine("Would you like to have any addons?");
        // visa alla rum med kundens filter
        
        // boka in








    }





}
