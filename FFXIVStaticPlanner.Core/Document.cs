using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Shapes;

namespace FFXIVStaticPlanner.Data
{

    public class Document : INotifyPropertyChanged
    {
        private string strFileName;
        private StrokeCollection _objStrokes = new();
        private ObservableCollection<ImageIcon> _objImages;
        private bool _bChanged;
        private ShapeDataCollection _objShapes;

        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class
        /// </summary>
        public Document ( )
        {
            _objStrokes = new ( );
            _objImages = new ( );
            _objShapes = new ( );

            _objStrokes.StrokesChanged += onStrokesChanged;
            _objImages.CollectionChanged += onImagesChanged;
            _objShapes.CollectionChanged += onShapesChanged;
        }

        /// <summary>
        /// Raised whenever the value of a property has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the filename that this document has been stored in
        /// </summary>
        public string FileName
        {
            get => strFileName;
            set
            {
                strFileName = value;
                raisePropertyChanged ( );
            }
        }

        /// <summary>
        /// Gets the strokes that this document holds
        /// </summary>
        public StrokeCollection Strokes => _objStrokes;

        /// <summary>
        /// Gets the Images that this document holds
        /// </summary>
        public ObservableCollection<ImageIcon> Images => _objImages;

        /// <summary>
        /// Gets all of the shapes in this document
        /// </summary>
        public ShapeDataCollection Shapes => _objShapes;

        /// <summary>
        /// Gets a value that indicates whether or not this document has changes
        /// </summary>
        public bool HasChanges
        {
            get => _bChanged;
            set
            {
                _bChanged = value;
                raisePropertyChanged ( );
            }
        }

        /// <summary>
        /// Clears out the contents of this document
        /// </summary>
        public void Clear ( )
        {
            FileName = string.Empty;
            _objStrokes.Clear ( );
            _objImages.Clear ( );
            _objShapes.Clear ( );

            HasChanges = false;
        }

        /// <summary>
        /// Returns a value that indicates whether or not there are elements in this document
        /// </summary>
        /// <returns><see langword="true"/> if there are any elements in this document, otherwise <see langword="false"/></returns>
        public bool HasElements => _objStrokes.Count > 0
            || _objImages.Count > 0
            || _objShapes.Count > 0;

        private void raisePropertyChanged ( [CallerMemberName] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) );

            if ( propertyName == nameof ( HasChanges ) )
            {
                raisePropertyChanged ( nameof ( HasElements ) );
            }
        }

        private void onImagesChanged ( object sender , NotifyCollectionChangedEventArgs e ) => HasChanges = true;

        private void onStrokesChanged ( object sender , StrokeCollectionChangedEventArgs e ) => HasChanges = true;

        private void onShapesChanged ( object sender , NotifyCollectionChangedEventArgs e ) => HasChanges = true;
        public void UpdateContents ( Canvas playerCanvas , Canvas bgCanvas )
        {
            foreach ( Image image in playerCanvas.Children )
            {
                if(Images.FirstOrDefault(x => image.Tag?.Equals(x.UUID) ?? false ) is ImageIcon icon)
                {
                    icon.Location = new System.Windows.Point
                        (
                        Canvas.GetLeft ( image ) ,
                        Canvas.GetTop ( image )
                        );
                    icon.Size = new System.Windows.Size
                        (
                        image.Width ,
                        image.Height
                        );
                }
            }

            foreach ( FrameworkElement element in bgCanvas.Children )
            {
                if ( element is Image image )
                {
                    if ( Images.FirstOrDefault ( x => image.Tag?.Equals ( x.UUID ) ?? false ) is ImageIcon icon )
                    {
                        icon.Location = new System.Windows.Point
                            (
                            Canvas.GetLeft ( image ) ,
                            Canvas.GetTop ( image )
                            );
                        icon.Size = new System.Windows.Size
                            (
                            image.Width ,
                            image.Height
                            );
                    }
                }
                else if(element is Shape shape)
                {
                    if ( Shapes.FirstOrDefault ( x => shape.Tag?.Equals ( x.UUID ) ?? false ) is ShapeData shapeData )
                    {
                        shapeData.Left = Canvas.GetLeft ( shape );
                        shapeData.Top = Canvas.GetTop ( shape );
                        shapeData.Width = shape.Width;
                        shapeData.Height = shape.Height;
                    }
                }
            }
        }
    }
}
