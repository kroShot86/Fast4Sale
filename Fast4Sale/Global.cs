using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fast4Sale
{
    public static class Global
    {
        // Глобальные переменные
        public static int ID = -1;

        //Данные с последнего объявления
        public static string Name;
        public static string Address;
        public static string Description;
        public static string Area;
        public static string Rooms;
        public static string Floor;
        public static string TotalFloors;
        public static string Price;
        public static string Contact;
        public static string PhoneNumber;
        public static string Email;

        public static bool Check;


        public static void Clear()
        {
            Name = "";
            Address = "";
            Description = "";
            Area = "";
            Rooms = "";
            Floor = "";
            TotalFloors = "";
            Price = "";
            Contact = "";
            PhoneNumber = "";
            Email = "";
        }

    }
}
