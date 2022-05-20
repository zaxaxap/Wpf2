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


namespace Wpf_Lab2_v3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewData data { get; set; }
        public static RoutedCommand BreakpointsC = new RoutedCommand("Break", typeof(Wpf_Lab2_v3.MainWindow));
        public static RoutedCommand DrawC = new RoutedCommand("PointsD", typeof(Wpf_Lab2_v3.MainWindow));
        public MainWindow()
        {

            InitializeComponent();



            data = new ViewData();
            winFormsHost.Child = data.chart;

            this.mdList.ItemsSource = data.sData.MData.XYinfo;
            this.spDeriv1List.ItemsSource = data.sData.Spline1Info;
            this.spDeriv2List.ItemsSource = data.sData.Spline2Info;

            this.Function.DataContext = data.sData.MData;
            this.Breakpoints.DataContext = data.sData.MData;
            this.Start.DataContext = data.sData.MData;
            this.End.DataContext = data.sData.MData;

            this.Points.DataContext = data.sData.Parameters;
            this.DStart1.DataContext = data.sData.Parameters;
            this.DStart2.DataContext = data.sData.Parameters;
            this.DEnd1.DataContext = data.sData.Parameters;
            this.DEnd2.DataContext = data.sData.Parameters;

        }

        private void Breakpoints_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                data.sData.MData.calc_grid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void Draw_Button_Click(object sender, RoutedEventArgs e)
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
        private void CanBreakpoints(object sender, CanExecuteRoutedEventArgs e)
        {
            if (Validation.GetHasError(Breakpoints) || Validation.GetHasError(Start) || Validation.GetHasError(End)) 
            {
                e.CanExecute = false;
            }
            else
                e.CanExecute = true;
        }
        private void CanDraw(object sender, CanExecuteRoutedEventArgs e)
        {
            if(Validation.GetHasError(Points))
            {
                e.CanExecute = false;
            }
            else
                e.CanExecute = true;
        }
        private void DoBreakpoints(object sender, ExecutedRoutedEventArgs e)
        {
            Breakpoints_Button_Click(sender, e);
        }
        private void DoDraw(object sender, ExecutedRoutedEventArgs e)
        {
            Draw_Button_Click(sender, e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}