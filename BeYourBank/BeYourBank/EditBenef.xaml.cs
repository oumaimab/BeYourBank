using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            string statutB = null;
            string dateN = null;
            con = new OleDbConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
            con.Open();
            OleDbCommand cmd = new OleDbCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            if (CINBenefEdit.Text != "")
            {
                if (BenefLNameEdit.Text != "")
                {
                    if (BenefFNameEdit.Text != "")
                    {
                        if (telBenefEdit.Text != "")
                        {
                            if (!string.IsNullOrEmpty(DayBEdit.SelectionBoxItem.ToString()))
                            {
                                if (!string.IsNullOrEmpty(MonthBEdit.SelectionBoxItem.ToString()))
                                {
                                    if (!string.IsNullOrEmpty(YearBEdit.SelectionBoxItem.ToString()))
                                    {
                                        if (prfEdit.Text != "")
                                        {
                                            if (adrEdit.Text != "")
                                            {
                                                if (villeBenefEdit.Text != "")
                                                {
                                                    if (codePEdit.Text.ToString().Length == 5)
                                                    {
                                                        if (!string.IsNullOrEmpty(sexComboEdit.SelectionBoxItem.ToString()))
                                                        {
                                                            if (!string.IsNullOrEmpty(titreComboEdit.SelectionBoxItem.ToString()))
                                                            {
                                                                if (!string.IsNullOrEmpty(statutComboEdit.SelectionBoxItem.ToString()))
                                                                {
                                                                    if (statutComboEdit.SelectionBoxItem.ToString() == "Célibataire")
                                                                    {
                                                                        statutB = "S";
                                                                    }
                                                                    else if (statutComboEdit.SelectionBoxItem.ToString() == "Marié(e)")
                                                                    {
                                                                        statutB = "Z";
                                                                    }
                                                                    else if (statutComboEdit.SelectionBoxItem.ToString() == "Veuf(ve)")
                                                                    {
                                                                        statutB = "V";
                                                                    }
                                                                    else if (statutComboEdit.SelectionBoxItem.ToString() == "Divorcé(e)")
                                                                    {
                                                                        statutB = "R";
                                                                    }
                                                                    else if (statutComboEdit.SelectionBoxItem.ToString() == "Séparé(e)")
                                                                    {
                                                                        statutB = "O";
                                                                    }
                                                                    else if (statutComboEdit.SelectionBoxItem.ToString() == "Conjoint(e)")
                                                                    {
                                                                        statutB = "D";
                                                                    }
                                                                    else if (statutComboEdit.SelectionBoxItem.ToString() == "Pas déclaré")
                                                                    {
                                                                        statutB = "X";
                                                                    }
                                                                    cmd.Connection = con;
                                                                    statutB = Regex.Replace(statutB, @"\s", "");
                                                                    dateN = Regex.Replace(DayBEdit.SelectionBoxItem.ToString() + MonthBEdit.SelectionBoxItem.ToString() + YearBEdit.SelectionBoxItem.ToString(), @"\s", "");
                                                                    cmd.CommandText = " update [Beneficiaire] set [nomBeneficiaire]= '" + BenefLNameEdit.Text + "' ,[prenomBeneficiaire] = '"+ BenefFNameEdit.Text+"', [noTelBeneficiaire] ='" + telBenefEdit.Text + "', [dateNaissance] ='"+ dateN +"', [profession] = '"+ prfEdit.Text +"', [adresse] = '" + adrEdit.Text + "' , [villeResidence]='"+villeBenefEdit.Text +"', [codePostal]='"+codePEdit.Text+"',[sex]='"+sexComboEdit.SelectionBoxItem.ToString()+"', [titre]='"+titreComboEdit.SelectionBoxItem.ToString()+"', [statut] ='"+statutB+"' where [noCINBeneficiaire]='"+ CINBenefEdit.Text +"';";
                                                                    cmd.ExecuteNonQuery();
                                                                    MessageBox.Show("Bénéficiaire modifié avec succès !");
                                                                    this.Close();
                                                                }
                                                                else
                                                                {
                                                                    MessageBox.Show("Veuillez sélectionner un statut !");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                MessageBox.Show("Veuillez sélectionner un titre !");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("Veuillez sélectionner le sexe du beneficiaire!");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Veuillez saisir un code Postale valide!");
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Veuillez saisir la ville de résidence !");
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Veuillez saisir l'adresse de résidence !");
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Veuillez saisir la profession du béneficiaire !");
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Veuillez saisir l'année de naissance !");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Veuillez sélectionner le mois de naissance !");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Veuillez sélectionner le jour de naissance !");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Veuillez saisir le numéro de télephone !");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Veuillez saisir le prénom! ");
                    }
                }
                else
                {
                    MessageBox.Show("Veuillez saisir le nom!");
                }
            }
            else
            {
                MessageBox.Show("Veuillez saisir le numéro CIN!  ");
            }





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
            for (int i = 1900; i < DateTime.Now.Year ; i++)
            {
                YearBEdit.Items.Add(i.ToString());
            }
        }
    }
}
