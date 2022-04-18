using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClassLibrary;
using System.Windows.Forms.DataVisualization.Charting;

namespace UI_Lab_2
{
    public class ChartData
    {
        SplinesData sData { get; set; }

        static System.Drawing.Color[] Colors =
                 new System.Drawing.Color[] { System.Drawing.Color.Red,
                                            System.Drawing.Color.Blue,
                                            System.Drawing.Color.Cyan,
                                            System.Drawing.Color.Magenta,
                                            System.Drawing.Color.Green,
                                            System.Drawing.Color.Brown,
                                            System.Drawing.Color.LightBlue};
        public ChartData(SplinesData sData_)
        {
            sData = sData_;
        }
        public void DrawChart(Chart chart)
        {
            chart.ChartAreas.Clear();
            chart.Series.Clear();
            chart.Legends.Clear();
            chart.Titles.Clear();
            Legend legend = new Legend("Custom");


            chart.ChartAreas.Add(new ChartArea("chartArea1"));
            chart.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chart.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chart.ChartAreas[0].AxisX.IsMarginVisible = false;
            legend.InsideChartArea = chart.ChartAreas[0].Name;

            chart.Titles.Add(new Title("Beautiful splines!"));
            chart.ChartAreas[0].AxisX.Title = "x";
            chart.ChartAreas[0].AxisY.Title = "F(x)";

            string name;
            double[] X = new double[sData.sp.cnt_nodes];
            double[] Y;
            double step = (sData.md.limits[1] - sData.md.limits[0]) / sData.sp.cnt_nodes;
            for (int i = 0; i < sData.sp.cnt_nodes; i++)
                X[i] = sData.md.limits[0] + step * i;

            for (int js = 0; js < 2; js++)
            {
                if (js == 0)
                {
                    Y = sData.values_spline1;
                    name = "First Spline";
                }
                else
                {
                    Y = sData.values_spline2;
                    name = "Second Spline";
                }
                System.Drawing.Color jsColor = Colors[js % Colors.Length];
                chart.Series.Add(js.ToString());
                chart.Series[js].Points.DataBindXY(X, Y);

                chart.Series[js].MarkerStyle = MarkerStyle.Circle;
                chart.Series[js].MarkerSize = 5;
                chart.Series[js].MarkerColor = jsColor;

                chart.Series[js].ChartType = SeriesChartType.Spline;
                chart.Series[js].Color = jsColor;

                chart.Series[js].IsVisibleInLegend = false;
                LegendItem legendItem = new LegendItem();
                legendItem.Name = name;
                legendItem.Color = jsColor;
                legend.CustomItems.Add(legendItem);
            }
            chart.Legends.Add(legend);
        }
    }
}
