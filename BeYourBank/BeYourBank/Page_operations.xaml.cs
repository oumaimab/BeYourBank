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
using System.Collections.ObjectModel;

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour Page_operations.xaml
    /// </summary>
    public partial class Page_operations : Page
    {
        private OleDbConnection connection = new OleDbConnection();
        ObservableCollection<BeneficiaireCard> listeBenef = new ObservableCollection<BeneficiaireCard>();
        ObservableCollection<BeneficiaireCard> listeSelected = new ObservableCollection<BeneficiaireCard>();
        public Page_operations(string idUser)
        {
            InitializeComponent();
            lbl_idUser.Content = idUser;
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            BindGrid_Opp();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            BindGrid_Opp();
            SelectAll_Unchecked(sender, e);
            btn_continue.IsEnabled = false;
            btn_cancel.IsEnabled = false;
        }

        public void BindGrid_Opp()
        {
            listeBenef.Clear();
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Beneficiaire, Carte where Carte.idBeneficiaire = Beneficiaire.noCINBeneficiaire and idUser ='" + lbl_idUser.Content.ToString() + "' and numCarte is not null ;";
                OleDbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    BeneficiaireCard bn = new BeneficiaireCard((string)reader[0], (string)reader[1], (string)reader[2], (string)reader[3], (string)reader[4], (string)reader[5], (string)reader[6], (string)reader[7], (string)reader[8], (string)reader[9], (string)reader[10], (string)reader[11], (string)reader[12]);
                    bn.numCarte = (string)reader[13];
                    bn.nomEmbosse = (string)reader[16];
                    bn.fullName = (string)reader[1] + " " + (string)reader[2];
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

        private void btn_continue_Click(object sender, RoutedEventArgs e)
        {
            listeSelected.Clear();
            foreach (BeneficiaireCard benef in listeBenef)
            {
                if (benef.MyBool == true)
                {
                    //MessageBox.Show(benef.CIN.ToString());
                    listeSelected.Add(benef);
                }
            }
            if (listeSelected.Count > 0)
            {
                if (comboBox.SelectionBoxItem.Equals("Recharge de différents montants"))
                {
                    RechargeDiffWindow rdw = new RechargeDiffWindow(lbl_idUser.Content.ToString());
                    foreach (BeneficiaireCard benef in listeSelected)
                    {
                        rdw.listView.Items.Add(benef);
                    }
                    rdw.ShowDialog();
                }
                else if (comboBox.SelectionBoxItem.Equals("Recharge du même montant"))
                {
                    RechargeSameWindow rsw = new RechargeSameWindow(lbl_idUser.Content.ToString());
                    rsw.txtBox_decimal.Text = "00";
                    foreach (BeneficiaireCard benef in listeSelected)
                    {
                        rsw.listBox_CIN.Items.Add(benef.CIN.ToString());
                        rsw.listBox_selected.Items.Add(benef.nom.ToString() + " " + benef.prenom.ToString());
                    }
                    rsw.ShowDialog();
                }
                else if (comboBox.SelectionBoxItem.Equals("Décharge des cartes"))
                {
                    DechargeWindow dw = new DechargeWindow(lbl_idUser.Content.ToString());
                    foreach (BeneficiaireCard benef in listeSelected)
                    {
                        dw.listBox_CIN.Items.Add(benef.CIN.ToString());
                        dw.listBox_selected.Items.Add(benef.nom.ToString() + " " + benef.prenom.ToString());
                    }
                    dw.ShowDialog();
                }
                else if (comboBox.SelectionBoxItem.Equals("Recalcul de PIN"))
                {
                    RecalculPINWindow rp = new RecalculPINWindow(lbl_idUser.Content.ToString());
                    foreach (BeneficiaireCard benef in listeSelected)
                    {
                        rp.listBox_CIN.Items.Add(benef.CIN.ToString());
                        rp.listBox_selected.Items.Add(benef.nom.ToString() + " " + benef.prenom.ToString());
                    }
                    rp.ShowDialog();
                }
                else if (comboBox.SelectionBoxItem.Equals("Remplacement"))
                {
                    ReplaceCardWindow rcw = new ReplaceCardWindow(lbl_idUser.Content.ToString());
                    foreach (BeneficiaireCard benef in listeSelected)
                    {
                        rcw.listBox_CIN.Items.Add(benef.CIN.ToString());
                        rcw.listBox_selected.Items.Add(benef.nom.ToString() + " " + benef.prenom.ToString());
                    }
                    rcw.ShowDialog();
                }
                else if (comboBox.SelectionBoxItem.Equals("Opposition sur carte"))
                {
                    OppositionCardWindow ocw = new OppositionCardWindow(lbl_idUser.Content.ToString());
                    foreach (BeneficiaireCard benef in listeSelected)
                    {
                        ocw.listBox_CIN.Items.Add(benef.CIN.ToString());
                        ocw.listBox_selected.Items.Add(benef.nom.ToString() + " " + benef.prenom.ToString());
                    }
                    ocw.ShowDialog();
                }

                else if (comboBox.SelectionBoxItem.Equals("Annulation de cartes"))
                {
                    CancelCardWindow ccw = new CancelCardWindow(lbl_idUser.Content.ToString());
                    foreach (BeneficiaireCard benef in listeSelected)
                    {
                        ccw.listBox_CIN.Items.Add(benef.CIN.ToString());
                        ccw.listBox_selected.Items.Add(benef.nom.ToString() + " " + benef.prenom.ToString());
                    }
                    ccw.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionnez des bénéficiaires avant de continuer !", "Aucun bénéficiaire sélectionné", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            BindGrid_Opp();
            CheckBox checkBox = dataGrid_beneficiaires.FindUid("selectAll") as CheckBox;
            checkBox.IsChecked = false;
            listeSelected.Clear();
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            BindGrid_Opp();
            CheckBox checkBox = dataGrid_beneficiaires.FindUid("selectAll") as CheckBox;
            checkBox.IsChecked = false;
        }

        private void import_sort_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            //dlg.DefaultExt = ".txt"; // Default file extension
            //dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                RetourData retourData = new RetourData();
                retourData.textData(filename);
            }
        }

        private void selectAll_Checked(object sender, RoutedEventArgs e)
        {
            foreach (BeneficiaireCard benef in listeBenef)
            {
                benef.MyBool = true;
                listeSelected.Add(benef);
            }
            dataGrid_beneficiaires.Items.Refresh();
        }

        private void SelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (BeneficiaireCard benef in listeBenef)
            {
                benef.MyBool = false;
            }
            dataGrid_beneficiaires.Items.Refresh();
            listeSelected.Clear();
        }
    }
}
