namespace FFXIVStaticPlanner.Data
{
    /// <summary>
    /// Defines the methods that a DocumentManager should implement
    /// </summary>
    public interface IDocumentManager
    {
        /// <summary>
        /// Returns a <see cref="Document"/> from a specific location
        /// </summary>
        /// <param name="filename">The name of the document to load</param>
        /// <returns>A new Document populated with the information from the file</returns>
        Document LoadDocument ( string filename );

        /// <summary>
        /// Saves a <see cref="Document"/> to a file location
        /// </summary>
        /// <param name="filename">The name of the file to save the document to</param>
        /// <param name="document">The document to save</param>
        void SaveDocument ( string filename , Document document );
    }
}
