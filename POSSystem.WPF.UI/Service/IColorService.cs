using System.Drawing;

namespace POSSystem.WPF.UI.Service
{
    public interface IColorService
    {
        Color GetColor(string colorName);
        string GetColorHex(Color color);
    }
}