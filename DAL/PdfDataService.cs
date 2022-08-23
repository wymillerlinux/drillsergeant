using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using Pastel;

public class PdfDataService : IDataService
{
    private string _fileName;
    private string _pathName;
    private string _dateTime;

    public PdfDataService()
    {
        _dateTime = DateTime.Now.ToString("yyyyMMddhhmm");
        _fileName = $"CommitReport-{_dateTime}.pdf";
        _pathName = Directory.GetCurrentDirectory() + "/" + _fileName;
    }

    public void WriteAll(SortedList<string, int> dictResults)
    {
        // sort the dictionary passed in
        var sortedResults = dictResults.OrderByDescending(d => d.Value).ToList();

        // setting the created document
        var document = new PdfDocument();
        var page = document.AddPage();
        
        // heading
        var gfx = XGraphics.FromPdfPage(page);
        var fontHeading = new XFont("Times New Roman", 36, XFontStyle.Underline);
        gfx.DrawString("Commit Report", fontHeading, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.TopCenter);
        
        // details
        var fontDetails = new XFont("Times New Roman", 16, XFontStyle.Regular);
        int positionValue = 50;
        
        foreach (var i in sortedResults)
        {
            gfx.DrawString($"{i.Key}: {i.Value}", fontDetails, XBrushes.Black, new XRect(0, positionValue, page.Width, page.Width), XStringFormats.TopCenter);
            positionValue += 30;
        }

        document.Save(_fileName);
    }
}
