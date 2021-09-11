using System;
using System.IO;

namespace FFXIVStaticPlanner.Data
{
    internal class ImageRepository : IImageRepository
    {
        const string StorageFolder = ".\\Images\\";

        public Guid AddImage ( byte[] data )
        {
            var id = Guid.NewGuid();
            var fileName = getFileName(id);

            File.WriteAllBytes ( fileName , data );

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

        public byte[] GetImage ( Guid id )
        {
            var filename = getFileName(id);

            return File.Exists ( filename ) ? File.ReadAllBytes ( filename ) : null;
        }

        private static string getFileName ( Guid id ) => StorageFolder + id.ToString ( ) + ".dat";
    }
}
