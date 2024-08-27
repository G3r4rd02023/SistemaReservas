using Newtonsoft.Json;
using Reservas.Shared.Data;

namespace Reservas.Frontend.Services
{
    public class ServicioUsuario : IServicioUsuario
    {
        private readonly HttpClient _httpClient;

        public ServicioUsuario(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7009/");
        }

        public async Task<string> GetRolById(int rolId)
        {
            var rol = await _httpClient.GetAsync($"/api/Roles/{rolId}");
            var json = await rol.Content.ReadAsStringAsync();
            var nombreRol = JsonConvert.DeserializeObject<Rol>(json);
            var descripcion = nombreRol!.Descripcion;
            return descripcion;
        }

        public async Task<Usuario> GetUsuarioByEmail(string email)
        {
            var userResponse = await _httpClient.GetAsync($"/api/Usuarios/email/{email}");
            var usuarioJson = await userResponse.Content.ReadAsStringAsync();
            var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
            return usuario!;
        }
    }
}