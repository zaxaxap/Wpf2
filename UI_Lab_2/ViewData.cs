using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClassLibrary;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Forms.DataVisualization.Charting;

namespace UI_Lab_2
{
    public class ViewData
    {
        public Chart chart { get; set; }
        public ChartData chartData { get; set; }
        public SplinesData sData { get; set; }
        public ViewData()
        {
            MeasuredData md = new MeasuredData();
            SplineParameters sp = new SplineParameters();
            sData = new SplinesData(new MeasuredData(), new SplineParameters());

            chart = new Chart();
            chartData = new ChartData(sData);
        }
        public void DrawChart()
        {
            chartData.DrawChart(chart);
        }
    }
}
