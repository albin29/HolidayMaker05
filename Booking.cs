using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace HolidayMaker05;

public class Booking
{
    public void Book()
    {
        bool exit = false;
        while (exit)
        {
            Console.WriteLine("What would you like to book?");

            Console.WriteLine("1 - Make a full reservation");
            Console.WriteLine("2 - Unknown");
            Console.WriteLine("3 - Unknown");

            Console.WriteLine("0 - Exit");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("");
                    break;
                case "2":
                    Console.WriteLine("");
                    break;
                case "3":
                    Console.WriteLine();
                    break;
                case "4":
                    exit = true;
                    break;
                    



            }






        }







    }





}
