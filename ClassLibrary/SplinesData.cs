using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.IO;

namespace ClassLibrary
{
    public class SplinesData
    {
        public MeasuredData md { get; set; }
        public SplineParameters sp { get; set; }
        public double[] values_spline1 { get; set; }
        public double[] values_spline2 { get; set; }
        public double[] derivatives_spline1 { get; set; }
        public double[] derivatives_spline2 { get; set; }
        public ObservableCollection<string> viewDerivSpline1 { get; set; }
        public ObservableCollection<string> viewDerivSpline2 { get; set; }
        public SplinesData(MeasuredData md_, SplineParameters sp_)
        {
            md = md_;
            sp = sp_;

            derivatives_spline1 = new double[4];
            derivatives_spline2 = new double[4];

            viewDerivSpline1 = new ObservableCollection<string>();
            viewDerivSpline2 = new ObservableCollection<string>();

            calculate_splines();
        }
        public void calculate_splines()
        {
            viewDerivSpline1.Clear();
            viewDerivSpline2.Clear();

            values_spline1 = new double[sp.cnt_nodes];
            values_spline2 = new double[sp.cnt_nodes];

            double[] all_values_spline1 = new double[sp.cnt_nodes * 2];
            double[] all_values_spline2 = new double[sp.cnt_nodes * 2];

            int error = 0;
            mkl_func(md.cnt_nodes, 1, sp.cnt_nodes, md.limits, md.y, sp.derivatives_spline1, all_values_spline1, ref error);
            if (error != 0)
                throw new Exception($"Error {error} in mkl func for spline 1");

            error = 0;
            mkl_func(md.cnt_nodes, 1, sp.cnt_nodes, md.limits, md.y, sp.derivatives_spline2, all_values_spline2, ref error);
            if (error != 0)
                throw new Exception($"Error {error} in mkl func for spline 2");
            
            for (int i=0; i < sp.cnt_nodes * 2; i++)
            {
                if (i % 2 == 0)
                {
                    values_spline1[i / 2] = all_values_spline1[i];
                    values_spline2[i / 2] = all_values_spline2[i];
                }
            }
            derivatives_spline1[0] = all_values_spline1[1];
            derivatives_spline1[1] = all_values_spline1[3];
            derivatives_spline1[2] = all_values_spline1[sp.cnt_nodes * 2 - 3];
            derivatives_spline1[3] = all_values_spline1[sp.cnt_nodes * 2 - 1];

            derivatives_spline2[0] = all_values_spline2[1];
            derivatives_spline2[1] = all_values_spline2[3];
            derivatives_spline2[2] = all_values_spline2[sp.cnt_nodes * 2 - 3];
            derivatives_spline2[3] = all_values_spline2[sp.cnt_nodes * 2 - 1];

            double step = (md.limits[1] - md.limits[0]) / md.cnt_nodes;
            double[] points = new double[] { md.limits[0], md.limits[0]+step, md.limits[1]-step, md.limits[1] };
            string[] output = new string[] { "a", "a+h", "b-h", "b" };
            

            for (int i=0; i < 4; i++)
            {
                viewDerivSpline1.Add("Derivative in " + output[i] + $"={points[i]:F3} " + $"equals {derivatives_spline1[i]:F3}");
                viewDerivSpline2.Add("Derivative in " + output[i] + $"={points[i]:F3} " + $"equals {derivatives_spline2[i]:F3}");
            }
        }

        [DllImport("..\\..\\..\\x64\\Debug\\CPP_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern
        bool mkl_func(int nx, int dim, int nRend, double[] x, double[] y, double[] derivations, double[] yRend, ref int err);
    }
}
