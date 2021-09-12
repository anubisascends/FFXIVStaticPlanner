using FFXIVStaticPlanner.Core;
using FFXIVStaticPlanner.Data;
using FFXIVStaticPlanner.Views;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace FFXIVStaticPlanner.ViewModels
{
    public class RootViewModel : ViewModel<RootView>
    {
        private const string c_strPoint = "point";
        private const string c_strStroke = "stroke";
        private const string c_strSelect = "select";

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
        private Cursor _objCursor;
        private ICommand _drawRect;
        private ICommand _drawEllip;
        private ICommand _openDoc;
        private bool _bEnableDrag;
        private bool _bImageMove;
        private Image _objCurrentImage;
        private Point _startPoint;
        private int _iClickCount;
        private bool _bDraw;
        public Shape _shape;
        private bool _bShapeMove;
        private double _dImageSize = 96;
        private Layers _eLayer = Layers.Annotations;

        public RootViewModel ( ) 
        {
            DrawingAttributes = new ( );
            DrawingAttributes.Color = Colors.Black;
            Document = new ( );
            Images = new ( );
            ImageManager = ManagerFactory.CreateImageManager ( );
            DocumentManager = ManagerFactory.CreateDocumentManager ( );
            onRefreshImages ( null );

            Window.lbxImages.PreviewMouseDown += onListboxMouseDown;
            Window.lbxImages.PreviewMouseMove += onListboxMouseMove;
            Window.lbxImages.PreviewMouseUp += onListboxMouseUp;

            Window.bgCanvas.Drop += onCanvasDrop;
            Window.playerCanvas.Drop += onCanvasDrop;
            Window.inkCanvas.Drop += onCanvasDrop;

            Window.bgCanvas.PreviewMouseDown += onBgCanvasMouseDown;
            Window.bgCanvas.PreviewMouseMove += onBgCanvasMouseMove;
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

        public bool DrawShape
        {
            get; set;
        } = false;

        public Layers SelectedLayer
        {
            get => _eLayer;
            set
            {
                _eLayer = value;
                RaisePropertyChanged ( );
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

        public int ShapeIndex
        {
            get; set;
        } = 1;

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

        public ICommand AddImage => _addImage ??= new CommandHandler ( onAddImage , canAlwaysExecute );

        public ICommand RefreshImages => _refrechImages ??= new CommandHandler ( onRefreshImages , canAlwaysExecute );

        public ICommand DeleteImage => _deleteImage ??= new CommandHandler ( onDeleteImage , canDeleteImage );

        public ICommand SaveDocument => _saveDocument ??= new CommandHandler ( onSaveDocument , canSaveDocument );

        public ICommand ChangeColor => _changeColor ??= new CommandHandler ( onChangeColor , canAlwaysExecute );

        public ICommand ChangeBrushSize => _changeBrushSize ??= new CommandHandler ( onChangeBrushSize , canAlwaysExecute );

        public ICommand ChangeEditMode => _changeEditMode ??= new CommandHandler ( onChangeEditMode , canAlwaysExecute );

        public ICommand Undo => _undo ??= new CommandHandler ( onUndo , canUndo );

        public ICommand Redo => _redo ??= new CommandHandler ( onRedo , canRedo );

        public ICommand ClearFilter => _clearFilter ??= new CommandHandler ( onClearFilter , canClearFilter );

        public ICommand DrawRectangle => _drawRect ??= new CommandHandler ( onDrawRect , canDrawShape );

        public ICommand DrawEllipse => _drawEllip ??= new CommandHandler ( onDrawEllipse , canDrawShape );

        public ICommand OpenDocument => _openDoc ??= new CommandHandler ( onOpenDocument , canAlwaysExecute );

        private void onOpenDocument ( object obj )
        {
            var dlgOpen = new OpenFileDialog
            {
                Title = "Please select a file to open...",
                Filter = "Document (*.xml)|*.xml|All Files (*.*)|*.*"
            };

            if ( dlgOpen.ShowDialog ( ) ?? false )
            {
                Document = DocumentManager.LoadDocument ( dlgOpen.FileName );
                Window.bgCanvas.Children.Clear ( );

                // we have to rebind the strokes since we created a new document
                // ----------------------------------------------------------
                var strokeBinding = new Binding
                {
                    Source = Document.Strokes
                };
                Window.inkCanvas.SetBinding ( InkCanvas.StrokesProperty , strokeBinding );
                // ----------------------------------------------------------

                // add shapes to the canvas
                foreach ( var item in Document.Shapes )
                {
                    Shape shape = item.ShapeType == 1 
                        ? new Rectangle() 
                        : new Ellipse();

                    shape.Fill = new SolidColorBrush ( item.Color );
                    shape.Stroke = Brushes.Black;
                    shape.Width = item.Width;
                    shape.Height = item.Height;
                    shape.Tag = item.UUID;
                    shape.PreviewMouseUp += onShapeMouseUp;
                    shape.PreviewMouseMove += onShapeMouseMove;
                    shape.PreviewMouseDown += onShapeMouseDown;

                    Window.bgCanvas.Children.Add ( shape );

                    Canvas.SetLeft ( shape , item.Left );
                    Canvas.SetTop ( shape , item.Top );
                }

                // add images to the canvas(es)
                foreach ( var item in Document.Images )
                {
                    var image = Images.FirstOrDefault(x => x.ID == item.Id);

                    if ( image != null )
                    {
                        Image img = new Image
                        {
                            Source = image.Source,
                            Width = _dImageSize,
                            Height = _dImageSize,
                            Tag = item.UUID
                        };

                        img.PreviewMouseDown += onImageMouseDown;
                        img.PreviewMouseMove += onImageMouseMove;
                        img.PreviewMouseUp += onImageMouseUp;

                        switch ( item.Canvas )
                        {
                            case 2:
                                Window.bgCanvas.Children.Add ( img );
                                break;
                            default:
                                Window.playerCanvas.Children.Add ( img );
                                break;
                        }

                        Canvas.SetLeft ( img , item.Location.X );
                        Canvas.SetTop ( img , item.Location.Y );
                    }
                }

            }
        }

        private void onDrawEllipse ( object obj )
        {
            ShapeIndex = 2;
            Cursor = Cursors.Cross;
            DrawShape = true;
        }

        private void onDrawRect ( object obj )
        {
            ShapeIndex = 1;
            Cursor = Cursors.Cross;
            DrawShape = true;
        }

        private bool canDrawShape ( object obj ) => SelectedLayer == Layers.Background;

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
                c_strPoint => InkCanvasEditingMode.EraseByPoint,
                c_strStroke => InkCanvasEditingMode.EraseByStroke,
                c_strSelect => InkCanvasEditingMode.Select,
                _ => InkCanvasEditingMode.Ink
            };

            Cursor = strMode switch
            {
                c_strStroke => Cursors.Hand,
                c_strPoint => Cursors.Hand,
                _ => Cursors.Arrow
            };
        }

        private void onChangeBrushSize ( object obj )
        {
            
        }

        private void onChangeColor ( object obj ) => DrawingAttributes.Color = obj.ToString ( ) switch
        {
            "blue" => Colors.Blue,
            "red" => Colors.Red,
            "yellow" => Colors.Yellow,
            "green" => Colors.Green,
            "gray" => Colors.Gray,
            _ => Colors.Black
        };

        private bool canSaveDocument ( object obj ) => Document?.HasChanges ?? false;

        private void onSaveDocument ( object obj )
        {
            string strFileName = string.Empty;

            if ( string.IsNullOrEmpty ( strFileName = Document.FileName ) )
            {
                var savDlg = new SaveFileDialog
                {
                    Title = "Please select a place to save this document to...",
                    Filter = "Document File (*.xml)|*.xml",
                    AddExtension = true,
                    ValidateNames = true
                };

                if ( savDlg.ShowDialog ( ) ?? false )
                {
                    strFileName = savDlg.FileName;
                }
                else
                {
                    return;
                }
            }

            DocumentManager.SaveDocument ( strFileName , Document );
        }

        private bool canAlwaysExecute ( object parameter ) => true;

        private void onAddImage ( object parameter )
        {
            var dlgView = new OpenFileDialog
            {
                Filter = "Image Files|*.png;*.svg|All Files|*.*",
                Title = "Please select the image(s) you want to add...",
                Multiselect = true
            };

            if ( dlgView.ShowDialog ( ) ?? false )
            {
                foreach ( var item in dlgView.FileNames )
                {
                    var name = Path.GetFileNameWithoutExtension(item);

                    switch ( Path.GetExtension(item) )
                    {
                        case ".svg":
                            ImageManager.AddImage ( File.ReadAllText(item) , name , name );
                            break;
                        default:
                            ImageManager.AddImage ( File.ReadAllBytes(item) , name , name );
                            break;
                    }
                }
            }

            onRefreshImages ( null );
        }

        private void onRefreshImages ( object parameter )
        {
            Images.Clear ( );

            foreach ( var item in ImageManager.GetAllImages ( ) )
            {
                Images.Add ( item );
            }

            _objView = CollectionViewSource.GetDefaultView ( Images );
        }

        private bool canDeleteImage ( object parameter ) => _objSelectedImage != null;

        private void onDeleteImage ( object parameter )
        {
            var result = MessageBox.Show ( $"Are you sure you want to remove {_objSelectedImage.Name}?  This cannot be undone!" ,
                "Confirm Delete" , MessageBoxButton.YesNo , MessageBoxImage.Warning );

            if ( result == MessageBoxResult.Yes )
            {
                ImageManager.DeleteImage ( _objSelectedImage.ID );
                Images.Remove ( _objSelectedImage );

            }
        }

        private void onListboxMouseDown ( object sender , MouseButtonEventArgs e )
        {
            _bEnableDrag = true;
        }

        private void onListboxMouseMove ( object sender , MouseEventArgs e )
        {
            if ( _bEnableDrag )
            {
                if ( sender is ListBox listBox )
                {
                    if ( listBox.SelectedItem is ImageData imageData )
                    {
                        DragDrop.DoDragDrop ( listBox , imageData , DragDropEffects.Link );
                        listBox.SelectedIndex = -1;
                    }
                }
            }
        }

        private void onListboxMouseUp ( object sender , MouseButtonEventArgs e )
        {
            _bEnableDrag = false;
        }

        private void onCanvasDrop ( object sender , DragEventArgs e )
        {
            var p = e.GetPosition(sender as IInputElement);
            var dropItem = e.Data.GetData(typeof(ImageData)) as ImageData;
            var window = Window as RootView;
            var image = new ImageIcon
            {
                Display = dropItem.Display,
                Id = dropItem.ID,
                Location = p,
                Scale = new Size(100,100),
                UUID = Guid.NewGuid()
            };

            Document.Images.Add ( image );
            var displayImage = new Image
            {
                Source = dropItem.Source,
                Width = _dImageSize,
                Height = _dImageSize,
                Tag = image.UUID
            };

            if ( EnablePlayers || EnableAnnotations )
            {
                window.playerCanvas.Children.Add ( displayImage );
                image.Canvas = 1;
            }
            else if ( EnableBackground )
            {
                window.bgCanvas.Children.Add ( displayImage );
                image.Canvas = 2;
            }

            Canvas.SetLeft ( displayImage , image.Location.X - (displayImage.Width / 2) );
            Canvas.SetTop ( displayImage , image.Location.Y - (displayImage.Height / 2) );

            _bEnableDrag = false;

            displayImage.PreviewMouseDown += onImageMouseDown;
            displayImage.PreviewMouseMove += onImageMouseMove;
            displayImage.PreviewMouseUp += onImageMouseUp;
        }

        private void onImageMouseUp ( object sender , MouseButtonEventArgs e )
        {
            if ( _bImageMove )
            {
                _bImageMove = false;

                if ( Document.Images.FirstOrDefault ( x => x.UUID == ( Guid )_objCurrentImage.Tag ) is ImageIcon imageIcon )
                {
                    imageIcon.Location = new Point
                        (
                        Canvas.GetLeft ( _objCurrentImage ) ,
                        Canvas.GetTop ( _objCurrentImage )
                        );
                }

                _objCurrentImage = null;
            }
        }

        private void onImageMouseMove ( object sender , MouseEventArgs e )
        {
            if ( _bImageMove )
            {
                var p = new Point(0,0);

                if ( SelectedLayer == Layers.Players )
                {
                    p = e.GetPosition ( Window.playerCanvas );
                }
                else if ( SelectedLayer == Layers.Background )
                {
                    p = e.GetPosition ( Window.bgCanvas );
                }

                double width = _objCurrentImage.Width / 2;
                double height = _objCurrentImage.Height / 2;

                if ( p.X > width )
                {
                    Canvas.SetLeft ( _objCurrentImage , p.X - width );
                }

                if ( p.Y > height )
                {
                    Canvas.SetTop ( _objCurrentImage , p.Y - height );
                }
            }
        }

        private void onImageMouseDown ( object sender , MouseButtonEventArgs e )
        {
            _objCurrentImage = sender as Image;
            var parent = _objCurrentImage?.Parent as Canvas;

            if ( parent.Cursor.Equals ( Cursors.Hand ) )
            {
                parent.Children.Remove ( _objCurrentImage );
                Document.Images.Remove ( x => x.UUID.Equals ( _objCurrentImage.Tag ) );
            }
            else
            {
                _objCurrentImage = sender as Image;
                _bImageMove = true;
            }
        }

        private void onBgCanvasMouseDown ( object sender , MouseButtonEventArgs e )
        {
            _startPoint = e.GetPosition ( Window.bgCanvas );

            if ( !DrawShape )
            {
                return;
            }

            if ( ++_iClickCount == 1 )
            {
                _bDraw = true;
                _shape = ShapeIndex == 1
                    ? new Rectangle
                    {
                        Fill = new SolidColorBrush ( DrawingAttributes.Color ) ,
                        Stroke = Brushes.Black
                    }
                    : new Ellipse
                    {
                        Fill = new SolidColorBrush ( DrawingAttributes.Color ) ,
                        Stroke = Brushes.Black
                    };

                _shape.PreviewMouseDown += onShapeMouseDown;
                _shape.PreviewMouseMove += onShapeMouseMove;
                _shape.PreviewMouseUp += onShapeMouseUp;

                Window.bgCanvas.Children.Add ( _shape );

                Canvas.SetLeft ( _shape , _startPoint.X );
                Canvas.SetTop ( _shape , _startPoint.Y );

                ShapeData shapeData = new ShapeData
                {
                    ShapeType = ShapeIndex ,
                    Width = _shape.Width ,
                    Height = _shape.Height ,
                    Left = _startPoint.X ,
                    Top = _startPoint.Y,
                    UUID = Guid.NewGuid(),
                    Color = ((SolidColorBrush)_shape.Fill).Color
                };

                _shape.Tag = shapeData.UUID;
                Document.Shapes.Add ( shapeData );
            }
            else
            {
                _bDraw = false;
                _iClickCount = 0;
                DrawShape = false;
                Cursor = Cursors.Arrow;
            }
        }

        private void onBgCanvasMouseMove ( object sender , MouseEventArgs e )
        {
            if ( _bDraw )   // we can draw something
            {
                Point p = e.GetPosition(Window.bgCanvas);  // this is the current end point that we will use

                if ( _shape != null )   // this prevents errors, just in case
                {
                    if ( Keyboard.Modifiers.HasFlag ( ModifierKeys.Shift ) )    // this makes the drawn shape a square/circle
                    {
                        double dX = p.X;
                        double dY = p.Y;

                        if ( dX > dY )
                        {
                            _shape.Width = Math.Abs ( dX - _startPoint.X );
                            _shape.Height = Math.Abs ( dX - _startPoint.X );
                        }
                        else if ( dY > dX )
                        {
                            _shape.Width = Math.Abs ( dY - _startPoint.X );
                            _shape.Height = Math.Abs ( dY - _startPoint.X );
                        }
                        else
                        {
                            _shape.Width = Math.Abs ( dX - _startPoint.X );
                            _shape.Height = Math.Abs ( dY - _startPoint.X );
                        }
                    }
                    else        // otherwise it's a rect/ellipse
                    {
                        _shape.Width = Math.Abs ( p.X - _startPoint.X );
                        _shape.Height = Math.Abs ( p.Y - _startPoint.Y );
                    }

                    // we need to get the last shape data that was added,
                    // and adjust the width based on the most recent
                    // sizing of the shapedata
                    // ----------------------------------------------
                    var shape = Document.Shapes.FirstOrDefault(x => _shape.Tag?.Equals(x.UUID) ?? false);

                    if ( shape != null )
                    {
                        shape.Width = _shape.Width;
                        shape.Height = _shape.Height;
                    }
                    // ----------------------------------------------
                }
            }
        }

        private void onShapeMouseUp ( object sender , MouseButtonEventArgs e )
        {
            if ( _bShapeMove )
            {
                _bShapeMove = false;

                var shape = Document.Shapes.FirstOrDefault(x => _shape.Tag?.Equals(x.UUID) ?? false);

                if ( shape != null )
                {
                    shape.Left = Canvas.GetLeft ( _shape );
                    shape.Top = Canvas.GetTop ( _shape );
                }
            }
        }

        private void onShapeMouseMove ( object sender , MouseEventArgs e )
        {
            if ( _bShapeMove )
            {
                Point p = e.GetPosition ( Window.bgCanvas );

                double width = _shape.Width / 2;
                double height = _shape.Height / 2;

                if ( p.X > width )
                {
                    Canvas.SetLeft ( _shape , p.X - width );
                }

                if ( p.Y > height )
                {
                    Canvas.SetTop ( _shape , p.Y - height );
                }
            }
        }

        private void onShapeMouseDown ( object sender , MouseButtonEventArgs e )
        {
            // this solves a problem when attempting to draw a shape
            // would select the new shape and try to redraw it, but do this
            // wierd move AND size...
            if ( _bDraw )
            {
                return;
            }

            _shape = sender as Shape;

            if ( Cursor.Equals ( Cursors.Hand ) )
            {
                Window.bgCanvas.Children.Remove ( _shape );
                _shape = null;
                Document.Shapes.Remove ( x => _shape.Tag?.Equals ( x.UUID ) ?? false );

                return;
            }

            _bShapeMove = true;
        }
    }
}