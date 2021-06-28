using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace POSSystem.WPF.UI.PDFViewer
{
    public partial class AboutWindow : Window
	{
		public AboutWindow()
		{
			InitializeComponent();

			//this.Icon = MoonPdf.Resources.moon.ToBitmapSource();
			var company = (AssemblyCompanyAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), true).First();
			var product = (AssemblyProductAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), true).First();
			var version = (AssemblyFileVersionAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true).First();
			var copyright = (AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true).First();

			this.Title = string.Format("About {0}", product.Product);
			this.lblInfo.Content = string.Format("{0}, Version {1}", product.Product, version.Version);
			this.lblCopyright.Content = string.Format("{0} - {1}", copyright.Copyright, company.Company);
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (e.Key == Key.Escape)
				this.Close();
		}
	}
}
