using log4net;

namespace POSSystem.UI.PDFViewer
{
    public class MainWindowDataContext
	{
		public Commands Commands { get; private set; }

		public MainWindowDataContext(PDFViewerWindow wnd, ILog logger)
		{
			this.Commands = new Commands(wnd, logger);
		}
	}
}
