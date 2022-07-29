using LibGit2Sharp;

static class Program
{
    internal static void Main(string[] args)
    {
        List<string> authors = new List<string>();
        Dictionary<string, int> commitReport = new Dictionary<string, int>();

        using (var repo = new Repository(Directory.GetCurrentDirectory()))
        {
            foreach (var c in repo.Commits)
            {
                if (!authors.Contains(c.Author.Name))
                {
                    authors.Add(c.Author.Name);
                }
            }
            
            foreach (var a in authors)
            {
                int commitCount = repo.Commits.Where(r => r.Author.Name == a).Count();
                commitReport.Add(a, commitCount);
            }

        }

        foreach (var r in commitReport)
        {
            System.Console.WriteLine($"{r.ToString()}");
        }
    }
}
