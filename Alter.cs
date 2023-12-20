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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        Console.WriteLine("1 - The date\n2 - The room\n3 - Delete reservation\n0 - Exit");
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
                await Delete(email);
                break;
            case "0":
                break;
        }
    }
    public async Task Delete(string email)
    {
        string qDelete = @"
        DELETE 
        FROM reservations
        WHERE email = @email;";

        await using var cmd = _db.CreateCommand(qDelete);
        cmd.Parameters.AddWithValue("email", email);
        await cmd.ExecuteNonQueryAsync();


        Console.Clear();
        Console.WriteLine($"Reservation with {email} is deleted");
        Console.ReadKey();

    }
    public async Task Date(string email)
    {
        Console.Clear();
        Console.WriteLine("Enter room ID");
        int? roomID = int.Parse(Console.ReadLine());
        Console.Clear();
        Console.WriteLine("What are the dates you would like to have instead?\nFormat XXXX/XX/XX");

        string qOccupied = @"
        SELECT starting_date, ending_date
        FROM reservations
        WHERE room_id = @roomID";

        string result = string.Empty;

        using var command = _db.CreateCommand(qOccupied);
        command.Parameters.AddWithValue("roomID", roomID);

        var reader = await command.ExecuteReaderAsync();
        Console.WriteLine();
        while (await reader.ReadAsync())
        {
            result += reader.GetDateTime(0);
            result += " - ";
            result += reader.GetDateTime(1);
            result += "\n";
        }

        Console.WriteLine(result);

        string? startingDate = Console.ReadLine();
        string? endingDate = Console.ReadLine();

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
        Console.WriteLine("Enter room ID");
        int? roomID = int.Parse(Console.ReadLine());
        Console.Clear();

        string qOccupied = @"
        SELECT starting_date, ending_date
        FROM reservations
        WHERE room_id = @roomID";

        string result = string.Empty;

        using var command = _db.CreateCommand(qOccupied);
        command.Parameters.AddWithValue("roomID", roomID);

        var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result += reader.GetDateTime(0);
            result += " - ";
            result += reader.GetDateTime(1);
            result += "\n";
        }
        await Preview(email);
        Console.WriteLine();
        Console.WriteLine(result);

        Console.WriteLine("Enter room ID");
        int room = int.Parse(Console.ReadLine());

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
}
