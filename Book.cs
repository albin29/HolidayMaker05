using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
    public async Task RegisterReservation(string room_id, string full_name, string email, string starting_date, string ending_date)
    {
        string insertQuery = @"
        INSERT INTO reservations (room_id, full_name,email, starting_date, ending_date)
        VALUES (@room_id, @full_name, @email, @starting_date, @ending_date)";

        await using var cmd = _db.CreateCommand(insertQuery);

        cmd.Parameters.AddWithValue("room_id", Convert.ToInt32(room_id));
        cmd.Parameters.AddWithValue("full_name", full_name);
        cmd.Parameters.AddWithValue("email", email);
        cmd.Parameters.AddWithValue("starting_date", DateTime.Parse(starting_date));
        cmd.Parameters.AddWithValue("ending_date", DateTime.Parse(ending_date));
        await cmd.ExecuteNonQueryAsync();
    }
    public async Task Reservation()
    {
        bool reservation = true;
        while (reservation)
        {
            Console.Clear();
            Console.WriteLine("What is the customers full name?");
            string? fullName = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("What is your email?");
            string? email = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Which hotel would you like to stay at?");
            string? hotelID = Console.ReadLine();
            if (Convert.ToInt32(hotelID) > 5 || Convert.ToInt32(hotelID) < 0)
            {
                Console.WriteLine("Invalid hotel ID");
                Console.ReadKey();
                break;
            }
            Console.Clear();
            Console.WriteLine("Which dates would you like to stay? [xxxx/xx/xx]");
            string? startingDate = Console.ReadLine();
            string? endingDate = Console.ReadLine();
            await ReserveQueries(startingDate, endingDate, hotelID);
            Console.WriteLine("Enter room ID");
            string? roomID = Console.ReadLine();
            string query = $"select room_id from rooms where room_id = {roomID} and hotel_id = {hotelID}";
            var cmd = _db.CreateCommand(query);
            cmd.Parameters.AddWithValue(int.Parse(roomID));
            cmd.Parameters.AddWithValue(int.Parse(hotelID));
            var count = await cmd.ExecuteReaderAsync();
            while (await count.ReadAsync())
            {
                if (count.GetInt32(0) == int.Parse(roomID))
                {
                    await RegisterReservation(roomID, fullName, email, startingDate, endingDate);
                    Console.WriteLine("Your reservation was successful! ");
                    Console.ReadKey();
                    reservation = false;
                }
            }
            if (reservation)
            {
                Console.WriteLine("Invalid combo of room and hotel id");
                Console.ReadKey();
            }
        }
    }
    public async Task ReserveQueries(string startingDate, string endingDate, string hotelID)
    {
        string insertQuery = $@"
        SELECT r.room_id, r.number, r.beds, r.price, ra.balcony, ra.ac, ra.jacuzzi, ra.smart_tv
        FROM rooms r
        LEFT JOIN room_addons ra ON r.room_addon_id = ra.room_addon_id
        WHERE r.hotel_ID = {hotelID}  and r.room_id NOT IN (
        SELECT res.room_id
        FROM reservations res
        WHERE res.starting_date BETWEEN '{startingDate}'::date AND '{endingDate}'::date
        OR res.ending_date BETWEEN '{startingDate}'::date AND '{endingDate}'::date);";
        await using var cmd = _db.CreateCommand(insertQuery);
        var reader = await cmd.ExecuteReaderAsync();
        string result = string.Empty;
        string header = "RoomID Number Beds Price Balcony AC Jacuzzi SmartTV";
        string divider = new string('-', header.Length);
        while (await reader.ReadAsync())
        {
            result += "[";
            result += reader.GetInt32(0).ToString("").PadRight(2);
            result += " ]  ";
            result += reader.GetInt32(1).ToString("").PadLeft(2);
            result += " ";
            result += reader.GetInt32(2).ToString("").PadLeft(4);
            result += "   ";
            result += reader.GetInt32(3).ToString("").PadLeft(4);
            result += "    ";
            result += reader.GetBoolean(4) ? "True " : "False";
            result += " ";
            result += reader.GetBoolean(5) ? "True " : "False";
            result += " ";
            result += reader.GetBoolean(6) ? "True " : "False";
            result += " ";
            result += reader.GetBoolean(7) ? "True " : "False";
            result += "\n";
            // 4,5
        }
        Console.WriteLine($"{divider}\n{header}\n{divider}");
        Console.WriteLine(result);
    }
}
