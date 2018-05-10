using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
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
using System.Windows.Shapes;

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour EditBenef.xaml
    /// </summary>
    public partial class EditBenef : Window
    {
        OleDbConnection con;
        public EditBenef()
        {
            InitializeComponent();
           
        }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            fillDayMonth();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            con = new OleDbConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            con.Open();
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            cmd.Connection = con;
            cmd.CommandText = " update [Beneficiaire] set [nomBeneficiaire]= '" + BenefLNameEdit.Text + "' ,[prenomBeneficiaire] = '" + BenefFNameEdit.Text + "', [noTelBeneficiaire] ='" + telBenefEdit.Text + "', [dateNaissance] =' " + DayBEdit.SelectedItem.ToString()+ MonthBEdit.SelectedItem.ToString()  + YearBEdit.Text + "', [profession] = '" + prfEdit.Text + "' , [adresse] = '" + adrEdit.Text + "' , [villeResidence]=' "+ villeBenefEdit.Text +"', [codePostal]='"+ codePEdit.Text +"',[sex]='" + sexComboEdit.SelectedItem.ToString() +"',[titre]='"+ titreComboEdit.SelectedItem.ToString() + "',[statut] ='" + statutComboEdit.SelectedItem.ToString() +" ' where [noCINBeneficiaire]='" + CINBenefEdit.Text + "';" ;
            cmd.ExecuteNonQuery();
            MessageBox.Show("Bénéficiaire modifié avec succès !");
            this.Hide();

        }
     public  void fillDayMonth()
        {
            for (int i = 1; i < 32; i++)
            {
                if (i < 10)
                {
                    DayBEdit.Items.Add("0" + i).ToString();
                }
                else
                {
                    DayBEdit.Items.Add(i.ToString());
                }

            }
            for (int i = 1; i < 13; i++)
            {
                if (i < 10)
                {
                    MonthBEdit.Items.Add("0" + i.ToString());
                }
                else
                {
                    MonthBEdit.Items.Add(i.ToString());
                }
            }
        }
    }
}
