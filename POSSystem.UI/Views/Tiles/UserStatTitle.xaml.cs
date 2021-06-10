using MahApps.Metro.IconPacks;
using System.Windows;
using System.Windows.Controls;

namespace POSSystem.UI.Views.Tiles
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
