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
            btn_listeBeneficiaires.Background = Brushes.Wheat;
            btn_operationsCartes.Background = (Brush)(new BrushConverter().ConvertFrom("#FFEEEEEE"));
            btn_historique.Background = (Brush)(new BrushConverter().ConvertFrom("#FFEEEEEE"));
            btn_rapports.Background = (Brush)(new BrushConverter().ConvertFrom("#FFEEEEEE"));
            btn_creationCartes.Background = (Brush)(new BrushConverter().ConvertFrom("#FFEEEEEE"));
            btn_tableauDeBord.Background = (Brush)(new BrushConverter().ConvertFrom("#FFEEEEEE"));
        }

        private void btn_creationCartes_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Page_creationDeCartes(lbl_idUser_welcome.Content.ToString());
            btn_listeBeneficiaires.Background = (Brush)(new BrushConverter().ConvertFrom("#FFEEEEEE"));
            btn_operationsCartes.Background = (Brush)(new BrushConverter().ConvertFrom("#FFEEEEEE"));
            btn_historique.Background = (Brush)(new BrushConverter().ConvertFrom("#FFEEEEEE"));
            btn_rapports.Background = (Brush)(new BrushConverter().ConvertFrom("#FFEEEEEE"));
            btn_creationCartes.Background = Brushes.Wheat;
            btn_tableauDeBord.Background = (Brush)(new BrushConverter().ConvertFrom("#FFEEEEEE"));
        }

        private void btn_operationsCartes_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Page_operations(lbl_idUser_welcome.Content.ToString());
            btn_listeBeneficiaires.Background = (Brush)(new BrushConverter().ConvertFrom("#FFEEEEEE"));
            btn_operationsCartes.Background = Brushes.Wheat;
            btn_historique.Background = (Brush)(new BrushConverter().ConvertFrom("#FFEEEEEE"));
            btn_rapports.Background = (Brush)(new BrushConverter().ConvertFrom("#FFEEEEEE"));
            btn_creationCartes.Background = (Brush)(new BrushConverter().ConvertFrom("#FFEEEEEE"));
            btn_tableauDeBord.Background = (Brush)(new BrushConverter().ConvertFrom("#FFEEEEEE"));
        }

        private void btnDeconnexion_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow main = new MainWindow();
            main.Show();
        }

        private void window_Loaded(object sender, RoutedEventArgs e)
        {
            btn_listeBeneficiaires_Click(sender, e);
        }

        private void btnProfil_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new PageProfilUser(lbl_idUser_welcome.Content.ToString());

        }

        private void btnConvention_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new PageConventionUser(lbl_idUser_welcome.Content.ToString());
        }

        private void btn_historique_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new PageHistorique();
            btn_listeBeneficiaires.Background = (Brush)(new BrushConverter().ConvertFrom("#FFEEEEEE"));
            btn_operationsCartes.Background = (Brush)(new BrushConverter().ConvertFrom("#FFEEEEEE"));
            btn_historique.Background = Brushes.Wheat;
            btn_rapports.Background = (Brush)(new BrushConverter().ConvertFrom("#FFEEEEEE"));
            btn_creationCartes.Background = (Brush)(new BrushConverter().ConvertFrom("#FFEEEEEE"));
            btn_tableauDeBord.Background = (Brush)(new BrushConverter().ConvertFrom("#FFEEEEEE"));
        }

        private void btn_tableauDeBord_Click(object sender, RoutedEventArgs e)
        {

            DatePickers datepickers = new DatePickers();
            datepickers.ShowDialog();
            Page_dashboard pd = new Page_dashboard();
            Main.Content = pd;
            pd.showCharts(datepickers.datePicker_de.DisplayDate, datepickers.datePicker_a.DisplayDate);


        }
    }
}
