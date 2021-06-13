using POS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.UIModel
{
    public enum EventAction
    {
        Add,
        Remove,
        Update
    }
    public class CategoryChangedEventArgs
    {
        public Category Category { get; set; }
        public EventAction Action { get; set; }
    }
}
