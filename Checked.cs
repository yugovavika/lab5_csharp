using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using NPOI.SS.UserModel;

namespace Lab5_Variant16;

internal static class Checked
{
    // проверка на инт
    public static int ReadInt(string prompt)
    {
        Console.WriteLine(prompt);
        string? input = Console.ReadLine();
        int x = 0;
        bool isCorrect = int.TryParse(input, out x);
        while (!isCorrect)
        {
            Console.WriteLine(
                "Ошибка ввода! Повторите ввод: ");
            input = Console.ReadLine();
            isCorrect = int.TryParse(input, out x);
        }

        return x;
    }

    // инт с диапазоном
    public static int ReadInt(
        string prompt,
        int min,
        int max)
    {
        Console.WriteLine(prompt);
        string? input = Console.ReadLine();
        int x = 0;
        bool isCorrect = int.TryParse(input, out x) &&
            InRange(x, min, max);
        while (!isCorrect)
        {
            Console.WriteLine(
                $"Введите число от {min} до {max}: ");
            input = Console.ReadLine();
            isCorrect = int.TryParse(input, out x) &&
                InRange(x, min, max);
        }

        return x;
    }

    // проверка на лонг
    public static long ReadLong(
        string prompt,
        long min,
        long max)
    {
        Console.WriteLine(prompt);
        string? input = Console.ReadLine();
        long x = 0;
        bool isCorrect = long.TryParse(input, out x) &&
            InRange(x, min, max);
        while (!isCorrect)
        {
            Console.WriteLine(
                "Ошибка ввода! Повторите ввод: ");
            input = Console.ReadLine();
            isCorrect = long.TryParse(input, out x) &&
                InRange(x, min, max);
        }

        return x;
    }

    // проверка на дабл
    public static double ReadDouble(string prompt)
    {
        Console.WriteLine(prompt);
        string? input = Console.ReadLine();
        double x = 0;
        bool isCorrect = TryDouble(input, out x);
        while (!isCorrect)
        {
            Console.WriteLine(
                "Ошибка ввода! Повторите ввод: ");
            input = Console.ReadLine();
            isCorrect = TryDouble(input, out x);
        }

        return x;
    }

    // дабл с диапазоном
    public static double ReadDouble(
        string prompt,
        double min,
        double max)
    {
        Console.WriteLine(prompt);
        string? input = Console.ReadLine();
        double x = 0;
        bool isCorrect = TryDouble(input, out x) &&
            InRange(x, min, max);
        while (!isCorrect)
        {
            Console.WriteLine(
                "Ошибка ввода! Повторите ввод: ");
            input = Console.ReadLine();
            isCorrect = TryDouble(input, out x) &&
                InRange(x, min, max);
        }

        return x;
    }

    // проверка на булл
    public static bool ReadBool(string prompt)
    {
        Console.WriteLine(prompt + " (д/н): ");
        string? input = Console.ReadLine();
        string answer = (input ?? string.Empty).Trim();
        answer = answer.ToLower();
        bool isCorrect = answer == "д" || answer == "н";
        while (!isCorrect)
        {
            Console.WriteLine(
                "Ошибка ввода! Введите д или н: ");
            input = Console.ReadLine();
            answer = (input ?? string.Empty).Trim();
            answer = answer.ToLower();
            isCorrect = answer == "д" || answer == "н";
        }

        return answer == "д";
    }

    // проверка на стринг
    public static string ReadString(string prompt)
    {
        Console.WriteLine(prompt);
        string? input = Console.ReadLine();
        string text = (input ?? string.Empty).Trim();
        bool isCorrect = HasText(text);
        while (!isCorrect)
        {
            Console.WriteLine(
                "Строка не должна быть пустой. " +
                "Повторите ввод: ");
            input = Console.ReadLine();
            text = (input ?? string.Empty).Trim();
            isCorrect = HasText(text);
        }

        return text;
    }

    // путь к файлу: пусто = значение по умолчанию
    public static string ReadPath(
        string prompt,
        string defaultPath)
    {
        Console.WriteLine(prompt + "[" + defaultPath + "]: ");
        string? input = Console.ReadLine();
        string text = (input ?? string.Empty).Trim();
        text = text.Trim('"');
        if (text.Length == 0)
        {
            return defaultPath;
        }

        return text;
    }

    private static bool TryDouble(
        string? input,
        out double x)
    {
        string text = input ?? string.Empty;
        text = text.Trim().Replace(',', '.');
        return double.TryParse(
            text,
            NumberStyles.Number,
            CultureInfo.InvariantCulture,
            out x);
    }

    // проверки значений модели

    public static int Id(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentException(
                "ID должен быть положительным.");
        }

        return value;
    }

    public static string Text(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "Название не может быть пустым.");
        }

        return value.Trim();
    }

    public static int Duration(int value)
    {
        if (value < 0)
        {
            throw new ArgumentException(
                "Недопустимая длительность.");
        }

        return value;
    }

    public static long Size(long value)
    {
        if (value < 0)
        {
            throw new ArgumentException(
                "Недопустимый размер.");
        }

        return value;
    }

    public static double Price(double value)
    {
        if (value < 0)
        {
            throw new ArgumentException(
                "Недопустимая стоимость.");
        }

        return Math.Round(value, 2);
    }

    // логические проверки

    public static bool HasText(string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    public static bool IsPositive(int value)
    {
        return value > 0;
    }

    public static bool IsPositive(long value)
    {
        return value > 0;
    }

    public static bool IsNotNegative(double value)
    {
        return value >= 0;
    }

    public static bool InRange(int value, int min, int max)
    {
        return value >= min && value <= max;
    }

    public static bool InRange(
        long value,
        long min,
        long max)
    {
        return value >= min && value <= max;
    }

    public static bool InRange(
        double value,
        double min,
        double max)
    {
        return value >= min && value <= max;
    }

    // проверки файла и ячеек Excel

    public static string FilePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException(
                "Не указан путь к файлу Excel.");
        }

        if (!File.Exists(path))
        {
            throw new FileNotFoundException(
                "Файл Excel не найден.", path);
        }

        return path;
    }

    public static ISheet Sheet(
        IWorkbook workbook,
        string name)
    {
        ISheet? sheet = workbook.GetSheet(name);
        if (sheet == null)
        {
            throw new InvalidDataException(
                $"В файле нет листа «{name}».");
        }

        return sheet;
    }

    public static ICell Cell(IRow row, int index)
    {
        ICell? cell = row.GetCell(index);
        if (cell == null)
        {
            throw new InvalidDataException(
                $"Строка {row.RowNum + 1}, столбец " +
                $"{index + 1}: пустая ячейка.");
        }

        return cell;
    }

    public static string CellText(IRow row, int index)
    {
        ICell cell = Cell(row, index);
        string result = cell.ToString() ?? string.Empty;
        result = result.Trim();
        if (result.Length == 0)
        {
            throw new InvalidDataException(
                $"Строка {row.RowNum + 1}, столбец " +
                $"{index + 1}: пустое значение.");
        }

        return result;
    }

    public static double CellNumber(IRow row, int index)
    {
        ICell cell = Cell(row, index);
        double value = 0;
        if (cell.CellType == CellType.Numeric)
        {
            return cell.NumericCellValue;
        }

        string text = CellText(row, index);
        text = text.Replace(',', '.');
        if (!double.TryParse(
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out value))
        {
            throw new InvalidDataException(
                $"Строка {row.RowNum + 1}, столбец " +
                $"{index + 1}: ожидалось число.");
        }

        return value;
    }
}