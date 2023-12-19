
namespace HolidayMaker05;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Npgsql;


public class Hotel

{
    private readonly NpgsqlDataSource _db;

    public Hotel(NpgsqlDataSource db )
     {


    _db=db;
      


      }
       
    public async Task Search()
    {
        string query = @"


        SELECT name, location
        FROM hotels
        WHERE hotel_id = 2;";

        
         


        string result = string.Empty;

        var reader = await _db.CreateCommand(query).ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            result += reader.GetString(0);
            result += ", ";
            result += reader.GetInt32(1);
            result += reader.GetString(2);
        }
        Console.WriteLine(result);
        
        Console.ReadKey();

    }


}   
       
   
