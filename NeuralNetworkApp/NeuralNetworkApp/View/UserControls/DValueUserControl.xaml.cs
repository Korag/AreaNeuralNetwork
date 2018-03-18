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
    /// Interaction logic for DValueUserControl.xaml
    /// </summary>
    public partial class DValueUserControl : UserControl
    {
        public static readonly DependencyProperty TextProperty1 =
       DependencyProperty.Register("DName", typeof(String),
       typeof(DValueUserControl), new FrameworkPropertyMetadata(string.Empty));

        public String DName
        {
            get { return GetValue(TextProperty1).ToString(); }
            set { SetValue(TextProperty1, value); }
        }

        public static readonly DependencyProperty TextProperty2 =
        DependencyProperty.Register("Dvalue", typeof(String),
        typeof(DValueUserControl), new FrameworkPropertyMetadata(string.Empty));
        public String Dvalue
        {
            get { return GetValue(TextProperty2).ToString(); }
            set { SetValue(TextProperty2, value); }

        }

        public DValueUserControl()
        {
            InitializeComponent();
        }
    }
}
