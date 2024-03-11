using System;
using System.Collections.Generic;

namespace Maestrodetalles.Models
{
    public partial class Tarea
    {
        public int Id { get; set; }
        public int IdProyecto { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Estado { get; set; } = null!;

        public virtual Proyecto IdProyectoNavigation { get; set; } = null!;
    }
}
