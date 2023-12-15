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
    public void Open()
    {
        bool exit = false;
        while (exit)
        {
            Console.Clear();
            Console.WriteLine("What would you like to book?");

            Console.WriteLine("1 - Make a full reservation");
            Console.WriteLine("2 - Book a customer by name and room id");
            Console.WriteLine("3 - Unknown");

            Console.WriteLine("0 - Exit");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("");
                    Reservation();
                    break;
                case "2":
                    Book_Customer();
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

    public void Book_Customer()
    {
        Console.WriteLine("What is the customers full name?");
        string full_name = Console.ReadLine();
        Console.WriteLine("What is the userEmail?");
        string email = Console.ReadLine();
        Console.WriteLine("What room to be reserved?");
        string room_id = Console.ReadLine();
        Console.WriteLine("What is the startingDate ?");
        string starting_date = Console.ReadLine();
        Console.WriteLine("What is the endingDate ?");
        string ending_date = Console.ReadLine();
    }

    public async Task RegisterReservation(string room_id, string full_name, string email, string starting_date, string ending_date)
    {
        string insertQuery = @"
        INSERT INTO reservations (room_id, full_name, starting_date, ending_date)
        VALUES (@room_id, @full_name, @email, @starting_date, @ending_date)";

        await using var cmd = _db.CreateCommand(insertQuery);

        cmd.Parameters.AddWithValue("room_id", room_id);
        cmd.Parameters.AddWithValue("full_name", full_name);
        cmd.Parameters.AddWithValue("email", email);
        cmd.Parameters.AddWithValue("starting_date", starting_date);
        cmd.Parameters.AddWithValue("starting_date", ending_date);

        await cmd.ExecuteNonQueryAsync();
    }

    public void Reservation() {
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
