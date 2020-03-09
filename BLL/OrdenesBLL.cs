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

        public static bool Guardar(Orden ordenes) {    //TODO: Restar la cantidad de productos vendidos al inventario
            bool paso = false;

            Contexto db = new Contexto();

            try {

                db.Ordenes.Add(ordenes);
                paso = db.SaveChanges() > 0;

            } catch (System.Exception) {

                throw;

            } finally {
                db.Dispose();
            }

            return paso;
        }

        public static bool Modificar(Orden orden) { //TODO: Ejecular la eliminacion con un RawQuery
            bool paso = false;

            Contexto _db = new Contexto();    // Creado un Contexto auxiliar, ya que no se puede tener la misma entidad dos veces en el mismo contexto

            //buscar las entidades que no estan para removerlas. 
            var Anterior = _db.Ordenes.Find(orden.OrdenId);
            _db.Dispose(); //Cerrando el contexto auxiliar

            Contexto db = new Contexto();
            foreach (var item in Anterior.DetalleProductos) {

                if (!orden.DetalleProductos.Exists(d => d.OrdenDetalleId == item.OrdenDetalleId)) {
                    db.Entry(item).State = EntityState.Deleted;
                }
            }    

            //recorrer el detalle
            foreach (var item in orden.DetalleProductos) {

                //Muy importante indicar que pasara con la entidad del detalle
                var estado = item.OrdenDetalleId > 0 ? EntityState.Modified : EntityState.Added;
                db.Entry(item).State = estado;
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

            } catch (System.Exception) {

                throw;

            } finally {
                db.Dispose();
            }

            return paso;

        }

    }
}


