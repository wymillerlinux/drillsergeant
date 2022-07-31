using LibGit2Sharp;

static class Program
{
    internal static void Main(string[] args)
    {
        try 
        {
            CommitDetail commits = new CommitDetail();
            commits.GetAllCommitsByName();
            // SortedList<string, int> results = commits.OrderCommitsByCount();
            //StdOutDataService outDataService = new StdOutDataService();
            ExcelDataService excelDataService = new ExcelDataService();
            //DataAccess dataAccess = new DataAccess(outDataService);
            DataAccess dataAccess = new DataAccess(excelDataService);
            dataAccess.WriteData(commits.CommitDetails);
        }
        catch (System.IO.DirectoryNotFoundException e)
        {
            Console.WriteLine($"Are you in a git repository? {e}");
            Environment.Exit(1);
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
