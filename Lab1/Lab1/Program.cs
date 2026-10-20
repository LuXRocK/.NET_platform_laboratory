using System.Text.Json;

public class Program
{
    public static void Main(string[] args)
    {
        // ===== 1. Utworzenie obiektu czujnika =====
        Console.WriteLine("=== 1. Tworzenie czujnika ===");
        ISensor sensor = new TemperatureSensor();
        FileManager fileManager = new FileManager();
        Console.WriteLine("Utworzono czujnik temperatury.");

        // ===== 2. Pojedynczy pomiar =====
        Console.WriteLine("\n=== 2. Pojedynczy pomiar ===");
        double? singleMeasurement = sensor.GenerateMeasurement();
        sensor.DisplayMeasurement(singleMeasurement);

        // ===== 3. Seria pomiarów =====
        Console.WriteLine("\n=== 3. Seria pomiarów ===");
        List<double?> measurements = sensor.GenerateNMeasurements(10);
        Console.WriteLine($"Wygenerowano {measurements.Count} pomiarów.");
        Console.WriteLine("Próba wygenerowania -5 pomiarów:");
        sensor.GenerateNMeasurements(-5);

        // ===== 4. Wyświetlenie pomiarów =====
        Console.WriteLine("\n=== 4. Wszystkie pomiary ===");
        sensor.DisplayMeasurements(measurements);

        // ===== 5. Wyświetlenie N pomiarów podanych przez użytkownika =====
        Console.WriteLine("\n=== 5. Wyświetlenie wybranej liczby pomiarów ===");
        sensor.DisplayNMeasurements(measurements);

        // ===== 6. Zapis i odczyt pliku tekstowego =====
        Console.WriteLine("\n=== 6. Plik tekstowy ===");
        fileManager.WriteToTextFile("plikDane.txt", measurements);
        Console.WriteLine("Odczyt 5 pomiarów z pliku:");
        fileManager.ReadFromTextFile("plikDane.txt", 5);
        Console.WriteLine("Próba odczytu z nieistniejącego pliku:");
        fileManager.ReadFromTextFile("nieistniejacy.txt", 5);

        // ===== 7. Serializacja do JSON =====
        Console.WriteLine("\n=== 7. Zapis do JSON ===");
        fileManager.WriteToJsonFile(measurements);
        Console.WriteLine("Zapisano pomiary do pliku JSON.");

        // ===== 8. i 9. Odczyt najnowszego JSON-a z usunięciem pliku =====
        Console.WriteLine("\n=== 8-9. Odczyt najnowszego pliku JSON (z usunięciem) ===");
        List<double?>? fromJson = fileManager.ReadFromLatestJsonFile(true);
        if (fromJson != null)
        {
            Console.WriteLine("Odczytane pomiary:");
            sensor.DisplayMeasurements(fromJson);
            Console.WriteLine("Plik został usunięty po odczycie.");
        }

        // ===== 10. Średnia =====
        Console.WriteLine("\n=== 10. Średnia arytmetyczna ===");
        Console.WriteLine($"Średnia: {DataAnalizer.MeanAvg(measurements):F2}");

        // ===== 11. Odchylenie standardowe =====
        Console.WriteLine("\n=== 11. Odchylenie standardowe ===");
        Console.WriteLine($"Odchylenie standardowe: {DataAnalizer.StandardDeviation(measurements):F2}");

        // ===== 12. Sortowanie =====
        // Sortuję kopię, żeby oryginalne pomiary zostały bez zmian.
        Console.WriteLine("\n=== 12. Sortowanie (null-e są usuwane) ===");
        List<double?> sorted = new List<double?>(measurements);
        DataAnalizer.Sort(sorted);
        sensor.DisplayMeasurements(sorted);

        // ===== 13. Usunięcie pomiarów z przedziału =====
        Console.WriteLine("\n=== 13. Usunięcie pomiarów z przedziału [-10, 10] ===");
        List<double?> filtered = new List<double?>(measurements);
        DataAnalizer.RemoveRange(filtered, -10, 10);
        sensor.DisplayMeasurements(filtered);

        // ===== 14. Zapis wyników przetwarzania do pliku =====
        Console.WriteLine("\n=== 14. Zapis wyników do plików ===");
        fileManager.WriteToTextFile("posortowane.txt", sorted);
        fileManager.WriteToTextFile("przefiltrowane.txt", filtered);
    }
}

public class HelperFunctions{
    public static int UserNInput(int count)
    {
        int n;
        while (true)
        {
            if (!int.TryParse(Console.ReadLine(), out n))
            {
                Console.WriteLine("Podaj LICZBĘ!");
                continue;
            }
            if (n <= 0)
            {
                Console.WriteLine("Podaj dodatnią ilość pomiarów do wyświetlenia.");
                continue;
            }
            else if (n > count)
            {
                Console.WriteLine($"Podaj ilość mniejszą niż całkowita ilość pomiarów ({count})");
                continue;
            }
            else
            {
                return n;
            }
        }
    }
}

public interface ISensor
{
    double? GenerateMeasurement();
    List<double?> GenerateNMeasurements(int n);
    void DisplayMeasurement(double? measurement);
    void DisplayMeasurements(List<double?> measurements);
    void DisplayNMeasurements(List<double?> measurements);

}
public class TemperatureSensor : ISensor
{
    private Random rnd = new Random();
    public double? GenerateMeasurement()
    {
        double? measurement = rnd.NextDouble() * 200 - 100;
        if (measurement < -80)
        {
            return null;
        }
        else
        {
            return measurement;
        }
    }

