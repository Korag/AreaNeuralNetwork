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
    /// Interaction logic for RadioButtonsUserControl.xaml
    /// </summary>
    public partial class RadioButtonsUserControl : UserControl
    {
       

        public RadioButtonsUserControl()
        {
            InitializeComponent();
        }
        public static readonly RoutedEvent CheckEvent1 =
        EventManager.RegisterRoutedEvent("Checked1", RoutingStrategy.Bubble,
        typeof(RoutedEventHandler), typeof(RadioButtonsUserControl));

        public event RoutedEventHandler Checked1
        {
            add { AddHandler(CheckEvent1, value); }
            remove { RemoveHandler(CheckEvent1, value); }
        }
        private void CheckedRadio1(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CheckEvent1));
        }


    //    public static readonly RoutedEvent CheckEvent2 =
    //  EventManager.RegisterRoutedEvent("Checked2", RoutingStrategy.Bubble,
    //  typeof(RoutedEventHandler), typeof(RadioButtonsUserControl));

    //    public event RoutedEventHandler Checked2
    //    {
    //        add { AddHandler(CheckEvent2, value); }
    //        remove { RemoveHandler(CheckEvent2, value); }
    //    }
    //    private void CheckedRadio2(object sender, RoutedEventArgs e)
    //    {
    //        RaiseEvent(new RoutedEventArgs(CheckEvent2));
    //    }


    //    public static readonly RoutedEvent CheckEvent3 =
    //EventManager.RegisterRoutedEvent("Checked3", RoutingStrategy.Bubble,
    //typeof(RoutedEventHandler), typeof(RadioButtonsUserControl));

    //    public event RoutedEventHandler Checked3
    //    {
    //        add { AddHandler(CheckEvent3, value); }
    //        remove { RemoveHandler(CheckEvent3, value); }
    //    }

    //    private void CheckedRadio3(object sender, RoutedEventArgs e)
    //    {
    //        RaiseEvent(new RoutedEventArgs(CheckEvent3));
    //    }

    //    public static readonly RoutedEvent CheckEvent4 =
    //EventManager.RegisterRoutedEvent("Checked4", RoutingStrategy.Bubble,
    //typeof(RoutedEventHandler), typeof(RadioButtonsUserControl));

    //    public event RoutedEventHandler Checked4
    //    {
    //        add { AddHandler(CheckEvent4, value); }
    //        remove { RemoveHandler(CheckEvent4, value); }
    //    }
    //    private void CheckedRadio4(object sender, RoutedEventArgs e)
    //    {
    //        RaiseEvent(new RoutedEventArgs(CheckEvent4));
    //    }

    }
}
