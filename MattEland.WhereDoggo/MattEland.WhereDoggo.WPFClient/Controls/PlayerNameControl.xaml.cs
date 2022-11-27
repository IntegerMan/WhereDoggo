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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MattEland.WhereDoggo.WPFClient.Controls
{
    /// <summary>
    /// Interaction logic for PlayerNameControl.xaml
    /// </summary>
    public partial class PlayerNameControl : UserControl
    {
        public static readonly DependencyProperty PlayerNameProperty = DependencyProperty.Register("PlayerName", typeof(string), typeof(PlayerNameControl), new PropertyMetadata(default(string)));

        public PlayerNameControl()
        {
            InitializeComponent();
        }
}
}
