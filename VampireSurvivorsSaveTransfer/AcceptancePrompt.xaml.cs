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

namespace VampireSurvivorsSaveTransfer
{
    /// <summary>
    /// Interaction logic for AcceptancePrompt.xaml
    /// </summary>
    public partial class AcceptancePrompt : Window
    {
        public AcceptancePrompt()
        {
            InitializeComponent();
        }

        private void ButtonAgree_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void ButtonDisagree_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
