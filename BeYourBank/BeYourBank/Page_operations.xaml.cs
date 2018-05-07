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
using System.IO;
using System.Data;

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour Page_operations.xaml
    /// </summary>
    public partial class Page_operations : Page
    {
        private OleDbConnection connection = new OleDbConnection();
        public Page_operations(string idUser)
        {
            InitializeComponent();
            lbl_idUser.Content = idUser;
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data Source=C:\Users\MYC\Documents\PFE\BeYourBankBD.accdb";
            BindGrid_Opp();
        }

        private void dataGrid_beneficiaires_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_continue.IsEnabled = true;
            btn_cancel.IsEnabled = true;
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            dataGrid_beneficiaires.UnselectAll();
            btn_continue.IsEnabled = false;
            btn_cancel.IsEnabled = false;
        }

        public void BindGrid_Opp()
        {
            try
            {
                connection.Open();
                string sql = "SELECT * FROM Beneficiaire, Carte where noCINBeneficiaire = idBeneficiaire and numCarte is not null ;";
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

        private void btn_continue_Click(object sender, RoutedEventArgs e)
        {
            if(comboBox.SelectionBoxItem.Equals("Recharger differents montants"))
            {

            }
            else if (comboBox.SelectionBoxItem.Equals("Recharger même montant"))
            {

            }
            else if (comboBox.SelectionBoxItem.Equals("Décharger les cartes"))
            {

            }
            else if (comboBox.SelectionBoxItem.Equals("Recalculer le PIN"))
            {

            }
            else if (comboBox.SelectionBoxItem.Equals("Remplacer"))
            {

            }
            else if (comboBox.SelectionBoxItem.Equals("Opposition sur carte"))
            {

            }


        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if((string)comboBox.SelectedItem == "recharge_unit_item" || (string)comboBox.SelectedItem == "recharge_mass_item" || (string)comboBox.SelectedItem == "decharge_item")
            {
               // dataGrid_beneficiaires.SelectionMode = SelectionMode.Extended;
            }
            else
            {
               // dataGrid_beneficiaires.SelectionMode = SelectionMode.Single;
            }
        }
    }
}
