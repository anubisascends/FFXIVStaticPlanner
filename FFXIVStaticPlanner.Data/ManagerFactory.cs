namespace FFXIVStaticPlanner.Data
{
    /// <summary>
    /// This provides a way to create new instances of the various managers that this application uses
    /// </summary>
    public static class ManagerFactory
    {
        /// <summary>
        /// Returns the a new <see cref="IImageManager"/>
        /// </summary>
        /// <returns>A new IImageManager instance</returns>
        public static IImageManager CreateImageManager ( ) => new ImageManager ( new ImageRepository ( ) );

        /// <summary>
        /// Returns a new <see cref="IDocumentManager"/>
        /// </summary>
        /// <returns>A new IDocumentManager instance</returns>
        public static IDocumentManager CreateDocumentManager ( ) => new DocumentManager ( );
    }
}
