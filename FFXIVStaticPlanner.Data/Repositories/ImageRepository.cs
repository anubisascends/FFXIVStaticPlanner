using System;
using System.IO;

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
                File.WriteAllText ( fileName , strData );
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
