using System.Globalization;
using Lab5_Variant16.Services;

namespace Lab5_Variant16;

internal class Program
{
    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        CultureInfo.CurrentCulture =
            CultureInfo.GetCultureInfo("ru-RU");

        string filePath = Path.Combine(
            AppContext.BaseDirectory, "LR5-var16.xls");
        string savePath = Path.Combine(
            AppContext.BaseDirectory, "LR5-var16-out.xls");
        string genreName = string.Empty;
        double minPrice = 0;
        int choice = 0;
        bool isRunning = true;
        bool isLoaded = false;
        bool isModified = false;

        AudioLibraryService library =
            new AudioLibraryService();
        ExcelRepository repository =
            new ExcelRepository();

        while (isRunning)
        {
            ShowMenu(isLoaded, isModified);
            choice = Checked.ReadInt("Ваш выбор: ", 0, 9);

            try
            {
                switch (choice)
                {
                    case 1:
                        filePath = Checked.ReadPath(
                            "Файл Excel ", filePath);
                        library.Clear();
                        repository.Load(
                            filePath,
                            library.Artists,
                            library.Albums,
                            library.Tracks,
                            library.Genres);
                        isLoaded = true;
                        isModified = false;
                        Console.WriteLine("База загружена.");
                        break;

                    case 2:
                        CheckLoaded(isLoaded);
                        ShowDatabase(library);
                        break;

                    case 3:
                        CheckLoaded(isLoaded);
                        if (AddData(library))
                        {
                            isModified = true;
                        }

                        break;

                    case 4:
                        CheckLoaded(isLoaded);
                        if (DeleteData(library))
                        {
                            isModified = true;
                        }

                        break;

                    case 5:
                        CheckLoaded(isLoaded);
                        minPrice = Checked.ReadDouble(
                            "Минимальная стоимость: ",
                            0,
                            double.MaxValue);
                        library.QueryOneTable(minPrice);
                        break;

                    case 6:
                        CheckLoaded(isLoaded);
                        library.QueryTwoTables();
                        break;

                    case 7:
                        CheckLoaded(isLoaded);
                        genreName = ReadGenre(library);
                        library.QueryThreeTablesCount(
                            genreName);
                        break;

                    case 8:
                        CheckLoaded(isLoaded);
                        genreName = ReadGenre(library);
                        library.QueryThreeTablesValue(
                            genreName);
                        break;

                    case 9:
                        CheckLoaded(isLoaded);
                        savePath = Checked.ReadPath(
                            "Куда сохранить ", savePath);
                        repository.Save(
                            filePath,
                            savePath,
                            library.Artists,
                            library.Albums,
                            library.Tracks,
                            library.Genres);
                        isModified = false;
                        Console.WriteLine(
                            "Изменения сохранены.");
                        break;

                    case 0:
                        if (isModified)
                        {
                            Console.WriteLine(
                                "Внимание: изменения " +
                                "не сохранены.");
                        }

                        isRunning = false;
                        break;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(
                    $"Ошибка: {exception.Message}");
            }

            if (isRunning)
            {
                Console.WriteLine(
                    "\nНажмите Enter для продолжения.");
                Console.ReadLine();
            }
        }
    }

    private static void ShowMenu(
        bool isLoaded,
        bool isModified)
    {
        Console.Clear();
        Console.WriteLine("БАЗА ДАННЫХ «АУДИОТЕКА»");
        Console.WriteLine(
            isLoaded ? "База загружена." : "База не загружена.");
        if (isModified)
        {
            Console.WriteLine(
                "Есть несохранённые изменения.");
        }

        Console.WriteLine();
        Console.WriteLine("1. Чтение базы данных из Excel");
        Console.WriteLine("2. Просмотр базы данных");
        Console.WriteLine("3. Добавление элемента");
        Console.WriteLine("4. Удаление элемента по ID");
        Console.WriteLine("5. Запрос к одной таблице");
        Console.WriteLine("6. Запрос к двум таблицам");
        Console.WriteLine("7. Количество композиций жанра");
        Console.WriteLine("8. Минимальный размер жанра");
        Console.WriteLine("9. Сохранение в Excel");
        Console.WriteLine("0. Выход");
    }

    private static void CheckLoaded(bool isLoaded)
    {
        if (!isLoaded)
        {
            throw new InvalidOperationException(
                "Сначала загрузите базу — пункт 1.");
        }
    }

