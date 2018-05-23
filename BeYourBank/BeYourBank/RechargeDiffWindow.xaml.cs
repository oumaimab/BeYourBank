﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
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
    /// <summary>
    /// Logique d'interaction pour RechargeDiffWindow.xaml
    /// </summary>
    public partial class RechargeDiffWindow : Window
    {
        private OleDbConnection connection = new OleDbConnection();
        private ObservableCollection<BeneficiaireCard> liste_recharge = new ObservableCollection<BeneficiaireCard>();
        public RechargeDiffWindow(string idUser)
        {
            InitializeComponent();
            lbl_idUser.Content = idUser;
            connection.ConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ToString();

        }

        private void txtBox_montant_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$|^[0-9]*[,]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_confirm_Click(object sender, RoutedEventArgs e)
        {
            string m_init = null;
            string m_int = null;
            string m_dec = null;
            string m_dec_3 = null;
            int m_dec_2 = 0;
            int mRange = 0;
            // string decimalM = null;

            string montantAvecDec = null;
            string referenceConvention = null;
            string codeProduit = null;
            string numCompte = null;
            string codeCompagnie = null;
            string nomOrganisme = null;
            string raisonSociale = null;
            string index = null;

            string dateTodayDay = null;
            string dateTodayMonth = null;
            string dateTodayYear2 = null;
            string dateTodayFormat = null;
            string idFichier = null;

            try
            {
                connection.Open();
                foreach (var item in listView.Items)
                {
                    BeneficiaireCard Bc = (BeneficiaireCard)item;
                    OleDbCommand command = new OleDbCommand();
                    command.Connection = connection;

                    m_init = Bc.montantRecharge;
                    for(int q=0; q<m_init.Length; q++)
                    {
                        char c = m_init[q];
                        if(c == ',' || c == '.')
                        {
                            mRange = q;
                        }
                    }

                    if (mRange > 0)
                    {
                        m_int = m_init.Substring(0, mRange);
                        m_dec = m_init.Substring(mRange + 1, m_init.Length - (mRange + 1));
                    }
                    else
                    {
                        m_int = m_init;
                        m_dec = "0";
                    }

                    if(m_dec.Length > 2)
                    {
                        m_dec_3 = m_dec.Substring(0, 3);
                        m_dec_2 = Int32.Parse(m_dec.Substring(0, 2));
                        if (m_dec_2 == 99)
                        {
                            m_dec = m_dec_2.ToString();
                        }
                        else if (m_dec_2 < 99)
                        {
                            int i = 0;
                            i = Int32.Parse(m_dec_3.Substring(2, 1)); //prendre le dernier entier des 3 nombres après la virgule
                            if (i <= 5) m_dec = m_dec_2.ToString(); //  ne rien faire si le 3ieme inf a 5
                            else if (i > 5) m_dec = (m_dec_2 + 1).ToString();//  incrémenter de 1 si le 3ieme sup a 5
                        }
                    }
                    else if(m_dec.Length == 1)
                    {
                        m_dec = m_dec + "0";
                    }
                    montantAvecDec = m_int + m_dec; //concaténation de la partie entiere et decimale
                
                    command.CommandText = "SELECT * FROM Beneficiaire, Carte where noCINBeneficiaire = idBeneficiaire and noCINBeneficiaire ='" + Bc.CIN.ToString() + "';";
                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Bc.prenom = (string)reader[1];
                        Bc.nom = (string)reader[2];
                        Bc.tel = (string)reader[3];
                        Bc.dateNaissance = (string)reader[4];
                        Bc.profession = (string)reader[5];
                        Bc.adresse = (string)reader[6];
                        Bc.ville = (string)reader[7];
                        Bc.codePostal = (string)reader[8];
                        Bc.sex = (string)reader[9];
                        Bc.titre = (string)reader[10];
                        Bc.statut = (string)reader[11];
                        Bc.idUser = (string)reader[12];
                        Bc.nomEmbosse = (string)reader[16];
                        Bc.montantRecharge = montantAvecDec;
                        liste_recharge.Add(Bc);
                    }
                    reader.Close();
                }
                OleDbCommand command1 = new OleDbCommand();
                OleDbCommand command2 = new OleDbCommand();
                command1.Connection = connection;
                command2.Connection = connection;

                command1.CommandText = "SELECT * FROM Convention WHERE refConvention= (SELECT idConvention FROM Utilisateurs WHERE noCINUser='" + lbl_idUser.Content.ToString() + "');";
                OleDbDataReader reader1 = command1.ExecuteReader();
                // extraction des informations sur la convention
                while (reader1.Read())
                {
                    referenceConvention = reader1[0].ToString();
                    MessageBox.Show(referenceConvention);
                    codeProduit = reader1[1].ToString();
                    numCompte = reader1[2].ToString();
                    codeCompagnie = reader1[3].ToString();
                    nomOrganisme = reader1[4].ToString();
                    raisonSociale = reader1[5].ToString();
                    index = reader1[6].ToString();
                }
                reader1.Close();
                
                //incrémenation de l'index et mise à jour de sa valeur dans la BD
                index = (Int32.Parse(index) + 1).ToString();
                command2.CommandText = " UPDATE Convention SET IndexFichier = '" + index + "' where refConvention = '" + referenceConvention + "';";
                command2.ExecuteNonQuery();

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

                //on definit l'id fichier
                idFichier = dateTodayYear2 + dateTodayMonth + dateTodayDay + index;

                //alimenter la table Operations
                OleDbCommand command3 = new OleDbCommand();
                command3.Connection = connection;
                for (int i=0; i<liste_recharge.Count; i++)
                {
                    command3.CommandText = " insert into Operations (dateOperation, numCarte, TypeOperation , idFichier , motif) Values ('" + DateTime.Now.Date.ToString("d") + "', '" + liste_recharge[i].numCarte.ToString() + "', 'Recharge' , '" + idFichier + "', '"+ liste_recharge[i].montantRecharge.ToString() +"');";
                    command3.ExecuteNonQuery();
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de connection" + ex);
            }

            string seq = "00001";
            string centreFrais = null;
            string codeVille = "780";
            string zoneLibre = null;

            // Code Compagnie sur 6 positions à compléter avec des espaces
            if (codeCompagnie.Length < 6)
            {
                int l = 6 - codeCompagnie.Length;
                string spaces = null;
                for (int i = 0; i < l; i++)
                {
                    spaces = spaces + " ";
                }
                codeCompagnie = codeCompagnie + spaces;
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

            //création du nom de fichier
            string fichier = AppDomain.CurrentDomain.BaseDirectory + "PREP_CONVENTION" + codeCompagnie + "." + idFichier;
            using (StreamWriter writer = new StreamWriter(fichier, true))
            {
                
                //header du fichier
                writer.WriteLine("7FH" + codeCompagnie + seq + dateTodayFormat + System.DateTime.Now.Hour + System.DateTime.Now.Minute + System.DateTime.Now.Second + index);

                //Boucle pour créer tous les enregistrements de la liste
                for (int k = 0; k < liste_recharge.Count; k++)
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

                    

                    string lblCard = (string)liste_recharge[k].nomEmbosse;
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
                    string CIN = liste_recharge[k].CIN.ToString();
                    if (CIN.Length < 8)
                    {
                        int l = 25 - CIN.Length;
                        string spaces = null;
                        for (int i = 0; i < l; i++)
                        {
                            spaces = spaces + " ";
                        }
                        CIN = CIN + spaces;
                    }

                    //le tel du béneficiaire est sur 20 positions
                    string telB = Regex.Replace(liste_recharge[k].tel.ToString(), @"\s", "");
                    string dTel = telB.Substring(0, 1);
                    string telF = null;
                    if (!dTel.Equals("0")) telB = "0" + telB;
                    if (telB.Length % 2 != 0) telB = telB + " ";
                    for (int i = 0; i < telB.Length; i++)
                    {
                        telF = telF + telB.Substring(i, 2) + " ";
                        i = i + 1;
                    }
                    if (telF.Length < 20)
                    {
                        int l = 20 - telF.Length;
                        string spaces = null;
                        for (int i = 0; i < l; i++)
                        {
                            spaces = spaces + " ";
                        }
                        telF = telF + spaces;
                    }

                    //la profession est sur 20 positions à compléter avec des espaces
                    string profession = liste_recharge[k].profession.ToString();
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
                    string full_adresse = liste_recharge[k].adresse.ToString() + " " + liste_recharge[k].ville.ToString();
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
                    string titre = liste_recharge[k].titre.ToString();
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

                    string numCarte = liste_recharge[k].numCarte.ToString();
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

                    string mR = liste_recharge[k].montantRecharge.ToString();
                    if (mR.Length < 12)
                    {
                        int l = 10 - mR.Length;
                        string zeros = null;
                        for (int i = 0; i < l; i++)
                        {
                            zeros = zeros + "0";
                        }
                        mR = zeros + mR ;
                    }


                    // zone libre sur 200 positions
                    for (int j = 0; j < 200; j++)
                    {
                        zoneLibre = zoneLibre + " ";
                    }

                    writer.WriteLine("7DR" + seq + "0011" + centreFrais + nomOrganisme + numCompte + referenceConvention + codeProduit + "R" + dateTodayFormat + numCarte + "10504" + mR + "                               " + CIN + "                    " + lblCard + telF + liste_recharge[k].dateNaissance.ToString() + profession + full_adresse + codeVille + liste_recharge[k].codePostal.ToString() + liste_recharge[k].sex.ToString() + titre + liste_recharge[k].statut.ToString() + zoneLibre);
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
            //MessageBox.Show("Le fichier a bien été créé dans l'emplacement spécifié!", "ok", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
            fichierGenreWindow fgW = new fichierGenreWindow(idFichier);
            fgW.ShowDialog();
        }
    }
}
