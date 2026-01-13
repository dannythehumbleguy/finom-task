using System.Text;

namespace ReportService.BusinessLogic.ReportGenerators;

public class Report
{
    private StringBuilder Content { get; set; } = new();

    public Report(string monthName)
    {
        Text(monthName, Styles.Bold);
        NewLine();
        NewLine();
    }
        
        
    public void NewLine() => Content.Append(Environment.NewLine);
    public void HorizonalLine(int length = 3) => Content.Append(new string('-', length));
    public void Text(string text, Styles? style = null) => Content.Append(style switch
    {
        Styles.Bold => $"**{text}**",
        Styles.H3 => $"### {text}",
        _ => text
    });

    public void Money(decimal money) => Content.Append($"{money:C}");
        
    public Stream Save()
    {
        var byteArray = Encoding.UTF8.GetBytes(Content.ToString());
        var stream =  new MemoryStream(byteArray);
        stream.Position = 0;
        
        return stream;
    }
}