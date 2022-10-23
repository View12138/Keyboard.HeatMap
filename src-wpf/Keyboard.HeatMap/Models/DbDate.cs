using System;

namespace Keyboard.HeatMap.Models
{
    public struct DbDate
    {
        private int year;
        private int month;
        public DbDate(int year, int month)
        {
            this.year = year;
            this.month = month;
        }
        public int Year
        {
            get => year; set
            {
                if (value <= 0)
                { throw new ArgumentException("错误的年份", nameof(Year)); }
                year = value;
            }
        }
        public int Month
        {
            get => month; set
            {
                if (value < 1 || value > 12)
                { throw new ArgumentException("错误的月份", nameof(Month)); }
                month = value;
            }
        }
        public static DbDate Now
        {
            get
            {
                var now = DateTime.Now;
                return new DbDate(now.Year, now.Month);
            }
        }

        public override string ToString()
        {
            return ((DateTime)this).ToString("yyyy-MM");
        }

        public override bool Equals(object obj)
        {
            return obj is DbDate date &&
                   year == date.year &&
                   month == date.month;
        }

        public override int GetHashCode()
        {
            int hashCode = -341903022;
            hashCode = hashCode * -1521134295 + year.GetHashCode();
            hashCode = hashCode * -1521134295 + month.GetHashCode();
            return hashCode;
        }

        public static bool operator !=(DbDate d1, DbDate d2)
            => d1.year != d2.year || d1.month != d2.month;
        public static bool operator ==(DbDate d1, DbDate d2)
            => d1.year == d2.year && d1.month == d2.month;

        public static implicit operator DbDate(DateTime date)
            => new DbDate(date.Year, date.Month);

        public static explicit operator DateTime(DbDate date)
            => new DateTime(date.year, date.month, 1, 0, 0, 0);
    }
}
