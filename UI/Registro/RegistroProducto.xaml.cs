using RegistroOrdenes.BLL;
using RegistroOrdenes.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RegistroOrdenes.UI.Registro {
	/// <summary>
	/// Interaction logic for RegistroProducto.xaml
	/// </summary>
	public partial class RegistroProducto : Window, INotifyPropertyChanged {
		public RegistroProducto() {
			InitializeComponent();

			producto = new Producto();
			_producto = new Producto();
			RegistroProductoGrid.DataContext = this;
		}

		public Producto _producto { get; set; }

		public Producto producto { get { return _producto; } set { _producto = value; MyPropertyChanged("producto"); } }

		

			
		

		public event PropertyChangedEventHandler PropertyChanged;
		private void MyPropertyChanged(string propiedad) {
			PropertyChanged?.Invoke(this , new PropertyChangedEventArgs(propiedad));
		}

		private void NuevoButton_Click(object sender , RoutedEventArgs e) {
			Limpiar();
		}

		private void GuardarButton_Click(object sender , RoutedEventArgs e) {

			if (!Validar()) {
				return;
			}

			bool guardado = false;

			if (producto.ProductoId == 0 || string.IsNullOrWhiteSpace(ProductoIdTextBox.Text)) {
				guardado = ProductosBLL.Guardar(producto);
			} else {
				if (ExisteEnBaseDatos()) {
					guardado = ProductosBLL.Modificar(producto);

				} else {
					MessageBox.Show("Este producto no existe; No se puede modificar");
				}
			}

			if (guardado) {
				MessageBox.Show("Guardado.");
				Limpiar();
			} else {
				MessageBox.Show("Error el guardar.");
			}

		}

		private void EliminarButton_Click(object sender , RoutedEventArgs e) {

			if (!Validar()) {
				return;
			}

			bool eliminado = ProductosBLL.Eliminar(producto.ProductoId);

			if (eliminado) {
				Limpiar();
				MessageBox.Show("Eliminado.");
			} else {
				MessageBox.Show("Este producto no existe.");
			}

		}

		private void BuscarButton_Click(object sender , RoutedEventArgs e) {

			if (ExisteEnBaseDatos()) {
				producto = ProductosBLL.Buscar(producto.ProductoId);
			} else {
				Limpiar();
				MessageBox.Show("Este producto no existe.");
			}

		}

		private void Limpiar() {
			producto.ProductoId = 0;
			producto.Descripcion = "";
			producto.Precio = 0.0m;
			producto.CantidadInventario = 0;
			MyPropertyChanged("producto");
		}

		private bool Validar() {
			bool validados = true;
			string mensaje = "Favor de: ";

			if (string.IsNullOrWhiteSpace(DescripcionTextBox.Text)) {
				validados = false;
				DescripcionTextBox.Focus();
				mensaje += "Ingresar una descripción.\n";
			}

			decimal.TryParse(PrecioTextBox.Text , out decimal precio);
			if (precio <= 0) {
				validados = false;
				PrecioTextBox.Focus();
				mensaje += "Ingresar un precio.\n";
			}

			int.TryParse(CantidadInventarioTextBox.Text , out int cantidad);
			if (cantidad <= 0) {
				validados = false;
				PrecioTextBox.Focus();
				mensaje += "Ingresar una cantidad.\n";
			}

			if (!validados) {
				MessageBox.Show(mensaje);
			}

			return validados;
		}

		private bool ExisteEnBaseDatos() {
			Producto producto = ProductosBLL.Buscar(Convert.ToInt32(ProductoIdTextBox.Text));
			return (producto != null);
		}
	}
}
