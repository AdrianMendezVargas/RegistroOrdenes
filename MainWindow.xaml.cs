using RegistroOrdenes.UI.Registro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RegistroOrdenes {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void RegitroClienteMenuItem_Click(object sender , RoutedEventArgs e) {
            RegistroCliente registroCliente = new RegistroCliente();
            registroCliente.Owner = this;
            registroCliente.ShowDialog();

        }

        private void ConsultaClienteMenuItem_Click(object sender , RoutedEventArgs e) {

        }

        private void RegitroProductoMenuItem_Click(object sender , RoutedEventArgs e) {
            RegistroProducto registroProducto = new RegistroProducto();
            registroProducto.Owner = this;
            registroProducto.ShowDialog();

        }

        private void ConsultaProductoMenuItem_Click(object sender , RoutedEventArgs e) {

        }
    }
}
