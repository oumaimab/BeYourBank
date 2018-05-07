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
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data Source=C:\Users\MYC\Documents\PFE\BeYourBankBD.accdb";
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid_beneficiaires.SelectedItems.Clear();
            try
            {
                connection.Open();
                string sql = "SELECT * FROM Beneficiaire";
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connection);
                DataTable ds = new DataTable("Beneficiare_table");
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
                if (comboBox_modeCreation.SelectionBoxItem.Equals("Création normale"))
                {
                    AddCard ac = new AddCard(lbl_user_id.Content.ToString(), "C");
                    for (int i = 0; i < dataGrid_beneficiaires.SelectedItems.Count; i++)
                    {
                        DataRowView row = (DataRowView)dataGrid_beneficiaires.SelectedItems[i];
                        ac.lstBox_CIN.Items.Add(row["noCINBeneficiaire"].ToString());
                        ac.lstBox_selected.Items.Add(row["nomBeneficiaire"].ToString() + " " + row["prenomBeneficiaire"].ToString());
                    }
                    ac.ShowDialog();
                }
                else if (comboBox_modeCreation.SelectionBoxItem.Equals("Création Anonyme"))
                {
                    AddCard ac = new AddCard(lbl_user_id.Content.ToString(), "B");
                    for (int i = 0; i < dataGrid_beneficiaires.SelectedItems.Count; i++)
                    {
                        DataRowView row = (DataRowView)dataGrid_beneficiaires.SelectedItems[i];
                        ac.lstBox_CIN.Items.Add(row["noCINBeneficiaire"].ToString());
                        ac.lstBox_selected.Items.Add(row["nomBeneficiaire"].ToString() + " " + row["prenomBeneficiaire"].ToString());
                    }
                    ac.ShowDialog();
                }
                else if (comboBox_modeCreation.SelectionBoxItem.Equals("Création Personalisée"))
                {
                    AddCardCustom adc = new AddCardCustom(lbl_user_id.Content.ToString());
                    for (int i = 0; i < dataGrid_beneficiaires.SelectedItems.Count; i++)
                    {
                        DataRowView row = (DataRowView)dataGrid_beneficiaires.SelectedItems[i];
                        adc.listBox_CIN.Items.Add(row["noCINBeneficiaire"].ToString());
                        adc.listView.Items.Add(new Carteperso { CIN = row["noCINBeneficiaire"].ToString(),  Name = row["nomBeneficiaire"].ToString() + " " + row["prenomBeneficiaire"].ToString(), Libelle="" });                                
                        //ListViewItem lvi = new ListViewItem { Content = row["nomBeneficiaire"].ToString() + " " + row["prenomBeneficiaire"].ToString() };
                        //adc.listView.Items.Add(lvi);
                    }
                    adc.ShowDialog();
                }
                else
                {
                    MessageBox.Show("on ne lit rien");
                }

           }
         }

        public class Carteperso
        {
            public string CIN { get; set; }
            public string Name { get; set; }
            public string Libelle { get; set; }
        }
       

        private void btn_type_Click(object sender, RoutedEventArgs e)
        {
            TypeCartesWindow tpW = new TypeCartesWindow();
            tpW.ShowDialog();
        }
    }
}
