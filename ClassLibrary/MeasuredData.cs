using System;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ClassLibrary
{
    public enum SPf { cubic, Func, randFunc }
    public class MeasuredData : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property_name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
        private int __cnt_nodes;
        public int cnt_nodes
        {
            get
            {
                return __cnt_nodes;
            }
            set
            {
                __cnt_nodes = value;
                OnPropertyChanged("cnt_nodes");
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
        public SPf function { get; set; }
        public double[] x { get; private set; } //trash
        public double[] y { get; private set; }
        public ObservableCollection<string> viewXY { get; set; }
        public MeasuredData(int cnt_nodes_ = 5, double left = 0, double right = 1, SPf function_ = SPf.cubic)
        {
            cnt_nodes = cnt_nodes_;
            limits = new double[2] { left, right };
            function = function_;
            viewXY = new ObservableCollection<string>();

            if (cnt_nodes < 2)
                throw new Exception("Count of nodes must be more 1");

            calc_grid();
        }
        public void calc_grid()
        {
            if (cnt_nodes < 2)
                throw new Exception("Count of nodes must be more 1");
            viewXY.Clear();
            x = new double[cnt_nodes];
            y = new double[cnt_nodes];

            Random rnd = new Random(42);
            double step = (limits[1] - limits[0]) / cnt_nodes;
            Func<double, double> func = x => x;

            if (function == SPf.cubic)
                func = (x) => (x * x * x + x * x + 1);
            if (function == SPf.Func)
                func = (x) => Math.Sin(x);
            if (function == SPf.randFunc)
                func = (x) => x * rnd.NextDouble();

            for (int i = 0; i < cnt_nodes; i++)
            {
                x[i] = limits[0] + i * step;
                y[i] = func(x[i]);
                viewXY.Add($"x[{i}]={x[i]:F3}, y[{i}]={y[i]:F3}");
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
                    case "cnt_nodes":
                        if (cnt_nodes < 2) msg = "Number of breakpoints must be more 1!";
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
    }
}
