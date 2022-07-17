namespace Strategy;

/// <summary>
/// Concrete Strategy
/// </summary>
public class XmlExportService : IExportService
{
    public void Export(Order order)
    {
        Console.WriteLine($"Exporting {order.Name} to Xml.");
    }
}