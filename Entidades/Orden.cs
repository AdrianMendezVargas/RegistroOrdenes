using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RegistroOrdenes.Entidades {
    public class Orden {

        [Key]
        public int OrdenId { get; set; }
        public decimal Monto { get; set; }

        [ForeignKey("ClienteId")]
        public List<OrdenDetalle> Detalle { get; set; }

        public Orden() {
            OrdenId = 0;
            Monto = 0.0m;
            Detalle = new List<OrdenDetalle>();
        }
    }
}
