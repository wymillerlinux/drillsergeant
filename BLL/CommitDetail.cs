using LibGit2Sharp;

public class CommitDetail
{
    private List<string>? _authors;
    private SortedList<string, int>? _commitDetails;

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

    public CommitDetail()
    {
        _authors = new List<string>();
        _commitDetails = new SortedList<string, int>();
    }

    public void GetAllCommitsByName() 
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

    public void GetAllCommitsByEmail()
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
}
