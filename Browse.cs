using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace HolidayMaker05;

public class Browse
{
    private readonly NpgsqlDataSource _db;

    public Browse(NpgsqlDataSource db)
    {
        _db = db;
    }
    public async Task Rooms(int hotelID, string query)
    {
        Console.Clear();

        string qRooms = $@"
        SELECT rooms.room_id, rooms.number, rooms.beds, price, room_addons.balcony, room_addons.ac,
        room_addons.jacuzzi, room_addons.smart_tv
        FROM rooms
        LEFT JOIN room_addons ON rooms.room_addon_id = room_addons.room_addon_id
        WHERE rooms.hotel_id = @hotelid{query};";
        string result = string.Empty;

        using var command = _db.CreateCommand(qRooms);
        command.Parameters.AddWithValue("hotelid", hotelID);

        var reader = await command.ExecuteReaderAsync();
        string header = "ID Number Beds Price    Blcny AC  Jacuz SmartTV";
        string divider = new string('-', header.Length);

        while (await reader.ReadAsync())
        {
            result += reader.GetInt32(0).ToString("").PadRight(2);
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

        Console.WriteLine($"{divider}\n{header}\n{divider}");

        Console.WriteLine(result);
        Console.WriteLine("1 - Order by price\n\n0 - return\nAny other key - Exit");

        string? pick = Console.ReadLine();
        switch (pick)
        {
            case "0":
                await Hotel("");
                break;
            case "1":
                await Rooms(hotelID, "\nORDER BY rooms.price ASC");
                break;
            default: return;
        }
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

        string header = " Rating  Name       dBeach    dCentral  LiveP Pool  cClub Diner Address";
        string divider = new string('-', header.Length);

        while (await reader.ReadAsync())
        {
            result += "[";
            result += reader.GetInt32(0);
            result += "] ";
            result += reader.GetInt32(1);
            result += "  ";
            result += reader.GetString(2).PadRight(11);
            result += reader.GetFloat(3).ToString("F1").PadLeft(6);
            result += "KM   ";
            result += reader.GetFloat(4).ToString("F1").PadLeft(6);
            result += "KM   ";
            result += reader.GetBoolean(5) ? "True " : "False";
            result += " ";
            result += reader.GetBoolean(6) ? "True " : "False";
            result += " ";
            result += reader.GetBoolean(7) ? "True " : "False";
            result += " ";
            result += reader.GetBoolean(8) ? "True " : "False";
            result += " ";
            result += reader.GetString(9);
            result += "\n";
        }

        Console.WriteLine("Which hotel would you like to explore?\n");
        Console.WriteLine($"{divider}\n{header}\n{divider}");
        Console.WriteLine(result);
        Console.WriteLine("6 - Sort by distance to beach\n7 - Sort by distance to central\n8 - Sort by rating\n\nAny other key - Return");

        int intID;
        string? stringID = Console.ReadLine();

        if (int.TryParse(stringID, out intID))
        {
            switch (intID)
            {
                case 1: await Rooms(intID, "");
                    break;
                case 2: await Rooms(intID, "");
                    break;
                case 3: await Rooms(intID, "");
                    break;
                case 4: await Rooms(intID, "");
                    break;
                case 5: await Rooms(intID, "");
                    break;
                case 6:
                    await Hotel("\nORDER BY locations.distance_from_beach ASC");
                    break;
                case 7:
                    await Hotel("\nORDER BY locations.distance_from_central ASC");
                    break;
                case 8:
                    await Hotel("\nORDER BY hotels.rating DESC");
                    break;
                default:
                    return;
            }
        }
        else
        {
            return;
        }
    }
}
