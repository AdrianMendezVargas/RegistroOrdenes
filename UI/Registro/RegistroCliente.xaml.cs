using RegistroOrdenes.BLL;
using RegistroOrdenes.Entidades;
using System;
using System.ComponentModel;
using System.Windows;

namespace RegistroOrdenes.UI.Registro {
	/// <summary>
	/// Interaction logic for RegistroCliente.xaml
	/// </summary>
	public partial class RegistroCliente : Window, INotifyPropertyChanged {

		public Cliente _cliente { get; set; }

		public Cliente cliente { get { return _cliente; } set { _cliente = value; MyPropertyChanged("cliente"); } }

		public RegistroCliente() {
			InitializeComponent();

			cliente = new Cliente();
			_cliente = new Cliente();
			MainGrid.DataContext = this;
		}

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

			if (cliente.ClienteId == 0 || string.IsNullOrWhiteSpace(ClienteIdTextBox.Text)) {
				guardado = ClientesBLL.Guardar(cliente);
			} else {
				if (ExisteEnBaseDatos()) {
					guardado = ClientesBLL.Modificar(cliente);

				} else {
					MessageBox.Show("Este cliente no existe; No se puede modificar");
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

			bool eliminado = ClientesBLL.Eliminar(cliente.ClienteId);

			if (eliminado) {
				Limpiar();
				MessageBox.Show("Eliminado.");
			} else {
				MessageBox.Show("Este cliente no existe.");
			}

		}

		private void BuscarButton_Click(object sender , RoutedEventArgs e) {

			if (ExisteEnBaseDatos()) {
				cliente = ClientesBLL.Buscar(cliente.ClienteId);
			} else {
				Limpiar();
				MessageBox.Show("Este cliente no existe.");
			}

		}

		private void Limpiar() {
			cliente.ClienteId = 0;
			cliente.Nombre = " ";
			MyPropertyChanged("cliente");
		}

		private bool Validar() {
			bool validados = true;

			if (string.IsNullOrWhiteSpace(NombreTextBox.Text)) {
				validados = false;
				NombreTextBox.Focus();
				MessageBox.Show("Ingrese un nombre");
			}

			return validados;
		}

		private bool ExisteEnBaseDatos() {
			Cliente cliente = ClientesBLL.Buscar(Convert.ToInt32(ClienteIdTextBox.Text));
			return (cliente != null);
		}
	}
}
