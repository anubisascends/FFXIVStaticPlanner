using FFXIVStaticPlanner.Core;
using System;

namespace FFXIVStaticPlanner.Data
{
    public interface IImageManager
    {
        ImageData GetImage ( Guid id );

        Guid AddImage ( object data , string name, string group );

        void DeleteImage ( Guid id );

        ImageData[] GetAllImages ( );
    }
}
