using Autofac;
using MahApps.Metro.Controls.Dialogs;
using POS.Data;
using POSSystem.UI.PDFViewer;
using POSSystem.UI.Service;
using POSSystem.UI.ViewModel;
using POSSystem.UI.ViewModel.Service;
using POSSystem.UI.Views;
using POSSystem.UI.Views.Dialog;
using POSSystem.UI.Views.Flyouts;
using Prism.Events;

namespace POSSystem.UI
{
    public class Startup
    {
        public IContainer BootstrapDependencies()
        {
            var builder = new ContainerBuilder();
            //Dialog Registration
            builder.RegisterType<UpdateInventoryDialog>().AsSelf();
            builder.RegisterType<AdminChangePassword>().AsSelf();
            builder.RegisterType<UpdateBillingInfoDialog>().AsSelf().SingleInstance();


            //Flyout Registration
            builder.RegisterType<SettingFlyout>().AsSelf();
            builder.RegisterType<AddCategoryFlyout>().AsSelf();
            builder.RegisterType<AddBrandFlyout>().AsSelf();
            builder.RegisterType<AddBranchFlyout>().AsSelf();

            //View Registration
            builder.RegisterType<CreateUserView>().AsSelf();
            builder.RegisterType<InventoryView>().AsSelf();
            builder.RegisterType<GraphView>().AsSelf();
            builder.RegisterType<SalesView>().AsSelf();
            builder.RegisterType<SalesReturnView>().AsSelf();
            builder.RegisterType<AboutView>().AsSelf();
            builder.RegisterType<UserProfileView>().AsSelf();
            
            

            //Window Registration
            builder.RegisterType<LoginWindow>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<ForgotPasswordWindow>().AsSelf();
            builder.RegisterType<PDFViewerWindow>().AsSelf();



            //Service Registration
            builder.RegisterType<CacheService>().As<ICacheService>();
            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();
            builder.RegisterType<NepDateConverter>().AsImplementedInterfaces();
            builder.RegisterType<NepDate>().AsSelf();
            builder.RegisterType<ColorService>().As<IColorService>();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();


            //View Model Registration
            builder.RegisterType<LoginViewModel>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf();
            builder.RegisterType<GraphViewModel>().AsSelf();
            builder.RegisterType<UserViewModel>().AsSelf();
            builder.RegisterType<ForgetPasswordViewModel>().AsSelf();
            builder.RegisterType<InventoryListViewModel>().AsSelf();
            builder.RegisterType<InventoryViewModel>().AsSelf();
            builder.RegisterType<SalesViewModel>().AsSelf();
            builder.RegisterType<SalesReturnViewModel>().AsSelf();
            builder.RegisterType<SettingsViewModel>().AsSelf();
            builder.RegisterType<UserProfileViewModel>().AsSelf();
            builder.RegisterType<CategoryViewModel>().AsSelf();
            builder.RegisterType<BrandViewModel>().AsSelf();
            builder.RegisterType<InventoryHistoryViewModel>().AsSelf();
            builder.RegisterType<AdminChangeUserPasswordViewModel>().AsSelf();
            builder.RegisterType<SalesListViewModel>().AsSelf();
            builder.RegisterType<BillingViewModel>().AsSelf();
            builder.RegisterType<BranchViewModel>().AsSelf();
            builder.RegisterType<UserBranchViewModel>().AsSelf();



            //Db Context Registration
            builder.RegisterType<POSDataContext>().AsSelf();

            //Framework DI
            builder.RegisterType<DialogCoordinator>().As<IDialogCoordinator>();

            return builder.Build();
        }
    }
}
