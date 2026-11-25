using Lab5_Variant16.Models;
using NPOI.SS.UserModel;

namespace Lab5_Variant16.Services;

internal class ExcelRepository
{
    public void Load(
        string path,
        List<Artist> artists,
        List<Album> albums,
        List<Track> tracks,
        List<Genre> genres)
    {
        Checked.FilePath(path);
        using (FileStream stream = File.OpenRead(path))
        {
            IWorkbook workbook =
                WorkbookFactory.Create(stream);

            ISheet sheet = Checked.Sheet(workbook, "Артисты");
            ReadArtists(sheet, artists);
            sheet = Checked.Sheet(workbook, "Альбомы");
            ReadAlbums(sheet, albums);
            sheet = Checked.Sheet(workbook, "Треки");
            ReadTracks(sheet, tracks);
            sheet = Checked.Sheet(workbook, "Жанры");
            ReadGenres(sheet, genres);
        }
    }

    public void Save(
        string sourcePath,
        string targetPath,
        List<Artist> artists,
        List<Album> albums,
        List<Track> tracks,
        List<Genre> genres)
    {
        Checked.FilePath(sourcePath);
        IWorkbook workbook = null!;
        using (FileStream input = File.OpenRead(sourcePath))
        {
            workbook = WorkbookFactory.Create(input);

            ISheet sheet = Checked.Sheet(workbook, "Артисты");
            WriteArtists(sheet, artists);
            sheet = Checked.Sheet(workbook, "Альбомы");
            WriteAlbums(sheet, albums);
            sheet = Checked.Sheet(workbook, "Треки");
            WriteTracks(sheet, tracks);
            sheet = Checked.Sheet(workbook, "Жанры");
            WriteGenres(sheet, genres);
        }

        using (FileStream output = File.Create(targetPath))
        {
            workbook.Write(output);
        }
    }

    private string ReadText(IRow row, int index)
    {
        return Checked.CellText(row, index);
    }

    private int ReadInt(IRow row, int index)
    {
        return Convert.ToInt32(
            Checked.CellNumber(row, index));
    }

    private long ReadLong(IRow row, int index)
    {
        return Convert.ToInt64(
            Checked.CellNumber(row, index));
    }

    private double ReadDouble(IRow row, int index)
    {
        return Math.Round(
            Checked.CellNumber(row, index), 2);
    }

    private void ReadArtists(
        ISheet sheet,
        List<Artist> artists)
    {
        var rows = Enumerable
            .Range(1, Math.Max(0, sheet.LastRowNum))
            .Select(index => sheet.GetRow(index))
            .Where(row => row != null);

        foreach (IRow row in rows)
        {
            artists.Add(new Artist(
                ReadInt(row, 0),
                ReadText(row, 1)));
        }
    }

    private void ReadAlbums(
        ISheet sheet,
        List<Album> albums)
    {
        var rows = Enumerable
            .Range(1, Math.Max(0, sheet.LastRowNum))
            .Select(index => sheet.GetRow(index))
            .Where(row => row != null);

        foreach (IRow row in rows)
        {
            albums.Add(new Album(
                ReadInt(row, 0),
                ReadText(row, 1),
                ReadInt(row, 2)));
        }
    }

    private void ReadTracks(
        ISheet sheet,
        List<Track> tracks)
    {
        var rows = Enumerable
            .Range(1, Math.Max(0, sheet.LastRowNum))
            .Select(index => sheet.GetRow(index))
            .Where(row => row != null);

        foreach (IRow row in rows)
        {
            tracks.Add(new Track(
                ReadInt(row, 0),
                ReadText(row, 1),
                ReadInt(row, 2),
                ReadInt(row, 3),
                ReadInt(row, 4),
                ReadLong(row, 5),
                ReadDouble(row, 6)));
        }
    }

    private void ReadGenres(
        ISheet sheet,
        List<Genre> genres)
    {
        var rows = Enumerable
            .Range(1, Math.Max(0, sheet.LastRowNum))
            .Select(index => sheet.GetRow(index))
            .Where(row => row != null);

        foreach (IRow row in rows)
        {
            genres.Add(new Genre(
                ReadInt(row, 0),
                ReadText(row, 1)));
        }
    }

    private void ClearRows(ISheet sheet)
    {
        int index = 0;
        IRow? row = null;

        for (index = sheet.LastRowNum; index >= 1; index--)
        {
            row = sheet.GetRow(index);
            if (row != null)
            {
                sheet.RemoveRow(row);
            }
        }
    }

    private void WriteArtists(
        ISheet sheet,
        List<Artist> artists)
    {
        ClearRows(sheet);
        int index = 0;
        IRow row = null!;

        for (index = 0; index < artists.Count; index++)
        {
            row = sheet.CreateRow(index + 1);
            row.CreateCell(0).SetCellValue(
                artists[index].Id);
            row.CreateCell(1).SetCellValue(
                artists[index].Name);
        }
    }

    private void WriteAlbums(
        ISheet sheet,
        List<Album> albums)
    {
        ClearRows(sheet);
        int index = 0;
        IRow row = null!;

        for (index = 0; index < albums.Count; index++)
        {
            row = sheet.CreateRow(index + 1);
            row.CreateCell(0).SetCellValue(
                albums[index].Id);
            row.CreateCell(1).SetCellValue(
                albums[index].Name);
            row.CreateCell(2).SetCellValue(
                albums[index].ArtistId);
        }
    }

    private void WriteTracks(
        ISheet sheet,
        List<Track> tracks)
    {
        ClearRows(sheet);
        int index = 0;
        IRow row = null!;

        for (index = 0; index < tracks.Count; index++)
        {
            row = sheet.CreateRow(index + 1);
            row.CreateCell(0).SetCellValue(
                tracks[index].Id);
            row.CreateCell(1).SetCellValue(
                tracks[index].Name);
            row.CreateCell(2).SetCellValue(
                tracks[index].AlbumId);
            row.CreateCell(3).SetCellValue(
                tracks[index].GenreId);
            row.CreateCell(4).SetCellValue(
                tracks[index].Duration);
            row.CreateCell(5).SetCellValue(
                tracks[index].Size);
            row.CreateCell(6).SetCellValue(
                tracks[index].Price);
        }
    }

    private void WriteGenres(
        ISheet sheet,
        List<Genre> genres)
    {
        ClearRows(sheet);
        int index = 0;
        IRow row = null!;

        for (index = 0; index < genres.Count; index++)
        {
            row = sheet.CreateRow(index + 1);
            row.CreateCell(0).SetCellValue(
                genres[index].Id);
            row.CreateCell(1).SetCellValue(
                genres[index].Name);
        }
    }
}