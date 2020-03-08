
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

		public Orden _orden { get; set; }
		public Orden orden { get { return _orden; } set { _orden = value; MyPropertyChanged("orden"); } }

		public int ProductoId { get; set; }
		public List<Producto> ProductosList { get; set; }


		public event PropertyChangedEventHandler PropertyChanged;

		public RegistroOrden() {
			InitializeComponent();
			orden = new Orden();
			_orden = new Orden();
			ProductosList = new List<Producto>();
			RegistroOrdenGrid.DataContext = this;
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

			int.TryParse(OrdenIdTextBox.Text , out int ordenId);

			bool eliminado = OrdenesBLL.Eliminar(ordenId);

			if (eliminado) {
				Limpiar();
				MessageBox.Show("Eliminado.");
			} else {
				MessageBox.Show("Esta orden no existe.");
			}

		}

		private void BuscarButton_Click(object sender , RoutedEventArgs e) {

			if (int.TryParse(OrdenIdTextBox.Text, out int ordenId)) {
				if (ExisteEnBaseDatos()) {
					Limpiar();

					orden = OrdenesBLL.Buscar(ordenId);

					foreach (var item in orden.DetalleProductos) {

						Producto producto = ProductosBLL.Buscar(item.ProductoId);

						if (producto != null) {
							ProductosList.Add(producto);
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

		}

		private void AgregarProductoButton_Click(object sender , RoutedEventArgs e) {

			if (int.TryParse(ProductoIdTextBox.Text, out int nuevoProductoId)) {

				Producto nuevoProducto = ProductosBLL.Buscar(nuevoProductoId);

				if (nuevoProducto == null) {
					MessageBox.Show("Este producto no existe");
					
				} else {
					ProductosList.Add(nuevoProducto);

					orden.Monto += nuevoProducto.Precio;
					orden.DetalleProductos.Add(new OrdenDetalle() { ProductoId = this.ProductoId , OrdenId = this.orden.OrdenId, OrdenDetalleId = 0});
					MyPropertyChanged("orden");

					CargarProductosDataGrid();
				}
			} else {
				MessageBox.Show("Este Producto Id no es valido");
			}

			
		}

		private void CargarProductosDataGrid() {
			ProductosDataGrid.ItemsSource = null;
			ProductosDataGrid.ItemsSource = ProductosList;
		}

		private void Limpiar() {
			orden.OrdenId = 0;
			orden.ClienteId = 0;
			orden.Monto = 0.0m;
			orden.DetalleProductos.Clear();
			ProductosList.Clear();
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

		
	}
}
