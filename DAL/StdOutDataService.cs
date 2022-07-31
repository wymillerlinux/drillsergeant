using Pastel;

public class StdOutDataService : IDataService
{
    public StdOutDataService()
    {
        
    }

    public void WriteAll(SortedList<string, int> dictResults)
    {
        ConsoleExtensions.Enable();
        // TODO: this is the ordered dictionary. replace the sorted list with this variable.
        var sortedResults = dictResults.OrderByDescending(d => d.Value).ToList();
        System.Console.WriteLine("--- Commit Report ---");
        
        foreach (var i in sortedResults)
        {
            if (i.Value == sortedResults[0].Value) {
                Console.WriteLine($"Author: {i.Key}, Commits: {i.Value.ToString().Pastel("ffd700")}");
            } else if (i.Value == sortedResults[1].Value) {
                Console.WriteLine($"Author: {i.Key}, Commits: {i.Value.ToString().Pastel("c0c0c0")}");
            } else if (i.Value == sortedResults[2].Value) {
                Console.WriteLine($"Author: {i.Key}, Commits: {i.Value.ToString().Pastel("f4a460")}");
            } else {
                Console.WriteLine($"Author: {i.Key}, Commits: {i.Value}");
            }
        }
    }
}
