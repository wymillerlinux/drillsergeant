using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

public class ExcelDataService : IDataService
{
    private SpreadsheetDocument? _xlsxFile;
    private Workbook? _xlsxWorkbook;
    private WorkbookPart? _xlsxWorkbookPart;
    private WorksheetPart? _xlsxWorksheetPart;
    private Sheets? _xlsxSheets;
    private Sheet? _xlsxSheet;
    private Cell? _cell;
    private string _fileName;
    private string _pathName;
    private string _dateTime;

    public ExcelDataService()
    {
        _dateTime = DateTime.Now.ToString("yyyyMMdd");
        _fileName = $"CommitReport-{_dateTime}.xlsx";
        _pathName = Directory.GetCurrentDirectory() + _fileName;
        _xlsxFile = SpreadsheetDocument.Create(_pathName, SpreadsheetDocumentType.Workbook);
        _xlsxWorkbookPart = _xlsxFile.AddWorkbookPart();
        _xlsxWorkbookPart.Workbook = new Workbook();
        _xlsxWorksheetPart = _xlsxWorkbookPart.AddNewPart<WorksheetPart>();
        _xlsxWorksheetPart.Worksheet = new Worksheet(new SheetData());
        _xlsxSheets = _xlsxFile.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
        _xlsxSheet = new Sheet() { Id = _xlsxFile.WorkbookPart.GetIdOfPart(_xlsxWorksheetPart), SheetId = 1, Name = "Commit Report" };
        _xlsxSheets.Append(_xlsxSheet);
        _xlsxWorkbookPart.Workbook.Save();
        _xlsxFile.Close();
    }
    
    public void WriteAll(SortedList<string, int> dictResults)
    {
        using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(_pathName, true))
        {
            WorksheetPart worksheetPart = spreadSheet.WorkbookPart.WorksheetParts.First();
            
            foreach (var d in dictResults)
            {
                var index = dictResults.GetEnumerator().Current;
                _cell = InsertCellInWorksheet("A", (index.Value), worksheetPart);
                _cell.CellValue = new CellValue(d.Key);
                _cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
                worksheetPart.Worksheet.Save();
                
                //_cell = InsertCellInWorksheet("B", (index.Value), worksheetPart);
                //_cell.CellValue = new CellValue(d.Value);
                //_cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
                //worksheetPart.Worksheet.Save();
            }



        }
    }

    private static Cell InsertCellInWorksheet(string columnName, int rowIndex, WorksheetPart worksheetPart)
    {
        Worksheet worksheet = worksheetPart.Worksheet;
        SheetData sheetData = worksheet.GetFirstChild<SheetData>();
        string cellReference = columnName + rowIndex;

        Row row;
        if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
        {
            row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
        }
        else
        {
            row = new Row() { RowIndex = ((uint)rowIndex) };
            sheetData.Append(row);
        }

        Cell refCell = row.Descendants<Cell>().LastOrDefault();

        Cell newCell = new Cell() { CellReference = cellReference };
        row.InsertAfter(newCell, refCell);

        worksheet.Save();
        return newCell;

    }
}
