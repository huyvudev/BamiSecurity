using System.Text;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.Wordprocessing;
using OpenXMLLibrary.Dtos;
using A = DocumentFormat.OpenXml.Drawing;
using A14 = DocumentFormat.OpenXml.Office2010.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using W = DocumentFormat.OpenXml.Wordprocessing;

namespace OpenXMLLibrary
{
    /// <summary>
    /// Các extention xử lý word
    /// </summary>
    public static class WordprocessingExtensions
    {
        public static (WordprocessingDocument wordDoc, MemoryStream memoryStream) OpenCloneDocument(
            string filePath
        )
        {
            using MemoryStream ms = new();
            new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite).CopyTo(
                ms
            );

            using WordprocessingDocument wordDoc = WordprocessingDocument.Open(ms, true);
            return (wordDoc, ms);
        }

        /// <summary>
        /// Sinh id cho phần tử NonVisualDrawing
        /// </summary>
        /// <param name="wordDoc"></param>
        /// <returns></returns>
        public static uint GenerateNonVisualDrawingUniqueId(this WordprocessingDocument wordDoc)
        {
            // Lấy tất cả các Id đã sử dụng trong tệp DOCX
            var allNonVisualId = wordDoc
                ?.MainDocumentPart
                ?.Document
                .Descendants<PIC.NonVisualDrawingProperties>()
                .Where(o => o.Id!.HasValue)
                .Select(o => o.Id!.Value);

            // Sinh một Id mới và kiểm tra xem nó đã tồn tại hay chưa
            Random random = new();
            uint newId = (uint)random.Next(1, int.MaxValue);
            while (allNonVisualId!.Contains(newId))
            {
                newId = (uint)random.Next(1, int.MaxValue);
            }
            return newId;
        }

        /// <summary>
        /// Sinh id cho phần tử doc
        /// </summary>
        /// <param name="wordDoc"></param>
        /// <returns></returns>
        public static uint GenerateDocUniqueId(this WordprocessingDocument wordDoc)
        {
            var allDocPropertyId = wordDoc
                ?.MainDocumentPart
                ?.Document
                .Descendants<DW.DocProperties>()
                .Where(o => o.Id!.HasValue)
                .Select(o => o.Id!.Value);

            // Sinh một Id mới và kiểm tra xem nó đã tồn tại hay chưa
            Random random = new();
            uint newId = (uint)random.Next(1, int.MaxValue);
            while (allDocPropertyId!.Contains(newId))
            {
                newId = (uint)random.Next(1, int.MaxValue);
            }
            return newId;
        }

        /// <summary>
        /// Thêm ảnh để lấy relationShipId (Id Of Part)
        /// </summary>
        /// <param name="wordDoc"></param>
        /// <param name="imageStream"></param>
        /// <param name="imagePartType"></param>
        /// <returns></returns>
        public static string AddImagePartToGetIdOfPart(
            this WordprocessingDocument wordDoc,
            Stream imageStream,
            ImagePartType imagePartType
        )
        {
            var mainPart = wordDoc?.MainDocumentPart;
            ImagePart imagePart = mainPart!.AddImagePart(imagePartType);
            imageStream.Position = 0;
            imagePart.FeedData(imageStream);
            return mainPart.GetIdOfPart(imagePart);
        }

        /// <summary>
        /// Thêm ảnh để lấy relationShipId (Id Of Part)
        /// </summary>
        /// <param name="wordDoc"></param>
        /// <param name="imageStream"></param>
        /// <param name="imageExtension"></param>
        /// <returns></returns>
        public static string AddImagePartToGetIdOfPart(
            this WordprocessingDocument wordDoc,
            Stream imageStream,
            string imageExtension
        )
        {
            ImagePartType imagePartType = imageExtension?.Trim().ToLower() switch
            {
                ".bmp" or "bmp" => ImagePartType.Bmp,
                ".gif" or "gif" => ImagePartType.Gif,
                ".png" or "png" => ImagePartType.Png,
                ".tiff" or "tiff" => ImagePartType.Tiff,
                ".pcx" or "pcx" => ImagePartType.Pcx,
                ".jpeg" or ".jpg" or "jpeg" or "jpg" => ImagePartType.Jpeg,
                ".emf" or "emf" => ImagePartType.Emf,
                ".wmf" or "wmf" => ImagePartType.Wmf,
                _ => throw new ArgumentException(imageExtension + " not allow")
            };
            return AddImagePartToGetIdOfPart(wordDoc, imageStream, imagePartType);
        }

