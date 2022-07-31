public class DataAccess
{
    private IDataService _dataService;
    
    public DataAccess(IDataService dataService)
    {
        _dataService = dataService;
    }

    public void WriteData(SortedList<string, int> dictResults)
    {
        _dataService.WriteAll(dictResults);
    }
}
