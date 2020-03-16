using Microsoft.EntityFrameworkCore;
using RegistroOrdenes.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegistroOrdenes.DAL {
	public class Contexto : DbContext {

		public DbSet<Cliente> Clientes { get; set; }
		public DbSet<Producto> Productos { get; set; }
		public DbSet<Orden> Ordenes { get; set; }
		

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder.EnableSensitiveDataLogging(true);
			optionsBuilder.EnableDetailedErrors(true);
			optionsBuilder.UseSqlite("Data Source = RegistroOrdenes.db");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Producto>().HasData(new Producto() { ProductoId = 1 , Precio = 25 , Descripcion = "Manzana" , CantidadInventario = 100 });
			modelBuilder.Entity<Producto>().HasData(new Producto() { ProductoId = 2 , Precio = 30 , Descripcion = "Coca Cola" , CantidadInventario = 50 });
			modelBuilder.Entity<Producto>().HasData(new Producto() { ProductoId = 3 , Precio = 125 , Descripcion = "Llaroa" , CantidadInventario = 30 });

		}
	}
}
