namespace AnhDev99.HashSignatureLocal
{
    using iText.IO.Font.Constants;
    using iText.IO.Image;
    using iText.Kernel.Colors;
    using iText.Kernel.Font;
    using iText.Kernel.Pdf;
    using iText.Kernel.Pdf.Canvas;
    using iText.Kernel.Pdf.Canvas.Parser;
    using iText.Kernel.Pdf.Canvas.Parser.Listener;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    public class PdfHashSinger
    {
        PdfDocument pdfDoc;
        List<PdfSignatureComment> _comments;
        public void AddSignatureComment(PdfSignatureComment comment)
        {
            if (_comments == null)
            {
                _comments = new List<PdfSignatureComment>();
            }
            if (comment != null)
            {
                _comments.Add(comment);
            }
        }

        private void AddAnnotation(PdfSignatureComment comment)
        {
            if (comment != null && comment.GetRectangle() != null)
            {
                switch (comment.Type)
                {
                    case 1:
                        AddImageAnnotation(comment);
                        break;
                    case 2:
                        AddTextAnnotation(comment);
                        break;
                }
            }
        }

        private void AddImageAnnotation(PdfSignatureComment comment)
        {
            PdfCanvas canvas = new PdfCanvas(pdfDoc.GetPage(comment.Page));
            ImageData imageData = ImageDataFactory.Create(comment.ImageBgBytes);

            canvas.AddImageFittedIntoRectangle(imageData, comment.GetRectangle(), true);
        }

        private void AddTextAnnotation(PdfSignatureComment comment)
        {
            PdfCanvas canvas = new PdfCanvas(pdfDoc.GetPage(comment.Page));
            canvas.BeginText();
            if (comment.FontWeight == "bold")
            {
                canvas.SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD), comment.FontSize);
            }
            else
            {
                canvas.SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN), comment.FontSize);
            }

            canvas.MoveText(comment.X, comment.Y)
                    .ShowText(comment.Text)
                    .EndText();
        }

        public void AddTextSignature(string text) {
            PdfCanvas canvas = new PdfCanvas(pdfDoc.GetFirstPage());
            canvas.BeginText()
              .SetFontAndSize(
                  PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN), 1)
              .SetFillColor(ColorConstants.WHITE)
                  .MoveText(0, 0)
                  .ShowText(text)
                  .EndText();
            pdfDoc.Close();
        }
        private string ExtractPDF(string path)
        {
            if (File.Exists(path))
            {
                var pdfDocument = new PdfDocument(new PdfReader(path));
                StringBuilder processed = new StringBuilder();
                for (int i = 1; i <= pdfDocument.GetNumberOfPages(); ++i)
                {
                    var page = pdfDocument.GetPage(i);
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string text = PdfTextExtractor.GetTextFromPage(page, strategy);
                    processed.Append(text);

                }
                string str = processed.ToString();
                str = string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
                return str;
            }
            else
            {
                return null;
            }
        }
        public void Sign(string inPath, string outPath)
        {
            pdfDoc = new PdfDocument(new PdfReader(inPath), new PdfWriter(outPath));

            if(pdfDoc.GetNumberOfPages() <= 0)
            {
                throw new Exception("Tệp tin không được trống!");
            }

            /// Chèn nội dung mã hóa vào tệp tin Ký số
            var strSign = ExtractPDF(inPath);
            if (string.IsNullOrEmpty(strSign))
            {
                throw new Exception("Chuỗi mã hóa không được trống!");
            }

            AddTextSignature(strSign);

            // Chèn danh sách chữ ký hoặc text
            if (_comments.Count > 0)
            {
                foreach (var item in _comments)
                {
                    AddAnnotation(item);
                }
            }
            else
            {
                throw new Exception("Danh sách chữ ký rỗng!");
            }

            pdfDoc.Close();
        }
    }
}