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
        int IDontHaveAnIdea = 0;//głupie ale na razie działa :D

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




        private void numberOfPointsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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



        private void userCtrl2_Checked1(object sender, RoutedEventArgs e)
        {
            Random rand = new Random();
            IDontHaveAnIdea++;
            if (IDontHaveAnIdea==2)
            {
                StartButton.IsEnabled = true;
                IDontHaveAnIdea = 0;
            }

            

            for (int i = 0; i < Convert.ToInt32(numberOfPointsComboBox.SelectedItem); i++)
            {
                var tempList = pointsWrapPanel.Children[i] as pointValueUserControl;
                tempList.FirstValueText = rand.Next(-5, 5).ToString();
                tempList.SecondValueText = rand.Next(-5, 5).ToString();
            }
        }

        private void userCtrl1_Checked1(object sender, RoutedEventArgs e)
        {
            IDontHaveAnIdea++;
            if (IDontHaveAnIdea == 2)
            {
                StartButton.IsEnabled = true;
                IDontHaveAnIdea = 0;
            }

        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            StopButton.IsEnabled = true;
            StartButton.IsEnabled = false;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
        }

        
    }
}
