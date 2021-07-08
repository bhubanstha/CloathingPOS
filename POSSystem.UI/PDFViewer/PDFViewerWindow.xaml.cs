using Autofac;
using log4net;
using MahApps.Metro.Controls;
using MoonPdfLib;
using MoonPdfLib.MuPdf;
using POS.Model;
using POS.Utilities.PDF;
using POSSystem.UI.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace POSSystem.UI.PDFViewer
{
    public partial class PDFViewerWindow : MetroWindow
	{
		private ILog log;
		private MainWindowDataContext dataContext;
		private string pdfFilePath;
		private string pdfPassword;
		private List<Inventory> selectedItems;
		int previousNoofLeaveLabel = 0;
		internal MoonPdfPanel MoonPdfPanel { get { return this.moonPdfPanel; } }

		public PDFViewerWindow()
		{
			InitializeComponent();
			borderLabelModifier.Visibility = Visibility.Collapsed;
			this.log = StaticContainer.Container.Resolve<ILogger>().GetLogger(typeof(PDFViewerWindow));
			this.dataContext = new MainWindowDataContext(this, log);
			//this.Icon = MoonPdf.Resources.moon.ToBitmapSource();
			this.DataContext = dataContext;
			//this.UpdateTitle();

			moonPdfPanel.ViewTypeChanged += moonPdfPanel_ViewTypeChanged;
			moonPdfPanel.ZoomTypeChanged += moonPdfPanel_ZoomTypeChanged;
			moonPdfPanel.PageRowDisplayChanged += moonPdfPanel_PageDisplayChanged;
            moonPdfPanel.PasswordRequired += moonPdfPanel_PasswordRequired;

			this.UpdatePageDisplayMenuItem();
			this.UpdateZoomMenuItems();
			this.UpdateViewTypeItems();

			this.Loaded += MainWindow_Loaded;
		}

		public PDFViewerWindow(string pdfFilePath) : this()
		{
			//InitializeComponent();
			this.pdfFilePath = pdfFilePath;
		}

		public PDFViewerWindow(string pdfFilePath, string password) : this()
		{
			//InitializeComponent();
			this.pdfFilePath = pdfFilePath;
			this.pdfPassword = password;
		}

		public PDFViewerWindow(string pdfFilePath, string password, List<Inventory> items) : this()
		{
			//InitializeComponent();
			this.pdfFilePath = pdfFilePath;
			this.pdfPassword = password;
			this.selectedItems = items;
			this.previousNoofLeaveLabel = 0;
			borderLabelModifier.Visibility = Visibility.Visible;
		}




		void moonPdfPanel_PasswordRequired(object sender, PasswordRequiredEventArgs e)
        {
            var dlg = new PdfPasswordDialog();

            if (dlg.ShowDialog() == true)
                e.Password = dlg.Password;
            else
                e.Cancel = true;
        }


		private void OpenPdf(string pdfPath)
		{
			//string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image", "companyLogo1.png");
			//CreatePDF createPDF = new CreatePDF();
			//string pdfPath = createPDF.CreatePdfTable(1, StaticContainer.PdfPassword, imagePath);
			////byte[] pdfbyte = createPDF.CreatePdfTableInMemory();
			MoonPdfPanel.OpenFile(pdfPath);
		}

		private void OpenPdf(string pdfPath, string password)
		{
			MoonPdfPanel.OpenFile(pdfPath, password);
		}
		void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			//string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image", "companyLogo1.png");
			//CreatePDF createPDF = new CreatePDF();
			//string pdfPath = createPDF.CreatePdfTable(1, StaticContainer.PdfPassword, imagePath);
			////byte[] pdfbyte = createPDF.CreatePdfTableInMemory();
			//MoonPdfPanel.OpenFile(pdfPath);

			if (!string.IsNullOrEmpty(this.pdfFilePath) && !string.IsNullOrEmpty(this.pdfPassword))
			{
				OpenPdf(this.pdfFilePath, this.pdfPassword);
			}
			else if (!string.IsNullOrEmpty(this.pdfFilePath))
			{
				OpenPdf(this.pdfFilePath);
			}
			
			//var args = Environment.GetCommandLineArgs();

			//// if a filename was given via command line
   //         if (args.Length > 1 && File.Exists(args[1]))
   //         {
   //             try
   //             {
   //                 this.moonPdfPanel.OpenFile(args[1]);
   //             }
   //             catch (Exception ex)
   //             {
   //                 MessageBox.Show(string.Format("An error occured: " + ex.Message));
   //             }
   //         }
		}

		void moonPdfPanel_PageDisplayChanged(object sender, EventArgs e)
		{
			this.UpdatePageDisplayMenuItem();
		}

		private void UpdatePageDisplayMenuItem()
		{
			this.itmContinuously.IsChecked = (this.moonPdfPanel.PageRowDisplay == MoonPdfLib.PageRowDisplayType.ContinuousPageRows);
		}

		void moonPdfPanel_ZoomTypeChanged(object sender, EventArgs e)
		{
			this.UpdateZoomMenuItems();
		}

		private void UpdateZoomMenuItems()
		{
			this.itmFitHeight.IsChecked = moonPdfPanel.ZoomType == ZoomType.FitToHeight;
			this.itmFitWidth.IsChecked = moonPdfPanel.ZoomType == ZoomType.FitToWidth;
			this.itmCustomZoom.IsChecked = moonPdfPanel.ZoomType == ZoomType.Fixed;
		}

		void moonPdfPanel_ViewTypeChanged(object sender, EventArgs e)
		{
			this.UpdateViewTypeItems();
		}

		private void UpdateViewTypeItems()
		{
			switch (this.moonPdfPanel.ViewType)
			{
				case MoonPdfLib.ViewType.SinglePage:
					this.viewSingle.IsChecked = true;
					break;
				case MoonPdfLib.ViewType.Facing:
					this.viewFacing.IsChecked = true;
					break;
				case MoonPdfLib.ViewType.BookView:
					this.viewBook.IsChecked = true;
					break;
				default:
					break;
			}
		}

		//private void UpdateTitle()
		//{
		//	if( appName == null )
		//		appName = ((AssemblyProductAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), true).First()).Product;

  //          if (IsPdfLoaded())
  //          {
  //              var fs = moonPdfPanel.CurrentSource as FileSource;

  //              if( fs != null )
  //              {
  //                  this.Title = string.Format("{0} - {1}", System.IO.Path.GetFileName(fs.Filename), appName);
  //                  return;
  //              }
  //          }
            
		//	this.Title = appName;
		//}

		internal bool IsPdfLoaded()
		{
            return moonPdfPanel.CurrentSource != null;
		}

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			base.OnPreviewKeyDown(e);
			
			if (e.SystemKey == Key.LeftAlt)
			{
				this.mainMenu.Visibility = (this.mainMenu.Visibility == System.Windows.Visibility.Collapsed ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed);

				if (this.mainMenu.Visibility == System.Windows.Visibility.Collapsed)
					e.Handled = true;
			}
		}

		internal void OnFullscreenChanged(bool isFullscreen)
		{
			this.itmFullscreen.IsChecked = isFullscreen;
		}

        private async void btnLeaveLabel_Click(object sender, RoutedEventArgs e)
        {
			int count = (int)txtNoOfLabel.Value;

			if (selectedItems !=null && selectedItems.Count>0 && previousNoofLeaveLabel != count)
            {
				if(MoonPdfPanel.IsLoaded)
                {
					MoonPdfPanel.Unload();
				}
				previousNoofLeaveLabel = count;
				CreateQRCode createQRCode = new CreateQRCode(pdfPassword);
				pdfFilePath = await createQRCode .CreateLabel(selectedItems, count);
				OpenPdf(pdfFilePath, pdfPassword);
			}
        }
    }
}
