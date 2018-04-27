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

namespace BeYourBank
{
    /// <summary>
    /// Logique d'interaction pour WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        public WelcomeWindow(string idUser)
        {
            InitializeComponent();
            lbl_idUser_welcome.Content = idUser;

        }
        private void btn_listeBeneficiaires_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Page_listeBeneficiaires(lbl_idUser_welcome.Content.ToString());
        }

        private void btn_creationCartes_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Page_creationDeCartes(lbl_idUser_welcome.Content.ToString());
        }

        private void btn_operationsCartes_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Page_operations(lbl_idUser_welcome.Content.ToString());
            
        }

        private void btnDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow main = new MainWindow();
            main.Show();
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            Main.Content = new Page_listeBeneficiaires(lbl_idUser_welcome.Content.ToString());
        }
    }
}
