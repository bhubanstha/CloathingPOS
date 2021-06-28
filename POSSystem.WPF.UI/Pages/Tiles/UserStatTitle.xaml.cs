using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POSSystem.WPF.UI.Pages.Tiles
{
    /// <summary>
    /// Interaction logic for UserStatTitle.xaml
    /// </summary>
    public partial class UserStatTitle : UserControl
    {
        public int StatCount { get; set; }
        public string StatName { get; set; }
        public PackIconBootstrapIconsKind Icon { get; set; }

        public UserStatTitle()
        {
            InitializeComponent();
            this.Loaded += UserStatTitle_Loaded;
        }

        private void UserStatTitle_Loaded(object sender, RoutedEventArgs e)
        {
            txtTotalCount.Text = StatCount.ToString();
            txtStatName.Text = StatName;
            statIcon.Kind = Icon;
        }
    }
}
