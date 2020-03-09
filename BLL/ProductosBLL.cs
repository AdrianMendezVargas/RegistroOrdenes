using Microsoft.EntityFrameworkCore;
using RegistroOrdenes.DAL;
using RegistroOrdenes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RegistroOrdenes.BLL {
	public class ProductosBLL {

		public static bool Guardar(Producto producto) {
			bool paso = false;

			Contexto db = new Contexto();

			try {

				db.Productos.Add(producto);
				paso = (db.SaveChanges() > 0);

			} catch (Exception) {

				throw;

			} finally {
				db.Dispose();
			}

			return paso;
		}

		public static bool Disponible(int productoId) {
			bool disponible;

			Producto producto = Buscar(productoId);

			disponible = (producto != null && producto.CantidadInventario > 0);

			return disponible;

		}
		public static Producto Buscar(int productoId) {

			Contexto db = new Contexto();
			Producto producto = new Producto();

			try {

				producto = db.Productos.Find(productoId);

			} catch (Exception) {

				throw;

			} finally {
				db.Dispose();
			}

			return producto;
		}
		public static bool Eliminar(int productoId) {
			bool paso = false;

			Contexto db = new Contexto();


			try {

				var producto = db.Productos.Find(productoId);
				if (producto != null) {
					db.Entry(producto).State = EntityState.Deleted;
					paso = (db.SaveChanges() > 0);
				}

			} catch (Exception) {

				throw;

			} finally {
				db.Dispose();
			}

			return paso;
		}
		public static bool Modificar(Producto producto) {
			bool paso = false;

			Contexto db = new Contexto();

			try {

				db.Entry(producto).State = EntityState.Modified;
				paso = (db.SaveChanges() > 0);

			} catch (Exception) {

				throw;

			} finally {
				db.Dispose();
			}

			return paso;
		}

		public static List<Producto> GetList(Expression<Func<Producto , bool>> expression) {

			List<Producto> productosList = new List<Producto>();
			Contexto db = new Contexto();

			try {

				productosList = db.Productos.Where(expression).ToList();

			} catch (Exception) {

				throw;
			} finally {
				db.Dispose();
			}

			return productosList;
		}
	}
}
