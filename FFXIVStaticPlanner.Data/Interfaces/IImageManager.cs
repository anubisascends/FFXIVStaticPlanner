using FFXIVStaticPlanner.Core;
using System;

namespace FFXIVStaticPlanner.Data
{
    /// <summary>
    /// Defines how an ImageManager should function
    /// </summary>
    public interface IImageManager
    {
        /// <summary>
        /// Returns the <see cref="ImageData"/> from a single image in the file system
        /// </summary>
        /// <param name="id">The id of the file to use</param>
        /// <returns>A new image data object from the file system</returns>
        ImageData GetImage ( Guid id );

        /// <summary>
        /// Returns the <see cref="Guid"/> of the image that was added to the data storage
        /// </summary>
        /// <param name="data">The image data</param>
        /// <param name="name">The name of the image, for naming in the app</param>
        /// <param name="group">The group that the image belongs to, for sorting</param>
        /// <returns>A new guid from the image that was stored</returns>
        Guid AddImage ( object data , string name, string group );

        /// <summary>
        /// Deletes the image with the given <see cref="Guid"/> for a file name
        /// </summary>
        /// <param name="id">The id of the image to remove</param>
        void DeleteImage ( Guid id );

        /// <summary>
        /// Returns a full list of all fo the <see cref="ImageData"/> objects in the data storage
        /// </summary>
        /// <returns>An array of image data from the file store</returns>
        ImageData[] GetAllImages ( );
    }
}
