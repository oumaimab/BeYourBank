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
using System.Windows.Shapes;
using System.Data.OleDb;
using System.Data;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Globalization;

namespace BeYourBank
{

    public partial class AddBenefWindow : Window
    {
        private OleDbConnection connection = new OleDbConnection();
        public AddBenefWindow(string idUser)
        {
            InitializeComponent();
            lbl_user_id.Content = idUser;
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
        }

        private void adButton_Click(object sender, RoutedEventArgs e)
        {
            string statutB = null;
            OleDbCommand cmd = new OleDbCommand();
            if (connection.State != ConnectionState.Open)
                connection.Open();
            if (CINBenef.Text != "")
            {
                if (BenefLName.Text != "")
                {
                    if (BenefFName.Text != "")
                    {
                        if (telBenef.Text != "")
                        {
                            if (!string.IsNullOrEmpty(DayB.SelectionBoxItem.ToString()))
                            {
                                if (!string.IsNullOrEmpty(MonthB.SelectionBoxItem.ToString()))
                                {
                                    if (!string.IsNullOrEmpty(YearB.SelectionBoxItem.ToString()))
                                    {
                                        if (prf.Text != "")
                                        {
                                            if (adr.Text != "")
                                            {
                                                if (villeBenef.Text != "")
                                                {
                                                    if (codeP.Text.ToString().Length == 5)
                                                    {
                                                        if (!string.IsNullOrEmpty(sexCombo.SelectionBoxItem.ToString()))
                                                        {
                                                            if (!string.IsNullOrEmpty(titreCombo.SelectionBoxItem.ToString()))
                                                            {
                                                                if (!string.IsNullOrEmpty(statutCombo.SelectionBoxItem.ToString()))
                                                                {
                                                                    if (statutCombo.SelectionBoxItem.ToString() == "Célibataire")
                                                                    {
                                                                        statutB = "S";
                                                                    }
                                                                    else if (statutCombo.SelectionBoxItem.ToString() == "Marié(e)")
                                                                    {
                                                                        statutB = "Z";
                                                                    }
                                                                    else if (statutCombo.SelectionBoxItem.ToString() == "Veuf(ve)")
                                                                    {
                                                                        statutB = "V";
                                                                    }
                                                                    else if (statutCombo.SelectionBoxItem.ToString() == "Divorcé(e)")
                                                                    {
                                                                        statutB = "R";
                                                                    }
                                                                    else if (statutCombo.SelectionBoxItem.ToString() == "Séparé(e)")
                                                                    {
                                                                        statutB = "O";
                                                                    }
                                                                    else if (statutCombo.SelectionBoxItem.ToString() == "Conjoint(e)")
                                                                    {
                                                                        statutB = "D";
                                                                    }
                                                                    else if (statutCombo.SelectionBoxItem.ToString() == "Pas déclaré")
                                                                    {
                                                                        statutB = "X";
                                                                    }
                                                                    cmd.Connection = connection;
                                                                    cmd.CommandText = "INSERT INTO Beneficiaire Values ('" + CINBenef.Text + "', '" + BenefLName.Text + "', '" + BenefFName.Text + "', '" + telBenef.Text + "', '" + DayB.SelectionBoxItem.ToString() + MonthB.SelectionBoxItem.ToString() + YearB.SelectionBoxItem.ToString() + "', '" + prf.Text + "', '" + adr.Text + "', '" + villeBenef.Text + "', '" + codeP.Text + "', '" + sexCombo.SelectionBoxItem.ToString() + "', '" + titreCombo.SelectionBoxItem.ToString() + "', '" + statutB + "', '" + lbl_user_id.Content.ToString() + "' )";
                                                                    cmd.ExecuteNonQuery();
                                                                    connection.Close();
                                                                    MessageBox.Show("Beneficiaire ajouté");
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
                                                            MessageBox.Show("Veuillez sélectionner le sexe du beneficiaire !");
                                                        }
                                                    } else
                                                    {
                                                        MessageBox.Show("Veuillez saisir un code Postale valide!");
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Veuillez saisir la ville de résidence !");
                                                }
                                            } else
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
                                } else
                                {
                                    MessageBox.Show("Veuillez sélectionner le mois de naissance !");
                                }
                            } else
                            {
                                MessageBox.Show("Veuillez sélectionner le jour de naissance !");
                            }
                        } else
                        {
                            MessageBox.Show("Veuillez saisir le numéro de télephone !");
                        }
                    } else
                    {
                        MessageBox.Show("Veuillez saisir le prénom! ");
                    }
                } else
                {
                    MessageBox.Show("Veuillez saisir le nom!");
                }
            }
            else
            {
                MessageBox.Show("Veuillez saisir le numéro CIN ! ");
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            String current_year = System.DateTime.Now.Year.ToString();
            for (int i = 1; i < 32; i++)
            {
                if (i < 10)
                {
                    DayB.Items.Add("0"+i.ToString());
                }
                else
                {
                    DayB.Items.Add(i.ToString());
                }
                
            }
            for (int i = 1; i < 13; i++)
            {
                if (i < 10)
                {
                    MonthB.Items.Add("0" + i.ToString());
                }
                else
                {
                    MonthB.Items.Add(i.ToString());
                } 
            }
            for (int i = 1900; i < DateTime.Now.Year +1; i++)
            {
                
                YearB.Items.Add(i.ToString());
            }
        }

        private void telBenef_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void codeP_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
    }
}