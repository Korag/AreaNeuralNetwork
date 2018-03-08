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
    /// Interaction logic for pointValueUserControl.xaml
    /// </summary>
    public partial class pointValueUserControl : UserControl
    {
        public static readonly DependencyProperty TextProperty1 =
        DependencyProperty.Register("Text1", typeof(String),
        typeof(pointValueUserControl), new FrameworkPropertyMetadata(string.Empty));

        public String Text1
        {
            get { return GetValue(TextProperty1).ToString(); }
            set { SetValue(TextProperty1, value); }
        }

        public static readonly DependencyProperty TextProperty2 =
        DependencyProperty.Register("Text2", typeof(String),
        typeof(pointValueUserControl), new FrameworkPropertyMetadata(string.Empty));
        public String Text2
        {
            get { return GetValue(TextProperty2).ToString(); }
            set { SetValue(TextProperty2, value); }

        }

        public static readonly DependencyProperty TextProperty3 =
        DependencyProperty.Register("Text3", typeof(String),
        typeof(pointValueUserControl), new FrameworkPropertyMetadata(string.Empty));
        public String Text3
        {
            get { return GetValue(TextProperty3).ToString(); }
            set { SetValue(TextProperty3, value); }
        }

        public pointValueUserControl()
        {
            InitializeComponent();
        }
       

    }
}
