using MahApps.Metro.Controls;
using System.Windows;

namespace POSSystem.WPF.UI.PDFViewer
{
    public partial class GotoPageDialog : MetroWindow
	{
		private int MaxPageNumber { get; set; }
		public int? SelectedPageNumber { get; private set; }

		public GotoPageDialog(int currentPageNumber, int maxPageNumber)
		{
			InitializeComponent();

			//this.Icon = MoonPdf.Resources.moon.ToBitmapSource();
			this.MaxPageNumber = maxPageNumber;
			this.txtPage.Value = currentPageNumber;
			this.lblMaxPageNumber.Text = maxPageNumber.ToString();
			this.Loaded += GotoPageDialog_Loaded;
		}

		void GotoPageDialog_Loaded(object sender, RoutedEventArgs e)
		{
			this.txtPage.Focus();
			this.txtPage.SelectAll();
		}

		private void BtnGotoPage_Click(object sender, RoutedEventArgs e)
		{
			int page;

			if (!int.TryParse(this.txtPage.Value.ToString(), out page) || page > MaxPageNumber || page < 1)
			{
				MessageBox.Show("Please enter a valid page number.");
				return;
			}

			this.SelectedPageNumber = page;
			this.DialogResult = true;
			this.Close();
		}

		private void BtnCancel_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
