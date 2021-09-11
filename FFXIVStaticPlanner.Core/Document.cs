using System.Collections.ObjectModel;
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

        public Document ( )
        {
            _objStrokes = new ( );
            _objImages = new ( );

            _objStrokes.StrokesChanged += onStrokesChanged;
            _objImages.CollectionChanged += onImagesChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string FileName
        {
            get => strFileName;
            set
            {
                strFileName = value;
                raisePropertyChanged ( );
            }
        }

        public StrokeCollection Strokes => _objStrokes;

        public ObservableCollection<ImageIcon> Images => _objImages;

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

        private void onImagesChanged ( object sender , System.Collections.Specialized.NotifyCollectionChangedEventArgs e ) => HasChanges = true;

        private void onStrokesChanged ( object sender , StrokeCollectionChangedEventArgs e ) => HasChanges = true;
    }
}
