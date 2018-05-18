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
using System.Text.RegularExpressions;

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour Page_listeBeneficiaires.xaml
    /// </summary>
    public partial class Page_listeBeneficiaires : Page
    {
        private OleDbConnection connection = new OleDbConnection();

        //ObservableCollection<Beneficiaire> listeInit;
        ObservableCollection<Beneficiaire> listeBenef = new ObservableCollection<Beneficiaire>();
        ObservableCollection<Beneficiaire> listeSelected = new ObservableCollection<Beneficiaire>();
        public Page_listeBeneficiaires(string idUser)
        {
            InitializeComponent();
            //listeInit = new ObservableCollection<Beneficiaire>();

            lbl_user_id.Content = idUser;
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
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
                dataGrid_beneficiaires.ItemsSource = ds.DefaultView;
                connection.Close();
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
                for (int i =0; i< dataGrid_beneficiaires.SelectedItems.Count; i++)
                {
                    DataRowView row = (DataRowView)dataGrid_beneficiaires.SelectedItems[i];
                    db.lstBox_CIN.Items.Add(row["noCINBeneficiaire"].ToString());
                    db.lstBox_selected.Items.Add(row["nomBeneficiaire"].ToString() + " " + row["prenomBeneficiaire"].ToString());
                } 
                db.ShowDialog();
                btn_supprimer.IsEnabled = false;
            }           
        }

        private void dataGrid_beneficiaires_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btn_supprimer.IsEnabled = true;
            btn_modifier.IsEnabled = true;
        }


        private void btn_modifier_Click(object sender, RoutedEventArgs e)
        {
            EditBenef eb = new EditBenef();
            DataRowView row = (DataRowView)dataGrid_beneficiaires.SelectedItems[0];
            eb.fillDayMonth();

            eb.CINBenefEdit.Text = row["noCINBeneficiaire"].ToString();
            eb.BenefFNameEdit.Text = row["nomBeneficiaire"].ToString();
            eb.BenefLNameEdit.Text = row["prenomBeneficiaire"].ToString();
            eb.telBenefEdit.Text = row["noTelBeneficiaire"].ToString();

            if (row["statut"].Equals("S")) eb.statutComboEdit.Text = "Célibataire";
            if (row["statut"].Equals("Z")) eb.statutComboEdit.Text = "Marié(e)";
            if (row["statut"].Equals("V")) eb.statutComboEdit.Text = "Veuf(ve)";
            if (row["statut"].Equals("R")) eb.statutComboEdit.Text = "Divorcé(e)";
            if (row["statut"].Equals("O")) eb.statutComboEdit.Text = "Séparé(e)";
            if (row["statut"].Equals("D")) eb.statutComboEdit.Text = "Conjoint(e)";
            if (row["statut"].Equals("X")) eb.statutComboEdit.Text = "Pas déclaré";


            eb.DayBEdit.Text = row["dateNaissance"].ToString().Substring(0, 2);
            //MessageBox.Show(row["dateNaissance"].ToString().Substring(0, 2));
            eb.MonthBEdit.Text = row["dateNaissance"].ToString().Substring(2, 2);
            eb.YearBEdit.Text = row["dateNaissance"].ToString().Substring(4, 4);
            eb.prfEdit.Text = row["profession"].ToString();
            eb.BenefLNameEdit.Text = row["prenomBeneficiaire"].ToString();
            eb.adrEdit.Text = row["adresse"].ToString();
            eb.villeBenefEdit.Text = row["villeResidence"].ToString();
            eb.codePEdit.Text = row["codePostal"].ToString();
            eb.sexComboEdit.Text = row["sex"].ToString();
            eb.titreComboEdit.Text = row["titre"].ToString();
            //eb.statutComboEdit.Text = row["statut"].ToString();
            //MessageBox.Show(row["titre"].ToString());
            // MessageBox.Show(eb.titreComboEdit.Text);

            eb.ShowDialog();
            btn_modifier.IsEnabled = false;
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
                                       //dlg.DefaultExt = ".txt"; // Default file extension
                                       //  dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                ExcelDataBenef exceldatabenef = new ExcelDataBenef();
                exceldatabenef.bindexcel(filename);
            }
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            BindGrid();
            btn_supprimer.IsEnabled = false;
            btn_modifier.IsEnabled = false;
        }
       
     
    }
}
