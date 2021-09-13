using System;

namespace FFXIVStaticPlanner.Data
{
    /// <summary>
    /// Defines how the image repository needs to work
    /// </summary>
    public interface IImageRepository
    {
        /// <summary>
        /// Returns an image from the file storage
        /// </summary>
        /// <param name="id">The id of image</param>
        /// <returns></returns>
        object GetImage ( Guid id );

        /// <summary>
        /// Adds a new image to the file storage
        /// </summary>
        /// <param name="data">The image data</param>
        /// <returns>The id of the image that was created in the repository</returns>
        Guid AddImage ( object data );

        /// <summary>
        /// Removes an image, with the given <see cref="Guid"/> from the repository
        /// </summary>
        /// <param name="id">The id of the image to remove</param>
        void DeleteImage ( Guid id );
    }
}
