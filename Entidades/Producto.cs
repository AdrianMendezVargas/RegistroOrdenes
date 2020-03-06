using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RegistroOrdenes.Entidades {
    public class Producto {

        [Key]
        public int ProductoId { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int CantidadInventario { get; set; }

        public Producto() {
            ProductoId = 0;
            Descripcion = "";
            Precio = 0;
            CantidadInventario = 0;
        }
    }
}
