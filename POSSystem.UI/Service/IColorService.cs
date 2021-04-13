using System.Drawing;

namespace POSSystem.UI.Service
{
    public interface IColorService
    {
        Color GetColor(string colorName);
        string GetColorHex(Color color);
    }
}