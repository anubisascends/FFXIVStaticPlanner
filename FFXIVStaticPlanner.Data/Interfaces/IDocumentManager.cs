namespace FFXIVStaticPlanner.Data
{
    public interface IDocumentManager
    {
        Document LoadDocument ( string filename );

        void SaveDocument ( string filename , Document document );
    }
}
