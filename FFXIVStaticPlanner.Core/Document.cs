using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Ink;

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

        private void raisePropertyChanged ( [CallerMemberName] string propertyName = null ) => PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) );

        private void onImagesChanged ( object sender , NotifyCollectionChangedEventArgs e ) => HasChanges = true;

        private void onStrokesChanged ( object sender , StrokeCollectionChangedEventArgs e ) => HasChanges = true;

        private void onShapesChanged ( object sender , NotifyCollectionChangedEventArgs e ) => HasChanges = true;
    }
}