    public List<double?> GenerateNMeasurements(int n)
    {
        if(n <= 0)
        {
            Console.WriteLine("Błędna ilość pomiarów.");
            return new List<double?>();
        }

        List<double?> measurements = new List<double?>();
        
        for(int i = 0; i < n; i++)
        {
            measurements.Add(GenerateMeasurement());
        }
        return measurements;
    }

    public void DisplayMeasurement(double? measurement)
    {
        if(measurement == null)
        {
            Console.WriteLine("Brak danych");
        }
        else
        {
            Console.WriteLine($"{measurement:F2}");
        }
    }
    public void DisplayMeasurements(List<double?> measurements)
    {
        foreach(double? measurement in measurements)
        {
            DisplayMeasurement(measurement);
        }
    }
    
    public void DisplayNMeasurements(List<double?> measurements)
    {
        Console.WriteLine("Podaj ilość pomiarów do wyświetlenia.");
        int n = HelperFunctions.UserNInput(measurements.Count());
        for(int i = 0; i < n; i++)
        {
            DisplayMeasurement(measurements[i]);
        }
    }
}

public class FileManager
{
    private string dataDirectory = "data";
    private string jsonDirectory = "json";

    public void WriteToTextFile(string fileName, List<double?> measurements)
    {
        Directory.CreateDirectory(dataDirectory);
        string filePath = Path.Combine(dataDirectory, fileName);
        using (StreamWriter sw = new StreamWriter(filePath, true))
        {
            foreach (double? measurement in measurements)
            {
                if (measurement != null)
                {
                    sw.WriteLine($"{measurement:F2}");
                }
                else
                {
                    sw.WriteLine("NULL");
                }
            }
        }
        Console.WriteLine($"Zapisano do pliku {filePath}");
    }

    public void ReadFromTextFile(string fileName, int n)
    {
        Directory.CreateDirectory(dataDirectory);
        string filePath = Path.Combine(dataDirectory, fileName);
        if (File.Exists(filePath))
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                for(int i = 0; i < n; i++)
                {
                    string line = sr.ReadLine();
                    if(line == null)
                    {
                        break;
                    }
                    if(line == "NULL")
                    {
                        Console.WriteLine("Brak danych");
                    }
                    else
                    {
                        Console.WriteLine(line);
                    }
                }
            }
        }
        else
        {
            Console.WriteLine($"Plik {fileName} nie istnieje.");
        }
    }

    public void WriteToJsonFile(List<double?> measurements)
    {
        string directory = Path.Combine(dataDirectory, jsonDirectory);
        string fileName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".json";
        Directory.CreateDirectory(directory);
        string filePath = Path.Combine(directory, fileName);
        
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            string json = JsonSerializer.Serialize(measurements);
            sw.WriteLine(json);
        }
    }

    public List<double?>? ReadFromLatestJsonFile(bool delete)
    {
        List<double?>? list = null;
        string directory = Path.Combine(dataDirectory, jsonDirectory);
        if(!Directory.Exists(directory))
        {
            Console.WriteLine($"Folder {directory} nie istnieje!");
            return null;
        }
        if(Directory.GetFiles(directory).Length != 0)
        {
            List<string> files = Directory.GetFiles(directory).OrderByDescending(p => p).ToList();
            string fileName = files[0];
            using (StreamReader sr = new StreamReader(fileName))
            {
                string json = sr.ReadToEnd();
                list = JsonSerializer.Deserialize<List<double?>>(json);
            }
            if (delete)
            {
                File.Delete(fileName);
            }
            return list;
        }
        else
        {
            Console.WriteLine($"W folderze {directory} nie ma zadnych plików");
            return null;
        }
    }
}

public class DataAnalizer
{
    //Wartości null są całkowicie pomijane.
    static public double? MeanAvg(List<double?> measurements)
    {
        List<double?> correct_values = new List<double?>();
        foreach(double? measurement in measurements)
        {
            if(measurement != null)
            {
                correct_values.Add(measurement);
            }
        }

        if(correct_values.Count() == 0)
        {
            return null;
        }

        double sum = 0;
        foreach(double correct_value in correct_values)
        {
            sum += correct_value;
        }
        return sum / correct_values.Count();
    }

    static public double? StandardDeviation(List<double?> measurements)
    {
        List<double?> correct_values = new List<double?>();
        foreach (double? measurement in measurements)
        {
            if (measurement != null)
            {
                correct_values.Add(measurement);
            }
        }

        if (correct_values.Count() == 0)
        {
            return null;
        }

        double sum = 0;
        double mean = MeanAvg(correct_values).Value;

        foreach (double correct_value in correct_values)
        {
            sum = sum + ((correct_value - mean) * (correct_value - mean));
        }

        return Math.Sqrt(sum / (correct_values.Count()-1));
    }

    static public void Sort(List<double?> measurements)
    {
        measurements.RemoveAll(measurement => measurement == null);
        measurements.Sort();
    }

    static public void RemoveRange(List<double?> measurements,  double min, double max)
    {
        measurements.RemoveAll(measurement => min <= measurement && measurement <= max);
    }
}