using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RecSpares {
    class DataGrid : System.Windows.Controls.DataGrid {


        public DataGrid() {
            CanUserAddRows = false;
            MaxColumnWidth = 500;
        }


    }
}
