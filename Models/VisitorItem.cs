namespace DVisit.Models;

public class VisitorItem
{
#pragma warning disable IDE1006 // Estilos de nombres
    public bool valid { get; set; }
    public string documentNumber { get; set; } = string.Empty;
    public string firstName { get; set; } = string.Empty;
    public string lastName { get; set; } = string.Empty;
    public string FullName { get { return $"{firstName} {lastName}"; } }
#pragma warning restore IDE1006 // Estilos de nombres
}
