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
        ObservableCollection<Beneficiaire> listeBenef = new ObservableCollection<Beneficiaire>();
        ObservableCollection<Beneficiaire> listeSelected = new ObservableCollection<Beneficiaire>();
        public Page_creationDeCartes(string idUser)
        {
            InitializeComponent();
            lbl_user_id.Content = idUser;
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            BindGrid();
        }

        public void BindGrid()
        {
            listeBenef.Clear();
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "select * from Beneficiaire";
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Beneficiaire bn = new Beneficiaire((string)reader[0], (string)reader[1], (string)reader[2], (string)reader[3], (string)reader[4], (string)reader[5], (string)reader[6], (string)reader[7], (string)reader[8], (string)reader[9], (string)reader[10], (string)reader[11], (string)reader[12]);
                    bn.MyBool = false;
                    listeBenef.Add(bn);
                }
                reader.Close();
                dataGrid_beneficiaires.ItemsSource = listeBenef;
                connection.Close();

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
            listeSelected.Clear();
            foreach (Beneficiaire benef in listeBenef)
            {
                if(benef.MyBool == true)
                {
                    //MessageBox.Show(benef.CIN.ToString());
                    listeSelected.Add(benef);
                }     
            }
            if (listeSelected.Count > 0)
            {
                if (comboBox_modeCreation.SelectionBoxItem.Equals("Création normale"))
                {
                    AddCard ac = new AddCard(lbl_user_id.Content.ToString(), "C");
                    foreach (Beneficiaire benef in listeSelected)
                    {
                        ac.lstBox_CIN.Items.Add(benef.CIN.ToString());
                        ac.lstBox_selected.Items.Add(benef.nom.ToString() + " " + benef.prenom.ToString());
                    }
                    ac.ShowDialog();
                }
                else if (comboBox_modeCreation.SelectionBoxItem.Equals("Création Anonyme"))
                {
                    AddCard ac = new AddCard(lbl_user_id.Content.ToString(), "B");
                    foreach (Beneficiaire benef in listeSelected)
                    {
                        ac.lstBox_CIN.Items.Add(benef.CIN.ToString());
                        ac.lstBox_selected.Items.Add(benef.nom.ToString() + " " + benef.prenom.ToString());
                    }
                    ac.ShowDialog();
                }
                else if (comboBox_modeCreation.SelectionBoxItem.Equals("Création Personalisée"))
                {
                    AddCardCustom adc = new AddCardCustom(lbl_user_id.Content.ToString());
                    foreach (Beneficiaire benef in listeSelected)
                    {
                        adc.listView.Items.Add(new BeneficiaireCard(benef.CIN.ToString(), benef.nom.ToString() + " " + benef.prenom.ToString()));
                    }
                    adc.ShowDialog();
                }
               
                BindGrid();
                SelectAll_Unchecked(sender, e);
            }
         }

        private void btn_type_Click(object sender, RoutedEventArgs e)
        {
            TypeCartesWindow tpW = new TypeCartesWindow();
            tpW.ShowDialog();
        }

        private void selectAll_Checked(object sender, RoutedEventArgs e)
        {
            foreach (Beneficiaire benef in listeBenef)
            {
                benef.MyBool = true;
                listeSelected.Add(benef);
            }
            dataGrid_beneficiaires.Items.Refresh();
            btn_creation.IsEnabled = true;
        }

        private void SelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (Beneficiaire benef in listeBenef)
            {
                benef.MyBool = false;
            }
            dataGrid_beneficiaires.Items.Refresh();
            listeSelected.Clear();
            btn_creation.IsEnabled = false;
        }

        private void chk_Checked(object sender, RoutedEventArgs e)
        {
            btn_creation.IsEnabled = true;
        }

        private void chk_Unchecked(object sender, RoutedEventArgs e)
        {
            btn_creation.IsEnabled = false;
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            BindGrid();
            SelectAll_Unchecked(sender, e);
        }
    }
}
