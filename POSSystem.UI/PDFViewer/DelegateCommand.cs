using System;
using System.Windows.Input;

namespace POSSystem.UI.PDFViewer
{
    public class PDFDelegateCommand : BaseCommand
	{
		private Action<object> executeAction;
		private Predicate<object> canExecuteFunc;
				
		public PDFDelegateCommand(string name, Action<object> executeAction, Predicate<object> canExecuteFunc, InputGesture inputGesture)
			: base(name, inputGesture)
		{
			this.executeAction = executeAction;
			this.canExecuteFunc = canExecuteFunc;
		}

		public override bool CanExecute(object parameter)
		{
			return this.canExecuteFunc != null ? this.canExecuteFunc(parameter) : true;
		}

		public override void Execute(object parameter)
		{
			this.executeAction(parameter);
		}
	}
}
