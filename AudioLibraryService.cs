using Lab5_Variant16.Models;

namespace Lab5_Variant16.Services;

internal class AudioLibraryService
{
    private const int PageSize = 20;

    private List<Artist> _artists = new List<Artist>();
    private List<Album> _albums = new List<Album>();
    private List<Track> _tracks = new List<Track>();
    private List<Genre> _genres = new List<Genre>();

    public List<Artist> Artists
    {
        get
        {
            return _artists;
        }
    }

    public List<Album> Albums
    {
        get
        {
            return _albums;
        }
    }

    public List<Track> Tracks
    {
        get
        {
            return _tracks;
        }
    }

    public List<Genre> Genres
    {
        get
        {
            return _genres;
        }
    }

    public void Clear()
    {
        Artists.Clear();
        Albums.Clear();
        Tracks.Clear();
        Genres.Clear();
    }

    private void ShowRows(List<string> rows)
    {
        int count = 0;
        string answer = string.Empty;

        foreach (string row in rows)
        {
            Console.WriteLine(row);
            count++;

            if (count % PageSize == 0 && count < rows.Count)
            {
                Console.Write("Enter — далее, q — стоп: ");
                answer = Console.ReadLine() ?? string.Empty;
                if (answer == "q")
                {
                    break;
                }
            }
        }

        Console.WriteLine($"Всего записей: {rows.Count}");
    }

    public void ShowArtists()
    {
        var rows = Artists
            .OrderBy(artist => artist.Id)
            .Select(artist => artist.ToString())
            .ToList();

        ShowRows(rows);
    }

    public void ShowAlbums()
    {
        var rows = Albums
            .OrderBy(album => album.Id)
            .Select(album => album.ToString())
            .ToList();

        ShowRows(rows);
    }

    public void ShowTracks()
    {
        var rows = Tracks
            .OrderBy(track => track.Id)
            .Select(track => track.ToString())
            .ToList();

        ShowRows(rows);
    }

    public void ShowGenres()
    {
        var rows = Genres
            .OrderBy(genre => genre.Id)
            .Select(genre => genre.ToString())
            .ToList();

        ShowRows(rows);
    }

    public int NextArtistId()
    {
        var codes = Artists.Select(artist => artist.Id);
        return codes.DefaultIfEmpty(0).Max() + 1;
    }

    public int NextAlbumId()
    {
        var codes = Albums.Select(album => album.Id);
        return codes.DefaultIfEmpty(0).Max() + 1;
    }

    public int NextTrackId()
    {
        var codes = Tracks.Select(track => track.Id);
        return codes.DefaultIfEmpty(0).Max() + 1;
    }

    public int NextGenreId()
    {
        var codes = Genres.Select(genre => genre.Id);
        return codes.DefaultIfEmpty(0).Max() + 1;
    }

    public bool ArtistExists(int id)
    {
        return Artists.Any(artist => artist.Id == id);
    }

    public bool AlbumExists(int id)
    {
        return Albums.Any(album => album.Id == id);
    }

    public bool GenreExists(int id)
    {
        return Genres.Any(genre => genre.Id == id);
    }

    public bool GenreNameExists(string name)
    {
        string lower = name.ToLower();
        return Genres.Any(
            genre => genre.Name.ToLower() == lower);
    }

    public bool HasAlbums(int artistId)
    {
        return Albums.Any(
            album => album.ArtistId == artistId);
    }

    public bool HasTracksForAlbum(int albumId)
    {
        return Tracks.Any(track => track.AlbumId == albumId);
    }

    public bool HasTracksForGenre(int genreId)
    {
        return Tracks.Any(track => track.GenreId == genreId);
    }

    public bool AddArtist(string name)
    {
        if (!Checked.HasText(name))
        {
            return false;
        }

        Artists.Add(new Artist(NextArtistId(), name));
        return true;
    }

    public bool AddAlbum(string name, int artistId)
    {
        if (!Checked.HasText(name))
        {
            return false;
        }

        if (!ArtistExists(artistId))
        {
            return false;
        }

        Albums.Add(new Album(
            NextAlbumId(), name, artistId));
        return true;
    }

    public bool AddGenre(string name)
    {
        if (!Checked.HasText(name))
        {
            return false;
        }

        Genres.Add(new Genre(NextGenreId(), name));
        return true;
    }

    public bool AddTrack(
        string name,
        int albumId,
        int genreId,
        int duration,
        long size,
        double price)
    {
        if (!Checked.HasText(name) ||
            !AlbumExists(albumId) ||
            !GenreExists(genreId) ||
            !Checked.IsPositive(duration) ||
            !Checked.IsPositive(size) ||
            !Checked.IsNotNegative(price))
        {
            return false;
        }

        Tracks.Add(new Track(
            NextTrackId(),
            name,
            albumId,
            genreId,
            duration,
            size,
            price));
        return true;
    }

    public bool DeleteArtist(int id)
    {
        if (HasAlbums(id))
        {
            return false;
        }

        return Artists.RemoveAll(
            artist => artist.Id == id) > 0;
    }

    public bool DeleteAlbum(int id)
    {
        if (HasTracksForAlbum(id))
        {
            return false;
        }

        return Albums.RemoveAll(album => album.Id == id) > 0;
    }

    public bool DeleteTrack(int id)
    {
        return Tracks.RemoveAll(track => track.Id == id) > 0;
    }

    public bool DeleteGenre(int id)
    {
        if (HasTracksForGenre(id))
        {
            return false;
        }

        return Genres.RemoveAll(genre => genre.Id == id) > 0;
    }

    public void QueryOneTable(double minPrice)
    {
        var query =
            from track in Tracks
            where track.Price >= minPrice
            orderby track.Price descending
            select track.ToString();

        Console.WriteLine(
            $"\nКомпозиции стоимостью от {minPrice:F2} руб.:");
        ShowRows(query.ToList());
    }

    public void QueryTwoTables()
    {
        var query =
            from album in Albums
            join artist in Artists
                on album.ArtistId equals artist.Id
            orderby artist.Name, album.Name
            select artist.Name + " — " + album.Name;

        Console.WriteLine("\nАльбомы и исполнители:");
        ShowRows(query.ToList());
    }

    public void QueryThreeTablesCount(string genreName)
    {
        var query =
            from track in Tracks
            join album in Albums
                on track.AlbumId equals album.Id
            join genre in Genres
                on track.GenreId equals genre.Id
            where genre.Name.ToLower() == genreName.ToLower()
            select track.Id;

        int count = query.Count();
        Console.WriteLine(
            $"Композиций жанра {genreName}: {count}");
    }

    public void QueryThreeTablesValue(string genreName)
    {
        var query =
            from track in Tracks
            join album in Albums
                on track.AlbumId equals album.Id
            join genre in Genres
                on track.GenreId equals genre.Id
            where genre.Name.ToLower() == genreName.ToLower()
            group track by album.ArtistId into artistGroup
            let totalSize =
                (from item in artistGroup
                 select item.Size).Sum()
            orderby totalSize
            select new
            {
                ArtistId = artistGroup.Key,
                TotalSize = totalSize
            };

        var best = query.FirstOrDefault();
        Console.WriteLine(
            $"\nМинимальный размер песен жанра {genreName}:");

        if (best == null)
        {
            Console.WriteLine("Композиции не найдены.");
            return;
        }

        long megabytes = best.TotalSize / 1024 / 1024;
        Console.WriteLine(
            $"Код исполнителя: {best.ArtistId}");
        Console.WriteLine($"Ответ: {megabytes} МБ");
    }
}