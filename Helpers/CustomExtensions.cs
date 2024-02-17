namespace VisitApp.Helpers
{
    public static class CustomExtensions
    {
        public static DateTime FirstTimeInMonth(this DateTime fecha)
        {
            return new DateTime(fecha.Year, fecha.Month, 1);
        }

        public static DateTime LastTimeInMonth(this DateTime fecha)
        {
            return (new DateTime(fecha.Year, fecha.Month, DateTime.DaysInMonth(fecha.Year, fecha.Month))).LastTimeInDay();
        }

        public static DateTime LastTimeInDay(this DateTime fecha)
        {
            return fecha.Date.AddDays(1).AddSeconds(-1);
        }

        public static T ToObject<T>(this object o)
        {
            if (o is null || o is not JsonElement && o is not string)
                return default;

            if (o is JsonElement element)
                return element.Deserialize<T>();
            else
            {
                string json = o.ToString() ?? string.Empty;
                if (json == string.Empty) return default;

                return JsonSerializer.Deserialize<T>(json);
            }
        }
    }
}