        /// <summary>
        /// Thêm ảnh vào và trả ra đối tượng <see cref="W.Drawing"/>
        /// </summary>
        /// <param name="wordDoc"></param>
        /// <param name="relationshipId"></param>
        /// <param name="widthInInch">Chiều rộng tính bằng inch</param>
        /// <param name="heightInInch">Chiều cao ảnh tính bằng inch</param>
        /// <returns></returns>
        public static W.Drawing AddImage(
            this WordprocessingDocument wordDoc,
            string relationshipId,
            double widthInInch,
            double heightInInch
        )
        {
            var emuPerInch = 914400;
            long widthInEmu = (long)(emuPerInch * widthInInch);
            long heightInEmu = (long)(emuPerInch * heightInInch);

            //long widthInEmu = 792000L;
            //long heightInEmu = 792000L;

            var graphicFrameLocks = new A.GraphicFrameLocks() { NoChangeAspect = true };
            graphicFrameLocks.AddNamespaceDeclaration(
                "a",
                "http://schemas.openxmlformats.org/drawingml/2006/main"
            );

            var useLocalDpi = new A14.UseLocalDpi() { Val = false };
            useLocalDpi.AddNamespaceDeclaration(
                "a14",
                "http://schemas.microsoft.com/office/drawing/2010/main"
            );

            uint nvId = wordDoc.GenerateNonVisualDrawingUniqueId();
            uint docId = wordDoc.GenerateDocUniqueId();

            var picture = new PIC.Picture(
                new PIC.NonVisualPictureProperties(
                    new PIC.NonVisualDrawingProperties() { Id = nvId, Name = $"Picture {nvId}" },
                    new PIC.NonVisualPictureDrawingProperties(
                        new A.PictureLocks() { NoChangeAspect = true, NoChangeArrowheads = true }
                    )
                ),
                new PIC.BlipFill(
                    new A.Blip(
                        new A.BlipExtensionList(
                            new A.BlipExtension(useLocalDpi)
                            {
                                Uri = "{28A0092B-C50C-407E-A947-70E740481C1C}"
                            }
                        )
                    )
                    {
                        Embed = relationshipId,
                        CompressionState = A.BlipCompressionValues.Print
                    },
                    new A.Stretch(new A.FillRectangle())
                ),
                new PIC.ShapeProperties(
                    new A.Transform2D(
                        new A.Offset() { X = 0L, Y = 0L },
                        new A.Extents() { Cx = widthInEmu, Cy = heightInEmu }
                    ),
                    new A.PresetGeometry(new A.AdjustValueList())
                    {
                        Preset = A.ShapeTypeValues.Rectangle
                    },
                    new A.NoFill(),
                    new A.Outline(new A.NoFill())
                )
                {
                    BlackWhiteMode = A.BlackWhiteModeValues.Auto
                }
            );
            picture.AddNamespaceDeclaration(
                "pic",
                "http://schemas.openxmlformats.org/drawingml/2006/picture"
            );

            var graphic = new A.Graphic(
                new A.GraphicData(picture)
                {
                    Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture"
                }
            );
            graphic.AddNamespaceDeclaration(
                "a",
                "http://schemas.openxmlformats.org/drawingml/2006/main"
            );

            var element = new W.Drawing(
                new DW.Inline(
                    new DW.Extent() { Cx = widthInEmu, Cy = heightInEmu, },
                    new DW.EffectExtent()
                    {
                        LeftEdge = 0L,
                        TopEdge = 0L,
                        RightEdge = 0L,
                        BottomEdge = 0L
                    },
                    new DW.DocProperties() { Id = docId, Name = $"Picture {docId}" },
                    new DW.NonVisualGraphicFrameDrawingProperties(graphicFrameLocks),
                    graphic
                )
                {
                    DistanceFromTop = (UInt32Value)0U,
                    DistanceFromBottom = (UInt32Value)0U,
                    DistanceFromLeft = (UInt32Value)0U,
                    DistanceFromRight = (UInt32Value)0U,
                }
            );
            return element;
        }

