﻿using System;
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
            btn_modifier.IsEnabled = true;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
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
      

        private void btn_modifier_Click(object sender, RoutedEventArgs e)
        {    
            EditBenef eb = new EditBenef();
            DataRowView row = (DataRowView)dataGrid_beneficiaires.SelectedItems[0];
            eb.fillDayMonth();

            eb.CINBenefEdit.Text = row["noCINBeneficiaire"].ToString();
            eb.BenefFNameEdit.Text = row["nomBeneficiaire"].ToString();
            eb.BenefLNameEdit.Text = row["prenomBeneficiaire"].ToString();
            eb.telBenefEdit.Text = row["noTelBeneficiaire"].ToString();
            if (row["statut"].ToString() == "S") eb.statutComboEdit.Text = "Célibataire";
            if (row["statut"].ToString() == "Z") eb.statutComboEdit.Text = "Marié";
            if (row["statut"].ToString() == "V") eb.statutComboEdit.Text = "Veuf";
            if (row["statut"].ToString() == "R") eb.statutComboEdit.Text = "Divorcé";
            if (row["statut"].ToString() == "O") eb.statutComboEdit.Text = "Séparé";
            if (row["statut"].ToString() == "D") eb.statutComboEdit.Text = "Conjoint";
            if (row["statut"].ToString() == "X") eb.statutComboEdit.Text = "Pas déclaré";


            eb.DayBEdit.Text = row["dateNaissance"].ToString().Substring(0, 2);
            MessageBox.Show(row["dateNaissance"].ToString().Substring(0, 2));
            eb.MonthBEdit.Text= row["dateNaissance"].ToString().Substring(3, 2);
            eb.YearBEdit.Text= row["dateNaissance"].ToString().Substring(6, 2);
            eb.prfEdit.Text = row["profession"].ToString();
            eb.BenefLNameEdit.Text = row["prenomBeneficiaire"].ToString();
            eb.adrEdit.Text = row["adresse"].ToString();
            eb.villeBenefEdit.Text = row["villeResidence"].ToString();
            eb.codePEdit.Text = row["codePostal"].ToString();
            eb.sexComboEdit.Text = row["sex"].ToString();
            eb.titreComboEdit.Text = row["titre"].ToString();
            //eb.statutComboEdit.Text = row["statut"].ToString();
            MessageBox.Show(row["statut"].ToString());
            
 eb.ShowDialog();

            


        }
    }
}
