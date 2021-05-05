﻿using POSSystem.UI.ViewModel;
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

namespace POSSystem.UI.Views
{
    /// <summary>
    /// Interaction logic for InventoryList.xaml
    /// </summary>
    public partial class InventoryListView : UserControl
    {
        private InventoryListViewModel model;
        public InventoryListView()
        {
            InitializeComponent();
            model = new InventoryListViewModel();
            this.DataContext = model;
        }
    }
}
