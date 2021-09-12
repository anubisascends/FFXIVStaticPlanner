using System;

namespace FFXIVStaticPlanner.Data
{
    public interface IImageRepository
    {
        object GetImage ( Guid id );

        Guid AddImage ( object data );

        void DeleteImage ( Guid id );
    }
}
