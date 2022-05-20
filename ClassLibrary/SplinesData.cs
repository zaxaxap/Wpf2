using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.IO;

namespace Wpf_Lab2_v3
{
    public class SplinesData
    {
        public MeasuredData MData { get; set; }
        public SplineParameters Parameters { get; set; }
        public double[] deriv1 { get; set; }
        public double[] deriv2 { get; set; }
        public double[] values1 { get; set; }
        public double[] values2 { get; set; }
        public ObservableCollection<string> Spline1Info { get; set; }
        public ObservableCollection<string> Spline2Info { get; set; }
        public SplinesData(MeasuredData MData, SplineParameters Parameters)
        {
            this.MData = MData;
            this.Parameters = Parameters;

            deriv1 = new double[4];
            deriv2 = new double[4];

            Spline1Info = new ObservableCollection<string>();
            Spline2Info = new ObservableCollection<string>();

            calculate_splines();
        }
        public void calculate_splines()
        {
            Spline1Info.Clear();
            Spline2Info.Clear();

            values1 = new double[Parameters.nodes];
            values2 = new double[Parameters.nodes];

            double[] all_values_spline1 = new double[Parameters.nodes * 2];
            double[] all_values_spline2 = new double[Parameters.nodes * 2];

            int ret = 0;
            double[] limits = new double[2] { MData.llimits, MData.rlimits };
            global_func(MData.nodes, 1, Parameters.nodes, limits, MData.y, Parameters.Right_Left_1, all_values_spline1, ref ret);
            if (ret != 0)
                throw new Exception($"Error {ret} global_func 1");

            ret = 0;
            global_func(MData.nodes, 1, Parameters.nodes, limits, MData.y, Parameters.Right_Left_2, all_values_spline2, ref ret);
            if (ret!= 0)
                throw new Exception($"Error {ret} global_func 2");
            
            for (int i=0; i < Parameters.nodes * 2; i++)
            {
                if (i % 2 == 0)
                {
                    values1[i / 2] = all_values_spline1[i];
                    values2[i / 2] = all_values_spline2[i];
                }
            }
            deriv_eq(deriv1, all_values_spline1);
            deriv_eq(deriv2, all_values_spline2);

            double step = (MData.rlimits - MData.llimits) / MData.nodes;
            double[] points = new double[] { MData.llimits, MData.llimits+step, MData.rlimits-step, MData.rlimits };
            string[] output = new string[] { "a", "a+h", "b-h", "b" };
            

            for (int i=0; i < 4; i++)
            {
                Spline1Info.Add("Производная в " + output[i] + $"={points[i]:F3} " + $"равна {deriv1[i]:F3}");
                Spline2Info.Add("Производная в " + output[i] + $"={points[i]:F3} " + $"равна {deriv2[i]:F3}");
            }
        }

        public void deriv_eq(double[] deriv, double[] Vspline)
        {
            deriv[0] = Vspline[1];
            deriv[1] = Vspline[3];
            deriv[2] = Vspline[Parameters.nodes * 2 - 3];
            deriv[3] = Vspline[Parameters.nodes * 2 - 1];
        }

        [DllImport("..\\..\\..\\x64\\Debug\\CPP_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern
        int global_func(int nx, int dim, int nRend, double[] x, double[] y, double[] derivations, double[] yRend, ref int ret);
    }
}
