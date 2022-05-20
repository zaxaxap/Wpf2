using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Forms.DataVisualization.Charting;
using System.ComponentModel;

namespace Wpf_Lab2_v3
{
    public class ViewData: IDataErrorInfo, INotifyPropertyChanged
    {
        public SplinesData sData { get; set; }
        public Chart chart { get; set; }
        public ChartData chartData { get; set; }

        public string Error => throw new NotImplementedException();

        public string this[string columnName] => throw new NotImplementedException();

        public ViewData()
        {
            MeasuredData md = new MeasuredData();
            SplineParameters sp = new SplineParameters();
            sData = new SplinesData(new MeasuredData(), new SplineParameters());

            chart = new Chart();
            chartData = new ChartData(sData);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void DrawChart()
        {
            chartData.DrawChart(chart);
        }
    }
}
