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
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour Page_creationDeCartes.xaml
    /// </summary>

    public partial class Page_creationDeCartes : Page
    {
        private OleDbConnection connection = new OleDbConnection();
        ObservableCollection<Beneficiaire> listeInit;
        ObservableCollection<Beneficiaire> listeSelectionnes = new ObservableCollection<Beneficiaire>();
        public Page_creationDeCartes(string idUser)
        {
            InitializeComponent();
            lbl_user_id.Content = idUser;
            listeInit = new ObservableCollection<Beneficiaire>();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid_beneficiaires.SelectedItems.Clear();
            try
            {

                string sql = "SELECT * FROM Beneficiaire";
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connection);
                DataTable ds = new DataTable("Beneficiare");
                connection.Open();
                dataAdapter.Fill(ds);
                connection.Close();
                dataGrid_beneficiaires.ItemsSource = ds.DefaultView;
                for( int i=0; i<ds.Rows.Count; i++)
                {
                    Beneficiaire benef = new Beneficiaire(ds.Rows[i]["noCINBeneficiaire"].ToString(), ds.Rows[i]["nomBeneficiaire"].ToString(), ds.Rows[i]["prenomBeneficiaire"].ToString(), ds.Rows[i]["noTelBeneficiaire"].ToString(), ds.Rows[i]["dateNaissance"].ToString(), ds.Rows[i]["profession"].ToString(), ds.Rows[i]["adresse"].ToString(), ds.Rows[i]["villeResidence"].ToString(), ds.Rows[i]["codePostal"].ToString(), ds.Rows[i]["sex"].ToString(), ds.Rows[i]["titre"].ToString(), ds.Rows[i]["statut"].ToString(), ds.Rows[i]["idUser"].ToString());
                    listeInit.Add(benef);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connection" + ex);
            }
        }

        private void dataGrid_beneficiaires_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_creation.IsEnabled = true;
        }

        private void btn_creation_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid_beneficiaires.SelectedItems.Count > 0)
            {
                AddCard ac = new AddCard(lbl_user_id.Content.ToString());
                for (int i = 0; i < dataGrid_beneficiaires.SelectedItems.Count; i++)
                {
                    DataRowView row = (DataRowView)dataGrid_beneficiaires.SelectedItems[i];
                    ac.lstBox_CIN.Items.Add(row["noCINBeneficiaire"].ToString());
                    ac.lstBox_selected.Items.Add(row["nomBeneficiaire"].ToString()+" "+ row["prenomBeneficiaire"].ToString());
                }
                ac.ShowDialog();
            }

        }

        private void btn_import_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();
            if(result == true)
            {
                string filename = dlg.FileName;
                RetourData retourData = new RetourData();
                retourData.textData(filename);









            }

        }
    }
}
