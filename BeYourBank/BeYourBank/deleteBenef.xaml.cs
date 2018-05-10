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
    /// Logique d'interaction pour deleteBenef.xaml
    /// </summary>
    public partial class deleteBenef : Window
    {
        private OleDbConnection connection = new OleDbConnection();


        public deleteBenef()
        {

            InitializeComponent();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void btn_confirm_Click(object sender, RoutedEventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            cmd.Connection = connection;
            for (int i= 0 ; i< lstBox_CIN.Items.Count ; i++) { 
            cmd.Parameters.AddWithValue("@cu", lstBox_CIN.Items[i].ToString());
            cmd.CommandText = "delete from Beneficiaire where noCINBeneficiaire= @cu ";
            cmd.ExecuteNonQuery();
            }
            connection.Close();
            
            MessageBox.Show("Bénéficiare(s) supprimé(s) avec succès");
            this.Close();
        }

        private void btn_Annuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
