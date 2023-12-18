using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HolidayMaker05;

public class Alter
{
    private readonly NpgsqlDataSource _db;

    public Alter(NpgsqlDataSource db)
    {
        _db = db;
    }
    public async Task Reservation()
    {
        Console.Clear();
        Console.WriteLine("Enter Email\n");

        string? email = Console.ReadLine();
        Console.Clear();

        if (!await Preview(email))
        {
            return;
        }
        Console.WriteLine("What about your reservation would you like to alter?");
        Console.WriteLine("1 - The date\n2 - The room\n3 - Browse\n\n0 - Exit");
        string orderBy = string.Empty;
        string? pick = Console.ReadLine();

        switch (pick)
        {
            case "1":
                await Date(email);
                break;
            case "2":
                await Room(email);
                break;
            case "3":
                await Hotel(orderBy);
                break;
            case "0":
                break;
        }
        //DELETE FROM Customers WHERE CustomerName='Alfreds Futterkiste';
    }
    public async Task Date(string email)
    {
        Console.Clear();
        Console.WriteLine("What are the dates you would like to have instead?\nFormat XXXX/XX/XX");

        string startingDate = Console.ReadLine();
        string endingDate = Console.ReadLine();

        string qDate = @"
        UPDATE reservations
        SET starting_date = @startingDate
        WHERE email = @email;
        UPDATE reservations
        SET ending_date = @endingDate
        WHERE email = @email;";

        await using var cmd = _db.CreateCommand(qDate);
        cmd.Parameters.AddWithValue("startingDate", DateTime.Parse(startingDate));
        cmd.Parameters.AddWithValue("endingDate", DateTime.Parse(endingDate));
        cmd.Parameters.AddWithValue("email", email);
        await cmd.ExecuteNonQueryAsync();
    }
    public async Task Room(string email)
    {
        Console.Clear();
        Console.WriteLine("What room did you want instead?");

        int room = Convert.ToInt32(Console.ReadLine());

        string qRoom = @"
        UPDATE reservations
        SET room_id = @room
        WHERE email = @email;";

        await using var cmd = _db.CreateCommand(qRoom);

        cmd.Parameters.AddWithValue("room", room);
        cmd.Parameters.AddWithValue("email", email);

        await cmd.ExecuteNonQueryAsync();

        Console.Clear();
        await Preview(email);
        Console.ReadKey();

    }
    public async Task<bool> Preview(string email)
    {

        string qEmail = @"
        SELECT room_id, full_name, email, starting_date, ending_date
        FROM reservations
        WHERE email = @email;";

        string result = string.Empty;

        using var command = _db.CreateCommand(qEmail);
        command.Parameters.AddWithValue("Email", email);

        var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result += reader.GetInt32(0);
            result += ", ";
            result += reader.GetString(1);
            result += ", ";
            result += reader.GetString(2);
            result += ", ";
            result += reader.GetDateTime(3);
            result += ", ";
            result += reader.GetDateTime(4);
            result += "\n";
        }

        if (result == string.Empty)
        {
            await Console.Out.WriteLineAsync("Sorry couldn't find a match");
            Console.ReadKey();
            return false;
        }
        Console.WriteLine(result);
        return true;
    }
    public async Task Rooms(int hotelID)
    {
        Console.Clear();

        string qRooms = @"
        SELECT rooms.room_id, rooms.number, rooms.beds, price, room_addons.balcony, room_addons.ac,
        room_addons.jacuzzi, room_addons.smart_tv
        FROM rooms
        LEFT JOIN room_addons ON rooms.room_addon_id = room_addons.room_addon_id
        WHERE rooms.hotel_id = @hotelid;";
        string result = string.Empty;

        using var command = _db.CreateCommand(qRooms);
        command.Parameters.AddWithValue("hotelid", hotelID);

        var reader = await command.ExecuteReaderAsync();

        Console.WriteLine("ID Number Beds Price Balcony AC Jacuzzi SmartTV");
        while (await reader.ReadAsync())
        {
            result += reader.GetInt32(0);
            result += " | ";
            result += reader.GetInt32(1);
            result += " | ";
            result += reader.GetInt32(2);
            result += " | ";
            result += reader.GetInt32(3);
            result += "      ";
            result += reader.GetBoolean(4);
            result += " ";
            result += reader.GetBoolean(5);
            result += " ";
            result += reader.GetBoolean(6);
            result += " ";
            result += reader.GetBoolean(7);
            result += "\n";
        }

        if (result == string.Empty)
        {
            await Console.Out.WriteLineAsync("Sorry couldn't find a match");
            Console.ReadKey();
        }
        Console.WriteLine(result);
        Console.ReadKey();
    }

    public async Task Hotel(string orderBy)
    {
        Console.Clear();

        string qHotels = $@"
        SELECT hotels.hotel_id, hotels.rating, hotels.name, locations.distance_from_central,
        locations.distance_from_beach, hotel_addons.live_performance, 
        hotel_addons.pool, hotel_addons.childrens_club, hotel_addons.restaurant, locations.address
        FROM hotels
        LEFT JOIN locations ON locations.location_id = hotels.location_id
        LEFT JOIN hotel_addons ON hotel_addons.hotel_addon_id = hotels.hotel_addon_id{orderBy};";

        string result = string.Empty;

        using var command = _db.CreateCommand(qHotels);

        var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result += "[";
            result += reader.GetInt32(0);
            result += "] ";
            result += reader.GetInt32(1);
            result += ", ";
            result += reader.GetString(2);
            result += ", ";
            result += reader.GetFloat(3);
            result += "KM, ";
            result += reader.GetFloat(4);
            result += "KM, ";
            result += reader.GetBoolean(5);
            result += ", ";
            result += reader.GetBoolean(6);
            result += ", ";
            result += reader.GetBoolean(7);
            result += ", ";
            result += reader.GetBoolean(8);
            result += ", ";
            result += reader.GetString(9);
            result += "\n";
        }

        Console.WriteLine("Which hotel would you like to explore?");
        Console.WriteLine("Rating Name dBeach dCentral LiveP Pool ChildrensC Restaurant Address");
        Console.WriteLine(result);
        Console.WriteLine("6 - Sort by distance to beach\n7 - Sort by distance to central\n8 - Sort by rating\n\nAny other key - Return");

        int intID;
        string? stringID = Console.ReadLine();

        if (int.TryParse(stringID, out intID))
        {
            switch (intID)
            {
                case 6:
                    orderBy = "\nORDER BY locations.distance_from_beach ASC"; await Hotel(orderBy);
                    break;
                case 7:
                    orderBy = "\nORDER BY locations.distance_from_central ASC"; await Hotel(orderBy);
                    break;
                case 8:
                    orderBy = "\nORDER BY hotels.rating DESC"; await Hotel(orderBy);
                    break;
                default:
                    await Rooms(intID);
                    break;
            }
        }
        else
        {
            return;
        }
    }
}
