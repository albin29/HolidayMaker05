using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace HolidayMaker05;
public class Admin
{
    private readonly NpgsqlDataSource _db;
    Book book;
    Alter alter;
    Browse browse;

    public Admin(NpgsqlDataSource db)
    {
        _db = db;
        book = new Book(db);
        alter = new Alter(db);
        browse = new Browse(db);
    }
    public async Task Menu()
    {
        bool menu = true;
        while (menu)
        {
            Console.Clear();
            Console.WriteLine("Welcome to our Holidaymaker");

            Console.WriteLine("1 - Book");
            Console.WriteLine("2 - Alter");
            Console.WriteLine("3 - Browse\n");
            Console.WriteLine("0 - Exit menu");

            string? pick = Console.ReadLine();
            switch (pick)
            {
                case "1": await book.Open();
                    break;
                case "2": await alter.Reservation();
                    break;
                case "3": await browse.Hotel("");
                    break;
                case "0": menu = false;
                    break;
            }
        }
    }
}