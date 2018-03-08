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
using NeuralNetworkApp.View.UserControls;
namespace NeuralNetworkApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private  List<int> NumberOfPointsList = new List<int>();
        private void FillTheList()
        {
            for (int i = 1; i <= 6; i++)
            {
                NumberOfPointsList.Add(i);
            }
        }
        public MainWindow()
        {
            FillTheList();
            
            InitializeComponent();
            numberOfPointsComboBox.ItemsSource = NumberOfPointsList;
            //ver 2.1

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (pointValueUserControl item in pointsWrapPanel.Children)
            {
                item.Visibility = Visibility.Hidden;
            }

            for (int i = 0; i < Convert.ToInt32(numberOfPointsComboBox.SelectedItem); i++)
            {
                var tempList = pointsWrapPanel.Children[i] as pointValueUserControl;
                tempList.Visibility = Visibility.Visible;
            }

            
        }
    }
}
