using System;
using System.Windows.Media;

namespace FFXIVStaticPlanner.Data
{
    public class ShapeData
    {
        public int ShapeType
        {
            get; set;
        }

        public double Left
        {
            get;set;
        }

        public double Top
        {
            get;set;
        }

        public double Width
        {
            get;set;
        }

        public double Height
        {
            get;set;
        }

        public Guid UUID
        {
            get;set;
        }

        public Color Color
        {
            get;set;
        }
    }
}
