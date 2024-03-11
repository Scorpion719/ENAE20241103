using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maestrodetalles.Models
{
    public partial class Proyecto
    {
        public Proyecto()
        {
            Tarea = new List<Tarea>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del proyecto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string Nombre { get; set; } = null!;
        [Required(ErrorMessage = "La Descripcion del proyecto es obligatorio.")]
        [StringLength(255, ErrorMessage = "El campo {0} debe tener como máximo {1} caracteres.")]
        public string? Descripcion { get; set; }
        [Display(Name = "Fecha de Inicio")]
        [Required(ErrorMessage = "La fecha de inicio de la tarea es obligatoria.")]
        public DateTime FechaInicio { get; set; }
        [Display(Name = "Fecha de Fin")]
        [Required(ErrorMessage = "La fecha de Fin de la tarea es obligatoria.")]
        public DateTime? FechaFin { get; set; }

        public virtual IList<Tarea> Tarea { get; set; }
    }
}
