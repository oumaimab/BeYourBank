using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
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

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour Page_dashboard.xaml
    /// </summary>
    public partial class Page_dashboard : Page

    {
        OleDbConnection con;

        public Page_dashboard()
        {
            InitializeComponent();
            
        }
        public List<KeyValuePair<string, int> > operationNumber()
        {
            List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
            con = new OleDbConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = " select TypeOperation ,count(numCarte) from [Operations] group by TypeOperation ";
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int value = (int)reader[1];
              
                valueList.Add(new KeyValuePair<string, int>(String.Format("{0}", reader[0]), value));
            
           }
            return valueList ;
        }
      
        public void showCharts()
        {
            pieChart.DataContext = operationNumber();
            //MessageBox.Show(dateDebut.ToString() + dateFin.ToString());
            //pieChart.DataContext = RechargeCounter(dateDebut,dateFin);
            // lineChart.DataContext = operationNumber();
        }

        private void btn_visualiser_Click(object sender, RoutedEventArgs e)
        {
            Graphe graphe = new BeYourBank.Graphe();
            graphe.RechargeCounter(datepicker1.SelectedDate.Value.Date, datepicker2.SelectedDate.Value.Date);
            graphe.ShowDialog();    
        }
    }
}
