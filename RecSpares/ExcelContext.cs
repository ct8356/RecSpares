using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTable = System.Data.DataTable;
using System.Data.OleDb;
using CJT;

namespace RecSpares {
    public class ExcelContext : CJT.ExcelContext {

        public ExcelContext() {
        }

        public void AddToMasterPartsList(string filePath) {
            string commandText = 
                "INSERT INTO [Sheet1$] ([Part No], Description) " +
                "SELECT DISTINCT [Part No], Description FROM [Sheet2$] aj " +
                "WHERE NOT EXISTS " +
                "(SELECT * FROM [Sheet1$] m " +
                "WHERE aj.[Part No] = m.[Part No])";
            using (OleDbCommand cmd = new OleDbCommand()) {
                cmd.CommandText = commandText;
                using (OleDbConnection conn = new OleDbConnection(GetConnectionString(filePath))) {
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddColumn(string filePath) {
            string commandText = 
                "Create Table [Sheet3$A3:K] (Det Int, [Part No] String, Description String, Price Real, Qty Int, Total Real, Spares Int)";
            using (OleDbCommand cmd = new OleDbCommand()) {
                cmd.CommandText = commandText; //@ not working!
                using (OleDbConnection conn = new OleDbConnection(GetConnectionString(filePath))) {
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ExecuteNonQuery(string commandText, string filePath) {
            using (OleDbCommand cmd = new OleDbCommand()) {
                cmd.CommandText = commandText;
                using (OleDbConnection conn = new OleDbConnection(GetConnectionString(filePath))) {
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void FillColumn(string filePath, string levelOfNeed) {
            string commandText = "UPDATE [Sheet3$A3:G] SET Spares=1 WHERE [Part No] IN " +
                "(SELECT [Part No] FROM [Sheet1$] WHERE [Level of need] IN ("+levelOfNeed+")) "+
                "";            
            using (OleDbCommand cmd = new OleDbCommand()) {
                cmd.CommandText = commandText;
                using (OleDbConnection conn = new OleDbConnection(GetConnectionString(filePath))) {
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void FillColumnWithZeros(string filePath) {
            string commandText = "UPDATE [Sheet3$A3:G] SET Spares=0" +
                "";
            using (OleDbCommand cmd = new OleDbCommand()) {
                cmd.CommandText = commandText;
                using (OleDbConnection conn = new OleDbConnection(GetConnectionString(filePath))) {
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetTables(string filePath) {
            using (OleDbCommand cmd = new OleDbCommand()) {
                using (OleDbConnection conn = new OleDbConnection(GetOtherConnectionString(filePath))) {
                    string[] restrictions = new string[4];
                    restrictions[3] = "Table";
                    conn.Open();
                    return conn.GetSchema("Tables", restrictions);
                }
            }
        }

    }
}
