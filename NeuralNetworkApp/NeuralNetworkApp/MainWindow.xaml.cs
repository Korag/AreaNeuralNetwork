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
        private List<int> NumberOfPointsList = new List<int>();
        public List<int[]> PointsList;
        public List<int[]> WeightsList;
        private List<int> resultList;
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
            FillTheList();           
            InitializeComponent();
            numberOfPointsComboBox.ItemsSource = NumberOfPointsList;
            CurrentIterationTextBlock.DataContext = this;
        }

        private void numberOfPointsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangePointsWeightsAndDValuesVisibility();
            ChangeChartsVisibility();
            SelectPointOptionFromRadioBoxes();
            
            SelectWeightOptionFromRadioBoxes();       
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Iteration = 0;
            SavePointsAndWeightsValuesToArray();
            StopButton.IsEnabled = true;
            StartButton.IsEnabled = false;

            MainCalculations();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            StartButton.IsEnabled = true;
            StopButton.IsEnabled = false;
        }

        private void ChangePointsWeightsAndDValuesVisibility()
        {           
            foreach (pointValueUserControl item in pointsWrapPanel.Children)
            {
               item.Visibility = Visibility.Hidden;
            }
            foreach (pointValueUserControl item in weightsWrapPanel.Children)
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
        private void ChangeChartsVisibility()
        {
            foreach (LiveChartUserControl item in ChartsStackPanel.Children)
            {
                item.Visibility = Visibility.Hidden;
            }

            for (int i = 0; i < Convert.ToInt32(numberOfPointsComboBox.SelectedItem); i++)
            {
                var tempList = ChartsStackPanel.Children[i];
                tempList.Visibility = Visibility.Visible;
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
            
            for (int i = 0; i < Convert.ToInt32(numberOfPointsComboBox.SelectedItem); i++)
            {
                int[] tempPointArray = new int[3];
                int[] tempWeightArray = new int[3];
                var tempList = pointsWrapPanel.Children[i] as pointValueUserControl;//punkty
                var tempList2 = weightsWrapPanel.Children[i] as pointValueUserControl;//wagi

                    tempPointArray[0] = Convert.ToInt32(tempList.pointValueTextBox1.Text);
                    tempPointArray[1] = Convert.ToInt32(tempList.pointValueTextBox2.Text);
                    tempPointArray[2] = Convert.ToInt32(tempList.pointValueTextBox3.Text);

                    tempWeightArray[0] = Convert.ToInt32(tempList2.pointValueTextBox1.Text);
                    tempWeightArray[1] = Convert.ToInt32(tempList2.pointValueTextBox2.Text);
                    tempWeightArray[2] = Convert.ToInt32(tempList2.pointValueTextBox3.Text);

                PointsList.Add(tempPointArray);
                WeightsList.Add(tempWeightArray);
            }

        }

        private void MainCalculations()
        {
            int C = Convert.ToInt32(ConstCTextBox.Text);
            int SleepTimer = Convert.ToInt32(SleepTimerTextBox.Text);
            var CurrentPoint = PointsList[0];
            while (Iteration < Convert.ToInt32(MaxIetrationsTextBox.Text))
            {
                
                var Iteration2 = Iteration % Convert.ToInt32(numberOfPointsComboBox.SelectedItem);
                UpdateTextBox(Iteration);//wyświetlanie wiadomości na konsoli
                CurrentPoint = PointsList[Iteration2];//wybór aktualnego punktu
                SgnFunction(CurrentPoint, WeightsList);//funkcja aktywacji
                ChangeWeightIfNeeded(Iteration);//zmiana wag
                
                Iteration++;
                CurrentIterationTextBlock.Text = Iteration.ToString();


                // Thread.Sleep(SleepTimer * 1000);               
            }
            StartButton.IsEnabled = true;
                      
        }

        private List<int> SgnFunction(int[] Point, List<int[]> WeightsList)//jeden punkt przemnożony przez każdą wagę
        {
            resultList = new List<int>();
            int sum = 0;
            for (int i = 0; i < WeightsList.Count; i++)
            {
                var Weight = WeightsList[i];
                for (int j = 0; j < Weight.Length; j++)
                {
                    sum += Point[j] * Weight[j];                    
                }
                if (sum<=0)
                {
                    resultList.Add(-1);
                }
                else
                {
                    resultList.Add(1);
                }
                sum = 0;
            }
            return resultList;
        }

        private void ChangeWeightIfNeeded(int MainIteration)
        {
            var Iteration = MainIteration % Convert.ToInt32(numberOfPointsComboBox.SelectedItem);
            //musi tak byc w przypadku gdyby 1 z Y pokrywała 
            List<int> tempDValueList = new List<int>();
            for (int i = 0; i < Convert.ToInt32(numberOfPointsComboBox.SelectedItem); i++)
            {
                if (i == Iteration)
                {
                    tempDValueList.Add(1);
                }
                else
                {
                    tempDValueList.Add(-1);
                }                
            }
            for (int i = 0; i < Convert.ToInt32(numberOfPointsComboBox.SelectedItem); i++)
            {
                if (resultList[i] != tempDValueList[i])
                {
                    var Weight = WeightsList[i];
                    var Point = PointsList[Iteration];
                    for (int j = 0; j < Weight.Length; j++)
                    {
                        Weight[j] =(int)(Weight[j] + (1.0/2.0) * Convert.ToInt32(ConstCTextBox.Text) * (tempDValueList[i] - resultList[i]) * Point[j]);
                    }
                    WeightsList[i] = Weight;

                }
            }

            LiveChartUserControl graph = new LiveChartUserControl();
            graph.Draw(PointsList,WeightsList);


        }

        private void UpdateTextBox(int MainIteration)
        { 
            ConsoleTextBox.Text += "Iteration: " + (MainIteration + 1) + "\n\r";
            string pointsString = "";
            for (int i = 0; i < PointsList.Count; i++)
            {
                var Point = PointsList[i];
                var Weight = WeightsList[i];
                foreach (var item in Point)
                {
                    pointsString += item+" ";
                }
                ConsoleTextBox.Text += "P" + (i + 1) + " [ " + pointsString + "] ";
                pointsString = "";
                foreach (var item in Weight)
                {
                    pointsString += item + " ";
                }
                ConsoleTextBox.Text += "W" + (i + 1) + " [ " + pointsString + "] " + "\n\r";
                pointsString = "";
            }
            
        }

        private void WeightRadioButtons_Checked1(object sender, RoutedEventArgs e)
        {
            
            SelectWeightOptionFromRadioBoxes();
        }

        private void PointRadioButtons_Checked1(object sender, RoutedEventArgs e)
        {
            SelectPointOptionFromRadioBoxes();
        }

        private void LiveChartUserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void LiveChartUserControl_Loaded_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
