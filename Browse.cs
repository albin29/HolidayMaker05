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
        Console.WriteLine(result);
        Console.WriteLine("0 - return\nAny key - Exit");

        string? pick = Console.ReadLine();
        switch (pick)
        {
            case "0":
                await Hotel(";");
                break;
            case "7":
                await Rooms(hotelID, "ORDER BY rooms.price ASC");
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
                    await Hotel("\nORDER BY locations.distance_from_beach ASC");
                    break;
                case 7:
                    await Hotel("\nORDER BY locations.distance_from_central ASC");
                    break;
                case 8:
                    await Hotel("\nORDER BY hotels.rating DESC");
                    break;
                default:
                    await Rooms(intID, "");
                    break;
            }
        }
        else
        {
            return;
        }
    }
}
