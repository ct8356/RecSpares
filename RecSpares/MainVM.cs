using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTable = System.Data.DataTable;
using System.Collections.ObjectModel;
using CJT;

namespace RecSpares {
    class MainVM : BaseClass {
        public ExcelContext ExcelContext { get; set; }
        public DataTableVM Master { get; set; } //Each entry like a PartClass
        public DataTableVM AllJobs { get; set; } //each entry bit like a PartClassInstance
        public DataTableVM Job { get; set; }
        public DataTableVM PartType { get; set; }
        public DataTableVM Test { get; set; }
        public DataTableVM MasterTranslations { get; set; }
        public DataTableVM EPLANTranslations { get; set; }
        public DataTableVM TIATranslations { get; set; }
        public DataTableVM AdditionalTranslations { get; set; }
        public string CommandText { get; set; }
        public ObservableCollection<int> LevelsOfNeed { get; set; }
        public int LevelOfNeed { get; set; }

        public MainVM() {
            CommandText =
                "SELECT German FROM [Master$] WHERE English IN "+
                "(SELECT [en-US*] FROM [TIA$])"+
                "";
                //"(SELECT [Part No] FROM [Sheet1$] WHERE PartType IN " +
                //"(SELECT PartType FROM [Sheet4$] WHERE IsRecSpare = 'y'))";
            //"INSERT INTO [Sheet1$] ([Part No], Description, Manufacturer) " +
            //"SELECT DISTINCT [Part No], Description, Manufacturer FROM [Sheet2$] " +
            //"WHERE NOT EXISTS " +
            //"(SELECT [Part No], Description, Manufacturer FROM [Sheet1$] " +
            //"WHERE [Sheet1$].[Part No] = [Sheet2$].[Part No])";
            ExcelContext = new ExcelContext();
            LevelsOfNeed = new ObservableCollection<int> { 1, 2, 3 };
            LevelOfNeed = 3;
            string commandText = "SELECT [Part No], Description, PartType FROM [Sheet1$]";
            Master = new DataTableVM("MasterFilePath", "RecSpares.xlsx", commandText, ExcelContext);
            commandText = "SELECT [Job No], Det, [Part No], Description, [Spares Qty], NotReqd FROM [Sheet2$]";
            AllJobs = new DataTableVM("AllJobsFilePath", "RecSpares.xlsx", commandText, ExcelContext);
            commandText = "SELECT * FROM [Sheet3$A3:K]";
            Job = new DataTableVM("JobFilePath", "RecSpares.xlsx", commandText, ExcelContext);
            commandText = "SELECT * FROM [Sheet4$]";
            PartType = new DataTableVM("RecSpares.xlsx", commandText, ExcelContext);
            commandText = "SELECT * FROM [Master$]";
            MasterTranslations = new DataTableVM("Translations.xlsx", commandText, ExcelContext);
            commandText = "SELECT * FROM [EPLAN$]";
            EPLANTranslations = new DataTableVM("Translations.xlsx", commandText, ExcelContext);
            commandText = "SELECT * FROM [TIA$]";
            TIATranslations = new DataTableVM("Translations.xlsx", commandText, ExcelContext);
            commandText = "SELECT * FROM [Additional$]";
            AdditionalTranslations = new DataTableVM("Translations.xlsx", commandText, ExcelContext);
            Test = new DataTableVM("Translations.xlsx", CommandText, ExcelContext);
            UpdateDataTables();
        }

        public void AddColumnAndFill() {
            string levelOfNeedString = "";
            switch (LevelOfNeed) {
                case 1: levelOfNeedString = "'1','2','3'"; break;
                case 2: levelOfNeedString = "'2','3'"; break;
                case 3: levelOfNeedString = "'3'"; break;
            }
            ExcelContext.AddColumn(Job.FilePath);
            ExcelContext.FillColumnWithZeros(Job.FilePath);
            ExcelContext.FillColumn(Job.FilePath, levelOfNeedString);
            UpdateDataTables();
        }

        public void AddToMasterPartsList() {
            ExcelContext.AddToMasterPartsList(Master.FilePath);
            UpdateDataTables();
        }

        public void AddToPartTypes() {
            string commandText = 
                "INSERT INTO [Sheet4$] (PartType) " +
                "SELECT DISTINCT PartType FROM [Sheet1$] m " +
                "WHERE NOT EXISTS " +
                "(SELECT * FROM [Sheet4$] pt " +
                "WHERE m.PartType = pt.PartType)";
            ExcelContext.ExecuteNonQuery(commandText, Master.FilePath);
            UpdateDataTables();
        }

        public void SaveSettings() {
            Properties.Settings.Default["JobFilePath"] = Job.FilePath;
            Properties.Settings.Default["MasterFilePath"] = Master.FilePath;
            Properties.Settings.Default["AllJobsFilePath"] = AllJobs.FilePath;
            Properties.Settings.Default.Save(); // Saves settings in application configuration file
        }

        public void TranslateEPLAN() {
            string commandText =
                "UPDATE [EPLAN$] e, [Master$] m SET e.[DE] = m.German " +
                "WHERE m.English = e.[EN]" +
                "";
            ExcelContext.ExecuteNonQuery(commandText, MasterTranslations.FilePath);
            UpdateDataTables();
        }

        public void TranslateTIA() {
            string commandText =
                "UPDATE [TIA$] t, [Master$] m SET t.[de-DE] = m.German "+
                "WHERE m.English = t.[en-US*]"+
                "";
            ExcelContext.ExecuteNonQuery(commandText, MasterTranslations.FilePath);
            UpdateDataTables();
        }

        public void UpdateSpares() {
            //FIRST WIPE IT!
            string commandText =
                "UPDATE [Sheet2$] SET [Spares Qty] = 0";
            ExcelContext.ExecuteNonQuery(commandText, Master.FilePath);
            commandText =
                "UPDATE [Sheet2$] SET [Spares Qty] = 1 WHERE [Part No] IN "+
                //Update allJobs where part number matches
                "(SELECT [Part No] from [Sheet1$] WHERE PartType IN "+
                //part number from master partslist where parttype matches
                "(SELECT PartType FROM [Sheet4$] WHERE IsRecSpare = 'y')) "+
                //parttype from PartTypes where spare is recommended.
                "AND Qty <> 0 AND NotReqd = FALSE "+
                "";
            ExcelContext.ExecuteNonQuery(commandText, Master.FilePath);
            UpdateDataTables();
        }

        public void UpdateDataTables() {
            Master.UpdateDataList();
            AllJobs.UpdateDataList();
            Job.UpdateDataList();
            PartType.UpdateDataList();
            //commandText = "SELECT MSysObjects.Name AS table_name FROM MSysObjects WHERE(((Left([Name], 1))<> \"~\") " +
            //"AND((Left([Name], 4))<> \"MSys\") AND((MSysObjects.Type)In(1, 4, 6))) order by MSysObjects.Name;";          
            MasterTranslations.UpdateDataList();
            EPLANTranslations.UpdateDataList();
            TIATranslations.UpdateDataList();
            AdditionalTranslations.UpdateDataList();
        }

        public void UpdateTestTable() {
            Test.DataTable = ExcelContext.GetDataTable(CommandText, "", MasterTranslations.FilePath);
        }

    }
}
