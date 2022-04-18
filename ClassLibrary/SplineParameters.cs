using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace ClassLibrary
{
    public class SplineParameters : INotifyPropertyChanged, IDataErrorInfo
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
        public double[] derivatives_spline1 { get; set; }
        public double[] derivatives_spline2 { get; set; }
        public SplineParameters(int cnt_nodes_ = 100, double l1 = 1.0, double r1 = 1.0, double l2 = -10.0, double r2 = 20.0)
        {
            cnt_nodes = cnt_nodes_;
            derivatives_spline1 = new double[2] { l1, r1 };
            derivatives_spline2 = new double[2] { l2, r2 };
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
                    default:
                        break;
                }
                return msg;
            }
        }
    }
}
