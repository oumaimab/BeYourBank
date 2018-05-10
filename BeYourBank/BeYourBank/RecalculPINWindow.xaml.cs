using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
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
    /// Logique d'interaction pour RecalculPINWindow.xaml
    /// </summary>
    public partial class RecalculPINWindow : Window
    {
        private OleDbConnection connection = new OleDbConnection();
        private ObservableCollection<BeneficiaireCard> liste_rec = new ObservableCollection<BeneficiaireCard>();
        public RecalculPINWindow(String idUser)
        {
            InitializeComponent();
            lbl_idUser.Content = idUser;
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_confirm_Click(object sender, RoutedEventArgs e)
        {
            string referenceConvention = null;
            string codeProduit = null;
            string numCompte = null;
            string codeCompagnie = null;
            string nomOrganisme = null;
            string raisonSociale = null;
            string index = null;

            try
            {
                connection.Open();
                for (int i = 0; i < listBox_CIN.Items.Count; i++)
                {
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;
                    command.CommandText = "select * from Beneficiaire, Carte where noCINBeneficiaire = idBeneficiaire and noCINBeneficiaire ='" + listBox_CIN.Items[i].ToString() + "';";
                    OleDbDataReader reader = command.ExecuteReader();
                    //création de l'objet BeneficiaireCard et alimentation de la liste
                    while (reader.Read())
                    {
                        BeneficiaireCard bn = new BeneficiaireCard((string)reader[0], (string)reader[1], (string)reader[2], (string)reader[3], (string)reader[4], (string)reader[5], (string)reader[6], (string)reader[7], (string)reader[8], (string)reader[9], (string)reader[10], (string)reader[11], (string)reader[12]);
                        bn.numCarte = (string)reader[13];
                        bn.nomEmbosse = (string)reader[16];
                        liste_rec.Add(bn);
                    }
                    reader.Close();
                }

                OleDbCommand command1 = new OleDbCommand();
                OleDbCommand command2 = new OleDbCommand();
                OleDbCommand command3 = new OleDbCommand();
                command1.Connection = connection;
                command2.Connection = connection;
                command3.Connection = connection;

                command1.CommandText = "SELECT * FROM Convention WHERE refConvention= (SELECT idConvention FROM Utilisateurs WHERE noCINUser='" + lbl_idUser.Content.ToString() + "');";
                OleDbDataReader reader1 = command1.ExecuteReader();

                // extraction des informations sur la convention
                while (reader1.Read())
                {
                    referenceConvention = (string)reader1[0];
                    codeProduit = (string)reader1[1];
                    numCompte = (string)reader1[2];
                    codeCompagnie = (string)reader1[3];
                    nomOrganisme = (string)reader1[4];
                    raisonSociale = (string)reader1[5];
                    index = (string)reader1[6];
                }
                reader1.Close();
                //incrémenation de l'index et mise à jour de sa valeur dans la BD
                index = (Int32.Parse(index) + 1).ToString();
                command2.CommandText = " UPDATE Convention SET IndexFichier = '" + index + "' where refConvention = '" + referenceConvention + "';";
                command2.ExecuteNonQuery();

                for (int j = 0; j < liste_rec.Count; j++)
                {
                    //alimenter la table OpRecalculPIN
                    command3.CommandText = " insert into OpRecalculPIN (dateRecalcul, numCarte) Values ('" + System.DateTime.Now.Date.ToString("d") + "', '" + (string)liste_rec[j].numCarte + "');";
                    command3.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connection" + ex);
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

            //dateTodayFormat représente la date sous le format DDMMYYYY
            dateTodayFormat = dateTodayDay + dateTodayMonth + System.DateTime.Now.Year.ToString();
            //dateTodayYear2 représente l'année sur 2 positions YY
            dateTodayYear2 = System.DateTime.Now.Year.ToString().ElementAt(2).ToString() + System.DateTime.Now.Year.ToString().ElementAt(3).ToString();

            //index est sur 3 positions
            if (string.IsNullOrEmpty(index))
            {
                index = "000";
            }
            if (index.Length > 3)
            {
                index = "000";
            }
            else if (index.Length == 1)
            {
                index = "00" + index;
            }
            else if (index.Length == 2)
            {
                index = "0" + index;
            }

            //création du nom de fichier
            string fichier = "PREP_CONVENTION000000." + dateTodayYear2 + dateTodayMonth + dateTodayDay + index + ".txt";
            using (StreamWriter writer = new StreamWriter(fichier, true))
            {
                //header du fichier
                writer.WriteLine("7FH" + codeCompagnie + seq + dateTodayFormat + System.DateTime.Now.Hour + System.DateTime.Now.Minute + System.DateTime.Now.Second + index);
                for (int k = 0; k < liste_rec.Count; k++)
                {
                    // la séquence de chaque enregistrement est sur 5 positions
                    if (k < 10)
                    {
                        int i = k + 2;
                        seq = i.ToString().PadLeft(5, '0');
                    }
                    else if (k >= 10 && k < 100)
                    {
                        int i = k + 2;
                        seq = i.ToString().PadLeft(4, '0');
                    }
                    else if (k >= 100 && k < 1000)
                    {
                        int i = k + 2;
                        seq = i.ToString().PadLeft(3, '0');
                    }
                    else if (k >= 1000 && k < 10000)
                    {
                        int i = k + 2;
                        seq = i.ToString().PadLeft(2, '0');
                    }
                    else
                    {
                        int i = k + 2;
                        seq = i.ToString();
                    }

                    // nom de l'organisme sur 25 positions à compléter avec des espaces
                    if (nomOrganisme.Length < 25)
                    {
                        int l = 25 - nomOrganisme.Length;
                        string spaces = null;
                        for (int i = 0; i < l; i++)
                        {
                            spaces = spaces + " ";
                        }
                        nomOrganisme = nomOrganisme + spaces;
                    }

                    // le code ville et centre de frais sont extraits à partir du numéro de compte
                    codeVille = numCompte.Substring(0, 3);
                    centreFrais = "0" + codeVille + numCompte.Substring(5, 2);

                    //numéro de compte est sur 24 positions à compléter avec des zéros
                    if (numCompte.Length < 24)
                    {
                        int l = 24 - numCompte.Length;
                        string zeros = null;
                        for (int i = 0; i < l; i++)
                        {
                            zeros = zeros + "0";
                        }
                        numCompte = numCompte + zeros;
                    }

                    //référence convention sur 14 positions à compléter avec des espaces
                    if (referenceConvention.Length < 14)
                    {
                        int l = 14 - referenceConvention.Length;
                        string spaces = null;
                        for (int i = 0; i < l; i++)
                        {
                            spaces = spaces + " ";
                        }
                        referenceConvention = referenceConvention + spaces;
                    }

                    //référence convention sur 5 positions à compléter avec des espaces
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

                    string lblCard = (string)liste_rec[k].nomEmbosse;
                    //le nom est sur 25 positions à compléter avec des espaces
                    if (lblCard.Length < 25)
                    {
                        int l = 25 - lblCard.Length;
                        string spaces = null;
                        for (int i = 0; i < l; i++)
                        {
                            spaces = spaces + " ";
                        }
                        lblCard = lblCard + spaces;
                    }
                    string telB = liste_rec[k].tel.ToString();
                    //le tel du béneficiaire est sur 20 positions
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

                    //la profession est sur 20 positions à compléter avec des espaces
                    string profession = liste_rec[k].profession.ToString();
                    if (profession.Length < 20)
                    {
                        int l = 20 - profession.Length;
                        string spaces = null;
                        for (int i = 0; i < l; i++)
                        {
                            spaces = spaces + " ";
                        }
                        profession = profession + spaces;
                    }

                    //l'adresse totale est sur 90 positions
                    string full_adresse = liste_rec[k].adresse.ToString() + " " + liste_rec[k].ville.ToString();
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

                    //le titre est sur 4 positions à compléter avec des espaces
                    string titre = liste_rec[k].titre.ToString();
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

                    string numCarte = liste_rec[k].numCarte.ToString();
                    if (numCarte.Length < 19)
                    {
                        int l = 19 - numCarte.Length;
                        string spaces = null;
                        for (int i = 0; i < l; i++)
                        {
                            spaces = spaces + " ";
                        }
                        numCarte = numCarte + spaces;
                    }

                    // zone libre sur 200 positions
                    for (int j = 0; j < 200; j++)
                    {
                        zoneLibre = zoneLibre + " ";
                    }

                    writer.WriteLine("7DR" + seq + "0011" + centreFrais + nomOrganisme + numCompte + referenceConvention + codeProduit + "I" + dateTodayFormat + numCarte + "10504" + "             " + "                              " + liste_rec[k].CIN.ToString() + "                    " + lblCard + telB + liste_rec[k].dateNaissance.ToString() + profession + full_adresse + codeVille + liste_rec[k].codePostal.ToString() + liste_rec[k].sex.ToString() + titre + liste_rec[k].statut.ToString() + zoneLibre);
                }
                seq = (Int32.Parse(seq) + 1).ToString();
                if (seq.Length < 5)
                {
                    int l = 5 - seq.Length;
                    string zeros = null;
                    for (int i = 0; i < l; i++)
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
