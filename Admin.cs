using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace HolidayMaker05;
public class Admin
{
    RegisterUser registeruser;
    private readonly NpgsqlDataSource _db;
    Book book;
    Alter alter;
    SearchPage searchPage;

    public Admin(NpgsqlDataSource db)
    {
        _db = db;
        book = new Book(db);
        alter = new Alter(db);
        searchPage = new SearchPage(db);
        registeruser = new RegisterUser(db);
    }
    public async Task Menu()
    {
        bool menu = true;
        while (menu)
        {
            Console.Clear();
            Console.WriteLine("Welcome to our Holidaymaker");

            Console.WriteLine("1 - Booking");
            Console.WriteLine("2 - Alter room");
            Console.WriteLine("3 - Searches");
            Console.WriteLine("4 - Register user");
            Console.WriteLine("0 - Exit menu");

            string pick = Console.ReadLine();
            switch (pick)
            {
                case "1": book.Open();
                    break;
                case "2": await alter.Open();
                    break;
                case "3": searchPage.Open();
                    break;
                case "4":
                    await registeruser.RegisterMenu();


                    break;
                case "0": menu = false;
                    break;
            }
        }
    }
}