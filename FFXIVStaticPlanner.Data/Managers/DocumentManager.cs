using System;
using System.Xml.Linq;

namespace FFXIVStaticPlanner.Data
{
    internal class DocumentManager : IDocumentManager
    {
        const string StrokesElement = "Strokes";
        const string StrokeElement = "Stroke";
        const string ImagesElement = "Images";
        const string ImageElement = "Image";
        const string ShapesElement = "Shapes";
        const string ShapeElement = "Shape";
        const string RootElement = "Document";
        const string StylusPointsElement = "StylusPoints";
        const string DescriptionElement = "Description";
        const string PropertyElement = "Property";

        const string XAttribute = "x";
        const string YAttribute = "Y";
        const string PressureAttributre = "pressurefactor";
        const string DisplayAttribute = "display";
        const string IDAttribute = "id";
        const string ScaleAttribute = "scale";
        const string UUIDAttribute = "uuid";
        const string IsButtonAttribute = "isButton";
        const string MaxAttribute = "maximum";
        const string ResolutionAttribute = "resolution";
        const string MinAttribute = "minimum";
        const string UnitAttribute = "unit";
        const string TypeAttribute = "type";
        const string WidthAttribute = "width";
        const string HeightAttribute = "height";


        public Document LoadDocument ( string filename ) => throw new NotImplementedException ( );

        public void SaveDocument ( string filename , Document document )
        {
            var xml = new XDocument( new XElement(RootElement,
                new XElement(StrokesElement),
                new XElement(ImagesElement),
                new XElement(ShapesElement)));

            var strokesElem = xml.Root.Element(StrokesElement);
            var shapesElem = xml.Root.Element(ShapesElement);
            var imagesElem = xml.Root.Element(ImagesElement);


            // add strokes
            foreach ( var stroke in document.Strokes )
            {
                var strokeElem = new XElement(StrokeElement,
                    new XAttribute("color", stroke.DrawingAttributes.Color));

                foreach ( var point in stroke.StylusPoints )
                {
                    var pointsElem = new XElement(StylusPointsElement,
                        new XAttribute(XAttribute, point.X),
                        new XAttribute(YAttribute, point.Y),
                        new XElement(DescriptionElement, point.Description),
                        new XAttribute(PressureAttributre, point.PressureFactor));

                    foreach ( var property in point.Description.GetStylusPointProperties ( ) )
                    {
                        pointsElem.Add ( new XElement ( PropertyElement ,
                            new XAttribute ( IDAttribute , property.Id ) ,
                            new XAttribute ( IsButtonAttribute , property.IsButton ) ,
                            new XAttribute ( MaxAttribute , property.Maximum ) ,
                            new XAttribute ( MinAttribute , property.Minimum ) ,
                            new XAttribute ( ResolutionAttribute , property.Resolution ) ,
                            new XAttribute ( UnitAttribute , property.Unit ) ) );
                    }

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
                    new XAttribute ( UUIDAttribute , image.UUID ) ) );
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
                    new XAttribute ( YAttribute , shape.Top ) ) );
            }

            xml.Save ( filename );
            document.FileName = filename;
        }
    }
}