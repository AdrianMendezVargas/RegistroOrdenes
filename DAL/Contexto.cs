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
            optionsBuilder.UseSqlite("Data Source = RegistroOrdenes.db");
        }
    }
}
