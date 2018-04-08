using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Win32;
using NeuralNetworkApp.View.UserControls;

namespace NeuralNetworkApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        private List<int> NumberOfPointsList = new List<int>();
        private List<int> ValuesFromSgnFunctionList;

        private List<int[]> PointsList;
        private List<int[]> WeightsList;
        private List<int[]> DeltaList;

        public int Iteration { get; set; }

        //wpisanie do listy w xamlu elementow(wybor ilosci punktow)
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

            //nierusz
            InitializeComponent();

            //bindowanie wartosci
            numberOfPointsComboBox.ItemsSource = NumberOfPointsList;

            //to tez, jak wyzej 
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
            Iteration = 0;//reset iteracji
            ConsoleTextBox.Text = "";
            SavePointsAndWeightsValuesToArray();//to jest chyba nizej opisane
            
            StartButton.IsEnabled = false;//zeby nie klikac jak pojeba...

            MainCalculations();//glowne obliczenia opisane ponizej
        }


        //zmiana widocznosci wag i punktow w zaleznosci od wyboru ilosci punktow
        private void ChangePointsWeightsAndDValuesVisibility()
        {     
            //ukrycie wag i punktow w razie zmiany ilosci punktow z wiekszej na mniejsza
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
                var tempList = pointsWrapPanel.Children[i] as pointValueUserControl;//punkty pobrane z WrapPanela
                var tempList2 = weightsWrapPanel.Children[i] as pointValueUserControl;//wagi pobrane z WrapPanela
                tempList.Visibility = Visibility.Visible;
                tempList2.Visibility = Visibility.Visible;
            }
        }
        //zmiana widocznosci wykresow
        //tutaj gorzej z estetyka programu bo widac scrollbara pomimo że wykresów jest np tylko 3
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
        //NIE ROZWIJAC!!!
        #region PointSelection

        private void SelectPointOptionFromRadioBoxes()
        {
            
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
        

        //wartosci losowe
        private void FillThePointsWithRandomValues()
        {
            DisablePointControls();
            Random randPoint = new Random();
            for (int i = 0; i < Convert.ToInt32(numberOfPointsComboBox.SelectedItem); i++)
            {
                var tempList = pointsWrapPanel.Children[i] as pointValueUserControl;
                tempList.FirstValueText = randPoint.Next(-5, 5).ToString();
                tempList.SecondValueText = randPoint.Next(-5, 5).ToString();
            }
        }

        //wartosci z ksiazki 
        private void FillThePointsWithBookValues()
        {
            if (Convert.ToInt32(numberOfPointsComboBox.SelectedItem)!=3)
            {
                numberOfPointsComboBox.SelectedIndex = 0;
                MessageBox.Show("The Book contains only 3 points!");

            }
            else
            {
                DisablePointControls();

                var FirstPoint = pointsWrapPanel.Children[0] as pointValueUserControl;
                FirstPoint.FirstValueText = "10";
                FirstPoint.SecondValueText = "2";

                var SecondPoint = pointsWrapPanel.Children[1] as pointValueUserControl;
                SecondPoint.FirstValueText = "2";
                SecondPoint.SecondValueText = "-5";

                var ThirdPoint = pointsWrapPanel.Children[2] as pointValueUserControl;
                ThirdPoint.FirstValueText = "-5";
                ThirdPoint.SecondValueText = "5";
            }
            
        }

        //wartosci ze zbioru(w tym przypadku z pliku txt)
        private void FillThePointsWithCollectionValues()
        {            
            
            string ValuesFromCollection =  SelectCollection();

            if (ValuesFromCollection != null)
            {
                DisablePointControls();
                List<string> numbersList = TakeNumbersFromString(ValuesFromCollection);
                FillPointControls(numbersList);
            }
            else
            {
                PointRadioButtons.radioRandom.IsChecked = true;
                
            }
        }

      
        #region PointCollectionFillMethods
        private string SelectCollection()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog();
            string text = null;
            if (result == true)
            {
                string file = openFileDialog.FileName;
                try
                {
                    text = File.ReadAllText(file);

                }
                catch (IOException)
                {
                }
            }
            return text;
        }


        private void FillPointControls(List<string> numbersList)
        {
           
                int ListCounter = 0;
            try
            {
                for (int i = 0; i < Convert.ToInt32(numberOfPointsComboBox.SelectedItem); i++)
                {
                    var tempList = pointsWrapPanel.Children[i] as pointValueUserControl;

                    tempList.FirstValueText = numbersList[ListCounter];
                    ListCounter++;
                    tempList.SecondValueText = numbersList[ListCounter];
                    ListCounter++;
                    tempList.ThirdValueText = numbersList[ListCounter];
                    ListCounter++;
                }
            }
            catch
            {
                MessageBox.Show("Not enough point values in file");
                PointRadioButtons.radioRandom.IsChecked = true;
            }
        }

        private static List<string> TakeNumbersFromString(string ValuesFromCollection)
        {
            Regex regex = new Regex(@"[^-,0-9]");

            string ValuesFromCollectionAfterFiltering = regex.Replace(ValuesFromCollection, " ");

            string[] numbers = ValuesFromCollectionAfterFiltering.Split(null);

            List<string> numbersList = new List<string>();

            foreach (string value in numbers)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    numbersList.Add(value);
                }
            }

            return numbersList;
        }
        #endregion


        //wartosci z klawiatury
        private void FillThePointsWithKeyboardValues()
        {
            EnablePointControls();
            for (int i = 0; i < Convert.ToInt32(numberOfPointsComboBox.SelectedItem); i++)
            {
                var tempList = pointsWrapPanel.Children[i] as pointValueUserControl;
                tempList.FirstValueText = "0";
                tempList.SecondValueText = "0";
                tempList.ThirdValueText = "0";
            }
        }
        #endregion

        //NIE ROZWIJAC!!!
        #region WeightSelection
            
        private void SelectWeightOptionFromRadioBoxes()
        {
            
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
            DisableWeightControls();

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

            if (Convert.ToInt32(numberOfPointsComboBox.SelectedItem) != 3)
            {
                numberOfPointsComboBox.SelectedIndex = 0;
                MessageBox.Show("The Book contains only 3 points!");

            }
            else
            {
                DisableWeightControls();

                var FirstPoint = weightsWrapPanel.Children[0] as pointValueUserControl;
                FirstPoint.FirstValueText = "1";
                FirstPoint.SecondValueText = "-2";
                FirstPoint.ThirdValueText = "0";

                var SecondPoint = weightsWrapPanel.Children[1] as pointValueUserControl;
                SecondPoint.FirstValueText = "0";
                SecondPoint.SecondValueText = "-1";
                SecondPoint.ThirdValueText = "2";

                var ThirdPoint = weightsWrapPanel.Children[2] as pointValueUserControl;
                ThirdPoint.FirstValueText = "1";
                ThirdPoint.SecondValueText = "3";
                ThirdPoint.ThirdValueText = "-1";
            }
        }
        #region WeightCollectionFillMethods
        private void FillTheWeightsWithCollectionValues()
        {
            string ValuesFromCollection = SelectWeightCollection();

            if (ValuesFromCollection != null)
            {
                DisableWeightControls();
                List<string> numbersList = TakeNumbersFromString(ValuesFromCollection);
                FillWeightControls(numbersList);
            }
            else
            {
                WeightRadioButtons.radioRandom.IsChecked = true;
            }
        }

        private void FillWeightControls(List<string> numbersList)
        {
            int ListCounter = 0;
            try
            {
                for (int i = 0; i < Convert.ToInt32(numberOfPointsComboBox.SelectedItem); i++)
                {
                    var tempList = weightsWrapPanel.Children[i] as pointValueUserControl;

                    tempList.FirstValueText = numbersList[ListCounter];
                    ListCounter++;
                    tempList.SecondValueText = numbersList[ListCounter];
                    ListCounter++;
                    tempList.ThirdValueText = numbersList[ListCounter];
                    ListCounter++;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Not enough weight values in file");
                WeightRadioButtons.radioRandom.IsChecked = true;
            }
        }

        
        private string SelectWeightCollection()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var result = openFileDialog.ShowDialog(); // Show the dialog.
            string text = null;
            if (result == true) // Test result.
            {
                string file = openFileDialog.FileName;
                try
                {
                    text = File.ReadAllText(file);

                }
                catch (IOException)
                {
                }
            }
            return text;
        }
       
        #endregion

        private void FillTheWeightsWithKeyboardValues()
        {
            EnableWeightControls();
            for (int i = 0; i < Convert.ToInt32(numberOfPointsComboBox.SelectedItem); i++)
            {
                var tempList = weightsWrapPanel.Children[i] as pointValueUserControl;
                tempList.FirstValueText = "0";
                tempList.SecondValueText = "0";
                tempList.ThirdValueText= "0";
            }
            
        }
        #endregion

        //funkcja odpowiedzialna za pobranie wartości z pol(xaml) i przeslanie ich do tablic w czesci logicznej programu
        private void SavePointsAndWeightsValuesToArray()
        {
            //utworzenie tablic ale takze wyzerowanie wartosci w srodku
           
            PointsList = new List<int[]>();
            WeightsList = new List<int[]>();
            DeltaList = new List<int[]>();

            for (int i = 0; i < Convert.ToInt32(numberOfPointsComboBox.SelectedItem); i++)
            {
                //tablice tymczasowe do ktorych wpisywane sa wartosci
                int[] tempPointArray = new int[3];
                int[] tempWeightArray = new int[3];
                int[] tempDeltaArray = new int[3] { 0,0,0};

                var tempList = pointsWrapPanel.Children[i] as pointValueUserControl;//punkty pobrane z WrpaPanela
                var tempList2 = weightsWrapPanel.Children[i] as pointValueUserControl;//wagi pobrane z WrpaPanela

                //pobranie i konwersja wartosci z pol oraz zapisanie ich do tablic tymczasowych
                
                    tempPointArray[0] = Convert.ToInt32(tempList.pointValueTextBox1.Text);
                    tempPointArray[1] = Convert.ToInt32(tempList.pointValueTextBox2.Text);
                    tempPointArray[2] = Convert.ToInt32(tempList.pointValueTextBox3.Text);

                    tempWeightArray[0] = Convert.ToInt32(tempList2.pointValueTextBox1.Text);
                    tempWeightArray[1] = Convert.ToInt32(tempList2.pointValueTextBox2.Text);
                    tempWeightArray[2] = Convert.ToInt32(tempList2.pointValueTextBox3.Text);


                //przeniesienie tablic tymczasowych do list(jedna tablica tymczasowa to jeden punkt/waga)
                PointsList.Add(tempPointArray);
                WeightsList.Add(tempWeightArray);
                DeltaList.Add(tempDeltaArray);
            }

        }
        private void Timer_Tick(object sender, EventArgs e, ref int StopChecker,int[] CurrentPoint)
        {                                    
            CalculationsInsideTheLoop(ref StopChecker, CurrentPoint);

            Iteration++;
            CurrentIterationTextBlock.Text = Iteration.ToString();
            SaveHistoryOfWeightsValues();
          
            if (Iteration > Convert.ToInt32(MaxIterationsTextBox.Text) || CheckStopCondition(StopChecker))
            {
                
                MainChart.Visibility = Visibility.Visible;                
                MainChart.DrawChart(WeightsList.Count, WeightsList);

                ChartsScrollViewer.Visibility = Visibility.Visible;
                DrawWeightGraphs();

                StartButton.IsEnabled = true;
                SaveFileButton.IsEnabled = true;

                dispatcherTimer.Stop();
            }
        }
        #region Calculations
        private void MainCalculations()
        {
            int StopChecker = 0;
            //pobranie wartosci z pol oraz konwersja na int
            int C = Convert.ToInt32(ConstCTextBox.Text);
            int SleepTimer = Convert.ToInt32(SleepTimerTextBox.Text);
            var CurrentPoint = PointsList[0];
            //licznik P+1

            MainLoopEventArgs mainLoopEventArgs = new MainLoopEventArgs();

            dispatcherTimer.Tick += (object s, EventArgs a) => Timer_Tick(s, a, ref StopChecker, CurrentPoint);
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(SleepTimer*500);
            dispatcherTimer.Start();
            
        }


        private void CalculationsInsideTheLoop(ref int StopChecker, int[] CurrentPoint)
        {
            var IterationForCalculations = Iteration % Convert.ToInt32(numberOfPointsComboBox.SelectedItem);

            UpdateTextBox(Iteration, StopChecker);//wyświetlanie wiadomości na konsoli

            CurrentPoint = PointsList[IterationForCalculations];//wybór aktualnego punktu

            SgnFunction(CurrentPoint, WeightsList);//funkcja aktywacji

            ChangeWeightIfNeeded(IterationForCalculations, ref StopChecker);//zmiana wag


            
        }


        //funkcja aktywacji sgn mnozenie macierzy a dokladniej ich skladowych na podstawie sumy program decyduje czy wartość funkcji sgn jest 1 czy -1
        private List<int> SgnFunction(int[] Point, List<int[]> WeightsList)//jeden punkt przemnożony przez każdą wagę
        {
            ValuesFromSgnFunctionList = new List<int>();
            int sum = 0;
            for (int i = 0; i < WeightsList.Count; i++)
            {
                var Weight = WeightsList[i];
                for (int j = 0; j < Weight.Length; j++)
                {
                    sum += Point[j] * Weight[j];
                }
                if (sum <= 0)
                {
                    ValuesFromSgnFunctionList.Add(-1);
                }
                else
                {
                    ValuesFromSgnFunctionList.Add(1);
                }
                sum = 0;
            }
            return ValuesFromSgnFunctionList;
        }

        private void ChangeWeightIfNeeded(int MainIteration, ref int PValue)
        {
            //każda zmiana wagi jest obliczana na podstawie konkretnego punktu wybieranego na podstawie iteracji
            //dla 3 punktow iteracje musza sie resetowac gdyz nie wybierzemy punktu z indeksem 20
            //(maksymalny indeks to 3 w tym przypadku)


            /*lista wartości domyślnych(przewidywanych) zmienająca się wraz z iteracją,
            stworzona została do porównywania wartości pochodzacych z obliczeń funkcji aktywacji SGN()*/
            List<int> tempDValueList = new List<int>();
            for (int i = 0; i < Convert.ToInt32(numberOfPointsComboBox.SelectedItem); i++)
            {
                if (i == MainIteration)
                {
                    tempDValueList.Add(1);
                }
                else
                {
                    tempDValueList.Add(-1);
                }
            }

            /*porównywanie wartości pochodzących z funkcji SGN() czyli z listy resultList z wartościami domyślnymi
             * w razie niezgodnosci nastepuje zmiana wag wedlug podanego w zadaniu wzoru*/
            for (int i = 0; i < Convert.ToInt32(numberOfPointsComboBox.SelectedItem); i++)
            {

                if (ValuesFromSgnFunctionList[i] != tempDValueList[i])
                {
                    PValue = 0;
                    var Weight = WeightsList[i];//pobranie jednej wagi z listy wszystkich wag
                    var Point = PointsList[MainIteration];//pobranie jednego punktu z listy wszystkich punktow
                    var Delta = DeltaList[i];
                    for (int j = 0; j < Weight.Length; j++)
                    {
                        //każda składowa wagi jest obsługiwana osobno
                        Delta[j] = (int)((1.0 / 2.0) * Convert.ToInt32(ConstCTextBox.Text) * (tempDValueList[i] - ValuesFromSgnFunctionList[i]) * Point[j]);
                        Weight[j] = Weight[j] + Delta[j];
                    }
                    WeightsList[i] = Weight;//zamiana wagi
                    DeltaList[i] = Delta;
                }
                else
                {
                    DeltaList[i] = new int[3] { 0, 0, 0 };
                }
            }
            PValue++;

        }
        //warunek stopu jest +2 a nie +1 zeby petla jeszcze jeden raz sie wykonala
        private bool CheckStopCondition(int PValue)
        {
            if (PValue == Convert.ToInt32(numberOfPointsComboBox.SelectedItem) + 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        //glowna petla z obliczeniami, serce programu(maly ale wariat :P)


        //wypisywanie danych w oknie "messages from application" po każdej iteracji
        private void UpdateTextBox(int MainIteration, int CheckColor)
        {        
            ConsoleTextBox.Text += "Iteration: " + (MainIteration + 1) + "\r\n";
            string pointsString = "";
            for (int i = 0; i < PointsList.Count; i++)
            {
                var Point = PointsList[i];
                var Weight = WeightsList[i];
                var Delta = DeltaList[i];
                foreach (var item in Point)
                {
                    pointsString += item+" ";
                }
                ConsoleTextBox.Text += "Point " + (i + 1) + " = [ " + pointsString + "] \t";
                pointsString = "";
                foreach (var item in Weight)
                {
                    pointsString += item + " ";
                }
                ConsoleTextBox.Text += "Weight " + (i + 1) + " = [ " + pointsString + "] \t";
                pointsString = "";
                foreach (var item in Delta)
                {
                    pointsString += item + " ";
                }
                ConsoleTextBox.Text += "\u0394" + (i + 1) + " = " + pointsString + "\r\n\r\n";
                pointsString = "";
            }
            
        }

        //Zdarzenie zmieniające wartości wag po zmianie radio buttona
        private void WeightRadioButtons_Checked1(object sender, RoutedEventArgs e)
        {           
            SelectWeightOptionFromRadioBoxes();
        }

        //Zdarzenie zmieniające wartości punktów po zmianie radio buttona
        private void PointRadioButtons_Checked1(object sender, RoutedEventArgs e)
        {
            SelectPointOptionFromRadioBoxes();
        }


        //zapis do pliku
        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, "");

                string CurrentDate = "Date: " + DateTime.Now + "\r\n";
                string School = "Akademia Techniczno-Humanistyczna w Bielsku-Białej \r\n";
                string WorkingGroup = "Kamil Haręża, Łukasz Czepielik, Bartosz Wróbel, Konrad Korzonkiewicz \r\n\r\n";

                File.AppendAllText(saveFileDialog.FileName, CurrentDate);
                File.AppendAllText(saveFileDialog.FileName, School);
                File.AppendAllText(saveFileDialog.FileName, WorkingGroup);
                File.AppendAllText(saveFileDialog.FileName, ConsoleTextBox.Text);
            }
                
        }


        #region DrawChartsMethods
        private void DrawWeightGraphs()
        {
            var Weights = ChartsStackPanel.Children;

            for (int i = 0; i < Convert.ToInt32(numberOfPointsComboBox.SelectedItem); i++)
            {
                var Weight = Weights[i] as LiveChartUserControl;
                Weight.MakeChart();

            }
        }
        private void SaveHistoryOfWeightsValues()
        {
            var Weights = ChartsStackPanel.Children;

            for (int i = 0; i < Convert.ToInt32(numberOfPointsComboBox.SelectedItem); i++)
            {
                var Weight = Weights[i] as LiveChartUserControl;
                Weight.AddToHistory(WeightsList[i], Iteration);
            }
        }
        #endregion



        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();
            //System.Windows.Forms.Application.Restart();

            // Lepsiejsze
            MainWindow window2 = new MainWindow();
            window2.Show();
            
            this.Close();
            
        }

        #region DataValidation
        private void ConstCTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ConstCTextBox.Text.Equals("") || !Regex.IsMatch(ConstCTextBox.Text, @"^\-*\d+$"))
            {
                ConstCTextBox.Text = "1";
                MessageBox.Show("The C field accepts only numeric values");
            }
        }

        private void MaxIterationsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (MaxIterationsTextBox.Text.Equals("") || !Regex.IsMatch(MaxIterationsTextBox.Text, @"^\d+$"))
            {

                MaxIterationsTextBox.Text = "100";
                MessageBox.Show("The Max itrations field accepts only numeric values");
            }
        }

        private void SleepTimerTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SleepTimerTextBox.Text.Equals("") || !Regex.IsMatch(SleepTimerTextBox.Text, @"^\d+$"))
            {

                SleepTimerTextBox.Text = "1";
                MessageBox.Show("The Sleep Timer field accepts only numeric values");
            }
        }

        private void CheckIfTextIsEmptyOrContainsLetter(object sender, RoutedEventArgs e)
        {
            var TextChecker = sender as pointValueUserControl;

                //sprawdzanie osobno dla kazdego pola znajdującego się w kontrolce uzytkownika
                if (TextChecker.pointValueTextBox1.Text.Equals("") || !Regex.IsMatch(TextChecker.pointValueTextBox1.Text, @"^\-*\d+$"))
                {
                    TextChecker.pointValueTextBox1.Text = "0";
                    MessageBox.Show($"The {TextChecker.pointValueTextBlock.Text.Substring(0, 2)} is empty or contains a letter");
                }

                if (TextChecker.pointValueTextBox2.Text.Equals("") || !Regex.IsMatch(TextChecker.pointValueTextBox2.Text, @"^\-*\d+$"))
                {
                    TextChecker.pointValueTextBox2.Text = "0";
                    MessageBox.Show($"The {TextChecker.pointValueTextBlock.Text.Substring(0, 2)} is empty or contains a letter");
                }

                if (TextChecker.pointValueTextBox3.Text.Equals("") || !Regex.IsMatch(TextChecker.pointValueTextBox3.Text, @"^\-*\d+$"))
                {
                    TextChecker.pointValueTextBox3.Text = "0";
                    MessageBox.Show($"The {TextChecker.pointValueTextBlock.Text.Substring(0, 2)} is empty or contains a letter");
                }
            
        }
        #endregion



        #region Disable or enable points and weights


        private void EnablePointControls()
        {
            foreach (pointValueUserControl item in pointsWrapPanel.Children)
            {
                item.IsEnabled = true;
            }
           
        }
        private void EnableWeightControls()
        {
            foreach (pointValueUserControl item in weightsWrapPanel.Children)
            {
                item.IsEnabled = true;
            }
        }

        private void DisablePointControls()
        {
            foreach (pointValueUserControl item in pointsWrapPanel.Children)
            {
                item.IsEnabled = false;
            }
           
        }
        private void DisableWeightControls()
        {
            foreach (pointValueUserControl item in weightsWrapPanel.Children)
            {
                item.IsEnabled = false;
            }
        }
        #endregion
    }
}
