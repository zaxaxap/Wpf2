using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Windows.Forms.DataVisualization.Charting;

namespace Wpf_Lab2_v3
{
    public class ChartData
    {
        SplinesData sData { get; set; }

        static System.Drawing.Color[] Colors =
                 new System.Drawing.Color[] { System.Drawing.Color.Red,
                                            System.Drawing.Color.Blue,
                                            System.Drawing.Color.Green };
        public ChartData(SplinesData sData)
        {
            this.sData = sData;
        }
        public void DrawChart(Chart chart)
        {
            string name;
            double[] X = new double[sData.Parameters.nodes];
            double[] Y;
            double step = (sData.MData.rlimits - sData.MData.llimits) / (sData.Parameters.nodes - 1);

            chart.ChartAreas.Clear();
            chart.Titles.Clear();
            chart.Legends.Clear();
            chart.Series.Clear();

            Legend legend = new Legend("Custom");

            chart.ChartAreas.Add(new ChartArea("chartArea1"));
            chart.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Black;
            chart.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Black;


            chart.Titles.Add(new Title("Splines"));
            chart.ChartAreas[0].AxisX.Title = "X";
            chart.ChartAreas[0].AxisY.Title = "Y";


            for (int i = 0; i < sData.Parameters.nodes; i++)
                X[i] = sData.MData.llimits + step * i;

            for (int js = 0; js < 2; js++)
            {
                if (js == 0)
                {
                    Y = sData.values1;
                    name = "First";
                }
                else
                {
                    Y = sData.values2;
                    name = "Second";
                }
                System.Drawing.Color jsColor = Colors[js];
                chart.Series.Add(js.ToString());
                chart.Series[js].Points.DataBindXY(X, Y);

                chart.Series[js].ChartType = SeriesChartType.Spline;
                chart.Series[js].Color = jsColor;

                chart.Series[js].IsVisibleInLegend = false;
                LegendItem legendItem = new LegendItem();
                legendItem.Name = name;
                legendItem.Color = jsColor;
                legend.CustomItems.Add(legendItem);
            }

            X = sData.MData.x;
            Y = sData.MData.y;
            chart.Series.Add("Breakpoints");
            chart.Series[2].Points.DataBindXY(X, Y);

            chart.Series[2].MarkerStyle = MarkerStyle.Cross;
            chart.Series[2].MarkerSize = 5;
            chart.Series[2].MarkerColor = Colors[2];

            chart.Series[2].ChartType = SeriesChartType.FastPoint;
            chart.Series[2].Color = Colors[2];
            chart.Legends.Add(legend);
        }
    }
}
