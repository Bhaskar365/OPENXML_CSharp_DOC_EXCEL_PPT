﻿// See https://aka.ms/new-console-template for more information
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;

Console.WriteLine("Program to add table to document");

AddTable();

// Take the data from a two-dimensional array and build a table at the 
// end of the supplied document.
static void AddTable()
{
    Console.WriteLine("Started generating table");
    string fileName = "C:\\OPENXML_C#\\Document_Generation_Using_OpenXML_With_Font_Syling\\Add_Table_To_Document\\Sample_Doc\\sample.docx";

    string json = "[" +
        "[\"Name\", \"Age\", \"Country\"]," +
        "[\"John\", \"30\", \"USA\"]," +
        "[\"Alice\", \"25\", \"Canada\"]," +
        "[\"Bob\", \"35\", \"UK\"]" +
        "]";

    // read the data from the json file
    var data = System.Text.Json.JsonSerializer.Deserialize<string[][]>(json);

    if (data is not null)
    {
        using (var document = WordprocessingDocument.Open(fileName, true))
        {
            if (document.MainDocumentPart is null || document.MainDocumentPart.Document.Body is null)
            {
                throw new ArgumentNullException("MainDocumentPart and/or Body is null.");
            }

            Console.WriteLine("Generation started");

            var doc = document.MainDocumentPart.Document;

            Table table = new();

            TableProperties props = new(
                new TableBorders(
                new TopBorder
                {
                    Val = new EnumValue<BorderValues>(BorderValues.Single),
                    Size = 12
                },
                new BottomBorder
                {
                    Val = new EnumValue<BorderValues>(BorderValues.Single),
                    Size = 12
                },
                new LeftBorder
                {
                    Val = new EnumValue<BorderValues>(BorderValues.Single),
                    Size = 12
                },
                new RightBorder
                {
                    Val = new EnumValue<BorderValues>(BorderValues.Single),
                    Size = 12
                },
                new InsideHorizontalBorder
                {
                    Val = new EnumValue<BorderValues>(BorderValues.Single),
                    Size = 12
                },
                new InsideVerticalBorder
                {
                    Val = new EnumValue<BorderValues>(BorderValues.Single),
                    Size = 12
                }));

            table.AppendChild<TableProperties>(props);

            for (var i = 0; i < data.Length; i++)
            {
                var tr = new TableRow();
                for (var j = 0; j < data[i].Length; j++)
                {
                    var tc = new TableCell();
                    tc.Append(new Paragraph(new Run(new Text(data[i][j]))));

                    // Assume you want columns that are automatically sized.
                    tc.Append(new TableCellProperties(
                        new TableCellWidth { Type = TableWidthUnitValues.Auto }));

                    tr.Append(tc);
                }
                table.Append(tr);
            }
            doc.Body.Append(table);
            doc.Save();
            Console.WriteLine("Generation Successful");
        }
    }
}