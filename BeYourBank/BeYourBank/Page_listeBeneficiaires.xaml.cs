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
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour Page_listeBeneficiaires.xaml
    /// </summary>
    public partial class Page_listeBeneficiaires : Page
    {
        private OleDbConnection connection = new OleDbConnection();
        public Page_listeBeneficiaires()
        {
            InitializeComponent();
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data Source=C:\Users\MYC\Documents\PFE\BeYourBankBD.accdb";
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
                string sql = "SELECT * FROM Beneficiaire";
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connection);
                DataTable ds = new DataTable("Beneficiare_table");
                connection.Open();
                dataAdapter.Fill(ds);
                connection.Close();
                dataGrid_beneficiaires.ItemsSource = ds.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connection" + ex);
            }
        }

    }
}