        /// <summary>
        /// Thêm ảnh vào và trả ra đối tượng <see cref="W.Drawing"/>
        /// </summary>
        /// <param name="wordDoc"></param>
        /// <param name="imageStream"></param>
        /// <param name="imagePartType"></param>
        /// <param name="widthInInch"></param>
        /// <param name="heightInInch"></param>
        /// <returns></returns>
        public static W.Drawing AddImage(
            this WordprocessingDocument wordDoc,
            Stream imageStream,
            ImagePartType imagePartType,
            double widthInInch,
            double heightInInch
        )
        {
            return wordDoc.AddImage(
                wordDoc.AddImagePartToGetIdOfPart(imageStream, imagePartType),
                widthInInch,
                heightInInch
            );
        }

        /// <summary>
        /// Thêm ảnh vào và trả ra đối tượng <see cref="W.Drawing"/>
        /// </summary>
        /// <param name="wordDoc"></param>
        /// <param name="imageStream"></param>
        /// <param name="imageExtension"></param>
        /// <param name="widthInInch"></param>
        /// <param name="heightInInch"></param>
        /// <returns></returns>
        public static W.Drawing AddImage(
            this WordprocessingDocument wordDoc,
            Stream imageStream,
            string imageExtension,
            double widthInInch,
            double heightInInch
        )
        {
            return wordDoc.AddImage(
                wordDoc.AddImagePartToGetIdOfPart(imageStream, imageExtension),
                widthInInch,
                heightInInch
            );
        }

        /// <summary>
        /// Đè thông tin vào text place holder
        /// </summary>
        /// <param name="wordDoc"></param>
        /// <param name="inputReplaces"></param>
        public static void ReplaceTextPlaceHolder(
            this WordprocessingDocument wordDoc,
            IEnumerable<InputReplaceDto> inputReplaces
        )
        {
            var mainPart = wordDoc.MainDocumentPart;
            foreach (var inputReplace in inputReplaces)
            {
                var textPlaceHolders = mainPart
                    ?.Document
                    ?.Body
                    ?.Descendants<W.Text>()
                    .Where(e => e.Text.Split(" ").Contains(inputReplace.FindText!))
                    .ToList();
                if (textPlaceHolders == null)
                {
                    continue;
                }
                foreach (var textPlaceHolder in textPlaceHolders)
                {
                    if (inputReplace.ReplaceText != null) //replace text
                    {
                        textPlaceHolder.Text = textPlaceHolder.Text.Replace(
                            inputReplace.FindText!,
                            inputReplace.ReplaceText
                        );
                    }
                    else if (inputReplace.ReplaceImage != null) //replace image
                    {
                        var parent = textPlaceHolder.Parent;
                        if (parent is not W.Run) // Parent should be a run element.
                        {
                            throw new InvalidOperationException(
                                "Parent element of text placehoder is not instance of Run"
                            );
                        }
                        else
                        {
                            parent.AppendChild(
                                wordDoc.AddImage(
                                    inputReplace.ReplaceImage,
                                    inputReplace.ReplaceImageExtension!,
                                    inputReplace.ReplaceImageWidth,
                                    inputReplace.ReplaceImageHeight
                                )
                            );
                            if (inputReplace.FindText == textPlaceHolder.Text.Trim())
                            {
                                textPlaceHolder.Remove();
                            }
                            else
                            {
                                var space = new string(' ', inputReplace.FindText!.Length);
                                textPlaceHolder.Text = textPlaceHolder.Text.Replace(
                                    inputReplace.FindText,
                                    space
                                );
                            }
                        }
                    }
                }
            }
            mainPart?.Document.Save();

        }

