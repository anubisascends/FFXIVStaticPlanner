﻿using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace FFXIVStaticPlanner.Data
{
    public class ImageIcon
    {
        public string Display
        {
            get;set;
        }

        public Guid Id
        {
            get;set;
        }

        public Point Location
        {
            get;set;
        }

        public Size Scale
        {
            get;set;
        }
    }
}
