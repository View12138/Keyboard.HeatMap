namespace DontTouchKeyboard.Core.Models;

public readonly struct DbDate
{
    public DbDate(int year, int month)
    {
        Year = year <= 0 ? throw new ArgumentException("错误的年份", nameof(year)) : year;
        Month = (month < 1 || month > 12) ? throw new ArgumentException("错误的月份", nameof(month)) : month;
    }

    public int Year { get; }
    public int Month { get; }

    public static DbDate Now => DateTime.Now;

    public override string ToString() => ((DateTime)this).ToString("yyyy-MM");
    public override bool Equals(object? obj) => obj is DbDate date && Year == date.Year && Month == date.Month;
    public override int GetHashCode() => HashCode.Combine(Year, Month);

    public static bool operator !=(DbDate d1, DbDate d2) => d1.Year != d2.Year || d1.Month != d2.Month;
    public static bool operator ==(DbDate d1, DbDate d2) => d1.Year == d2.Year && d1.Month == d2.Month;
    public static implicit operator DbDate(DateTime date) => new(date.Year, date.Month);
    public static explicit operator DateTime(DbDate date) => new(date.Year, date.Month, 1, 0, 0, 0);
}
