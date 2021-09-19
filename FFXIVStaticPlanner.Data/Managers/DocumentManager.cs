using System;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace FFXIVStaticPlanner.Data
{
    internal class DocumentManager : IDocumentManager
    {
        private const string StrokesElement = "Strokes";
        private const string StrokeElement = "Stroke";
        private const string ImagesElement = "Images";
        private const string ImageElement = "Image";
        private const string ShapesElement = "Shapes";
        private const string ShapeElement = "Shape";
        private const string RootElement = "Document";
        private const string StylusPointsElement = "StylusPoints";
        private const string TextElement = "Text";

        private const string XAttribute = "x";
        private const string YAttribute = "y";
        private const string PressureAttributre = "pressurefactor";
        private const string DisplayAttribute = "display";
        private const string IDAttribute = "id";
        private const string ScaleAttribute = "scale";
        private const string UUIDAttribute = "uuid";
        private const string TypeAttribute = "type";
        private const string WidthAttribute = "width";
        private const string HeightAttribute = "height";
        private const string CanvasAttribute = "canvas";
        private const string ColorAttribute = "color";
        private const string HighlightAttribute = "highlight";

        public Document LoadDocument ( string filename )
        {
            var result = new Document();
            var document = XDocument.Load(filename);

            // add strokes
            foreach ( var strokeElem in document.Root.Element ( StrokesElement ).Elements ( ) )
            {
                var points = new StylusPointCollection();

                foreach ( var pointElem in strokeElem.Elements ( StylusPointsElement ) )
                {
                    var point = new StylusPoint(
                        double.Parse(pointElem.Attribute(XAttribute)?.Value ?? "0"),
                        double.Parse(pointElem.Attribute(YAttribute)?.Value ?? "0"),
                        float.Parse(pointElem.Attribute(PressureAttributre)?.Value ?? "0"));

                    points.Add ( point );

                }

                result.Strokes.Add ( new Stroke ( points , new DrawingAttributes
                {
                    Color = ( Color )ColorConverter.ConvertFromString ( strokeElem.Attribute ( ColorAttribute ).Value ?? "#FFFFFF" ) ,
                    IsHighlighter = bool.Parse ( strokeElem.Attribute ( HighlightAttribute )?.Value ?? "false" ) ,
                    Width = double.Parse ( strokeElem.Attribute ( WidthAttribute )?.Value ?? "0" ) ,
                    Height = double.Parse ( strokeElem.Attribute ( HeightAttribute )?.Value ?? "0" )
                } ) );
            }

            // add images
            foreach ( var imageElem in document.Root.Element ( ImagesElement ).Elements ( ) )
            {
                result.Images.Add ( new ImageIcon
                {
                    Display = imageElem.Attribute ( DisplayAttribute )?.Value ?? string.Empty ,
                    Id = Guid.Parse ( imageElem.Attribute ( IDAttribute )?.Value ?? Guid.Empty.ToString ( ) ) ,
                    Location = new System.Windows.Point
                        (
                            double.Parse ( imageElem.Attribute ( XAttribute )?.Value ?? "0" ) ,
                            double.Parse ( imageElem.Attribute ( YAttribute )?.Value ?? "0" )
                        ) ,
                    UUID = Guid.Parse ( imageElem.Attribute ( UUIDAttribute )?.Value ?? Guid.Empty.ToString ( ) ) ,
                    Canvas = int.Parse ( imageElem.Attribute ( CanvasAttribute )?.Value ?? "1" ) ,
                    Size = new System.Windows.Size
                        (
                            double.Parse ( imageElem.Attribute ( WidthAttribute )?.Value ?? "0" ) ,
                            double.Parse ( imageElem.Attribute ( HeightAttribute )?.Value ?? "0" )
                        )
                } );
            }

            // add shapes
            foreach ( var shapeElem in document.Root.Element ( ShapesElement ).Elements ( ) )
            {
                result.Shapes.Add ( new ShapeData
                {
                    ShapeType = int.Parse ( shapeElem.Attribute ( TypeAttribute )?.Value ?? "1" ) ,
                    Color = ( Color )ColorConverter.ConvertFromString ( shapeElem.Attribute ( ColorAttribute )?.Value ?? "#FFFFFF" ) ,
                    Height = double.Parse ( shapeElem.Attribute ( HeightAttribute )?.Value ?? "0" ) ,
                    Width = double.Parse ( shapeElem.Attribute ( WidthAttribute )?.Value ?? "0" ) ,
                    Left = double.Parse ( shapeElem.Attribute ( XAttribute )?.Value ?? "0" ) ,
                    Top = double.Parse ( shapeElem.Attribute ( YAttribute )?.Value ?? "0" ) ,
                    UUID = Guid.Parse ( shapeElem.Attribute ( UUIDAttribute )?.Value ?? Guid.Empty.ToString ( ) )
                } );
            }

            // add text
            foreach ( var textElem in document.Root.Element ( TextElement ).Elements ( ) )
            {
                result.Text.Add ( new TextData
                {
                    Color = new SolidColorBrush ( ( Color )ColorConverter.ConvertFromString ( textElem.Attribute ( ColorAttribute )?.Value ?? "#FFFFFF" ) ) ,
                    Height = double.Parse ( textElem.Attribute ( HeightAttribute )?.Value ?? "0" ) ,
                    Width = double.Parse ( textElem.Attribute ( WidthAttribute )?.Value ?? "0" ) ,
                    X = double.Parse ( textElem.Attribute ( XAttribute )?.Value ?? "0" ) ,
                    Y = double.Parse ( textElem.Attribute ( YAttribute )?.Value ?? "0" ) ,
                    UUID = Guid.Parse ( textElem.Attribute ( UUIDAttribute )?.Value ?? Guid.Empty.ToString ( ) ) ,
                    Text = textElem.Value
                } );
            }

            result.FileName = filename;

            return result;
        }

        public void SaveDocument ( string filename , Document document )
        {
            var xml = new XDocument( new XElement(RootElement,
                new XElement(StrokesElement),
                new XElement(ImagesElement),
                new XElement(ShapesElement),
                new XElement(TextElement)));

            var strokesElem = xml.Root.Element(StrokesElement);
            var shapesElem = xml.Root.Element(ShapesElement);
            var imagesElem = xml.Root.Element(ImagesElement);
            var textElem = xml.Root.Element(TextElement);


            // add strokes
            foreach ( var stroke in document.Strokes )
            {
                var strokeElem = new XElement(StrokeElement,
                    new XAttribute(ColorAttribute, stroke.DrawingAttributes.Color),
                    new XAttribute(HighlightAttribute, stroke.DrawingAttributes.IsHighlighter),
                    new XAttribute(WidthAttribute, stroke.DrawingAttributes.Width),
                    new XAttribute(HeightAttribute, stroke.DrawingAttributes.Height));

                foreach ( var point in stroke.StylusPoints )
                {
                    var pointsElem = new XElement(StylusPointsElement,
                        new XAttribute(XAttribute, point.X),
                        new XAttribute(YAttribute, point.Y),
                        new XAttribute(PressureAttributre, point.PressureFactor));

                    strokeElem.Add ( pointsElem );
                }

                strokesElem.Add ( strokeElem );
            }

            // add images
            foreach ( var image in document.Images )
            {
                imagesElem.Add ( new XElement ( ImageElement ,
                    new XAttribute ( DisplayAttribute , image.Display ?? string.Empty ) ,
                    new XAttribute ( IDAttribute , image.Id ) ,
                    new XAttribute ( XAttribute , image.Location.X ) ,
                    new XAttribute ( YAttribute , image.Location.Y ) ,
                    new XAttribute ( ScaleAttribute , image.Scale ) ,
                    new XAttribute ( UUIDAttribute , image.UUID ) ,
                    new XAttribute ( CanvasAttribute , image.Canvas ) ,
                    new XAttribute ( WidthAttribute , image.Size.Width ) ,
                    new XAttribute ( HeightAttribute , image.Size.Height ) ) );
            }

            // add shapes
            foreach ( var shape in document.Shapes )
            {
                shapesElem.Add ( new XElement ( ShapeElement ,
                    new XAttribute ( UUIDAttribute , shape.UUID ) ,
                    new XAttribute ( TypeAttribute , shape.ShapeType ) ,
                    new XAttribute ( WidthAttribute , shape.Width ) ,
                    new XAttribute ( HeightAttribute , shape.Height ) ,
                    new XAttribute ( XAttribute , shape.Left ) ,
                    new XAttribute ( YAttribute , shape.Top ) ,
                    new XAttribute ( ColorAttribute , shape.Color ) ) );
            }

            // add text
            foreach ( var text in document.Text )
            {
                textElem.Add ( new XElement ( TextElement ,
                    new XAttribute ( UUIDAttribute , text.UUID ) ,
                    new XAttribute ( ColorAttribute , text.Color ) ,
                    new XAttribute ( XAttribute , text.X ) ,
                    new XAttribute ( YAttribute , text.Y ) ,
                    new XAttribute ( HeightAttribute , text.Height ) ,
                    new XAttribute ( WidthAttribute , text.Width ) ,
                    text.Text ) );
            }

            xml.Save ( filename );
            document.FileName = filename;
        }
    }
}