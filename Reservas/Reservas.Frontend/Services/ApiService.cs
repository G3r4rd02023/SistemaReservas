using Newtonsoft.Json;
using Reservas.Frontend.Models;
using Reservas.Shared.Data;
using System.Text;

namespace Reservas.Frontend.Services
{
    public class ApiService
    {
        ////private static string _usuario = string.Empty;
        //private static string _clave = string.Empty;
        //private static string _baseUrl = string.Empty;
        ////private static string _token = string.Empty;

        //public ApiService()
        //{
        //    var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        //    _clave = builder.GetSection("ApiSetting:clave").Value!;
        //    _baseUrl = builder.GetSection("ApiSetting:baseUrl").Value!;
        //}

        public async Task<string> Autenticar(Usuario usuario)
        {
            var cliente = new HttpClient();
            cliente.BaseAddress = new Uri("https://localhost:7009/");

            var credenciales = new Usuario()
            {
                PrimerNombre = usuario.PrimerNombre,
                Contrasena = usuario.Contrasena,
                Email = usuario.Email,
                SegundoNombre = usuario.SegundoNombre,
                PrimerApellido = usuario.PrimerApellido,
                SegundoApellido = usuario.SegundoApellido,
                RolId = usuario.RolId
            };

            var content = new StringContent(JsonConvert.SerializeObject(credenciales), Encoding.UTF8, "application/json");
            var response = await cliente.PostAsync("api/Autenticacion/Validar", content);
            var json = await response.Content.ReadAsStringAsync();

            var resultado = JsonConvert.DeserializeObject<ResultadoCredencial>(json);
            var token = resultado!.Token;
            return token;
        }
    }
}