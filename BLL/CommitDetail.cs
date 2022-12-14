using LibGit2Sharp;

public class CommitDetail
{
    private List<string>? _authors;
    private SortedList<string, int>? _commitDetails;
    private string _currentBranch;

    public List<string>? Authors 
    { 
        get { return _authors; }
        set { _authors = value; }
    }
    
    public SortedList<string, int>? CommitDetails
    {
        get { return _commitDetails; }
        set { _commitDetails = value; }
    }

    public string CurrentBranch
    {
        get { return _currentBranch; }
        set { _currentBranch = value; }
    }

    public CommitDetail()
    {
        _authors = new List<string>();
        _commitDetails = new SortedList<string, int>();
        _currentBranch = GetCurrentBranch();
    }

    public void GetCurrentCommitsByName() 
    {
        using (var repo = new Repository(Directory.GetCurrentDirectory()))
        {
            foreach (var c in repo.Commits)
            {
                if (!_authors.Contains(c.Author.Name))
                {
                    _authors.Add(c.Author.Name);
                }
            }
            
            foreach (var a in _authors)
            {
                int commitCount = repo.Commits.Where(r => r.Author.Name == a).Count();
                _commitDetails.Add(a, commitCount);
            }
        }
    }

    public void GetCurrentCommitsByEmail()
    {
        using (var repo = new Repository(Directory.GetCurrentDirectory()))
        {
            foreach (var c in repo.Commits)
            {
                if (!_authors.Contains(c.Author.Email))
                {
                    _authors.Add(c.Author.Email);
                }
            }
            
            foreach (var a in _authors)
            {
                int commitCount = repo.Commits.Where(r => r.Author.Email == a).Count();
                _commitDetails.Add(a, commitCount);
            }
        }
    }

    public int GetCommitTotal()
    {
        using (var repo = new Repository(Directory.GetCurrentDirectory()))
        {
            return repo.Commits.Count();
        }
    }

    public string GetCurrentBranch()
    {
        using (var repo = new Repository(Directory.GetCurrentDirectory()))
        {
            return repo.Head.Reference.TargetIdentifier;
        }
    }

    public void GetCommitsByBranch(string branchName)
    {
        using (var repo = new Repository(Directory.GetCurrentDirectory()))
        {
            var branchResult = repo.Branches[branchName];

            try
            {
                if (branchResult == null)
                {
                    branchResult = repo.Branches[$"origin/{branchName}"];
                    var remoteBranch = repo.CreateBranch(branchName, branchResult.Tip);
                    repo.Branches.Update(remoteBranch, b => b.UpstreamBranch = $"refs/heads/{branchName}");
                }   
            }
            catch (System.Exception)
            {
                Console.WriteLine($"Cannot fetch {branchName} branch.");
                Environment.Exit(1);
            }

            foreach (var c in branchResult.Commits)
            {
                if (!_authors.Contains(c.Author.Name))
                {
                    _authors.Add(c.Author.Name);
                }
            }
            
            foreach (var a in _authors)
            {
                int commitCount = branchResult.Commits.Where(r => r.Author.Name == a).Count();
                _commitDetails.Add(a, commitCount);
            }
        }
    }
    
    public void GetCommitsByTag(string tagName)
    {
        using (var repo = new Repository(Directory.GetCurrentDirectory()))
        {
            try
            {
                var tagResult = repo.Tags[tagName].Target.Sha;
                
                var commitFilter = new CommitFilter
                {
                    IncludeReachableFrom = tagResult,
                };

                var query = repo.Commits.QueryBy(commitFilter);

                foreach (var c in query)
                {
                    if (!_authors.Contains(c.Author.Name))
                    {
                        _authors.Add(c.Author.Name);
                    }
                }

                foreach (var a in _authors)
                {
                    int commitCount = query.Where(r => r.Author.Name == a).Count();
                    _commitDetails.Add(a, commitCount);
                }
            }
            catch (System.Exception)
            {
                Console.WriteLine($"Cannot find the tag {tagName}");
                Environment.Exit(3);
            }
        }
    }
}
