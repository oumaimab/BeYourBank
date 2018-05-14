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
using System.Collections.ObjectModel;
using System.IO;
using System.Configuration;

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour AddCard.xaml
    /// </summary>
    public partial class AddCard : Window
    {
        private OleDbConnection connection = new OleDbConnection();
        private ObservableCollection<Beneficiaire> liste_creation = new ObservableCollection<Beneficiaire>();
        private string modeC = null;
        public AddCard(string idUser, string mode)
        {
            InitializeComponent();
            lbl_idUser.Content = idUser;
            modeC = mode;
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "SELECT nomType FROM TypeCarte";
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connection);
                DataTable dt = new DataTable("TypeCarte");
                connection.Open();
                dataAdapter.Fill(dt);
                connection.Close();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comboBox_type.Items.Add(dt.Rows[i]["nomType"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connection" + ex);
            }

        }

        private void btn_Annuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                connection.Open();
                for (int i=0; i<lstBox_CIN.Items.Count; i++)
                {
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    command.CommandText = "select * from Beneficiaire where noCINBeneficiaire ='" + lstBox_CIN.Items[i].ToString() + "';";
                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Beneficiaire bn = new Beneficiaire(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), reader[8].ToString(), reader[9].ToString(), reader[10].ToString(), reader[11].ToString(), reader[12].ToString());
                        liste_creation.Add(bn);
                    }
                    
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connection" + ex);
            }

            string referenceConvention = null;
            string codeProduit = null;
            string numCompte = null;
            string codeCompagnie = null;
            string nomOrganisme = null;
            string raisonSociale = null;
            string idFichier = null;
            string index = null;
            try
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Convention WHERE refConvention= (SELECT idConvention FROM Utilisateurs WHERE noCINUser='" + lbl_idUser.Content.ToString() + "');";
                OleDbDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    referenceConvention = reader[0].ToString();
                    codeProduit = reader[1].ToString();
                    numCompte = reader[2].ToString();
                    codeCompagnie = reader[3].ToString();
                    nomOrganisme = reader[4].ToString();
                    raisonSociale = reader[5].ToString();
                    index = reader[6].ToString();
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connexion" + ex);
            }

            string dateTodayDay = null;
            string dateTodayMonth = null;
            string dateTodayYear2 = null;
            string dateTodayFormat = null;
            string seq = "00001";
            string centreFrais = null;
            string codeVille = "780";
            string zoneLibre = null;

            if (System.DateTime.Now.Day.ToString().Length == 1)
            {
                dateTodayDay = "0" + System.DateTime.Now.Day;
            }
            else if (System.DateTime.Now.Day.ToString().Length == 2)
            {
                dateTodayDay = System.DateTime.Now.Day.ToString();
            }

            if (System.DateTime.Now.Month.ToString().Length == 1)
            {
                dateTodayMonth = "0" + System.DateTime.Now.Month;
            }
            else if (System.DateTime.Now.Day.ToString().Length == 2)
            {
                dateTodayMonth = System.DateTime.Now.Month.ToString();
            }

            dateTodayFormat = dateTodayDay + dateTodayMonth + System.DateTime.Now.Year.ToString();
            dateTodayYear2 = System.DateTime.Now.Year.ToString().ElementAt(2).ToString() + System.DateTime.Now.Year.ToString().ElementAt(3).ToString();
            connection.Open();
            index = (Int32.Parse(index) + 1).ToString();
            OleDbCommand command1 = new OleDbCommand();
            command1.Connection = connection;
            command1.CommandText = " UPDATE Convention SET IndexFichier = '" + index + "' where refConvention = '" + referenceConvention + "';";
            command1.ExecuteNonQuery();
            connection.Close();
            if (string.IsNullOrEmpty(index))
            {
                index = "000";
            }
            if (index.Length > 3)
            {
                index = "000";
            }
            else if(index.Length == 1)
            {
                index = "00" + index;
            }
            else if (index.Length == 2)
            {
                index = "0" + index;
            }

            idFichier = dateTodayYear2 + dateTodayMonth + dateTodayDay + index;
            //string fichier = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "PREP_CONVENTION000000." + dateTodayYear2 + dateTodayMonth + dateTodayDay + index;
            string fichier = AppDomain.CurrentDomain.BaseDirectory + "PREP_CONVENTION000000." + idFichier ;
            using (StreamWriter writer = new StreamWriter(fichier, true))
            {
                writer.WriteLine("7FH" + codeCompagnie + seq + dateTodayFormat + System.DateTime.Now.Hour + System.DateTime.Now.Minute + System.DateTime.Now.Second + index);
                for (int k=0; k<liste_creation.Count; k++)
                {
                    if (k < 10)
                    {
                        int i = k+2;
                        seq = i.ToString().PadLeft(5, '0');
                    }
                    else if (k >= 10 && k<100)
                    {
                        int i = k+2;
                        seq = i.ToString().PadLeft(4, '0');
                    }
                    else if (k >= 100 && k < 1000)
                    {
                        int i = k+2;
                        seq = i.ToString().PadLeft(3, '0');
                    }
                    else if (k >= 1000 && k < 10000)
                    {
                        int i = k+2;
                        seq = i.ToString().PadLeft(2, '0');
                    }
                    else
                    {
                        int i = k+2;
                        seq = i.ToString();
                    }
                    
                    if(nomOrganisme.Length < 25)
                    {
                        int l = 25 - nomOrganisme.Length;
                        string spaces = null;
                        for (int i = 0; i < l; i++)
                        {
                            spaces = spaces + " ";
                        }
                        nomOrganisme = nomOrganisme + spaces;
                    }
                    codeVille = numCompte.Substring(0, 3);
                    centreFrais = "0" + codeVille + numCompte.Substring(5, 2);
                    if (numCompte.Length < 24)
                    {
                        int l = 24 - numCompte.Length;
                        string zeros = null;
                        for (int i=0; i<l; i++)
                        {
                            zeros = zeros + "0";
                        }
                        numCompte = numCompte + zeros ;
                    }

                    if(referenceConvention.Length < 14)
                    {
                        int l = 14 - referenceConvention.Length;
                        string spaces = null;
                        for (int i = 0; i < l; i++)
                        {
                            spaces = spaces + " ";
                        }
                        referenceConvention = referenceConvention + spaces;
                    }

                    if (codeProduit.Length < 5)
                    {
                        int l = 5 - codeProduit.Length;
                        string spaces = null;
                        for (int i = 0; i < l; i++)
                        {
                            spaces = spaces + " ";
                        }
                        codeProduit = codeProduit + spaces;
                    }
                    string np = liste_creation[k].nom.ToString() +" "+ liste_creation[k].prenom.ToString();
                    np = np.ToUpper();
                    if(np.Length < 25)
                    {
                        int l = 25 - np.Length;
                        string spaces = null;
                        for (int i = 0; i < l; i++)
                        {
                            spaces = spaces + " ";
                        }
                        np = np + spaces;
                    }
                    string telB = liste_creation[k].tel.ToString();
                    if (telB.Length < 20)
                    {
                        int l = 20 - telB.Length;
                        string spaces = null;
                        for (int i = 0; i < l; i++)
                        {
                            spaces = spaces + " ";
                        }
                        telB = telB + spaces;
                    }

                    string profession = liste_creation[k].profession.ToString();
                    if( profession.Length < 20)
                    {
                        int l = 20 - profession.Length;
                        string spaces = null;
                        for (int i = 0; i < l; i++)
                        {
                            spaces = spaces + " ";
                        }
                        profession = profession+ spaces;
                    }

                    string full_adresse = liste_creation[k].adresse.ToString() +" "+ liste_creation[k].ville.ToString();
                    if (full_adresse.Length < 90)
                    {
                        int l = 90 - full_adresse.Length;
                        string spaces = null;
                        for (int i = 0; i < l; i++)
                        {
                            spaces = spaces + " ";
                        }
                        full_adresse = full_adresse + spaces;
                    }

                    string titre = liste_creation[k].titre.ToString();
                    if (titre.Length < 4)
                    {
                        int l = 4 - titre.Length;
                        string spaces = null;
                        for (int i = 0; i < l; i++)
                        {
                            spaces = spaces + " ";
                        }
                        titre = titre + spaces;
                    }

                    for(int j=0; j < 200; j++)
                    {
                        zoneLibre = zoneLibre + " ";
                    }
                    writer.WriteLine("7DR" + seq + "0011" + centreFrais + nomOrganisme + numCompte + referenceConvention + codeProduit + modeC + dateTodayFormat + "                   " + "10504" + "             " + "                              " + liste_creation[k].CIN.ToString() + "                    " + np + telB + liste_creation[k].dateNaissance.ToString() + profession + full_adresse + codeVille + liste_creation[k].codePostal.ToString() + liste_creation[k].sex.ToString() + titre + liste_creation[k].statut.ToString() + zoneLibre);
                }
                seq = (Int32.Parse(seq) + 1).ToString();
                if (seq.Length < 5)
                {
                    int l = 5 - seq.Length;
                    string zeros = null;
                    for(int i=0; i<l; i++)
                    {
                        zeros = zeros + "0";
                    }
                    seq = zeros + seq;

                }
                writer.WriteLine("7FT" + seq + dateTodayFormat + System.DateTime.Now.Hour + System.DateTime.Now.Minute + System.DateTime.Now.Second + index);

            }
            MessageBox.Show("Le fichier a bien été créé dans l'emplacement spécifié!", "ok", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
