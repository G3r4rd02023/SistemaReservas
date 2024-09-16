using Newtonsoft.Json;
using Reservas.Shared.Data;
using System.Text;

namespace Reservas.Frontend.Services
{
    public class ServicioBitacora : IServicioBitacora
    {
        private readonly HttpClient _httpClient;

        public ServicioBitacora(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7009/");
        }

        public async Task<Bitacora> AgregarRegistro(Bitacora bitacora)
        {
            Bitacora registro = new()
            {
                TipoAccion = bitacora.TipoAccion,
                UsuarioId = bitacora.UsuarioId,
                Fecha = DateTime.Now,
                Tabla = bitacora.Tabla,
            };

            var json = JsonConvert.SerializeObject(registro);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/Bitacora", content);
            if (response.IsSuccessStatusCode)
            {
                return bitacora;
            }
            return registro;
        }
    }
}