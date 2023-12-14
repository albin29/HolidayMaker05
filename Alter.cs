using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HolidayMaker05;

public class Alter
{
    private readonly NpgsqlDataSource _db;

    public Alter(NpgsqlDataSource db)
    {
        _db = db;
    }
    public async Task Open()
    {
        bool open = true;

        while (open)
        {
            Console.Clear();
            Console.WriteLine("What would you like to alter?");

            string pick = Console.ReadLine();

            switch (pick)
            {
                case "1": //function
                    Console.WriteLine(await Name());
                    break;
                case "0":
                    open = false;
                    break;
            }
        }
    }
    public async Task<string> Name()
    {
        const string query = @"
        UPDATE reservations
        SET room_id = < new_room_id >
        WHERE reservation_id = < existing_reservation_id >;";


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
