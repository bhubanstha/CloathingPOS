﻿using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Service;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class InventoryViewModel
    {
        private DateTime _purchaseDate = DateTime.Now;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public int CategoryId { get; set; }

        public decimal PurchaseRate { get; set; }

        public decimal RetailRate { get; set; }

        public int Quantity { get; set; }
        public DateTime FirstPurchaseDate { 
            get { return _purchaseDate; }
            set { _purchaseDate = value; }
        }

        public CultureInfo CultureInfo { get; set; }

        public virtual List<Category> Categories { get; set; }

        private InventoryBO InventoryBO;
        private CategoryBO CategoryBO;
        private MetroWindow _window;

        public ICommand SaveCommand { get;  }
        public ICommand AddCategoryCommand { get; }

        public InventoryViewModel()
        {
            _window = Application.Current.MainWindow as MetroWindow;
            
            CategoryBO = new CategoryBO();
           

            Categories = CategoryBO.GetCategories();
            SaveCommand = new DelegateCommand(SaveProduct);
            AddCategoryCommand = new DelegateCommand(OpenAddCategoryFlyout);
            SetCurrencyCulture();
        }

        private void OpenAddCategoryFlyout()
        {
            Flyout f = StaticContainer.AddCategoryFlyout;
            f.IsOpen = !f.IsOpen;
        }

        private async void SaveProduct()
        {
            Inventory inventory = new Inventory
            {
                Id = this.Id,
                CategoryId =  this.CategoryId,
                Color = this.Color,
                FirstPurchaseDate = this.FirstPurchaseDate,
                Name = this.Name,
                PurchaseRate = this.PurchaseRate,
                Quantity = this.Quantity,
                RetailRate = this.RetailRate,
                Size = this.Size
            };
            InventoryBO = new InventoryBO();
            int c = await InventoryBO.Save(inventory);
            if(c>0)
            {
               await _window.ShowMessageAsync("Product Added", $"Product: {this.Name} - {this.Size} purchased on {this.FirstPurchaseDate} added into inventory.", MessageDialogStyle.Affirmative, StaticContainer.DialogSettings);
                this.Size = "";
                this.Id = 0;
            }
        }
        private void SetCurrencyCulture()
        {
            //string culture = ConfigurationReader.GetConfiguration<string>(AppSettingKey.CurrencyCulture);
            CultureInfo = StaticContainer.CultureInfo;
        }
    }
}