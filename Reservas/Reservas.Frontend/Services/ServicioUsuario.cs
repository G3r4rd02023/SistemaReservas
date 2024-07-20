using Newtonsoft.Json;
using Reservas.Shared.Data;

namespace Reservas.Frontend.Services
{
    public class ServicioUsuario: IServicioUsuario
    {
        private readonly HttpClient _httpClient;

        public ServicioUsuario(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7009/");
        }

        public async Task<Usuario> ObtenerUsuarioPorEmailAsync(string correo)
        {
            try
            {                
                var email = Uri.EscapeDataString(correo);                
                var userResponse = await _httpClient.GetAsync($"/api/Usuarios/email/{email}");               
                userResponse.EnsureSuccessStatusCode();                
                var usuarioJson = await userResponse.Content.ReadAsStringAsync();              
                var usuario = JsonConvert.DeserializeObject<Usuario>(usuarioJson);
                return usuario!;
            }
            catch (Exception ex)
            {                
                throw new Exception("Error al obtener el usuario por email", ex);
            }
        }

        public async Task<string> ObtenerNombreRolAsync(Usuario usuario)
        {
            try
            {
                var result = await _httpClient.GetAsync($"/api/Roles/{usuario!.RolId}");
                var rolJson = await result.Content.ReadAsStringAsync();
                var rol = JsonConvert.DeserializeObject<Rol>(rolJson);
                var descripcion = rol!.Descripcion;
                return descripcion;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el nombre del rol", ex);
            }
        }
    }
}
