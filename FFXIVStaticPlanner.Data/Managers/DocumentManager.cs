using System;

namespace FFXIVStaticPlanner.Data
{
    internal class DocumentManager : IDocumentManager
    {
        public Document LoadDocument ( string filename ) => throw new NotImplementedException ( );
        public void SaveDocument ( string filename , Document document ) => throw new NotImplementedException ( );
    }
}
