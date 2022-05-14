using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;

namespace Wpf_Lab2_v3 
{ 
    public class SplineParameters : INotifyPropertyChanged, IDataErrorInfo
    {
        private double[] __Right_Left_1;
        public double[] Right_Left_1 
        {
            get { return __Right_Left_1; }
            set 
            {
                __Right_Left_1 = value;
                OnPropertyChanged("Right_Left_1");
            }
        }
        private double[] __Right_Left_2;
        public double[] Right_Left_2
        {
            get { return __Right_Left_2; }
            set
            {
                __Right_Left_2 = value;
                OnPropertyChanged("Right_Left_2");
            }
        }

        private int __nodes;
        public int nodes
        {
            get
            { return __nodes; }
            set
            {
                __nodes = value;
                OnPropertyChanged("nodes");
            }
        }

        public SplineParameters(int nodes = 100, double Left_1 = 1.0, double Right_1 = 1.0, double Left_2 = -5.0, double Right_2 = 10.0)
        {
            this.nodes = nodes;
            Right_Left_1 = new double[2] { Left_1, Right_1 };
            Right_Left_2 = new double[2] { Left_2, Right_2 };
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property_name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
        public string Error { get { return "Error"; } }
        public string this[string property]
        {
            get
            {
                string messange = null;
                switch (property)
                {
                    case "nodes":
                        if (nodes < 2) messange = "Number of points must be more 1!";
                        break;
                    default:
                        break;
                }
                return messange;
            }
        }
    }
}