    private static void ShowDatabase(
        AudioLibraryService library)
    {
        Console.WriteLine("1. Артисты  2. Альбомы");
        Console.WriteLine("3. Треки    4. Жанры");
        int choice = Checked.ReadInt("Таблица: ", 1, 4);

        if (choice == 1)
        {
            library.ShowArtists();
        }
        else if (choice == 2)
        {
            library.ShowAlbums();
        }
        else if (choice == 3)
        {
            library.ShowTracks();
        }
        else
        {
            library.ShowGenres();
        }
    }

    private static bool AddData(AudioLibraryService library)
    {
        Console.WriteLine("1. Артист  2. Альбом");
        Console.WriteLine("3. Трек    4. Жанр");
        int choice = Checked.ReadInt("Тип элемента: ", 1, 4);
        string name = Checked.ReadString("Название: ");
        int artistId = 0;
        int albumId = 0;
        int genreId = 0;
        int duration = 0;
        long size = 0;
        double price = 0;
        bool result = false;

        if (choice == 1)
        {
            result = library.AddArtist(name);
        }
        else if (choice == 2)
        {
            artistId = Checked.ReadInt(
                "ID существующего артиста: ",
                1,
                int.MaxValue);
            if (!library.ArtistExists(artistId))
            {
                Console.WriteLine("Артист не найден.");
                return false;
            }

            result = library.AddAlbum(name, artistId);
        }
        else if (choice == 3)
        {
            albumId = Checked.ReadInt(
                "ID существующего альбома: ",
                1,
                int.MaxValue);
            if (!library.AlbumExists(albumId))
            {
                Console.WriteLine("Альбом не найден.");
                return false;
            }

            genreId = Checked.ReadInt(
                "ID существующего жанра: ",
                1,
                int.MaxValue);
            if (!library.GenreExists(genreId))
            {
                Console.WriteLine("Жанр не найден.");
                return false;
            }

            duration = Checked.ReadInt(
                "Длительность в миллисекундах: ",
                1,
                int.MaxValue);
            size = Checked.ReadLong(
                "Размер в байтах: ", 1, long.MaxValue);
            price = Checked.ReadDouble(
                "Стоимость: ", 0, double.MaxValue);
            result = library.AddTrack(
                name, albumId, genreId,
                duration, size, price);
        }
        else
        {
            result = library.AddGenre(name);
        }

        if (result)
        {
            Console.WriteLine("Элемент добавлен.");
        }
        else
        {
            Console.WriteLine(
                "Не удалось добавить элемент.");
        }

        return result;
    }

    private static bool DeleteData(
        AudioLibraryService library)
    {
        Console.WriteLine("1. Артист  2. Альбом");
        Console.WriteLine("3. Трек    4. Жанр");
        int choice = Checked.ReadInt("Тип элемента: ", 1, 4);
        int id = Checked.ReadInt(
            "ID для удаления: ", 1, int.MaxValue);
        bool result = false;

        if (choice == 1)
        {
            if (library.HasAlbums(id))
            {
                Console.WriteLine(
                    "У артиста есть альбомы.");
                return false;
            }

            result = library.DeleteArtist(id);
        }
        else if (choice == 2)
        {
            if (library.HasTracksForAlbum(id))
            {
                Console.WriteLine(
                    "В альбоме есть композиции.");
                return false;
            }

            result = library.DeleteAlbum(id);
        }
        else if (choice == 3)
        {
            result = library.DeleteTrack(id);
        }
        else
        {
            if (library.HasTracksForGenre(id))
            {
                Console.WriteLine(
                    "Жанр используется в композициях.");
                return false;
            }

            result = library.DeleteGenre(id);
        }

        if (result)
        {
            Console.WriteLine("Элемент удалён.");
        }
        else
        {
            Console.WriteLine(
                $"Элемент с ID {id} не найден.");
        }

        return result;
    }

    private static string ReadGenre(
        AudioLibraryService library)
    {
        string name = string.Empty;
        bool isCorrect = false;

        while (!isCorrect)
        {
            Console.Write("Название жанра [Metal]: ");
            name = Console.ReadLine() ?? string.Empty;
            name = name.Trim();
            if (name.Length == 0)
            {
                name = "Metal";
            }

            isCorrect = library.GenreNameExists(name);
            if (!isCorrect)
            {
                Console.WriteLine("Такого жанра нет.");
                library.ShowGenres();
            }
        }

        return name;
    }
}