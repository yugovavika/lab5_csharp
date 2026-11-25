namespace Lab5_Variant16.Models;

internal class Album
{
    private int _id = 0;
    private string _name = string.Empty;
    private int _artistId = 0;

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

    public int ArtistId
    {
        get
        {
            return _artistId;
        }

        set
        {
            _artistId = Checked.Id(value);
        }
    }

    public Album(int id, string name, int artistId)
    {
        Id = id;
        Name = name;
        ArtistId = artistId;
    }

    public override string ToString()
    {
        return $"{Id}. {Name} (ID артиста: {ArtistId})";
    }
}