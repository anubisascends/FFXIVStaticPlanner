using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.IO;
using System.Windows;
using System.Xml;
using DpiScale = SharpVectors.Runtime.DpiScale;

namespace FFXIVStaticPlanner.Data
{
    internal class ImageRepository : IImageRepository
    {
        const string StorageFolder = ".\\Images\\";

        public Guid AddImage ( object data )
        {
            var id = Guid.NewGuid();
            var fileName = getFileName(id);

            if ( data is byte[] byteData )
            {
                File.WriteAllBytes ( fileName , byteData );
            }
            else if ( data is string strData )
            {
                WpfDrawingSettings wpfDrawingSettings = new();
                wpfDrawingSettings.IncludeRuntime = true;
                wpfDrawingSettings.TextAsGeometry = false;
                wpfDrawingSettings.IgnoreRootViewbox = true;

                using var converter = new StreamSvgConverter(wpfDrawingSettings);
                using XmlReader input = XmlReader.Create(new StringReader(strData), new XmlReaderSettings{ DtdProcessing = DtdProcessing.Parse } );
                using Stream output = File.Create(fileName);

                converter.Convert ( input , output );
                output.Flush ( );
                output.Close ( );
            }

            return id;
        }

        public void DeleteImage ( Guid id )
        {
            var filename = getFileName(id);

            if ( File.Exists ( filename ) )
            {
                File.Delete ( filename );
            }
        }

        public object GetImage ( Guid id )
        {
            var filename = getFileName(id);

            return File.Exists ( filename ) ? File.ReadAllBytes ( filename ) : null;
        }

        private static string getFileName ( Guid id ) => StorageFolder + id.ToString ( ) + ".dat";
    }
}
