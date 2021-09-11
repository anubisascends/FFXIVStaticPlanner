using FFXIVStaticPlanner.Core;
using FFXIVStaticPlanner.Data;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace FFXIVStaticPlanner.ViewModels
{
    public class RootViewModel : ViewModel
    {
        private const string c_strAnnotations = "Annotations";
        private const string c_strBackground = "Background";
        private const string c_strPlayers = "Players";

        private ImageData _objSelectedImage;
        private ICommand _addImage;
        private ICommand _refrechImages;
        private ICommand _deleteImage;
        private ICommand _saveDocument;
        private ICommand _changeColor;
        private ICommand _changeBrushSize;
        private InkCanvasEditingMode _eMode = InkCanvasEditingMode.Ink;
        private ICommand _changeEditMode;
        private ICommand _undo;
        private ICommand _redo;
        private ICommand _clearFilter;
        private bool _bAnnotate = true;
        private bool _bPlayer = false;
        private bool _bBG = false;
        private string _strFilter;
        private ICollectionView _objView;
        private string _strLayer;
        private Cursor _objCursor;

        public RootViewModel ( ) : base ( )
        {
            DrawingAttributes = new ( );
            DrawingAttributes.Color = Colors.Black;
            Document = new ( );
            Images = new ( );
            ImageManager = ManagerFactory.CreateImageManager ( );
            DocumentManager = ManagerFactory.CreateDocumentManager ( );
            OnRefreshImages ( null );
        }

        private IImageManager ImageManager
        {
            get;
        }

        private IDocumentManager DocumentManager
        {
            get;
        }

        public Cursor Cursor
        {
            get => _objCursor ??= Cursors.Arrow;
            set
            {
                _objCursor = value;
                RaisePropertyChanged ( );
            }
        }

        public string SelectedLayer
        {
            get => _strLayer ??= c_strAnnotations;
            set
            {
                _strLayer = value;
                RaisePropertyChanged ( );
                setLayer ( );
            }
        }

        public string FilterText
        {
            get => _strFilter ??= string.Empty;
            set
            {
                _strFilter = value;
                RaisePropertyChanged ( );
                updateFilter ( );
            }
        }

        public bool EnableAnnotations
        {
            get => _bAnnotate;
            set
            {
                _bAnnotate = value;
                RaisePropertyChanged ( );
            }
        }

        public bool EnablePlayers
        {
            get => _bPlayer;
            set
            {
                _bPlayer = value;
                RaisePropertyChanged ( );
            }
        }

        public bool EnableBackground
        {
            get => _bBG;
            set
            {
                _bBG = value;
                RaisePropertyChanged ( );
            }
        }

        public ImageData SelectedImage
        {
            get => _objSelectedImage;
            set
            {
                _objSelectedImage = value;
                RaisePropertyChanged ( );
            }
        }

        public ObservableCollection<ImageData> Images
        {
            get;
        }

        public ICollectionView ImageView
        {
            get => _objView;
            set
            {
                _objView = value;
                RaisePropertyChanged ( );
            }
        }

        public Document Document
        {
            get;
            private set;
        }

        public DrawingAttributes DrawingAttributes
        {
            get;
        }

        public InkCanvasEditingMode EditingMode
        {
            get => _eMode;
            set
            {
                _eMode = value;
                RaisePropertyChanged ( );
            }
        }

        public ICommand AddImage => _addImage ??= new CommandHandler ( OnAddImage , CanAlwaysExecute );

        public ICommand RefreshImages => _refrechImages ??= new CommandHandler ( OnRefreshImages , CanAlwaysExecute );

        public ICommand DeleteImage => _deleteImage ??= new CommandHandler ( OnDeleteImage , CanDeleteImage );

        public ICommand SaveDocument => _saveDocument ??= new CommandHandler ( OnSaveDocument , CanSaveDocument );

        public ICommand ChangeColor => _changeColor ??= new CommandHandler ( OnChangeColor , CanAlwaysExecute );

        public ICommand ChangeBrushSize => _changeBrushSize ??= new CommandHandler ( onChangeBrushSize , CanAlwaysExecute );

        public ICommand ChangeEditMode => _changeEditMode ??= new CommandHandler ( onChangeEditMode , CanAlwaysExecute );

        public ICommand Undo => _undo ??= new CommandHandler ( onUndo , canUndo );

        public ICommand Redo => _redo ??= new CommandHandler ( onRedo , canRedo );

        public ICommand ClearFilter => _clearFilter ??= new CommandHandler ( onClearFilter , canClearFilter );

        private void onClearFilter ( object obj ) => FilterText = string.Empty;

        private bool canClearFilter ( object obj ) => !string.IsNullOrEmpty ( FilterText );

        private void updateFilter ( ) => _objView.Filter = ( object obj ) =>
                                       {
                                           return obj is ImageData objImageData
                                               ? objImageData.Name.Contains ( FilterText, StringComparison.InvariantCultureIgnoreCase ) 
                                                    || objImageData.Group.Contains ( FilterText, StringComparison.InvariantCultureIgnoreCase )
                                               : false;
                                       };

        private void onRedo ( object obj ) => throw new System.NotImplementedException ( );

        private bool canRedo ( object obj ) => throw new System.NotImplementedException ( );

        private void onUndo ( object obj ) => throw new System.NotImplementedException ( );

        private bool canUndo ( object obj ) => throw new System.NotImplementedException ( );

        private void onChangeEditMode ( object obj )
        {
            string strMode = obj.ToString();

            EditingMode = strMode switch
            {
                "point" => InkCanvasEditingMode.EraseByPoint,
                "stroke" => InkCanvasEditingMode.EraseByStroke,
                "select" => InkCanvasEditingMode.Select,
                _ => InkCanvasEditingMode.Ink
            };

            Cursor = strMode switch
            {
                "stroke" => Cursors.Hand,
                "point" => Cursors.Hand,
                _ => Cursors.Arrow
            };
        }

        private void onChangeBrushSize ( object obj )
        {
            
        }

        private void OnChangeColor ( object obj ) => DrawingAttributes.Color = obj.ToString ( ) switch
        {
            "blue" => Colors.Blue,
            "red" => Colors.Red,
            "yellow" => Colors.Yellow,
            "green" => Colors.Green,
            _ => Colors.Black
        };

        private bool CanSaveDocument ( object obj ) => Document?.HasChanges ?? false;

        private void OnSaveDocument ( object obj )
        {
            MessageBox.Show ( "Not Implemented" );
        }

        private static bool CanAlwaysExecute ( object parameter ) => true;

        private void OnAddImage ( object parameter )
        {
            var dlgView = new OpenFileDialog
            {
                Filter = "Image Files|*.png|All Files|*.*",
                Title = "Please select the image(s) you want to add...",
                Multiselect = true
            };

            if ( dlgView.ShowDialog ( ) ?? false )
            {
                foreach ( var item in dlgView.FileNames )
                {
                    var raw  = File.ReadAllBytes(item);
                    var name = Path.GetFileNameWithoutExtension(item);
                    ImageManager.AddImage ( raw , name , name );
                }
            }

            OnRefreshImages ( null );
        }

        private void OnRefreshImages ( object parameter )
        {
            Images.Clear ( );

            foreach ( var item in ImageManager.GetAllImages ( ) )
            {
                Images.Add ( item );
            }

            _objView = CollectionViewSource.GetDefaultView ( Images );
        }

        private bool CanDeleteImage ( object parameter ) => _objSelectedImage != null;

        private void OnDeleteImage ( object parameter )
        {
            var result = MessageBox.Show ( $"Are you sure you want to remove {_objSelectedImage.Name}?  This cannot be undone!" ,
                "Confirm Delete" , MessageBoxButton.YesNo , MessageBoxImage.Warning );

            if ( result == MessageBoxResult.Yes )
            {
                ImageManager.DeleteImage ( _objSelectedImage.ID );
                Images.Remove ( _objSelectedImage );

            }
        }

        private void setLayer ( )
        {
            switch ( _strLayer )
            {
                case c_strAnnotations:
                    EnableAnnotations = true;
                    EnableBackground = false;
                    EnablePlayers = false;
                    break;
                case c_strBackground:
                    EnableAnnotations = false;
                    EnableBackground = true;
                    EnablePlayers = false;
                    break;
                default:
                    EnableAnnotations = false;
                    EnableBackground = false;
                    EnablePlayers = true;
                    break;
            }
        }
    }
}