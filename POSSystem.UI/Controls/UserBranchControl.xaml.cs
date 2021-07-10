using Autofac;
using log4net;
using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Event;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using POSSystem.UI.Wrapper;
using Prism.Events;
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
        private ILog _log;

        public long BranchValue
        {
            get { return (long)GetValue(BranchValueProperty); }
            set { SetValue(BranchValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BranchId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BranchValueProperty =
            DependencyProperty.Register("BranchValue", typeof(long), typeof(UserBranchControl),
                new FrameworkPropertyMetadata(long.MinValue, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, BranchPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        public string StyleString
        {
            get { return (string)GetValue(StyleStringProperty); }
            set { SetValue(StyleStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BranchId.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StyleStringProperty =
            DependencyProperty.Register("StyleString", typeof(string), typeof(UserBranchControl),
                new FrameworkPropertyMetadata("ComboBox.Branch", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, StylePropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));



        public UserBranchControl()
        {
            InitializeComponent();
            _log = StaticContainer.Container.Resolve<ILogger>().GetLogger(typeof(UserBranchControl));
            model = StaticContainer.Container.Resolve<UserBranchViewModel>();
            (this.Content as FrameworkElement).DataContext = model;
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
                cmbBranch.SelectedValue = BranchValue;
        }


        private static void StylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UserBranchControl branchComboBox)
            {
                branchComboBox.UpdateStyle();
            }
        }

        private void UpdateStyle()
        {
            Style s = this.FindResource(StyleString) as Style;
            cmbBranch.Style = s;
        }

        private void cmbBranch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StaticContainer.Shop != null)
            {
                BranchWrapper b = (BranchWrapper)e.AddedItems[0];
                StaticContainer.Shop.Address = b.BranchAddress;

                if (model._loggedInUser != null)
                {
                    string previousBranch = "";
                    if (e.RemovedItems.Count > 0)
                    {
                        previousBranch = $" from {((BranchWrapper)e.RemovedItems[0]).BranchName}";
                    }
                    _log.Info($"{model._loggedInUser.UserName} switched to {b.BranchName} branch {previousBranch}");
                }
                //Publish branch change event
                IEventAggregator eventAggregator = StaticContainer.Container.Resolve<IEventAggregator>();
                eventAggregator.GetEvent<BranchSwitchedEvent>().Publish(b);

            }
        }
    }
}
