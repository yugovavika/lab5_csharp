namespace Lab5_Variant16.Models;

internal class Track
{
    private int _id = 0;
    private string _name = string.Empty;
    private int _albumId = 0;
    private int _genreId = 0;
    private int _duration = 0;
    private long _size = 0;
    private double _price = 0;

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

    public int AlbumId
    {
        get
        {
            return _albumId;
        }

        set
        {
            _albumId = Checked.Id(value);
        }
    }

    public int GenreId
    {
        get
        {
            return _genreId;
        }

        set
        {
            _genreId = Checked.Id(value);
        }
    }

    public int Duration
    {
        get
        {
            return _duration;
        }

        set
        {
            _duration = Checked.Duration(value);
        }
    }

    public long Size
    {
        get
        {
            return _size;
        }

        set
        {
            _size = Checked.Size(value);
        }
    }

    public double Price
    {
        get
        {
            return _price;
        }

        set
        {
            _price = Checked.Price(value);
        }
    }

    public Track(
        int id,
        string name,
        int albumId,
        int genreId,
        int duration,
        long size,
        double price)
    {
        Id = id;
        Name = name;
        AlbumId = albumId;
        GenreId = genreId;
        Duration = duration;
        Size = size;
        Price = price;
    }

    public override string ToString()
    {
        return $"{Id}. {Name}; альбом: {AlbumId}; " +
            $"жанр: {GenreId}; длит.: {Duration} мс; " +
            $"размер: {Size}; стоимость: {Price:F2}";
    }
}