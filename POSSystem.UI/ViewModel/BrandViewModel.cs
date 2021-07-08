using log4net;
using POS.BusinessRule;
using POS.Model;
using POSSystem.UI.Event;
using POSSystem.UI.Service;
using POSSystem.UI.UIModel;
using POSSystem.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POSSystem.UI.ViewModel
{
    public class BrandViewModel : ViewModelBase
    {
        private IEventAggregator _eventAggregator;
        private ILog _log;
        private BrandBO brandBO;
        private ObservableCollection<BrandWrapper> brands;

        public BrandWrapper NewBrand { get; set; }
        public ObservableCollection<BrandWrapper> Brands
        {
            get { return brands; }
            set
            {
                brands = value;
                OnPropertyChanged();
            }
        }


        
        public ICommand CreateBrandCommand { get; }
        public ICommand DeleteBrandCommand { get;  }

        public ICommand EditBrandCommand { get; }
        public ICommand ResetCommand { get; }
        public BrandViewModel(IEventAggregator eventAggregator, ILogger logger)
        {
            _eventAggregator = eventAggregator;
            _log = logger.GetLogger(typeof(BrandViewModel));

            NewBrand = new BrandWrapper(new Brand());
           
            CreateBrandCommand = new DelegateCommand(OnSaveBrand);
            DeleteBrandCommand = new DelegateCommand<BrandWrapper>(OnDeleteBrand);
            EditBrandCommand = new DelegateCommand<BrandWrapper>(OnEditBrand);
            ResetCommand = new DelegateCommand(ResetEdit);

            LoadCategories();
        }


        private void OnEditBrand(BrandWrapper obj)
        {
            if(obj != null)
            {
                this.NewBrand.Id = obj.Id;
                this.NewBrand.Name = obj.Name;
            }
        }

        private async void OnDeleteBrand(BrandWrapper obj)
        {
            try
            {
                if (obj != null)
                {
                    brandBO = new BrandBO();
                    int i =  await brandBO.Delete(obj.Id);
                    if (i > 0)
                    {
                        RemoveCategoryFromList(obj.Model);
                        BrandChangedEventArgs brandChangedEventArgs = new BrandChangedEventArgs
                        {
                            Brand = obj.Model,
                            Action = EventAction.Remove
                        };
                        _eventAggregator.GetEvent<BrandChangedEvent>().Publish(brandChangedEventArgs);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("OnDeleteBrand", ex);
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, Notifications.Wpf.NotificationType.Error);
            }
        }

        private async void OnSaveBrand()
        {
            try
            {
                brandBO = new BrandBO();
                Brand brand = new Brand
                {
                    Id = NewBrand.Id,
                    Name = NewBrand.Name
                };

                BrandChangedEventArgs brandChangedEventArgs = await ManageCategory(brand, brandBO);  
                
                string msg = brandChangedEventArgs.Action == EventAction.Add? 
                    "New Brand Added Successfully." 
                    : "Brand Changed Successfully.";

                _eventAggregator.GetEvent<BrandChangedEvent>().Publish(brandChangedEventArgs);
                StaticContainer.ShowNotification("Brand", msg, Notifications.Wpf.NotificationType.Success);
            }
            catch (Exception ex)
            {
                _log.Error("OnSaveBrand", ex);
                StaticContainer.ShowNotification("Error", StaticContainer.ErrorMessage, Notifications.Wpf.NotificationType.Error);
            }
            finally
            {
                ResetEdit();
            }
        }

        private async Task<BrandChangedEventArgs> ManageCategory(Brand brand, BrandBO bO)
        {
            BrandChangedEventArgs categoryChangedEventArgs = new BrandChangedEventArgs
            {
                Brand = brand
            };
            if (brand.Id>0)
            {
                await bO.Update(brand);
                categoryChangedEventArgs.Action = EventAction.Update;
            }
            else
            {
                await bO.Save(brand);
                categoryChangedEventArgs.Action = EventAction.Add;
            }
            ManageCategoryInList(categoryChangedEventArgs);
            return categoryChangedEventArgs;
        }

        private void LoadCategories()
        {
            brandBO = new BrandBO();
            var brandList = brandBO.GetBrands();
            Brands = new ObservableCollection<BrandWrapper>();
            foreach (Brand brand in brandList)
            {
                BrandWrapper categoryWrapper = new BrandWrapper(brand);
                Brands.Add(categoryWrapper);
            }
        }

        private void ManageCategoryInList(BrandChangedEventArgs args)
        {
            if(args.Action == EventAction.Add)
            {
                AddNewCategoryToList(args.Brand);
            }
            else if(args.Action == EventAction.Update)
            {
                UpdateCategoryInList(args.Brand);
            }
            else
            {
                RemoveCategoryFromList(args.Brand);
            }
        }

        private void AddNewCategoryToList(Brand obj)
        {
            BrandWrapper categoryWrapper = new BrandWrapper(obj);
            Brands.Add(categoryWrapper);
        }

        private void UpdateCategoryInList(Brand obj)
        {
            var item = Brands.Where(x => x.Id == obj.Id).FirstOrDefault();
            item.Name = obj.Name;
        }

        private void RemoveCategoryFromList(Brand obj)
        {
            var item = Brands.Where(x => x.Id == obj.Id).FirstOrDefault();
            Brands.Remove(item);
        }
        


        private void ResetEdit()
        {
            this.NewBrand.Name = "";
            this.NewBrand.Id = 0;
        }

    }
}
