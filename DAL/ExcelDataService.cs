using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

public class ExcelDataService : IDataService
{
    private string _fileName;
    private string _pathName;
    private string _dateTime;

    public ExcelDataService()
    {
        _dateTime = DateTime.Now.ToString("yyyyMMddhhmm");
        _fileName = $"CommitReport-{_dateTime}.xlsx";
        _pathName = Directory.GetCurrentDirectory() + "/" + _fileName;
    }
    
    public void WriteAll(SortedList<string, int> dictResults)
    {
        int index = 1;
        
        if (!File.Exists(_pathName))
        {
            this.CreateSpreadsheetWorkbook(_pathName);
        }

        foreach (var d in dictResults)
        {
            this.InsertText(_pathName, d.Key, index, "A");
            this.InsertText(_pathName, d.Value.ToString(), index, "B");
            index++;
        }
    }

    private void CreateSpreadsheetWorkbook(string filepath)
    {
        SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.
            Create(filepath, SpreadsheetDocumentType.Workbook);

        WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
        workbookpart.Workbook = new Workbook();

        WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
        worksheetPart.Worksheet = new Worksheet(new SheetData());

        Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.
            AppendChild<Sheets>(new Sheets());

        Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.
            GetIdOfPart(worksheetPart), SheetId = 1, Name = "Commit Report" };
        sheets.Append(sheet);

        workbookpart.Workbook.Save();
        spreadsheetDocument.Close();
    }

    // Given a document name and text, 
    private void InsertText(string docName, string text, int iterator, string column)
    {
        // Open the document for editing.
        using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(docName, true))
        {
            // Get the SharedStringTablePart. If it does not exist, create a new one.
            SharedStringTablePart shareStringPart;
            if (spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
            {
                shareStringPart = spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
            }
            else
            {
                shareStringPart = spreadSheet.WorkbookPart.AddNewPart<SharedStringTablePart>();
            }

            // Insert the text into the SharedStringTablePart.
            int index = this.InsertSharedStringItem(text, shareStringPart);

            // Grab the first worksheet created earlier.
            WorksheetPart worksheetPart = spreadSheet.WorkbookPart.Workbook.WorkbookPart.WorksheetParts.First();

            // Insert cell into the worksheet with the given column and row (iterator).
            Cell cell = this.InsertCellInWorksheet(column, (uint)iterator, worksheetPart);

            // Set the value of cell
            cell.CellValue = new CellValue(index.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);

            // Save the new worksheet.
            worksheetPart.Worksheet.Save();
        }
    }

    // Given text and a SharedStringTablePart, creates a SharedStringItem with the specified text 
    // and inserts it into the SharedStringTablePart. If the item already exists, returns its index.
    private int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
    {
        // If the part does not contain a SharedStringTable, create one.
        if (shareStringPart.SharedStringTable == null)
        {
            shareStringPart.SharedStringTable = new SharedStringTable();
        }

        int i = 0;

        // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
        foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
        {
            if (item.InnerText == text)
            {
                return i;
            }

            i++;
        }

        // The text does not exist in the part. Create the SharedStringItem and return its index.
        shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
        shareStringPart.SharedStringTable.Save();

        return i;
    }

    // Given a column name, a row index, and a WorksheetPart, inserts a cell into the worksheet. 
    // If the cell already exists, returns it. 
    private Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
    {
        Worksheet worksheet = worksheetPart.Worksheet;
        SheetData sheetData = worksheet.GetFirstChild<SheetData>();
        string cellReference = columnName + rowIndex;

        // If the worksheet does not contain a row with the specified row index, insert one.
        Row row;
        if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
        {
            row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
        }
        else
        {
            row = new Row() { RowIndex = rowIndex };
            sheetData.Append(row);
        }

        // If there is not a cell with the specified column name, insert one.  
        if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
        {
            return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
        }
        else
        {
            // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
            Cell refCell = null;
            foreach (Cell cell in row.Elements<Cell>())
            {
                if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                {
                    refCell = cell;
                    break;
                }
            }

            Cell newCell = new Cell() { CellReference = cellReference };
            row.InsertBefore(newCell, refCell);

            worksheet.Save();
            return newCell;
        }
    }
}
