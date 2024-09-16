using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservas.Shared.Data
{
    public class Bitacora
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        public string TipoAccion { get; set; } = null!;

        public string Tabla { get; set; } = null!;

        public DateTime Fecha { get; set; }
    }
}