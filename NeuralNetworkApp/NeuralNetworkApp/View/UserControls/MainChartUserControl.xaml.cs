using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Charts.Base;
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
    /// Interaction logic for MainChartUserControl.xaml
    /// </summary>
    public partial class MainChartUserControl : UserControl
    {
        private List<double> YValuesForFirstFunction = new List<double>();
        private List<double> YValuesForSecondFunction = new List<double>();
        private List<double> YValuesForThirdFunction = new List<double>();
        private List<double> XValues = new List<double>();

        public static readonly DependencyProperty ChartName =
        DependencyProperty.Register("MainChartTitle", typeof(String),
        typeof(MainChartUserControl), new FrameworkPropertyMetadata(string.Empty));
        public String MainChartTitle
        {
            get { return GetValue(ChartName).ToString(); }
            set { SetValue(ChartName, value); }

        }
        public MainChartUserControl()
        {
            InitializeComponent();
            FillXValues();
            YFormatter = value => value.ToString();
        }

        private void FillXValues()
        {
            for (double i = -1; i < 1; i += 0.1)
            {
                XValues.Add(Math.Round(i,1));
            }
        }

        public void FillYValues(int[] Weight1, int[] Weight2, int[] Weight3)
        {
            double YValue1 = 0;
            double YValue2 = 0;
            double YValue3 = 0;
            for (int i = 0; i < XValues.Count; i++)
            {
                if (Weight1[1] != 0)
                {
                    YValue1 = -(Weight1[0] * XValues[i] - Weight1[2]) / (Weight1[1]);
                }
                if (Weight2[1] != 0)
                {
                    YValue2 = -(Weight2[0] * XValues[i] - Weight2[2]) / (Weight2[1]);
                }
                if (Weight2[1] != 0)
                {
                    YValue3 = -(Weight3[0] * XValues[i] - Weight3[2]) / (Weight3[1]);
                }
                
                YValuesForFirstFunction.Add(Math.Round(YValue1,2));               
                YValuesForSecondFunction.Add(Math.Round(YValue2,2));                
                YValuesForThirdFunction.Add(Math.Round(YValue3,2));
            }
        }
        private SeriesCollection MakeMainChart()
        {

            SeriesCollection seriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Function 1",
                    Values = YValuesForFirstFunction.AsChartValues(),
                    Stroke = Brushes.Green,
                    
                },
                new LineSeries
                {
                    Title = "Function 2",
                    Values = YValuesForSecondFunction.AsChartValues(),
                },
                new LineSeries
                {
                    Title = "Function 3",
                    Values = YValuesForThirdFunction.AsChartValues(),
                }
            };

            return seriesCollection;
        }


        public void DrawMainChart()
        {
            SeriesCollection = MakeMainChart();

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
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
    }
}
