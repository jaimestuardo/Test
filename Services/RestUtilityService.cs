using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text;
using VisitApp.Helpers;

namespace VisitApp.Services
{
    public class RestUtilityService
    {
        public event EventHandler<ThreadExceptionEventArgs> Exception;
        private void OnException(Exception ex)
        {
            Exception?.Invoke(this, new ThreadExceptionEventArgs(ex));
        }

        readonly Settings settings;
        readonly HttpClient client;
        readonly JsonSerializerOptions serializerOptions;
        static string baseUrl;

        public RestUtilityService(IConfiguration config)
        {
#if DEBUG
            HttpsClientHandlerService handler = new();
            client = new(handler.GetPlatformMessageHandler());
#else
            client = new HttpClient();
#endif
            serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            settings = config.GetRequiredSection("Settings").Get<Settings>();
            baseUrl = settings.Api != null && settings.Api.BaseUrl != null ? (DeviceInfo.Platform == DevicePlatform.Android ? settings.Api.BaseUrl.Android : settings.Api.BaseUrl.Ios) : string.Empty;
            if (string.IsNullOrEmpty(baseUrl))
                baseUrl = SettingsData.GetZkServerAddress().Result;
        }

        private async Task<string> GetBearerToken()
        {
            Uri uri = new($"{baseUrl}/api/Account/CreateToken");
            try
            {
                if (client.DefaultRequestHeaders.Contains("Authorization"))
                    client.DefaultRequestHeaders.Remove("Authorization");

                string usuario = settings.Api.User;
                string clave = settings.Api.Password;
                HttpResponseMessage response = await client.PostAsJsonAsync(uri, new
                {
                    usuario,
                    clave
                });
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<string>(content, serializerOptions);
                }

                throw (new Exception($"La respuesta del servidor fue: {response.ReasonPhrase}"));
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return string.Empty;
        }

        public async Task<T> GetDataAsync<T>(string endPoint)
        {
            string token = await GetBearerToken();
            if (string.IsNullOrEmpty(token))
                throw (new Exception("No se pudo obtener el token de seguridad."));

            Uri uri = new($"{baseUrl}{endPoint}");
            try
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    ApiResult<T> result = JsonSerializer.Deserialize<ApiResult<T>>(content, serializerOptions);
                    if (!result.success)
                        throw new Exception(result.errorList == null || !result.errorList.Any() ? result.detail : result.errorList.First());

                    return (T)result.data;
                }

                var contenido = (await response.Content.ReadAsStringAsync()).ToObject<ErrorItem>();
                throw new Exception($"La respuesta del servidor fue: {(contenido == null || contenido.errors == null || !contenido.errors.Any() ? response.ReasonPhrase : contenido.errors.First().Value.First())}");
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return default;
        }

        public async Task<T> PostDataAsync<T>(string endPoint, object parameters)
        {
            string token = await GetBearerToken();
            if (string.IsNullOrEmpty(token))
                throw new Exception("No se pudo obtener el token de seguridad.");

            Uri uri = new($"{baseUrl}{endPoint}");
            try
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                var param = JsonSerializer.Serialize(parameters);
                HttpContent postContent = new StringContent(param, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(uri, postContent);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    ApiResult<object> result = JsonSerializer.Deserialize<ApiResult<object>>(content, serializerOptions);

                    /*
                    if (result == null) // Viene un error JSON
                    {
                        var status = JsonSerializer.Deserialize<ApiStatusItem>(content, serializerOptions);
                        result = new ApiResult<string> { message = status.detail };
                    }
                    */

                    if (!result.success)
                        throw new Exception(result.errorList == null || !result.errorList.Any() ? result.detail : result.errorList.First());

                    return typeof(T) == typeof(bool) ? (T)Convert.ChangeType(result.success, typeof(T)) : result.data.ToObject<T>();
                }

                var contenido = (await response.Content.ReadAsStringAsync()).ToObject<ErrorItem>();
                throw new Exception($"La respuesta del servidor fue: {(contenido == null || contenido.errors == null || !contenido.errors.Any() ? response.ReasonPhrase : contenido.errors.First().Value.First())}");
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return default;
        }
    }
}

