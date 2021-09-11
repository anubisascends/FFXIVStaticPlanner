using System;

namespace FFXIVStaticPlanner.Data
{
    public interface IImageRepository
    {
        byte[] GetImage ( Guid id );

        Guid AddImage ( byte[] data );

        void DeleteImage ( Guid id );
    }
}
