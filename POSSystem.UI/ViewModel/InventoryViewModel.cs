using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Notifications.Wpf;
using POS.BusinessRule;
using POS.Model;
using POS.Utilities.Extension;
using POSSystem.UI.Service;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class InventoryViewModel : NotifyPropertyChanged
    {
        private DateTime _purchaseDate = DateTime.Now;

        private string _colorName="";
        private bool _ColorNameEntryEnabled = false;
        private string _color;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Color { 
            get { return _color; }
            set
            {
                _color = value;
                GetKnownColorName();
                OnPropertyChanged();
            }
        }

        private void GetKnownColorName()
        {
            string colorName = Color.ToKnownColourName();
            if(string.IsNullOrEmpty(colorName))
            {
                ColorNameEntryEnabled = true;
                ColorName = "";
            }
            else
            {
                ColorNameEntryEnabled = false;
                ColorName = colorName;
            }
        }

        public Int64 CategoryId { get; set; }

        public decimal PurchaseRate { get; set; }

        public decimal RetailRate { get; set; }

        public int Quantity { get; set; }
        public DateTime FirstPurchaseDate { 
            get { return _purchaseDate; }
            set { _purchaseDate = value; }
        }

       

        public string ColorName
        {
            get { return _colorName; }
            set {
                _colorName = value;
                OnPropertyChanged("ColorName");

                //_colorName = _color.ToKnownColourName(); 
                
                //if(string.IsNullOrEmpty(_colorName))
                //{
                //    ColorNameEntryEnabled = true;
                //    _colorName = value;
                //}
                //else
                //{
                //    ColorNameEntryEnabled = false;
                //}
            }
        }


        

        public bool ColorNameEntryEnabled
        {
            get { return _ColorNameEntryEnabled; }
            set { _ColorNameEntryEnabled = value; OnPropertyChanged(); }
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
            try
            {
                Inventory inventory = new Inventory
                {
                    Id = this.Id,
                    CategoryId = this.CategoryId,
                    Color = this.Color,
                    FirstPurchaseDate = this.FirstPurchaseDate,
                    Name = this.Name,
                    PurchaseRate = this.PurchaseRate,
                    Quantity = this.Quantity,
                    RetailRate = this.RetailRate,
                    Size = this.Size,
                    ColorName = this.ColorName
                };
                InventoryBO = new InventoryBO();
                int c = await InventoryBO.Save(inventory);
                if (c > 0)
                {
                    StaticContainer.NotificationManager.Show(new NotificationContent
                    {
                        Message = $"Product: {this.Name} - {this.Size} purchased on {this.FirstPurchaseDate.ToString("yyyy/MM/dd")} added into inventory.",
                        Title = "Product Added",
                        Type = NotificationType.Success
                    });
                    this.Size = "";
                    this.Id = 0;
                }
            }
            catch (Exception ex)
            {
                StaticContainer.NotificationManager.Show(new NotificationContent
                {
                    Message = "Could not create inventory. Please provide all the required item information.",
                    Title = "Error",
                    Type = NotificationType.Error
                });
            }
        }
        private void SetCurrencyCulture()
        {
            //string culture = ConfigurationReader.GetConfiguration<string>(AppSettingKey.CurrencyCulture);
            CultureInfo = StaticContainer.CultureInfo;
        }


        private string GetColorName()
        {
            //string nameOfTheColor = MahApps.Metro.Controls.ColorHelper.DefaultInstance.GetColorName(myColor, theDictionaryToUse);

            return "";
        }
    }
}
