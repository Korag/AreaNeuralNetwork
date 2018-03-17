using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using NeuralNetworkApp.View.UserControls;
namespace NeuralNetworkApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        private  List<int> NumberOfPointsList = new List<int>();
        private List<int[]> PointsList;
        private List<int[]> WeightsList;
        public int Iteration { get; set; }
        private void FillTheList()
        {
            for (int i = 3; i <= 6; i++)
            {
                NumberOfPointsList.Add(i);
                
            }
        }
        public MainWindow()
        {
            Iteration = 0;
            FillTheList();           
            InitializeComponent();
            numberOfPointsComboBox.ItemsSource = NumberOfPointsList;
            CurrentIterationTextBlock.DataContext = this;
            //ver 2.1

        }

        private void numberOfPointsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangePointsAndWeightsVisibility();
            SelectPointOptionFromRadioBoxes();
            
            SelectWeightOptionFromRadioBoxes();
            SavePointsAndWeightsValuesToArray();
        
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {                       
            StopButton.IsEnabled = true;
            StartButton.IsEnabled = false;

            MainCalculations();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
        }

        private void ChangePointsAndWeightsVisibility()
        {
            
            foreach (pointValueUserControl item in pointsWrapPanel.Children)
            {
               item.Visibility = Visibility.Hidden;
            }

            for (int i = 0; i < Convert.ToInt32(numberOfPointsComboBox.SelectedItem); i++)
            {
                var tempList = pointsWrapPanel.Children[i] as pointValueUserControl;//punkty
                var tempList2 = weightsWrapPanel.Children[i] as pointValueUserControl;//wagi
                tempList.Visibility = Visibility.Visible;
                tempList2.Visibility = Visibility.Visible;
            }
        }

        #region PointSelection
        private void SelectPointOptionFromRadioBoxes()
        {
            //do przemyślenia
            if ((bool)PointRadioButtons.radioRandom.IsChecked)
            {
                FillThePointsWithRandomValues();
            }
            else if ((bool)PointRadioButtons.radioBook.IsChecked)
            {
                FillThePointsWithBookValues();
            }
            else if ((bool)PointRadioButtons.radioCollection.IsChecked)
            {
                FillThePointsWithCollectionValues();
            }
            else
            {
                FillThePointsWithKeyboardValues();
            }

        }

        private void FillThePointsWithRandomValues()
        {
            Random randPoint = new Random();
            for (int i = 0; i < Convert.ToInt32(numberOfPointsComboBox.SelectedItem); i++)
            {
                var tempList = pointsWrapPanel.Children[i] as pointValueUserControl;
                tempList.FirstValueText = randPoint.Next(-5, 5).ToString();
                tempList.SecondValueText = randPoint.Next(-5, 5).ToString();
            }
        }
        private void FillThePointsWithBookValues()
        {
            if (Convert.ToInt32(numberOfPointsComboBox.SelectedItem)==3)
            {

            }
            else
            {
                numberOfPointsComboBox.SelectedIndex = 0;
                MessageBox.Show("The Book contains only 3 points!");
            }
        }
        private void FillThePointsWithCollectionValues()
        {
            MessageBox.Show("Read from file");
        }
        private void FillThePointsWithKeyboardValues()
        {
           //to chyba jest do wyjebania chyba że jakieś sprawdzanie robimy?
        }
        #endregion

        #region WeightSelection
        private void SelectWeightOptionFromRadioBoxes()
        {
            //do przemyślenia
            if ((bool)WeightRadioButtons.radioRandom.IsChecked)
            {
                FillTheWeightsWithRandomValues();
            }
            else if ((bool)WeightRadioButtons.radioBook.IsChecked)
            {
                FillTheWeightsWithBookValues();
            }
            else if ((bool)WeightRadioButtons.radioCollection.IsChecked)
            {
                FillTheWeightsWithCollectionValues();
            }
            else
            {
                FillTheWeightsWithKeyboardValues();
            }

        }

        private void FillTheWeightsWithRandomValues()
        {
            Thread.Sleep(100);//żeby wartości punktow i wag nie były identyczne
            Random randWeight = new Random();
            
            for (int i = 0; i < Convert.ToInt32(numberOfPointsComboBox.SelectedItem); i++)
            {
                var tempList = weightsWrapPanel.Children[i] as pointValueUserControl;
                tempList.FirstValueText = randWeight.Next(-5, 5).ToString();
                tempList.SecondValueText = randWeight.Next(-5, 5).ToString();
            }
        }
        private void FillTheWeightsWithBookValues()
        {
            if (Convert.ToInt32(numberOfPointsComboBox.SelectedItem) == 3)
            {

            }
            else
            {
                numberOfPointsComboBox.SelectedIndex = 0;
                MessageBox.Show("The Book contains only 3 points!");
            }
        }
        private void FillTheWeightsWithCollectionValues()
        {
            MessageBox.Show("Read from file");
        }
        private void FillTheWeightsWithKeyboardValues()
        {
            //to chyba jest do wyjebania chyba że jakieś sprawdzanie robimy?
        }
        #endregion

        private void SavePointsAndWeightsValuesToArray()
        {
            PointsList = new List<int[]>();
            WeightsList = new List<int[]>();
            int[] tempPointArray = new int[3];
            int[] tempWeightArray = new int[3];
            for (int i = 0; i < Convert.ToInt32(numberOfPointsComboBox.SelectedItem); i++)
            {
                var tempList = pointsWrapPanel.Children[i] as pointValueUserControl;//punkty
                var tempList2 = weightsWrapPanel.Children[i] as pointValueUserControl;//wagi

                    tempPointArray[0] = Convert.ToInt32(tempList.pointValueTextBox1.Text);
                    tempPointArray[1] = Convert.ToInt32(tempList.pointValueTextBox1.Text);
                    tempPointArray[2] = Convert.ToInt32(tempList.pointValueTextBox1.Text);

                    tempWeightArray[0] = Convert.ToInt32(tempList2.pointValueTextBox1.Text);
                    tempWeightArray[1] = Convert.ToInt32(tempList2.pointValueTextBox1.Text);
                    tempWeightArray[2] = Convert.ToInt32(tempList2.pointValueTextBox1.Text);

                PointsList.Add(tempPointArray);
                WeightsList.Add(tempWeightArray);
            }

        }

        private void MainCalculations()
        {           
            int C = Convert.ToInt32(ConstCTextBox.Text);
            int SleepTimer = Convert.ToInt32(SleepTimerTextBox.Text);

            while (Iteration <= Convert.ToInt32(MaxIetrationsTextBox.Text))
            {



                Iteration++;
                CurrentIterationTextBlock.Text = Iteration.ToString();
               // Thread.Sleep(SleepTimer * 1000);               
            }
            StartButton.IsEnabled = true;
                      
        }

        private void WeightRadioButtons_Checked1(object sender, RoutedEventArgs e)
        {
            
            SelectWeightOptionFromRadioBoxes();
        }

        private void PointRadioButtons_Checked1(object sender, RoutedEventArgs e)
        {
            SelectPointOptionFromRadioBoxes();
        }
    }
}
