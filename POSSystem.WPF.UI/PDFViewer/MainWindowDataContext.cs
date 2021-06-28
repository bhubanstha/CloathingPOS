namespace POSSystem.WPF.UI.PDFViewer
{
    public class MainWindowDataContext
	{
		public Commands Commands { get; private set; }

		public MainWindowDataContext(PDFViewerWindow wnd)
		{
			this.Commands = new Commands(wnd);
		}
	}
}
