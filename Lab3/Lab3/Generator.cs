using System;

public class Generator
{
	static private Random rnd = new Random();
	static public async Task<List<int>> generateAsync(int size, int min, int max, IProgress<double> progress)
	{
		List<int> generatedNumbers = new List<int>();
		for(int i = 0; i < size; i++)
		{
			generatedNumbers.Add(rnd.Next(min, max));
			await Task.Delay(1000);
			progress.Report((i + 1) * 100.0 / size);
        }
		return generatedNumbers;
    }
}
