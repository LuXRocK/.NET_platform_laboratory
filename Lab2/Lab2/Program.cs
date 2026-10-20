using System.Net;
using System.Text;
using System.Text.RegularExpressions;
public class Program
{
    private readonly static HttpClient _httpClient = new HttpClient();

    public static void ZnakStop(CancellationToken ct)
    {
        while(!ct.IsCancellationRequested)
        {
            Console.WriteLine("Znak STOP aktywny");
            Task.Delay(100).Wait();
        }
        Console.WriteLine("Znak STOP wyłączony");
    }
    public static async Task ZnakStopAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            Console.WriteLine("Znak STOP aktywny");
            await Task.Delay(100);
        }
        Console.WriteLine("Znak STOP wyłączony");
    }

    public static void WymianaKola(int numerKola)
    {
        Console.WriteLine($"Początek wymiany koła: {numerKola}");
        Task.Delay(2000).Wait();
        Console.WriteLine($"Koniec wymiany koła: {numerKola}");
    }
    public static async Task WymianaKolaAsync(int numerKola)
    {
        Console.WriteLine($"Początek wymiany koła: {numerKola}");
        await Task.Delay(2000);
        Console.WriteLine($"Koniec wymiany koła: {numerKola}");
    }

    public static void Tankowanie()
    {
        Console.WriteLine($"Początek tankowania");
        Task.Delay(5000).Wait();
        Console.WriteLine($"Koniec tankowania");
    }
    public static async Task TankowanieAsync()
    {
        Console.WriteLine($"Początek tankowania");
        await Task.Delay(5000);
        Console.WriteLine($"Koniec tankowania");
    }

    public static void UstawSkrzydlo()
    {
        Console.WriteLine($"Początek ustawiania skrzydła");
        Task.Delay(1000).Wait();
        Console.WriteLine($"Koniec ustawiania skrzydła");
    }
    public static async Task UstawSkrzydloAsync()
    {
        Console.WriteLine($"Początek ustawiania skrzydła");
        await Task.Delay(1000);
        Console.WriteLine($"Koniec ustawiania skrzydła");
    }

    public static void WyczyscKask()
    {
        Console.WriteLine($"Początek czyszczenia kasku");
        Task.Delay(500).Wait();
        Console.WriteLine($"Koniec czyszczenia kasku");
    }
    public static async Task WyczyscKaskAsync()
    {
        Console.WriteLine($"Początek czyszczenia kasku");
        await Task.Delay(500);
        Console.WriteLine($"Koniec czyszczenia kasku");
    }

    public static void PitStopSynchronous()
    {
        var cts = new CancellationTokenSource();
        CancellationToken ct = cts.Token;

        var task = Task.Run(() => ZnakStop(ct), ct);
        WymianaKola(1);
        WymianaKola(2);
        WymianaKola(3);
        WymianaKola(4);
        Tankowanie();
        UstawSkrzydlo();
        WyczyscKask();
        cts.Cancel();
        task.Wait();
    }

    public static async Task PitStopAsynchronous()
    {
        var cts = new CancellationTokenSource();
        CancellationToken ct = cts.Token;

        var znak = ZnakStopAsync(ct);
        await WymianaKolaAsync(1);
        await WymianaKolaAsync(2);
        await WymianaKolaAsync(3);
        await WymianaKolaAsync(4);
        await TankowanieAsync();
        await UstawSkrzydloAsync();
        await WyczyscKaskAsync();
        cts.Cancel();
        await znak;
    }

    public static async Task<string> getHtmlAsync(string target)
    {
        var getHtmlTask = _httpClient.GetStringAsync(target);

        Console.WriteLine($"Czytam tekst strony: {target}");

        var html = await getHtmlTask;

        return html;
        
    }

    public static string ReadTextFromHtml(string html)
    {
        // Jeśli strona ma <main>, bierzemy tylko je – pomijamy menu, stopkę itp.
        var main = Regex.Match(html, @"<main\b.*?</main>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        if (main.Success)
            html = main.Value;

        // <script> i <style> zawierają kod, nie tekst – wycinamy je w całości
        html = Regex.Replace(html, @"<(script|style)\b.*?</\1>", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        // Nagłówki, akapity i elementy list – w kolejności, w jakiej są na stronie
        var bloki = Regex.Matches(html, @"<(h[1-3]|p|li)\b[^>]*>(.*?)</\1>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        var wynik = new StringBuilder();
        foreach (Match blok in bloki)
        {
            string tag = blok.Groups[1].Value.ToLower();
            string tekst = Regex.Replace(blok.Groups[2].Value, "<[^>]+>", "");   // tagi w środku, np. <a>, <code>
            tekst = WebUtility.HtmlDecode(Regex.Replace(tekst, @"\s+", " ")).Trim();

            if (tekst.Length == 0)
                continue;

            switch (tag)
            {
                case "h1":
                    wynik.AppendLine();
                    wynik.AppendLine(tekst.ToUpper());
                    wynik.AppendLine(new string('=', tekst.Length));
                    break;
                case "h2":
                case "h3":
                    wynik.AppendLine();
                    wynik.AppendLine(tekst);
                    wynik.AppendLine(new string('-', tekst.Length));
                    break;
                case "li":
                    wynik.AppendLine($"  • {tekst}");
                    break;
                default:
                    wynik.AppendLine(tekst);
                    wynik.AppendLine();
                    break;
            }
        }

        return wynik.ToString();
    }

    public static List<string> getLinksFromHtml(string html)
    {
        // href z tagów <a>, w cudzysłowach "..." lub '...'
        var matches = Regex.Matches(html, @"<a\b[^>]*?\bhref\s*=\s*([""'])(.*?)\1", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        return matches
            .Select(m => WebUtility.HtmlDecode(m.Groups[2].Value).Trim())   // &amp; -> & w adresach
            .Where(href => href.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            .Distinct()
            .ToList();
    }

    public static async Task ReadPageAndSubpagesAsync(string primary_page)
    {
        string primary_html = await getHtmlAsync(primary_page);
        List<string> sub_pages = getLinksFromHtml(primary_html);
        var tasks = new List<Task<string>> { };
        var dict = new Dictionary<Task<string>, string>();
        foreach(string page in sub_pages)
        {
            var task = getHtmlAsync(page);
            tasks.Add(task);
            dict.Add(task, page);
        }
        Console.WriteLine();
        Console.WriteLine(new string('█', 80));
        Console.WriteLine($"  STRONA: {primary_page}");
        Console.WriteLine(new string('█', 80));
        Console.WriteLine(ReadTextFromHtml(primary_html));
        while(tasks.Count > 0)
        {
            Task<string> finishedTask = await Task.WhenAny(tasks);
            tasks.Remove(finishedTask);
            string url = dict[finishedTask];
            Console.WriteLine();
            Console.WriteLine(new string('█', 80));
            Console.WriteLine($"  STRONA: {url}");
            Console.WriteLine(new string('█', 80));
            string html = await finishedTask;
            Console.WriteLine(ReadTextFromHtml(html));
        }
    }

    public static async Task Main(string[] args)
    {
        Console.WriteLine("Prezentacja synchronicznej wersji metody PitStop");
        Task.Delay(2000).Wait();

        //Zadanie 1.1
        PitStopSynchronous();

        Console.WriteLine("Koniec wersji synchronicznej, Prezentacja asynchronicznej wersji metody PitStop");
        Task.Delay(2000).Wait();

        //Zadanie 1.2
        await PitStopAsynchronous();

        Console.WriteLine("Różnica między metodami widoczna w implementacji.");
        Task.Delay(2000).Wait();

        //Zadanie 2
        await ReadPageAndSubpagesAsync("https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/");
    }

}

