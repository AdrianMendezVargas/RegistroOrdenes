using RegistroOrdenes.BLL;
using RegistroOrdenes.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RegistroOrdenes.UI.Consulta {
    /// <summary>
    /// Interaction logic for ConsultaClientes.xaml
    /// </summary>
    public partial class ConsultaClientes : Window {
        public ConsultaClientes() {
            InitializeComponent();
        }

        private void BuscarButton_Click(object sender , RoutedEventArgs e) {

            List<Cliente> clienteList = new List<Cliente>();

            if (!string.IsNullOrWhiteSpace(CriterioTextBox.Text)) {

                switch (FiltroComboBox.SelectedIndex) {
                    case 0:  //todo
                        clienteList = ClientesBLL.GetList(c => true);
                        break;

                    case 1: //id
                        int.TryParse(CriterioTextBox.Text , out int clienteId);
                        clienteList = ClientesBLL.GetList(c => c.ClienteId == clienteId);
                        break;

                    case 2:  //Nombre
                        clienteList = ClientesBLL.GetList(c => c.Nombre.Contains(CriterioTextBox.Text));
                        break;
                }


            } else {

                clienteList = ClientesBLL.GetList(c => true);

            }

            ResultadosDataGrid.ItemsSource = clienteList;

        }
    }
}
