using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayMaker05;

public class RegisterUser
{
    private readonly NpgsqlDataSource _db;

    public RegisterUser()
    {
    }

    public RegisterUser(NpgsqlDataSource db)
    {
        _db = db;
    }
    public async Task RegisterMenu()
    {
        Console.WriteLine("What is the user name?");
        string name = Console.ReadLine();
        Console.WriteLine("What is the user last name");
        string lastName = Console.ReadLine();
        Console.WriteLine("What is the userEmail?");
        string email = Console.ReadLine();
        Console.WriteLine("What is the user phone number?");
        string phoneNumber = Console.ReadLine();
        await RegisterUserAsync(name, lastName, email, phoneNumber);


    }

    public async Task RegisterUserAsync(string name, string lastName, string email, string phoneNumber)
    {
        string insertQuery = @"
            INSERT INTO guests (name, last_name, email, phone_number)
            VALUES (@name, @last_name, @email, @phone_number)";

        await using var cmd = _db.CreateCommand(insertQuery);

        cmd.Parameters.AddWithValue("name", name);
        cmd.Parameters.AddWithValue("last_name", lastName);
        cmd.Parameters.AddWithValue("email", email);
        cmd.Parameters.AddWithValue("phone_number", phoneNumber);

        await cmd.ExecuteNonQueryAsync();
    }


}
