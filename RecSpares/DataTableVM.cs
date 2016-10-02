using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CJT;

namespace RecSpares {
    class DataTableVM : CJT.DataTableVM {
        private string directory = "C:\\Users\\CJT\\OneDrive\\Documents\\RecSpares\\";
    
        public DataTableVM(string fileName, string commandText, DbContext dbContext) {
            initialise(fileName, commandText, dbContext);
        }

        public DataTableVM(string filePathName, string fileName, string commandText, DbContext dbContext) {
            FilePath = Properties.Settings.Default[filePathName].ToString();
            initialise(fileName, commandText, dbContext);
        }

        private void initialise(string fileName, string commandText, DbContext dbContext) {
            if (FilePath == null) FilePath = directory + fileName;
            CommandText = commandText;
            DbContext = dbContext;
        }

    }
}
