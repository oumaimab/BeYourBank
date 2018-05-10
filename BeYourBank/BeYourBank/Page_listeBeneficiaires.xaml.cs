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
        public Page_listeBeneficiaires(string idUser)
        {
            InitializeComponent();
            lbl_user_id.Content = idUser;
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data Source=C:\Users\MYC\Documents\PFE\BeYourBankBD.accdb";
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BindGrid();
        }

        public void BindGrid()
        {
            try
            {
                connection.Open();
                string sql = "SELECT * FROM Beneficiaire";
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connection);
                DataTable ds = new DataTable("Beneficiare_table");
                dataAdapter.Fill(ds);
                connection.Close();
                dataGrid_beneficiaires.ItemsSource = ds.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connection" + ex);
            }
        }

        private void btn_ajouter_Click(object sender, RoutedEventArgs e)
        {
            AddBenefWindow abw = new AddBenefWindow(lbl_user_id.Content.ToString());
            abw.ShowDialog();
        }

        private void btn_supprimer_Click(object sender, RoutedEventArgs e)
        {
            if(dataGrid_beneficiaires.SelectedItems.Count > 0)
            {
                DataRowView row = (DataRowView)dataGrid_beneficiaires.SelectedItems[0];
                OleDbCommand cmd = new OleDbCommand();
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                cmd.Connection = connection;
                cmd.Parameters.AddWithValue("@cu", row["noCINBeneficiaire"]);
                cmd.CommandText = "delete from Beneficiaire where noCINBeneficiaire= @cu ";
                cmd.ExecuteNonQuery();
                connection.Close();
                BindGrid();
                MessageBox.Show("Employee Deleted Successfully...");
                
            }
        }

        private void dataGrid_beneficiaires_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_supprimer.IsEnabled = true;
            btn_edit.IsEnabled = true;
        }
    }
}
