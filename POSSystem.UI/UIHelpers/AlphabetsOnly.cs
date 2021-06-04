using System.Windows.Forms;

namespace POSSystem.UI.UIHelpers
{
    public class AlphabetsOnly
    {
        public static void HandleaplhabetsOnly(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // If you want to allow decimal numeric value in you textBox then add this too 
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}
