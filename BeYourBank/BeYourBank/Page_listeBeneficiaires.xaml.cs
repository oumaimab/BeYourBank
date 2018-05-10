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
using System.Configuration;
using System.Collections.ObjectModel;

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour Page_listeBeneficiaires.xaml
    /// </summary>
    public partial class Page_listeBeneficiaires : Page
    {
        private OleDbConnection connection = new OleDbConnection();

        //ObservableCollection<Beneficiaire> listeInit;
        //ObservableCollection<Beneficiaire> listeSelectionnes = new ObservableCollection<Beneficiaire>();
        public Page_listeBeneficiaires(string idUser)
        {
            InitializeComponent();
            //listeInit = new ObservableCollection<Beneficiaire>();

            lbl_user_id.Content = idUser;
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
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
            if (dataGrid_beneficiaires.SelectedItems.Count > 0)
            {
                deleteBenef db = new deleteBenef();
                for (int i =0; i< dataGrid_beneficiaires.SelectedItems.Count; i++) { 

                DataRowView row = (DataRowView)dataGrid_beneficiaires.SelectedItems[i];
                    db.lstBox_CIN.Items.Add(row["noCINBeneficiaire"].ToString());
                    db.lstBox_selected.Items.Add(row["nomBeneficiaire"].ToString() + " " + row["prenomBeneficiaire"].ToString());
                }
                db.ShowDialog();
            }
        }

        private void dataGrid_beneficiaires_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_supprimer.IsEnabled = true;
        }
    }
}
