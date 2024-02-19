namespace DVisit.Models
{
#pragma warning disable IDE1006 // Estilos de nombres
    public class ApiResult<T> : WebStatus
    {
        public bool success { get; set; }
        public string[]? errorList { get; set; }
        public T? data { get; set; }
    }
#pragma warning restore IDE1006 // Estilos de nombres
}
