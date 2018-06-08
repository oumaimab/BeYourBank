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
            List<KeyValuePair<string, int>> liste1 = new List<KeyValuePair<string, int>>();
            List<KeyValuePair<string, int>> liste2 = new List<KeyValuePair<string, int>>();
            List<KeyValuePair<string, int>> liste3 = new List<KeyValuePair<string, int>>();


            List<int> top5 = new List<int>() ;
            con = new OleDbConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@debut", dateDebut);
            cmd.Parameters.AddWithValue("@fin", dateFin);
            cmd.CommandText = " select [motif], [nomBeneficiaire], [prenomBeneficiaire] from [Operations]  , [Beneficiaire]  where [Operations].[Idbeneficiaire]=[Beneficiaire].[noCINBeneficiaire] and [TypeOperation] ='Recharge' and [dateOperation] between  @debut and @fin ;";
            OleDbDataReader reader = cmd.ExecuteReader();
       
            while (reader.Read())
            {
                 int value= Int32.Parse(reader[0].ToString()) ;
                string  key = reader[1].ToString();
                liste1.Add(new KeyValuePair<string, int>(key, value));

            }

            List<string> CINs = (from kvp in liste1 select kvp.Key).Distinct().ToList();
            for (int i = 0; i < CINs.Count(); i++) {
                List<int> recharges = (from kvp1 in liste1 where kvp1.Key == CINs[i] select kvp1.Value).ToList();
                int s = 0;
                for(int j =0; j<recharges.Count(); j++)
                {
                    s = s + recharges[j];
                }
                liste2.Add(new KeyValuePair<string, int>(CINs[i], s));


            }

            List<string> CINOrdered = liste2.OrderBy(x => x.Value).Select(x => x.Key).Take(5).ToList();
            for (int i = 0; i < CINOrdered.Count(); i++)
            {
                List<int> Somme = (from kvp in liste2 where kvp.Key == CINOrdered[i] select kvp.Value).ToList();
                linechartList.Add(new KeyValuePair<string, int>(CINOrdered[i], Somme[0]));
                
            }
            barChart.DataContext= linechartList;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
