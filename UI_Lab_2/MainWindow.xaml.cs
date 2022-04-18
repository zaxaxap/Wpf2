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

using System.Globalization;


namespace UI_Lab_2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewData data { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            data = new ViewData();
            winFormsHost.Child = data.chart;

            this.mdParam.DataContext = data.sData.md;
            this.spParam.DataContext = data.sData.sp;
            this.mdList.ItemsSource = data.sData.md.viewXY;
            this.spDeriv1List.ItemsSource = data.sData.viewDerivSpline1;
            this.spDeriv2List.ItemsSource = data.sData.viewDerivSpline2;

            this.DataContext = data;
        }
        private void button_calc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                data.sData.md.calc_grid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void button_draw_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                data.sData.calculate_splines();
                data.DrawChart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
    [ValueConversion(typeof(Double[]), typeof(String))]
    public class StringToDoubleSConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double[] val = (double[])value;
            return $"{val[0]};{val[1]}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string[] words = ((string)value).Split(';');
                if (words.Length != 2)
                    return new double[2] { 0.0, 0.0 };
                double[] values = new double[2];
                values[0] = double.Parse(words[0]);
                values[1] = double.Parse(words[1]);
                return values;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
