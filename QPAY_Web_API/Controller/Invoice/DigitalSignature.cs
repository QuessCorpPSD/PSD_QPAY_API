using Microsoft.AspNetCore.Mvc;
using SelectPdf;
using System;
using System.Drawing;
using System.IO;
namespace QPay.API.Controller.Invoice
{
    public class DigitalSignature
    {

        public static byte[] AddSingleDiagonalWatermark(byte[] pdfBytes, string watermarkText, int fontSize = 35, int transparency = 35)
        {
            if (pdfBytes == null) throw new ArgumentNullException(nameof(pdfBytes));
            if (string.IsNullOrWhiteSpace(watermarkText)) throw new ArgumentNullException(nameof(watermarkText));
            PdfDocument doc = new PdfDocument(new MemoryStream(pdfBytes));
            try
            {
                foreach (PdfPage page in doc.Pages)
                {
                    PdfCanvas canvas = page;
                    var rect = page.ClientRectangle;
                    float pageW = page.ClientRectangle.Width;
                    float pageH = page.ClientRectangle.Height;
                    PdfFont font = doc.AddFont(PdfStandardFont.Helvetica);
                    font.Size = fontSize;
                    SizeF size = canvas.MeasureString(watermarkText, font);
                    PdfTextElement txt = new PdfTextElement(0, 0, watermarkText, font)
                    {
                        ForeColor = new PdfColor(180, 180, 180),
                        Transparency = transparency
                    };
                    txt.ForeColor = System.Drawing.Color.Gray;
                    float tx = (page.ClientRectangle.Width / 2f) - (size.Width / 2f);
                    //float ty = (page.ClientRectangle.Height / 2f) - (size.Height / 2f);
                    float ty = page.ClientRectangle.Height - size.Height - 150;
                    txt.Translate(tx, ty);
                    txt.Rotate(0f);
                    page.Add(txt);
                }
                using (var ms = new MemoryStream())
                {
                    doc.Save(ms);
                    return ms.ToArray();
                }
            }
            finally
            {
                doc.Close();
            }
        }
    }   
}