        /// <summary>
        /// Đọc thông tin từ file text theo mẫu
        /// </summary>
        /// <param name="wordDoc"></param>
        /// <param name="inputReplaces"></param>
        public static List<OutputTextModelDto> ReadTextPlaceHolder(
            this WordprocessingDocument wordDoc
        )
        {
            var mainPart = wordDoc.MainDocumentPart;
            var body = mainPart?.Document?.Body;
            var paragraphPlaceHolders = body?.Descendants<W.Paragraph>()
                .Where(x => !string.IsNullOrEmpty(x.InnerText))
                .ToList();
            List<OutputTextModelDto> outputTextModels = [];
            int startQuestion = 1;
            while (true)
            {
                if (paragraphPlaceHolders is null)
                {
                    break;
                }
                if (
                    !paragraphPlaceHolders.Exists(x => x.InnerText.Contains($"Câu {startQuestion}"))
                )
                {
                    break;
                }
                // Biến cờ check văn bản nằm giữa kí từ đã định sẵn
                bool isBetween = false;
                // Lưu câu hỏi
                StringBuilder stringBuilder = new();
                // Lưu câu trả lời
                List<string> textChildren = [];
                foreach (var paragraph in paragraphPlaceHolders ?? [])
                {
                    if (
                        paragraph.InnerText.Trim().Contains($"Câu {startQuestion}:")
                        || paragraph.InnerText.Trim().Contains($"Câu {startQuestion + 1}:")
                    )
                    {
                        // Thay đổi cờ khi văn bản chứa 2 điểm mốc.
                        isBetween = !isBetween;

                        // Nếu là phần tử cuối thì break.
                        if (paragraph.InnerText.Trim().Contains($"Câu {startQuestion + 1}:"))
                            break;
                    }
                    if (isBetween)
                    {
                        if (
                            (
                                (
                                    char.IsUpper(paragraph.InnerText[0])
                                    || int.TryParse(
                                        paragraph.InnerText[0].ToString(),
                                        out int lineId
                                    )
                                )
                                && paragraph.InnerText[1] == '.'
                            ) || paragraph.ParagraphProperties?.NumberingProperties is not null
                        )
                        {
                            textChildren.Add(paragraph.InnerText);
                        }
                        else
                        {
                            stringBuilder.Append($"{paragraph.InnerText}\n");
                        }
                    }
                }
                outputTextModels.Add(
                    new() { Text = stringBuilder.ToString(), Children = textChildren }
                );
                startQuestion++;
            }
            return outputTextModels;
        }

        public static string ConvertTableToMarkdown(W.Table table)
        {
            StringBuilder markdownBuilder = new StringBuilder();

            // Loop through each row in the table.
            foreach (var row in table.Elements<W.TableRow>())
            {
                // Loop through each cell in the row.
                foreach (var cell in row.Elements<W.TableCell>())
                {
                    // Get the text within the cell.
                    string cellText = cell.InnerText.Trim();

                    // Append the cell text to the Markdown builder with separator.
                    markdownBuilder.Append("| ");
                    markdownBuilder.Append(cellText.Replace("\n", " ")); // Replace newline characters with space.
                    markdownBuilder.Append(" ");
                }

                // End the row with a Markdown line break.
                markdownBuilder.AppendLine("|");
            }

            // Add Markdown table headers based on the number of columns.
            int numColumns = table.Elements<W.TableRow>().First().Elements<W.TableCell>().Count();
            for (int i = 0; i < numColumns; i++)
            {
                markdownBuilder.Append("| --- ");
            }
            markdownBuilder.AppendLine("|");

            return markdownBuilder.ToString();
        }
    }
}
