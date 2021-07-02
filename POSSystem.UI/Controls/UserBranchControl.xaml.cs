using Autofac;
using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using POSSystem.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace POSSystem.UI.Controls
{
    /// <summary>
    /// Interaction logic for UserBranchControl.xaml
    /// </summary>
    public partial class UserBranchControl : UserControl
    {
        private bool _isBranchChanging = false;
        private UserBranchViewModel model;

        public long BranchValue
        {
            get { return (long)GetValue(BranchValueProperty); }
            set { SetValue(BranchValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BranchId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BranchValueProperty =
            DependencyProperty.Register("BranchValue", typeof(long), typeof(UserBranchControl),
                new FrameworkPropertyMetadata(long.MinValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, BranchPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));


        public UserBranchControl()
        {
            InitializeComponent();
            model = StaticContainer.Container.Resolve<UserBranchViewModel>();
            (this.Content as FrameworkElement).DataContext = model;
            //LoadBranches();
        }

        private void UserBranchControl_Loaded(object sender, RoutedEventArgs e)
        {
            //this.DataContext = model;
        }

        private static void BranchPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UserBranchControl branchComboBox)
            {
                branchComboBox.UpdateBranch();
            }
        }

        private void UpdateBranch()
        {
            if (!_isBranchChanging)
            {
                cmbBranch.SelectedValue = BranchValue;
            }
        }

        private void cmbBranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbBranch.SelectedValue != null)
            {
                _isBranchChanging = true;
                BranchValue = (long)cmbBranch.SelectedValue;
                _isBranchChanging = false;
            }
            else
            {
                BranchValue = 0;
            }
        }

        private void LoadBranches()
        {
            ObservableCollection<BranchWrapper> Branches;
            BranchBO bo = new BranchBO();
            List<Branch> branchlist = bo.GetAll();
            Branches = new ObservableCollection<BranchWrapper>();
            foreach (Branch branch in branchlist)
            {
                BranchWrapper branchWrapper = new BranchWrapper(new Branch())
                {
                    Id = branch.Id,
                    ShopId = branch.ShopId,
                    BranchName = branch.BranchName,
                    BranchAddress = branch.BranchAddress
                };
                Branches.Add(branchWrapper);
            }
            cmbBranch.ItemsSource = Branches;
        }
    }
}
