using System;
using System.Windows.Input;

namespace POSSystem.UI.PDFViewer
{
    public class Commands
	{
		public PDFDelegateCommand OpenCommand { get; private set; }
		public PDFDelegateCommand ExitCommand { get; private set; }

		public PDFDelegateCommand RotateRightCommand { get; private set; }
		public PDFDelegateCommand RotateLeftCommand { get; private set; }

		public PDFDelegateCommand NextPageCommand { get; private set; }
		public PDFDelegateCommand PreviousPageCommand { get; private set; }
		public PDFDelegateCommand FirstPageCommand { get; private set; }
		public PDFDelegateCommand LastPageCommand { get; private set; }
		
		public PDFDelegateCommand SinglePageCommand { get; private set; }
		public PDFDelegateCommand FacingCommand { get; private set; }
		public PDFDelegateCommand BookViewCommand { get; private set; }

		public PDFDelegateCommand TogglePageDisplayCommand { get; private set; }

		public FullscreenCommand FullscreenCommand { get; private set; }

		public PDFDelegateCommand ZoomInCommand { get; private set; }
		public PDFDelegateCommand ZoomOutCommand { get; private set; }
		public PDFDelegateCommand FitWidthCommand { get; private set; }
		public PDFDelegateCommand FitHeightCommand { get; private set; }
		public PDFDelegateCommand CustomZoomCommand { get; private set; }

		public PDFDelegateCommand ShowAboutCommand { get; private set; }
		public PDFDelegateCommand GotoPageCommand { get; private set; }

		public PDFDelegateCommand PrintCommand { get; private set; }

		public Commands(PDFViewerWindow wnd)
		{
			var pdfPanel = wnd.MoonPdfPanel;
			Predicate<object> isPdfLoaded = f => wnd.IsPdfLoaded(); // used for the CanExecute callback

			this.OpenCommand = new PDFDelegateCommand("Open...", f =>
				{
					var dlg = new Microsoft.Win32.OpenFileDialog { Title = "Select PDF file...", DefaultExt = ".pdf", Filter = "PDF file (.pdf)|*.pdf",CheckFileExists = true };

                    if (dlg.ShowDialog() == true)
                    {
                        try
                        {
                            pdfPanel.OpenFile(dlg.FileName);
                        }
                        catch (Exception ex)
                        {
							POSSystem.UI.Service.StaticContainer.NotificationManager.Show(new Notifications.Wpf.NotificationContent
							{
								Title="Error",
								Message = $"An error occured: {ex.Message}",
								Type=Notifications.Wpf.NotificationType.Error
							});
                        }
                    }
				}, f => true, new KeyGesture(Key.O, ModifierKeys.Control));

			this.ExitCommand = new PDFDelegateCommand("Close", f => wnd.Close(), f => true, new KeyGesture(Key.Q, ModifierKeys.Control));

			this.PreviousPageCommand = new PDFDelegateCommand("Previous page", f => pdfPanel.GotoPreviousPage(), isPdfLoaded, new KeyGesture(Key.Left));
			this.NextPageCommand = new PDFDelegateCommand("Next page", f => pdfPanel.GotoNextPage(), isPdfLoaded, new KeyGesture(Key.Right));
			this.FirstPageCommand = new PDFDelegateCommand("First page", f => pdfPanel.GotoFirstPage(), isPdfLoaded, new KeyGesture(Key.Home));
			this.LastPageCommand = new PDFDelegateCommand("Last page", f => pdfPanel.GotoLastPage(), isPdfLoaded, new KeyGesture(Key.End));
			this.GotoPageCommand = new PDFDelegateCommand("Goto page...", f =>
			{
				var dlg = new GotoPageDialog(pdfPanel.GetCurrentPageNumber(), pdfPanel.TotalPages);

				if (dlg.ShowDialog() == true)
					pdfPanel.GotoPage(dlg.SelectedPageNumber.Value);
			}, isPdfLoaded, new KeyGesture(Key.G, ModifierKeys.Control));

			this.RotateRightCommand = new PDFDelegateCommand("Rotate right", f => pdfPanel.RotateRight(), isPdfLoaded, new KeyGesture(Key.Add, ModifierKeys.Control | ModifierKeys.Shift));
			this.RotateLeftCommand = new PDFDelegateCommand("Rotate left", f => pdfPanel.RotateLeft(), isPdfLoaded, new KeyGesture(Key.Subtract, ModifierKeys.Control | ModifierKeys.Shift));

			this.ZoomInCommand = new PDFDelegateCommand("Zoom in", f => pdfPanel.ZoomIn(), isPdfLoaded, new KeyGesture(Key.Add));
			this.ZoomOutCommand = new PDFDelegateCommand("Zoom out", f => pdfPanel.ZoomOut(), isPdfLoaded, new KeyGesture(Key.Subtract));

			this.FitWidthCommand = new PDFDelegateCommand("Fit width", f => pdfPanel.ZoomToWidth(), isPdfLoaded, new KeyGesture(Key.D4, ModifierKeys.Control));
			this.FitHeightCommand = new PDFDelegateCommand("Fit height", f => pdfPanel.ZoomToHeight(), isPdfLoaded, new KeyGesture(Key.D5, ModifierKeys.Control));
			this.CustomZoomCommand = new PDFDelegateCommand("Custom zoom", f => pdfPanel.SetFixedZoom(), isPdfLoaded, new KeyGesture(Key.D6, ModifierKeys.Control));

			this.TogglePageDisplayCommand = new PDFDelegateCommand("Show pages continuously", f => pdfPanel.TogglePageDisplay(), isPdfLoaded, new KeyGesture(Key.D7, ModifierKeys.Control));

			this.FullscreenCommand = new FullscreenCommand("Full screen", wnd, new KeyGesture(Key.L, ModifierKeys.Control));

			this.SinglePageCommand = new PDFDelegateCommand("Single page", f => { pdfPanel.ViewType = MoonPdfLib.ViewType.SinglePage; }, isPdfLoaded, new KeyGesture(Key.D1, ModifierKeys.Control));
			this.FacingCommand = new PDFDelegateCommand("Facing", f => { pdfPanel.ViewType = MoonPdfLib.ViewType.Facing; }, isPdfLoaded, new KeyGesture(Key.D2, ModifierKeys.Control));
			this.BookViewCommand = new PDFDelegateCommand("Book view", f => { pdfPanel.ViewType = MoonPdfLib.ViewType.BookView; }, isPdfLoaded, new KeyGesture(Key.D3, ModifierKeys.Control));

			this.ShowAboutCommand = new PDFDelegateCommand("About", f => new AboutWindow().ShowDialog(), f => true, null);
			this.PrintCommand = new PDFDelegateCommand("Print", f=> pdfPanel.Print(),isPdfLoaded, new KeyGesture(Key.P,ModifierKeys.Control));
			this.RegisterInputBindings(wnd);
		}

		private void RegisterInputBindings(PDFViewerWindow wnd)
		{
			// register inputbindings for all commands (properties)
			foreach (var pi in typeof(Commands).GetProperties())
			{
				var cmd = (BaseCommand)pi.GetValue(this, null);

				if (cmd.InputBinding != null)
					wnd.InputBindings.Add(cmd.InputBinding);
			}
		}

		private void PrintPdf()
		{

		}
	}
}
