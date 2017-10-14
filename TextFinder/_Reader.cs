using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text;
using System.Windows.Documents;
using System.Text.RegularExpressions;

namespace TextFinder {
    static class _Reader {
        public static void ReadDoc(string fileName, RichTextBox rtb, Dictionary<string, bool[]> sent) {
            Microsoft.Office.Interop.Word.Application app = 
                        new Microsoft.Office.Interop.Word.Application();

            Microsoft.Office.Interop.Word.Document document =
                                app.Documents.Open(fileName);

            String read = string.Empty;
            List<string> data = new List<string>();

            var fullText = new StringBuilder();
            for (int i = 0; i < document.Paragraphs.Count; i++)
            {
                fullText.Append(document.Paragraphs[i + 1].Range.Text);
            }

            var res = Regex.Replace(fullText.ToString(), @"[-]\s*(\n|\r|\r\n)\s*", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var res2 = Regex.Replace(res, @",\s*(\n|\r|\r\n)\s*", ", ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            var resultText = Regex.Replace(res2, @"[\s*](\n|\r|\r\n)*[\s*]", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            foreach (var item in MainWindow.Keys)
            {
                foreach (var key in item)
                {
                    if (key == "Conclusion")
                    {

                    }
                    
                    //var regexRemoveRN = new Regex(Regex.Escape("(\n|\r|\r\n)*" + key + "(\n|\r|\r\n)*"));
                    //resultText = regexRemoveRN.Replace(resultText, key, 1);

                    //var regex = new Regex(Regex.Escape(key));
                    //resultText = regex.Replace(resultText, "\n" + key + ": \n", 1);

                    var pattern = "(\n|\r|\r\n)*\\s?" + key + "\\s*(\n|\r|\r\n)*\\s?";
                    var regexRemoveRN = new Regex(@pattern);
                    resultText = regexRemoveRN.Replace(resultText, "\n" + key + ": \n", 1);

                }
            }

            rtb.AppendText(resultText);

            string temp;
            for (int i = 0; i < document.Paragraphs.Count; i++) {
                temp = document.Paragraphs[i + 1].Range.Text.Trim();
                                
                //if (temp != string.Empty)
                //{
                //    rtb.AppendText(temp + '\r'); //.Replace(Environment.NewLine, String.Empty)
                //} 

                try
                {
                    if (rtb.Name == "rtbInput")
                        foreach (Microsoft.Office.Interop.Word.Range r in document.Paragraphs[i + 1].Range.Sentences)
                            if (!sent.ContainsKey(r.Text))
                                sent.Add(r.Text.Replace(Environment.NewLine, String.Empty).Replace("  ", " ").Trim(), new bool[] { r.Bold == -1, r.Italic == -1 });
                }
                catch {
                    sent = new Dictionary<string, bool[]>();
                }

            }

            document.Close(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges);
            app.Quit();
        }

        public static void ReadTxt(string fileName, RichTextBox rtb) {
            StreamReader reader = new StreamReader(fileName,
                                System.Text.Encoding.Default);

            string line;
            while ((line = reader.ReadLine()) != null)
                rtb.AppendText(line.Replace(Environment.NewLine, String.Empty) + "\r");
            
            reader.Close();
        }

        public static void ReadPDF(string fileName, RichTextBox rtb) {
            using (PdfReader reader = new PdfReader(fileName))
            {
                string fullText = String.Empty;
                for (int i = 1; i <= reader.NumberOfPages; i++) {                    
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string text = PdfTextExtractor.GetTextFromPage(reader, i, strategy).Replace(Environment.NewLine, String.Empty);
                    fullText += text;
                    //rtb.AppendText(text);
                }
                var res = Regex.Replace(fullText, @"[-]\s*(\n|\r|\r\n)\s*", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                var res2 = Regex.Replace(res, @",\s*(\n|\r|\r\n)\s*", ", ", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                var resultText = Regex.Replace(res2, @"[\s*](\n|\r|\r\n)*[\s*]", " ", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                foreach (var item in MainWindow.Keys)
                {
                    foreach (var key in item)
                    {
                    //    var regex = new Regex(Regex.Escape(key));
                    //    resultText = regex.Replace(resultText, "\n" + key + ": \n", 1);

                        var pattern = "(\n|\r|\r\n)*\\s?" + key + "\\s*(\n|\r|\r\n)*\\s?";
                        var regexRemoveRN = new Regex(@pattern);
                        resultText = regexRemoveRN.Replace(resultText, "\n" + key + ": \n", 1);
                    }
                }

                rtb.AppendText(resultText);

                TextWithFontExtractionStategy S = new TextWithFontExtractionStategy();
                string rtbText = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd).Text;
                string F = iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, 4, S).Replace(Environment.NewLine, String.Empty);
            }  
            
        }
    
    }

    public class TextWithFontExtractionStategy : iTextSharp.text.pdf.parser.SimpleTextExtractionStrategy
        {
            //HTML buffer
            private StringBuilder result = new StringBuilder();

            //Store last used properties
            private Vector lastBaseLine;
            private string lastFont;
            private float lastFontSize;


            //http://api.itextpdf.com/itext/com/itextpdf/text/pdf/parser/TextRenderInfo.html
            private enum TextRenderMode
            {
                FillText = 0,
                StrokeText = 1,
                FillThenStrokeText = 2,
                Invisible = 3,
                FillTextAndAddToPathForClipping = 4,
                StrokeTextAndAddToPathForClipping = 5,
                FillThenStrokeTextAndAddToPathForClipping = 6,
                AddTextToPaddForClipping = 7
            }



            override public void RenderText(iTextSharp.text.pdf.parser.TextRenderInfo renderInfo)
            {
                string curFont = renderInfo.GetFont().PostscriptFontName;
                //Check if faux bold is used
                if ((renderInfo.GetTextRenderMode() == (int)TextRenderMode.FillThenStrokeText))
                {
                    curFont += "-Bold";
                }

                //This code assumes that if the baseline changes then we're on a newline
                Vector curBaseline = renderInfo.GetBaseline().GetStartPoint();
                Vector topRight = renderInfo.GetAscentLine().GetEndPoint();
                iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(curBaseline[Vector.I1], curBaseline[Vector.I2], topRight[Vector.I1], topRight[Vector.I2]);
                Single curFontSize = rect.Height;

                //See if something has changed, either the baseline, the font or the font size
                if ((this.lastBaseLine == null) || (curBaseline[Vector.I2] != lastBaseLine[Vector.I2]) || (curFontSize != lastFontSize) || (curFont != lastFont))
                {
                    //if we've put down at least one span tag close it
                    if ((this.lastBaseLine != null))
                    {
                        this.result.AppendLine("</span>");
                    }
                    //If the baseline has changed then insert a line break
                    if ((this.lastBaseLine != null) && curBaseline[Vector.I2] != lastBaseLine[Vector.I2])
                    {
                        this.result.AppendLine("<br />");
                    }
                    //Create an HTML tag with appropriate styles
                    this.result.AppendFormat("<span style=\"font-family:{0};font-size:{1}\">", curFont, curFontSize);
                }

                //Append the current text
                this.result.Append(renderInfo.GetText());

                //Set currently used properties
                this.lastBaseLine = curBaseline;
                this.lastFontSize = curFontSize;
                this.lastFont = curFont;
            }

            override public string GetResultantText()
            {
                //If we wrote anything then we'll always have a missing closing tag so close it here
                if (result.Length > 0)
                {
                    result.Append("</span>");
                }
                return result.ToString();
            }

            //Not needed
            public void BeginTextBlock() { }
            public void EndTextBlock() { }
            public void RenderImage(ImageRenderInfo renderInfo) { }
        }
}
