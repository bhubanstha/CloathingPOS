using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace POSSystem.UI
{
    /// <summary>
    /// Interaction logic for EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : Window
    {
        public EditorWindow()
        {
            InitializeComponent();
            List<ParagraphStyle> paragraphStyles = new List<ParagraphStyle>
            {
                new ParagraphStyle{Text = "Heading1", FontSize=40},
                new ParagraphStyle{Text = "Heading2", FontSize=34},
                new ParagraphStyle{Text = "Heading3", FontSize=28},
                new ParagraphStyle{Text = "Heading4", FontSize=24},
                new ParagraphStyle{Text = "Heading5", FontSize=18},
                new ParagraphStyle{Text = "Heading6", FontSize=14},
                new ParagraphStyle{Text = "Paragraph", FontSize=12}

            };

            cmbParagraphs.ItemsSource = paragraphStyles;
            //rtbTest.PreviewKeyDown += RtbTest_PreviewKeyDown;
        }

        private void RtbTest_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && Keyboard.IsKeyDown(Key.LeftCtrl))


            {
                // do whatever you want
                // if u want to add a new line just uncomment the next lines
                // rtb.AppendText(Environment.NewLine);
                // rtb.CaretPosition = rtb.CaretPosition.DocumentEnd;
                e.Handled = true;
            }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            EditingCommands.ToggleBold.Execute(null, rtbTest);
            rtbTest.Focus();
        }

        private void CommandBinding_Executed_1(object sender, ExecutedRoutedEventArgs e)
        {
            EditingCommands.ToggleBullets.Execute(null, rtbTest);
            rtbTest.Focus();
        }

        private void CommandBinding_Executed_2(object sender, ExecutedRoutedEventArgs e)
        {
            EditingCommands.ToggleNumbering.Execute(null, rtbTest);
            rtbTest.Focus();
        }

        private void CommandBinding_Executed_3(object sender, ExecutedRoutedEventArgs e)
        {
            EditingCommands.ToggleUnderline.Execute(null, rtbTest);
            rtbTest.Focus();
        }

        private void CommandBinding_Executed_4(object sender, ExecutedRoutedEventArgs e)
        {
            EditingCommands.ToggleItalic.Execute(null, rtbTest);

            rtbTest.Focus();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (rtbTest != null)
            {
                ParagraphStyle style = (ParagraphStyle)e.AddedItems[0];
                if (!rtbTest.Selection.IsEmpty)
                {
                    rtbTest.Selection.ApplyPropertyValue(Inline.FontSizeProperty, style.FontSize.ToString());
                    if (style.Text != "Paragraph")
                    {
                        rtbTest.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
                    }
                    else
                    {
                        rtbTest.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
                    }
                }
                else
                {
                    rtbTest.FontSize = (double)style.FontSize;
                    if (style.Text != "Paragraph")
                    {
                        rtbTest.FontWeight = FontWeights.Bold;
                    }
                    else
                    {
                        rtbTest.FontWeight = FontWeights.Normal;
                    }
                }
                
               
            }
        }
    }

    public class ParagraphStyle
    {
        public string Text { get; set; }
        public int FontSize { get; set; }



        public ParagraphStyle()
        {

        }
    }
}
