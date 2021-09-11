namespace FFXIVStaticPlanner.Data
{
    public static class ManagerFactory
    {
        public static IImageManager CreateImageManager ( ) => new ImageManager ( new ImageRepository ( ) );

        public static IDocumentManager CreateDocumentManager ( ) => new DocumentManager ( );
    }
}
