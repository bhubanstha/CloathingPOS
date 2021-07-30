using MahApps.Metro.IconPacks;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace POSSystem.UI.Views.Tiles
{
    /// <summary>
    /// Interaction logic for UserStatTitle.xaml
    /// </summary>
    public partial class UserStatTitle : UserControl
    {
        //public int StatCount { get; set; }
        public string StatName { get; set; }
        public PackIconBootstrapIconsKind Icon { get; set; }





        public int StatCount
        {
            get { return (int)GetValue(StatCountProperty); }
            set { SetValue(StatCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StatCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatCountProperty =
            DependencyProperty.Register("StatCount", typeof(int), typeof(UserStatTitle),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, StatCountPropertyChange, null, false, UpdateSourceTrigger.PropertyChanged));




        public UserStatTitle()
        {
            InitializeComponent();
            this.Loaded += UserStatTitle_Loaded;
        }

        private void UserStatTitle_Loaded(object sender, RoutedEventArgs e)
        {
            txtStatName.Text = StatName;
            statIcon.Kind = Icon;
        }

        private static void StatCountPropertyChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UserStatTitle st)
            {
                st.UpdateCount();
            }
        }

        private void UpdateCount()
        {
            txtTotalCount.Text = StatCount.ToString();
        }
    }
}
