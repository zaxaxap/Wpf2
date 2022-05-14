﻿using System;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Wpf_Lab2_v3
{
    public enum SPf 
    { 
        Cubic,
        Func, 
        Random, 
    };
    public class MeasuredData : INotifyPropertyChanged, IDataErrorInfo
    {
        public SPf func { get; set; }
        public double[] x { get; private set; }
        public double[] y { get; private set; }
        private int __nodes;
        public int nodes
        {
            get
            {
                return __nodes;
            }
            set
            {
                __nodes = value;
                OnPropertyChanged("nodes");
            }
        }
        private double[] __limits;
        public double[] limits
        {
            get
            {
                return __limits;
            }
            set
            {
                __limits = value;
                OnPropertyChanged("limits");
            }
        }
        public ObservableCollection<string> XYinfo { get; set; }
        public MeasuredData(int nodes = 5, double left = 0, double right = 1, SPf func = SPf.Cubic)
        {
            if (nodes < 2)
            {
                throw new Exception("Nodes must be more than 1");
            }
            this.nodes = nodes;
            limits = new double[2] { left, right };
            this.func = func;
            XYinfo = new ObservableCollection<string>();

            calc_grid();
        }
        public void calc_grid()
        {

            XYinfo.Clear();
            x = new double[nodes];
            y = new double[nodes];
            double step = (limits[1] - limits[0]) / nodes;
            Func<double, double> lambda = x => x;
            if (func == SPf.Cubic)
                lambda = (x) => (x*x*x + x * x + 1);
            if (func == SPf.Func)
                lambda = (x) => Math.Cos(x);
            if (func == SPf.Random)
            {
                Random rnd = new Random(12345);
                lambda = (x) => x * rnd.NextDouble();
            }
            for (int i = 0; i < nodes; i++)
            {
                x[i] = limits[0] + i * step;
                y[i] = lambda(x[i]);
                XYinfo.Add($"X[{i}]={x[i]:F3}, Y[{i}]={y[i]:F3}");
            }
        }
        public string Error { get { return "Error"; } }
        public string this[string property]
        {
            get
            {
                string msg = null;
                switch (property)
                {
                    case "nodes":
                        if (nodes < 2) msg = "Number of breakpoints must be more 1!";
                        break;
                    case "limits":
                        if (limits[0] > limits[1]) msg = "Left limit must be equal/less right limit!";
                        break;
                    default:
                        break;
                }
                return msg;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property_name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
    }
}
