
using RegistroOrdenes.BLL;
using RegistroOrdenes.Entidades;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace RegistroOrdenes.UI.Registro {
	/// <summary>
	/// Interaction logic for RegistroOrden.xaml
	/// </summary>
	public partial class RegistroOrden : Window, INotifyPropertyChanged {

		public Orden orden { get; set; }

		public int ProductoId { get; set; }
		public List<Producto> ProductosAgregadosList { get; set; }

		private List<Producto> productosDisponibles = new List<Producto>();


		public event PropertyChangedEventHandler PropertyChanged;

		public RegistroOrden() {
			InitializeComponent();
			orden = new Orden();
			ProductosAgregadosList = new List<Producto>();
			productosDisponibles = ProductosBLL.GetList(p => true);
			//RegistroOrdenGrid.DataContext = this;
			this.DataContext = this;

		}


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

			if (orden.OrdenId == 0 || string.IsNullOrWhiteSpace(OrdenIdTextBox.Text)) {
				guardado = OrdenesBLL.Guardar(orden);
			} else {
				if (ExisteEnBaseDatos()) {
					guardado = OrdenesBLL.Modificar(orden);

				} else {
					MessageBox.Show("Esta orden no existe; No se puede modificar");
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

			bool eliminado = false;

			if (int.TryParse(OrdenIdTextBox.Text , out int ordenId)) {
				if (ExisteEnBaseDatos()) {
					eliminado = OrdenesBLL.Eliminar(ordenId);
				} else {
					MessageBox.Show("Esta Orden no existe.");
					return;
				}

			} else {
				MessageBox.Show("Esta Orden id es invalida.");
				return;
			}

			if (eliminado) {
				Limpiar();
				MessageBox.Show("Eliminado.");
			} else {
				MessageBox.Show("Error al eliminar.");
			}

		}

		private void BuscarButton_Click(object sender , RoutedEventArgs e) {

			if (int.TryParse(OrdenIdTextBox.Text , out int ordenId)) {
				if (ExisteEnBaseDatos()) {
					Limpiar();

					orden = OrdenesBLL.Buscar(ordenId);

					foreach (var item in orden.DetalleProductos) {

						Producto producto = ProductosBLL.Buscar(item.ProductoId);

						if (producto != null) {
							ProductosAgregadosList.Add(producto);
						}
					}
					CargarProductosDataGrid();

				} else {
					Limpiar();
					MessageBox.Show("Esta orden no existe.");
				}
			} else {
				MessageBox.Show("Este Orden Id no es valido.");
			}

		}

		private void EliminarProductoButton_Click(object sender , RoutedEventArgs e) {

			if (ProductosAgregadosList.Count > 0 && ProductosDataGrid.SelectedIndex != -1) {

				orden.Monto -= ProductosAgregadosList[ProductosDataGrid.SelectedIndex].Precio; //Restando el precio del producto eliminado al monto


				//Devolviendo el producto al inventario
				for (int i = 0 ; i < productosDisponibles.Count ; i++) {
					if (productosDisponibles[i].ProductoId == ProductosAgregadosList[ProductosDataGrid.SelectedIndex].ProductoId) {
						productosDisponibles[i].CantidadInventario++;
						break;
					}

				}

				ProductosAgregadosList.RemoveAt(ProductosDataGrid.SelectedIndex);
				orden.DetalleProductos.RemoveAt(ProductosDataGrid.SelectedIndex); //Removemos el producto en las 2 listas

				CargarProductosDataGrid();
				MyPropertyChanged("orden");

			}

		}

		private void AgregarProductoButton_Click(object sender , RoutedEventArgs e) {

			if (int.TryParse(ProductoIdTextBox.Text , out int nuevoProductoId)) {

				Producto nuevoProducto = ProductosBLL.Buscar(nuevoProductoId);

				if (nuevoProducto == null) {
					MessageBox.Show("Este producto no existe");
					return;
				} else {

					int pos = -1;                        //Buscando la posición del producto en productosDisponibles
					for (int i = 0 ; i < productosDisponibles.Count ; i++) {
						if (productosDisponibles[i].ProductoId == nuevoProductoId) {
							pos = i;
							break;
						}

					}

					if (productosDisponibles[pos].CantidadInventario > 0) { //Verificando que aun quedan en el inventario
						productosDisponibles[pos].CantidadInventario--;
						ProductosAgregadosList.Add(nuevoProducto);

						orden.Monto += nuevoProducto.Precio;
						orden.DetalleProductos.Add(new OrdenDetalle() { ProductoId = this.ProductoId , OrdenId = this.orden.OrdenId , OrdenDetalleId = 0 });
						MyPropertyChanged("orden");

						CargarProductosDataGrid();

					} else {
						MessageBox.Show("Este producto se ha agotado");
					}
				}
			} else {
				MessageBox.Show("Este Producto Id no es valido");
			}


		}

		private void CargarProductosDataGrid() {
			ProductosDataGrid.ItemsSource = null;
			ProductosDataGrid.ItemsSource = ProductosAgregadosList;
		}

		private void Limpiar() {
			orden.OrdenId = 0;
			orden.ClienteId = 0;
			orden.Monto = 0.0m;
			orden.DetalleProductos.Clear();
			ProductosAgregadosList.Clear();
			ProductoId = 0;
			CargarProductosDataGrid();
			MyPropertyChanged("orden");
			MyPropertyChanged("ProductoId");
		}

		private bool Validar() {
			bool validados = true;
			string mensaje = "Errores: \n\n";

			//Verificando cliente
			if (!int.TryParse(ClienteIdTextBox.Text , out int clienteId)) {
				validados = false;
				mensaje += "Este cliente Id es invalido.\n";
			} else {
				Cliente cliente = ClientesBLL.Buscar(clienteId);
				if (cliente == null) {
					validados = false;
					mensaje += "Este cliente no existe.\n";
				}
			}

			//Verificando productos
			if (orden.DetalleProductos.Count == 0) {
				validados = false;
				mensaje += "Debe agregar por lo menos un producto.\n";
			}

			if (!validados) {
				MessageBox.Show(mensaje);
			}

			return validados;
		}

		private bool ExisteEnBaseDatos() {
			Orden orden = OrdenesBLL.Buscar(int.Parse(OrdenIdTextBox.Text));
			return (orden != null);
		}

		private void ProductoIdTextBox_TextChanged(object sender , System.Windows.Controls.TextChangedEventArgs e) {
			if (int.TryParse(ProductoIdTextBox.Text , out int nuevoProductoId)) {
				Producto producto = ProductosBLL.Buscar(nuevoProductoId);
				if (producto != null) {
					DescripcionProductoTextBox.Text = producto.Descripcion;
				} 
				else {
					DescripcionProductoTextBox.Text = "No encontrado";
				}
			} else {
				DescripcionProductoTextBox.Text = "Producto Id invalido";
			}

			
		}
	}
}
