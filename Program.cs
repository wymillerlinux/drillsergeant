using LibGit2Sharp;

static class Public
{
    internal static void Main(string[] args)
    {
        List<string> authors = new List<string>();
        Dictionary<string, int> commitReport = new Dictionary<string, int>();

        using (var repo = new Repository(@"."))
        {
            foreach (var c in repo.Commits)
            {
                if (!authors.Contains(c.Author.Email))
                {
                    authors.Add(c.Author.Email);
                }
            }
            
            foreach (var a in authors)
            {
                int commitCount = repo.Commits.OrderBy(a => a.Author).Count();
                commitReport.Add(a, commitCount);
            }

        }

        foreach (var r in commitReport)
        {
            System.Console.WriteLine($"{r.ToString()}");
        }
    }
}
