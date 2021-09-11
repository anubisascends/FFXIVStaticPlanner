using FFXIVStaticPlanner.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FFXIVStaticPlanner.Data
{
    internal class ImageManager : IImageManager
    {
#if DEBUG
        const string MetadataFile = @"C:\Users\anubi\source\repos\FFXIVStaticPlanner\FFXIVStaticPlanner.Data\Metadata.xml";
#else
        const string MetadataFile = ".\\Metadata.xml";
#endif

        const string IdAttribute = "id";
        const string NameAttribute = "name";
        const string GroupAttribute = "group";

        const string ImagesNode = "Images";
        const string ImageNode = "Image";

        const string DefaultGroup = "General";

        public ImageManager ( IImageRepository repository ) => Repository = repository;

        private IImageRepository Repository
        {
            get;
        }

        public Guid AddImage ( byte[] data , string name , string group = DefaultGroup )
        {
            var id = Repository.AddImage(data);

            var objDoc = XDocument.Load(MetadataFile);
            var images = objDoc.Root.Element(ImagesNode);
            var imageData = images.Elements().Where(x => x.Attribute("id")?.Value == id.ToString()).FirstOrDefault();

            if ( imageData == null )
            {
                images.Add ( new XElement ( ImageNode ,
                    new XAttribute ( IdAttribute , id ) ,
                    new XAttribute ( NameAttribute , name ) ,
                    new XAttribute ( GroupAttribute , group ) ) );
            }

            objDoc.Save ( MetadataFile );

            return id;
        }

        public void DeleteImage ( Guid id )
        {
            Repository.DeleteImage ( id );

            var objDoc = XDocument.Load(MetadataFile);
            var images = objDoc.Root.Element(ImagesNode);
            var metadata = images.Elements ( )
                .Where ( x => x.Attribute ( IdAttribute )?.Value == id.ToString() )
                .FirstOrDefault();

            if ( metadata != null )
            {
                metadata.Remove ( );
                objDoc.Save ( MetadataFile );
            }
        }

        public ImageData GetImage ( Guid id )
        {

            var objDoc = XDocument.Load(MetadataFile);
            var images = objDoc.Root.Element(ImagesNode);
            var metadata = images.Elements ( )
                .Where ( x => x.Attribute ( IdAttribute )?.Value == id.ToString() )
                .FirstOrDefault();

            var result = new ImageData
            {
                ID = id,
                Name = metadata?.Attribute(NameAttribute)?.Value ?? "",
                Group = metadata?.Attribute(GroupAttribute)?.Value ?? DefaultGroup
            };

            result.SetRawData ( Repository.GetImage ( id ) );


            return result;
        }

        public ImageData[] GetAllImages ( )
        {
            var result = new List<ImageData>();

            var objDoc = XDocument.Load(MetadataFile);
            var images = objDoc.Root.Element(ImagesNode);

            foreach ( var item in images.Elements() )
            {
                var id = Guid.Parse(item.Attribute(IdAttribute)?.Value ?? Guid.Empty.ToString());

                var imageData = new ImageData
                {
                    ID = id ,
                    Name = item.Attribute ( NameAttribute )?.Value ?? "Image" ,
                    Group = item.Attribute ( GroupAttribute )?.Value ?? DefaultGroup
                };

                imageData.SetRawData ( Repository.GetImage ( id ) );

                result.Add ( imageData );
            }

            return result.ToArray ( );
        }
    }
}
