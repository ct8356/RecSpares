using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CJT;

namespace RecSpares {
    class PartClassVM : BaseClass {

        private string partNumber;
        public string PartNumber {
            get { return partNumber; }
            set { partNumber = value; NotifyPropertyChanged("PartNumber"); }
        }

        private string category;
        public string Category {
            get { return category; }
            set { category = value; NotifyPropertyChanged("Category"); }
        }

        private int spares;
        public int Spares {
            get { return spares; }
            set { spares = value; NotifyPropertyChanged("Spares"); }
        }

    }
}

