using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
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
using System.Windows.Shapes;

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour Graphe.xaml
    /// </summary>
    public partial class Graphe : Window
    {
        OleDbConnection con;
        public Graphe()
        {
            InitializeComponent();
        }
        public void RechargeCounter(DateTime dateDebut, DateTime dateFin)
        {
            List<KeyValuePair<string, int>> linechartList = new List<KeyValuePair<string, int>>();
            con = new OleDbConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@debut", dateDebut);
            cmd.Parameters.AddWithValue("@fin", dateFin);
            cmd.CommandText = " select [motif], [nomBeneficiaire] from [Operations], [Beneficiaire] where [Operations].[idBeneficiaire]=[Beneficiaire].[NoCINBeneficiaire] and  [TypeOperation] ='Recharge' and [dateOperation] between  @debut and @fin ;";
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int value = Int32.Parse(reader[0].ToString());
                string key = reader[1].ToString();
                linechartList.Add(new KeyValuePair<string, int>(key, value));
               

            }
            barChart.DataContext= linechartList;

        }
        
    }
}
