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
        public int ClienteId { get; set; }

        [ForeignKey("OrdenId")]
        public virtual List<OrdenDetalle> DetalleProductos { get; set; }

        public Orden() {
            OrdenId = 0;
            Monto = 0.0m;
            ClienteId = 0;
            DetalleProductos = new List<OrdenDetalle>();
        }
    }
}
