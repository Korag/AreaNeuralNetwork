using LiveCharts;
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
 
        public LiveChartUserControl()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                },
                new LineSeries
                {
                    Title = "Series 2",
                    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
                    PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Series 3",
                    Values = new ChartValues<double> { 4,2,7,2,7 },
                    PointGeometry = DefaultGeometries.Square,
                    PointGeometrySize = 15
                }
            };

            Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            YFormatter = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
            SeriesCollection.Add(new LineSeries
            {
                Title = "Series 4",
                Values = new ChartValues<double> { 5, 3, 2, 4 },
                LineSmoothness = 0, //0: straight lines, 1: really smooth lines
                PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
                PointGeometrySize = 50,
                PointForeground = Brushes.Gray
            });

            //modifying any series values will also animate and update the chart
            SeriesCollection[3].Values.Add(5d);
            DataContext = this;
        }


        public void Draw(List<int[]> PointsList, List<int[]> WeightsList)
        {
            InitializeComponent();

            for (int i = 0; i < PointsList.Count; i++)
            {
                SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = $"P{i+1}",
                    Values = new ChartValues<double>{ (double)(PointsList[i].GetValue(0))}
                },
            };
                Labels = new[] { (string)(PointsList[i].GetValue(1)) };
            }

            for (int i = 0; i < WeightsList.Count; i++)
            {
                SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = $"W{i+1}",
                    Values = new ChartValues<double>
                     {

                        // tutaj trzeba jakos ta prosta wyciagnac
                         (double)(WeightsList[i].GetValue(0))

                     }
                 }
            };
                Labels = new[] { (string)(WeightsList[i].GetValue(1)) };
            }

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
    }
    }

