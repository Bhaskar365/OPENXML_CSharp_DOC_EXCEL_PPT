﻿using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.IO;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;

Console.WriteLine("Hello, World!");

static void InsertAPicture()
{
    string documentLocation = "C:\\OPENXML_C#\\Document_Generation_Using_OpenXML_With_Font_Syling\\Image_Insert_In_Document\\Destination\\testBlankDoc.docx";
    string imageLocation = "C:\\OPENXML_C#\\Document_Generation_Using_OpenXML_With_Font_Syling\\Image_Insert_In_Document\\Destination\\sampleLogo.jpg";

    using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(documentLocation, true))
    {
        if (wordprocessingDocument.MainDocumentPart is null)
        {
            throw new ArgumentNullException("MainDocumentPart is null.");
        }

        MainDocumentPart mainPart = wordprocessingDocument.MainDocumentPart;

        ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

        using (FileStream stream = new FileStream(imageLocation, FileMode.Open))
        {
            imagePart.FeedData(stream);
        }

        AddImageToBody(wordprocessingDocument, mainPart.GetIdOfPart(imagePart));
    }
}

static void AddImageToBody(WordprocessingDocument wordDoc, string relationshipId)
{
    Console.WriteLine("Image Generation Started");
    // Define the reference of the image.
    var element =
         new Drawing(
             new DW.Inline(
                 new DW.Extent() { Cx = 150000L, Cy = 150000L },
                 new DW.EffectExtent()
                 {
                     LeftEdge = 0L,
                     TopEdge = 0L,
                     RightEdge = 0L,
                     BottomEdge = 0L
                 },
                 new DW.DocProperties()
                 {
                     Id = (UInt32Value)1U,
                     Name = "Picture 1"
                 },
                 new DW.NonVisualGraphicFrameDrawingProperties(
                     new A.GraphicFrameLocks() { NoChangeAspect = true }),
                 new A.Graphic(
                     new A.GraphicData(
                         new PIC.Picture(
                             new PIC.NonVisualPictureProperties(
                                 new PIC.NonVisualDrawingProperties()
                                 {
                                     Id = (UInt32Value)0U,
                                     Name = "New Bitmap Image.jpg"
                                 },
                                 new PIC.NonVisualPictureDrawingProperties()),
                             new PIC.BlipFill(
                                 new A.Blip(
                                     new A.BlipExtensionList(
                                         new A.BlipExtension()
                                         {
                                             Uri =
                                                "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                         })
                                 )
                                 {
                                     Embed = relationshipId,
                                     CompressionState =
                                     A.BlipCompressionValues.Print
                                 },
                                 new A.Stretch(
                                     new A.FillRectangle())),
                             new PIC.ShapeProperties(
                                 new A.Transform2D(
                                     new A.Offset() { X = 0L, Y = 0L },
                                     new A.Extents() { Cx = 990000L, Cy = 792000L }),
                                 new A.PresetGeometry(
                                     new A.AdjustValueList()
                                 )
                                 { Preset = A.ShapeTypeValues.Rectangle }))
                     )
                     { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
             )
             {
                 DistanceFromTop = (UInt32Value)0U,
                 DistanceFromBottom = (UInt32Value)0U,
                 DistanceFromLeft = (UInt32Value)0U,
                 DistanceFromRight = (UInt32Value)0U,
                 EditId = "50D07946"
             });

    if (wordDoc.MainDocumentPart is null || wordDoc.MainDocumentPart.Document.Body is null)
    {
        throw new ArgumentNullException("MainDocumentPart and/or Body is null.");
    }

    // Append the reference to body, the element should be in a Run.
    wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph(new Run(element)));
    Console.WriteLine("Image Generation Successful");
}

InsertAPicture();