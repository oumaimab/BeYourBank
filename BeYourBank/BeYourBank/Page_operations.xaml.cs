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
using System.Configuration;

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
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
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
            if (dataGrid_beneficiaires.SelectedItems.Count > 0)
            {
                if (comboBox.SelectionBoxItem.Equals("Recharger differents montants"))
                {
                    RechargeDiffWindow rdw = new RechargeDiffWindow(lbl_idUser.Content.ToString());
                    for (int i = 0; i < dataGrid_beneficiaires.SelectedItems.Count; i++)
                    {
                        DataRowView row = (DataRowView)dataGrid_beneficiaires.SelectedItems[i];
                        rdw.listView.Items.Add(new BeneficiaireCard (row["noCINBeneficiaire"].ToString(),row["nomBeneficiaire"].ToString() + " " + row["prenomBeneficiaire"].ToString(),row["numCarte"].ToString(),"")); 
                    }
                    rdw.ShowDialog();
                }
                else if (comboBox.SelectionBoxItem.Equals("Recharger même montant"))
                {
                    RechargeSameWindow rsw = new RechargeSameWindow(lbl_idUser.Content.ToString());
                    rsw.txtBox_decimal.Text = "00";
                    for (int i = 0; i < dataGrid_beneficiaires.SelectedItems.Count; i++)
                    {
                        DataRowView row = (DataRowView)dataGrid_beneficiaires.SelectedItems[i];
                        rsw.listBox_CIN.Items.Add(row["noCINBeneficiaire"].ToString());
                        rsw.listBox_selected.Items.Add(row["nomBeneficiaire"].ToString() + " " + row["prenomBeneficiaire"].ToString());
                    }
                    rsw.ShowDialog();
                }
                else if (comboBox.SelectionBoxItem.Equals("Décharger les cartes"))
                {
                    DechargeWindow dw = new DechargeWindow(lbl_idUser.Content.ToString());
                    for (int i = 0; i < dataGrid_beneficiaires.SelectedItems.Count; i++)
                    {
                        DataRowView row = (DataRowView)dataGrid_beneficiaires.SelectedItems[i];
                        dw.listBox_CIN.Items.Add(row["noCINBeneficiaire"].ToString());
                        dw.listBox_selected.Items.Add(row["nomBeneficiaire"].ToString() + " " + row["prenomBeneficiaire"].ToString());
                    }
                    dw.ShowDialog();
                }
                else if (comboBox.SelectionBoxItem.Equals("Recalculer le PIN"))
                {

                }
                else if (comboBox.SelectionBoxItem.Equals("Remplacer"))
                {
                    ReplaceCardWindow rcw = new ReplaceCardWindow(lbl_idUser.Content.ToString());
                    for (int i = 0; i < dataGrid_beneficiaires.SelectedItems.Count; i++)
                    {
                        DataRowView row = (DataRowView)dataGrid_beneficiaires.SelectedItems[i];
                        rcw.listBox_CIN.Items.Add(row["noCINBeneficiaire"].ToString());
                        rcw.listBox_selected.Items.Add(row["nomBeneficiaire"].ToString() + " " + row["prenomBeneficiaire"].ToString());
                    }
                    rcw.ShowDialog();
                }
                else if (comboBox.SelectionBoxItem.Equals("Opposition sur carte"))
                {
                    OppositionCardWindow ocw = new OppositionCardWindow(lbl_idUser.Content.ToString());
                    for (int i = 0; i < dataGrid_beneficiaires.SelectedItems.Count; i++)
                    {
                        DataRowView row = (DataRowView)dataGrid_beneficiaires.SelectedItems[i];
                        ocw.listBox_CIN.Items.Add(row["noCINBeneficiaire"].ToString());
                        ocw.listBox_selected.Items.Add(row["nomBeneficiaire"].ToString() + " " + row["prenomBeneficiaire"].ToString());
                    }
                    ocw.ShowDialog();
                }
            }

        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            BindGrid_Opp();
            dataGrid_beneficiaires.UnselectAll();
        }
    }
}
