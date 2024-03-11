using System;
using System.Collections.Generic;

namespace Maestrodetalles.Models
{
    public partial class Proyecto
    {
        public Proyecto()
        {
            Tarea = new List<Tarea>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        public virtual IList<Tarea> Tarea { get; set; }
    }
}
