using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeYourBank
{
    public struct DateOnly
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public  static class   DateOnlyExtensions
    {
        public static DateOnly GetDateOnly(this DateTime dt)
        {
            return new DateOnly
            {
                Day = dt.Day,
                Month = dt.Month,
                Year = dt.Year
            };
        }
    }

    
}
