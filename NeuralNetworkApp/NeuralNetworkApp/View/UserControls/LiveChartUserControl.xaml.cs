using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
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

namespace NeuralNetworkApp.View.UserControls
{
    /// <summary>
    /// Interaction logic for LiveChartUserControl.xaml
    /// </summary>
    public partial class LiveChartUserControl : UserControl
    {
        //to jest dla wag
        private List<double> FirstWeightList = new List<double>();
        private List<double> SecondWeightList = new List<double>();
        private List<double> ThirdWeightList = new List<double>();
        private List<string> IterationsList = new List<string>();
        //to jest dla glownego wykresu
        private List<double> YValuesForFirstFunction = new List<double>();
        private List<double> YValuesForSecondFunction = new List<double>();
        private List<double> YValuesForThirdFunction = new List<double>();
        private List<double> XValues = new List<double>();
        


        private void FillXValues()
        {
            for (double i = -1; i < 1; i+=0.1)
            {                
                XValues.Add(i);
            }
        }

        public void FillYValues(int[] Weight1, int[] Weight2, int[] Weight3)
        {
            for (int i = 0; i < XValues.Count; i++)
            {
                double YValue1 = -(Weight1[0] * XValues[i] - Weight1[2])/(Weight1[1]);

                YValuesForFirstFunction.Add(YValue1);

                double YValue2 = -(Weight2[0] * XValues[i] - Weight2[2])/(Weight2[1]);

                YValuesForSecondFunction.Add(YValue2);

                double YValue3 = -(Weight3[0] * XValues[i] - Weight3[2])/(Weight3[1]);

                YValuesForThirdFunction.Add(YValue3);
            }
        }

        private SeriesCollection DrawMainChart()
        {

            SeriesCollection seriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Weight1",
                    Values = YValuesForFirstFunction.AsChartValues()

                },
                new LineSeries
                {
                    Title = "Weight2",
                    Values = YValuesForSecondFunction.AsChartValues(),
                },
                new LineSeries
                {
                    Title = "Weight3",
                    Values = YValuesForThirdFunction.AsChartValues(),
                }
            };

            return seriesCollection;
        }

        public void MakeMainGraph()
        {
            SeriesCollection = DrawMainChart();

            Labels = ConvertFromDoubleToString();
            DataContext = this;
        }

        private string[] ConvertFromDoubleToString()
        {
            string[] TempArray = new string[XValues.Count];

            for (int i = 0; i < XValues.Count; i++)
            {
                TempArray[i] = XValues[i].ToString();
            }
            return TempArray;
        }

        public LiveChartUserControl()
        {
            InitializeComponent();
            FillXValues();
            YFormatter = value => value.ToString();

            //modifying the series collection will animate and update the chart


            //modifying any series values will also animate and update the chart
            
        }

        public void AddToHistory(int[] Weight, int Iteration)
        {
            FirstWeightList.Add(Weight[0]);
            SecondWeightList.Add(Weight[1]);
            ThirdWeightList.Add(Weight[2]);
            IterationsList.Add((Iteration+1).ToString());
        }

        public void MakeGraph()
        {
            SeriesCollection = DrawTest();
            
            Labels = IterationsList.ToArray();
            DataContext = this;
        }

        private SeriesCollection DrawTest()
        {

            SeriesCollection seriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Weight1",
                    Values = FirstWeightList.AsChartValues()

                },
                new LineSeries
                {
                    Title = "Weight2",
                    Values = SecondWeightList.AsChartValues(),
                },
                new LineSeries
                {
                    Title = "Weight3",
                    Values = ThirdWeightList.AsChartValues(),
                }
            };

            return seriesCollection;
        }



        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
    }
    }

