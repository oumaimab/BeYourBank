using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeYourBank
{
    public class Beneficiaire
    {
        public string CIN { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string tel { get; set; }
        public string dateNaissance { get; set; }
        public string profession { get; set; }
        public string adresse { get; set; }
        public string ville { get; set; }
        public string codePostal { get; set; }
        public string sex { get; set; }
        public string titre { get; set; }
        public string statut { get; set; }
        public string idUser { get; set; }
        public bool MyBool { get; set; }

        public Beneficiaire(string noCIN)
        {
            this.CIN = noCIN;
        }

        public Beneficiaire(string noCIN, string nom, string prenom)
        {
            this.CIN = noCIN;
            this.prenom = prenom;
            this.nom = nom;
        }

        public Beneficiaire(string noCIN, string nom, string prenom, string tel, string dateNaissance, string profession, string adresse, string ville, string codePostal, string sex, string titre, string statut, string idUser)
        {
            this.CIN = noCIN;
            this.prenom = prenom;
            this.nom = nom;
            this.tel = tel;
            this.dateNaissance = dateNaissance;
            this.profession = profession;
            this.adresse= adresse;
            this.ville = ville;
            this.codePostal = codePostal;
            this.sex = sex;
            this.titre = titre;
            this.statut = statut;
            this.idUser = idUser;
         }
    }

    public class BeneficiaireCard
    {
        public string CIN { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string tel { get; set; }
        public string dateNaissance { get; set; }
        public string profession { get; set; }
        public string adresse { get; set; }
        public string ville { get; set; }
        public string codePostal { get; set; }
        public string sex { get; set; }
        public string titre { get; set; }
        public string statut { get; set; }
        public string idUser { get; set; }
        public string numCarte { get; set; }
        public string montantRecharge { get; set; }
        public string nomEmbosse { get; set; }
        public string motif { get; set; }
        public string fullName { get; set; }
        public bool MyBool { get; set; }

        public BeneficiaireCard(string noCIN, string nom, string prenom, string tel, string dateNaissance, string profession, string adresse, string ville, string codePostal, string sex, string titre, string statut, string idUser)
        {
            this.CIN = noCIN;
            this.prenom = prenom;
            this.nom = nom;
            this.tel = tel;
            this.dateNaissance = dateNaissance;
            this.profession = profession;
            this.adresse = adresse;
            this.ville = ville;
            this.codePostal = codePostal;
            this.sex = sex;
            this.titre = titre;
            this.statut = statut;
            this.idUser = idUser;
        }

        public BeneficiaireCard(string noCIN, string Name, string numCarte, string montant)
        {
            this.CIN = noCIN;
            this.fullName = Name;
            this.numCarte = numCarte;
            this.montantRecharge = montant;
        }

        public BeneficiaireCard(string noCIN, string Name,string nomEmbosse)
        {
            this.CIN = noCIN;
            this.fullName = Name;
            this.nomEmbosse = nomEmbosse;
        }

        public BeneficiaireCard(string noCIN, string Name)
        {
            this.CIN = noCIN;
            this.fullName = Name;
        }
    }
}
