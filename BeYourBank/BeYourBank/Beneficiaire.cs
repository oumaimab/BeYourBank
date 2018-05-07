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

        public string getNoCIN()
        {
            string noCIN = this.CIN;
            return noCIN;
        }

        public string getNom()
        {
            string nom = this.nom;
            return nom;
        }

        public string getPrenom()
        {
            string prenom = this.prenom;
            return prenom;
        }

        public void setnoCIN(string noCIN)
        {
            this.CIN = noCIN;
        }

        public void setNom(string nom)
        {
            this.nom = nom;
        }

        public void setPrenom(string prenom)
        {
            this.prenom = prenom;
        }

        public void setTel(string tel)
        {
            this.tel = tel;
        }

        public void setDateNaissance(string dateN)
        {
            this.dateNaissance = dateN;
        }

        public void setProfession(string pr)
        {
            this.profession = pr;
        }

        public void setAdresse(string adresse)
        {
            this.adresse = adresse;
        }
    }
}
