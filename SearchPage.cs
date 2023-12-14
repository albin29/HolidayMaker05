using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace HolidayMaker05;

public class SearchPage
{
    private readonly NpgsqlDataSource _db;

    public SearchPage(NpgsqlDataSource db)
    {
        _db = db;
    }
    public async void Open()
    {
        bool open = true;
        while (open)
        {
            Console.Clear();
            Console.WriteLine("1 - See hotels locations");
            Console.WriteLine("2 - Sort after reviews");
            Console.WriteLine("Search with sort by price");

            Console.WriteLine("Search for available rooms");
            Console.WriteLine("Search for rooms with date");
            Console.WriteLine("Search for user credentials");

            Console.WriteLine("0 - Exit");

            string pick = Console.ReadLine();

            switch (pick)
            {
                case "1": Locations();
                    break;
                case "0": open = false;
                    break;

            }
        }
    }
    public async Task<string> Locations()
    {
        const string query = @"
        SELECT hotels.hotel_id, hotels.name, locations.name
        FROM hotels
        JOIN locations ON hotels.location_id = locations.location_id;";

        string result = string.Empty;

        var reader = await _db.CreateCommand(query).ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result += reader.GetInt32(0);
            result += ", ";
            result += reader.GetString(1);
        }
        return result;

    }
}
