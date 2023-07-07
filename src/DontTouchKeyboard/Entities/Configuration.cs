using System.ComponentModel.DataAnnotations;

namespace DontTouchKeyboard.Entities;

internal class Configuration
{
    public Configuration(string name, string value)
    {
        Name = name;
        Value = value;
    }

    [Key]
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Value { get; set; }

    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Updated { get; set; }
}
