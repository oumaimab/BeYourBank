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

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour AddBenefWindow.xaml
    /// </summary>
    public partial class AddBenefWindow : Window
    {
        private OleDbConnection connection = new OleDbConnection();
        public AddBenefWindow(string idUser)
        {
            InitializeComponent();
            lbl_user_id.Content = idUser;
            connection.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" + @"Data Source=C:\Users\MYC\Documents\PFE\BeYourBankBD.accdb";
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
                            if (DayB.SelectedItem.ToString() != null)
                            {
                                if (MonthB.SelectedItem.ToString() != null)
                                {
                                    if (YearB.Text.ToString().Length == 4)
                                    {
                                        if (prf.Text != "")
                                        {
                                            if (adr.Text != "")
                                            {
                                                if (villeBenef.Text != "")
                                                {
                                                    if (codeP.Text.ToString().Length == 5)
                                                    {
                                                        if (sexCombo.SelectedItem.ToString() != null)
                                                        {
                                                            if (titreCombo.SelectedItem.ToString() != null)
                                                            {
                                                                if (statutCombo.SelectedItem.ToString() != null)
                                                                {
                                                                    if (statutCombo.SelectedItem.ToString() == "Célibataire")
                                                                    {
                                                                        statutB = "S";
                                                                    }
                                                                    else if (statutCombo.SelectedItem.ToString() == "Marié")
                                                                    {
                                                                        statutB = "Z";
                                                                    }
                                                                    else if (statutCombo.SelectedItem.ToString() == "Veuf")
                                                                    {
                                                                        statutB = "V";
                                                                    }
                                                                    else if (statutCombo.SelectedItem.ToString() == "Divorcé")
                                                                    {
                                                                        statutB = "R";
                                                                    }
                                                                    else if (statutCombo.SelectedItem.ToString() == "Séparé")
                                                                    {
                                                                        statutB = "O";
                                                                    }
                                                                    else if (statutCombo.SelectedItem.ToString() == "Conjoint")
                                                                    {
                                                                        statutB = "D";
                                                                    }
                                                                    else if (statutCombo.SelectedItem.ToString() == "Pas déclaré")
                                                                    {
                                                                        statutB = "X";
                                                                    }
                                                                    cmd.Connection = connection;
                                                                    cmd.CommandText = "INSERT INTO Beneficiaire Values ('" + CINBenef.Text + "', '" + BenefLName.Text + "', '" + BenefFName.Text + "', '" + telBenef.Text + "', '" + DayB.SelectedItem.ToString() + MonthB.SelectedItem.ToString() + YearB.Text + "', '" + prf.Text + "', '" + adr.Text + "', '" + villeBenef.Text + "', '" + codeP.Text + "', '" + sexCombo.SelectedItem.ToString() + "', '" + titreCombo.SelectedItem.ToString() + "', '" + statutB + "', '" + lbl_user_id + "' )";
                                                                    cmd.ExecuteNonQuery();
                                                                    connection.Close();
                                                                    MessageBox.Show("Beneficiaire ajouté");
                                                                    this.Hide();
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
                                                            MessageBox.Show("Veuillez sélectionner le sex du beneficiaire");
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
                        MessageBox.Show("Veuillez saisir le prénom ");
                    }
                } else
                {
                    MessageBox.Show("Veuillez saisir le nom");
                }
            }
            else
            {
                MessageBox.Show("Veuillez saisir le numéro CIN  ");
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 1; i < 32; i++)
            {
                if (i < 10)
                {
                    DayB.Items.Add("0"+i);
                }
                else
                {
                    DayB.Items.Add(i);
                }
                
            }
            for (int i = 1; i < 13; i++)
            {
                if (i < 10)
                {
                    MonthB.Items.Add("0" + i);
                }
                else
                {
                    MonthB.Items.Add(i.ToString());
                } 
            }
        }

        private void YearB_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
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