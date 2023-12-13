using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolidayMaker05;
public class Admin
{
    Booking booking = new Booking();
    public void Menu()
    {
        bool menu = true;
        while (menu)
        {
            Console.Clear();
            Console.WriteLine("Welcome to our Holidaymaker");

            Console.WriteLine("1 - Booking");
            Console.WriteLine("2 - Alter room");
            Console.WriteLine("3 - Searches");

            Console.WriteLine("0 - Exit menu");


            string pick = Console.ReadLine();
            switch (pick)
            {
                case "1":
                    booking.Book();
                    break;
                case "2":
                    break;
                

                case "3":
                   
                    break;
                case "0":
                    menu = false;
                    break;
            }
        }
    }
}