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
    /// Interaction logic for ConsultaProductos.xaml
    /// </summary>
    public partial class ConsultaProductos : Window {
        public ConsultaProductos() {
            InitializeComponent();
        }

        private void BuscarButton_Click(object sender , RoutedEventArgs e) {

            List<Producto> productoList = new List<Producto>();

            if (!string.IsNullOrWhiteSpace(CriterioTextBox.Text)) {

                switch (FiltroComboBox.SelectedIndex) {
                    case 0:  //todo
                        productoList = ProductosBLL.GetList(p => true);
                        break;

                    case 1: //id
                        int.TryParse(CriterioTextBox.Text , out int productoId);
                        productoList = ProductosBLL.GetList(p => p.ProductoId == productoId);
                        break;

                    case 2:  //Descripción
                        productoList = ProductosBLL.GetList(p => p.Descripcion.Contains(CriterioTextBox.Text));
                        break;
                }


            } else {

                productoList = ProductosBLL.GetList(p => true);

            }

            ResultadosDataGrid.ItemsSource = productoList;

        }
    }
}
