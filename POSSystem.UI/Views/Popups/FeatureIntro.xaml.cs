using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace POSSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for FeatureIntro.xaml
    /// </summary>
    public partial class FeatureIntro : UserControl
    {


        public string IntroText
        {
            get { return (string)GetValue(IntroTextProperty); }
            set { SetValue(IntroTextProperty, value); }
        }

        public bool IsPopupOpen
        {
            get { return (bool)GetValue(IsPopupOpenProperty); }
            set { SetValue(IsPopupOpenProperty, value); }
        }


        // Using a DependencyProperty as the backing store for IntroText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IntroTextProperty =
            DependencyProperty.Register("IntroText", typeof(string), typeof(FeatureIntro),
        new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, IntroTextPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));




       

        // Using a DependencyProperty as the backing store for IsPopupOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPopupOpenProperty =
            DependencyProperty.Register("IsPopupOpen", typeof(bool), typeof(FeatureIntro),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, null, false, UpdateSourceTrigger.PropertyChanged));




        public FeatureIntro()
        {
            InitializeComponent();
        }

        private static void IntroTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FeatureIntro lbl)
            {
                lbl.UpdateText();
            }
        }

        private void UpdateText()
        {
            lblFeatureInfo.Text = IntroText;
        }

        private void btnClosePopup_Click(object sender, RoutedEventArgs e)
        {
            IsPopupOpen = false;
            Application.Current.Properties["FeatureShown"] = false;
        }
    }
}
