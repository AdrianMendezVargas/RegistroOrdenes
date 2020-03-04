using Microsoft.EntityFrameworkCore;
using RegistroOrdenes.DAL;
using RegistroOrdenes.Entidades;
using System;

namespace RegistroOrdenes.BLL {
	public class ClientesBLL {

		public static bool Guardar(Cliente cliente) {
			bool paso = false;

			Contexto db = new Contexto();

			try {

				db.Clientes.Add(cliente);
				paso = (db.SaveChanges() > 0);

			} catch (Exception) {

				throw;

			} finally {
				db.Dispose();
			}

			return paso;
		}
		public static Cliente Buscar(int clienteId) {

			Contexto db = new Contexto();
			Cliente cliente = new Cliente();

			try {

				cliente = db.Clientes.Find(clienteId);

			} catch (Exception) {

				throw;

			} finally {
				db.Dispose();
			}

			return cliente;
		}
		public static bool Eliminar(int clienteId) {
			bool paso = false;

			Contexto db = new Contexto();


			try {

				var cliente = db.Clientes.Find(clienteId);
				if (cliente != null) {
					db.Entry(cliente).State = EntityState.Deleted;
					paso = (db.SaveChanges() > 0);
				}

			} catch (Exception) {

				throw;

			} finally {
				db.Dispose();
			}

			return paso;
		}
		public static bool Modificar(Cliente cliente) {
			bool paso = false;

			Contexto db = new Contexto();

			try {

				db.Entry(cliente).State = EntityState.Modified;
				paso = (db.SaveChanges() > 0);

			} catch (Exception) {

				throw;

			} finally {
				db.Dispose();
			}

			return paso;
		}

	}
}
