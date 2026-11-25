namespace Lab5_Variant16.Models;

internal class Genre
{
    private int _id = 0;
    private string _name = string.Empty;

    public int Id
    {
        get
        {
            return _id;
        }

        set
        {
            _id = Checked.Id(value);
        }
    }

    public string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = Checked.Text(value);
        }
    }

    public Genre(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public override string ToString()
    {
        return $"{Id}. {Name}";
    }
}