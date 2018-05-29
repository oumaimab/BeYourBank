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
        public List<KeyValuePair<string, int>> operationNumber()
        //  public void operationNumber()
        {
            List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
            con = new OleDbConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = " select TypeOperation ,count(numCarte)  from [Operations] group by TypeOperation ";
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int value = (int)reader[1];

                //list.Add(new KeyValuePair<string, int>("Cat", 1));
                valueList.Add(new KeyValuePair<string, int>(String.Format("{0}", reader[0]), value));
                // MessageBox.Show(String.Format("{0}", reader[0]) + value.ToString());

                // MessageBox.Show(valueList <[0],[0] >.ToString())

            }
            return valueList;
        }
        //public DateTime DateConvertion(string Input)
        //{
        //    DateTime DateValue = DateTime.ParseExact(Input, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //    return DateValue;
        //}
        public List<KeyValuePair<DateTime, int>> RechargeCounter(DateTime dateDebut, DateTime dateFin)
        {

            con = new OleDbConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();

            OleDbCommand cmd = new OleDbCommand();

            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            //DateOnly resultDebut = dateDebut.GetDateOnly();
            //DateOnly resultFin = dateFin.GetDateOnly();
            cmd.Parameters.AddWithValue("@debut", dateDebut);
            cmd.Parameters.AddWithValue("@fin", dateFin);
            cmd.CommandText = " select [motif], [IdBeneficiaire] from [Operations] where [TypeOperation] ='Recharge' and [dateOperation] between  @debut and @fin ;";
            // cmd.CommandText = " select [motif], [dateOperation] from [Operations] where [TypeOperation] ='Recharge' and[dateOperation] between #15/05/2018# and #19/05/2018#";

            OleDbDataReader reader = cmd.ExecuteReader();
            // List<KeyValuePair<DateTime, int>> linechartList = new List<KeyValuePair<DateTime, int>>();
            List<KeyValuePair<DateTime, int>> linechartList = new List<KeyValuePair<DateTime, int>>();

            while (reader.Read())
            {
                int value = Int32.Parse(reader[0].ToString());
                string key = String.Format("{0}", reader[1]);
                // linechartList.Add(new KeyValuePair<string, int>(key, value));

                linechartList.Add(new KeyValuePair<DateTime, int>(Convert.ToDateTime(reader[1]), Int32.Parse(reader[0].ToString())));
                //MessageBox.Show(String.Format("{0}", reader[1]) + Int32.Parse(reader[0].ToString()).ToString() + linechartList.Count.ToString());

            }
            //linechartList.Add(new KeyValuePair<string, int>("sara", 500));
            //linechartList.Add(new KeyValuePair<string, int>("jihane", 2500));
            //linechartList.Add(new KeyValuePair<string, int>("laila", 5000));



            return linechartList;

        }

        public void showCharts(DateTime dateDebut, DateTime dateFin)
        {
            //pieChart.DataContext = operationNumber();
            lineChart.DataContext = RechargeCounter(dateDebut, dateFin);
            // lineChart.DataContext = operationNumber();


        }
    }
}

