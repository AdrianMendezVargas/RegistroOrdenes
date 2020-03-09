using Microsoft.EntityFrameworkCore;
using RegistroOrdenes.DAL;
using RegistroOrdenes.Entidades;
using System.Linq;

namespace RegistroOrdenes.BLL {
	public class OrdenesBLL {

		public static Orden Buscar(int OrdenId) {
			Contexto db = new Contexto();

			var Orden = db.Ordenes.Where(o => o.OrdenId == OrdenId)
							  .Include(o => o.DetalleProductos)
							  .SingleOrDefault();

			return Orden;
		}

		public static bool Guardar(Orden orden) {    
			bool paso = false;

			Contexto db = new Contexto();

			try {

				db.Ordenes.Add(orden);

				paso = db.SaveChanges() > 0;

				if (paso) {
					foreach (var item in orden.DetalleProductos) {
						Producto producto = ProductosBLL.Buscar(item.ProductoId);
						producto.CantidadInventario--;
						ProductosBLL.Modificar(producto);
					}
				}

			} catch (System.Exception) {

				throw;

			} finally {
				db.Dispose();
			}

			return paso;
		}

		public static bool Modificar(Orden orden) {
			bool paso = false;

            //buscar las entidades que no estan para removerlas. 
            var Anterior = Buscar(orden.OrdenId);

			Contexto db = new Contexto();
			foreach (var item in Anterior.DetalleProductos) {

				if (!orden.DetalleProductos.Exists(d => d.OrdenDetalleId == item.OrdenDetalleId)) {
					Producto producto = ProductosBLL.Buscar(item.ProductoId);
					producto.CantidadInventario++;        //Devolviendo el producto eliminado al inventario
					ProductosBLL.Modificar(producto);

					db.Entry(item).State = EntityState.Deleted;
				}
			}    

			//recorrer el detalle para verificar si se agrego o se modifico un producto
			foreach (var item in orden.DetalleProductos) {

                //Muy importante indicar que pasara con la entidad del detalle
                var estado = item.OrdenDetalleId > 0 ? EntityState.Modified : EntityState.Added;
				db.Entry(item).State = estado;

				if (estado == EntityState.Added) {
					Producto producto = ProductosBLL.Buscar(item.ProductoId);
					producto.CantidadInventario--;        //Restando el producto agregado al inventario
					ProductosBLL.Modificar(producto);
				}
			}  

			try {
				//Indicar que se esta modificando el encabezado
				db.Entry(orden).State = EntityState.Modified;
				paso = db.SaveChanges() > 0;

			} catch (System.Exception) {

				throw;

			} finally {
				db.Dispose();
			}

			return paso;
		}

		public static bool Eliminar(int OrdenId) {
			bool paso = false;

			Contexto db = new Contexto();

			try {

				var ordenEliminada = Buscar(OrdenId);
				db.Entry(ordenEliminada).State = EntityState.Deleted;
				paso = (db.SaveChanges() > 0);

			} catch (System.Exception) {

				throw;

			} finally {
				db.Dispose();
			}

			return paso;

		}

	}
}


