using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecSpares {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            UpdateGrids();
            WindowState = WindowState.Maximized;
        }



        public void AddColumnAndFill_Click(object sender, EventArgs e) {
            (DataContext as MainVM).AddColumnAndFill();
            UpdateGrids();
        }

        public void AddToMasterPartsList_Click(object sender, EventArgs e) {
            (DataContext as MainVM).AddToMasterPartsList();
            UpdateGrids();
        }

        public void TranslateEPLAN_Click(object sender, EventArgs e) {
            (DataContext as MainVM).TranslateEPLAN();
            UpdateGrids();
        }

        public void TranslateTIA_Click(object sender, EventArgs e) {
            (DataContext as MainVM).TranslateTIA();
            UpdateGrids();
        }

        public void UpdateGrids() {
            IOGrid.DataContext = (DataContext as MainVM).Job.DataTable.DefaultView;
            BaseGrid.DataContext = (DataContext as MainVM).Master.DataTable.DefaultView;
            AllJobsGrid.DataContext = (DataContext as MainVM).AllJobs.DataTable.DefaultView;    
            PartTypeGrid.DataContext = (DataContext as MainVM).PartType.DataTable.DefaultView;
            MasterTranslationsGrid.DataContext = (DataContext as MainVM).MasterTranslations.DataTable.DefaultView;
            EPLANTranslationsGrid.DataContext = (DataContext as MainVM).EPLANTranslations.DataTable.DefaultView;
            TIATranslationsGrid.DataContext = (DataContext as MainVM).TIATranslations.DataTable.DefaultView;
            AdditionalTranslationsGrid.DataContext = (DataContext as MainVM).AdditionalTranslations.DataTable.DefaultView;
            //AHAH! perhaps if I make DefaultView fire a NOTIFYPropChanged event, THEN binding would work.
            //NOTE: this does work, BUT remember, have to GET NEW DATA FROM sql of course!
        }

        public void UpdatePartTypes_Click(object sender, EventArgs e) {
            (DataContext as MainVM).AddToPartTypes();
            UpdateGrids();
        }

        public void UpdateSpares_Click(object sender, EventArgs e) {
            (DataContext as MainVM).UpdateSpares();
            UpdateGrids();
        }

        public void UpdateTestGrid() {
            TestGrid.DataContext = (DataContext as MainVM).Test.DataTable.DefaultView;
        }

        public void Save_Click(object sender, EventArgs e) {
            (DataContext as MainVM).SaveSettings();
        }

        public void TestQuery_Click(object sender, EventArgs e) {
            (DataContext as MainVM).UpdateTestTable();
            UpdateTestGrid();
        }

    }
}
