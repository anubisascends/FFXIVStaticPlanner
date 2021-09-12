using FFXIVStaticPlanner.Core;
using FFXIVStaticPlanner.Data;
using FFXIVStaticPlanner.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace FFXIVStaticPlanner.Views
{
    /// <summary>
    /// Interaction logic for RootView.xaml
    /// </summary>
    internal partial class RootView : RibbonWindow
    {
        private bool _bEnableDrag;
        private bool _bImageMove;
        private Image _objCurrentImage;
        private int _iClickCount;
        private Point _startPoint;
        private bool _bDraw;
        private bool _bShapeMove;
        private Shape _shape;

        public RootView ( ) => InitializeComponent ( );

        private RootViewModel getModel ( ) => DataContext as RootViewModel;

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
            var model = getModel();
            var dropItem = e.Data.GetData(typeof(ImageData)) as ImageData;
            var image = new ImageIcon
            {
                Display = dropItem.Display,
                Id = dropItem.ID,
                Location = p,
                Scale = new Size(100,100),
                UUID = Guid.NewGuid()
            };

            model.Document.Images.Add ( image );
            var imageSize = 96;
            var displayImage = new Image
            {
                Source = dropItem.Source,
                Width = imageSize,
                Height = imageSize,
                Tag = image.UUID
            };

            if ( model.EnablePlayers || model.EnableAnnotations )
            {
                playerCanvas.Children.Add ( displayImage );
            }
            else if ( model.EnableBackground )
            {
                bgCanvas.Children.Add ( displayImage );
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
                var model = getModel();

                if ( model.Document.Images.FirstOrDefault ( x => x.UUID == ( Guid )_objCurrentImage.Tag ) is ImageIcon imageIcon )
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
                var model = getModel();
                var p = new Point(0,0);

                if ( model.EnablePlayers )
                {
                    p = e.GetPosition ( playerCanvas );
                }
                else if ( model.EnableBackground )
                {
                    p = e.GetPosition ( bgCanvas );
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
            }
            else
            {
                _objCurrentImage = sender as Image;
                _bImageMove = true;
            }
        }

        private void onBgCanvasMouseDown ( object sender , MouseButtonEventArgs e )
        {
            var model = getModel();
            _startPoint = e.GetPosition ( bgCanvas );

            if ( !model.DrawShape )
            {
                return;
            }

            if ( ++_iClickCount == 1 )
            {
                _bDraw = true;
                _shape = model.ShapeIndex == 1
                    ? new Rectangle
                    {
                        Fill = new SolidColorBrush ( model.DrawingAttributes.Color ) ,
                        Stroke = Brushes.Black
                    }
                    : new Ellipse
                    {
                        Fill = new SolidColorBrush ( model.DrawingAttributes.Color ) ,
                        Stroke = Brushes.Black
                    };

                _shape.PreviewMouseDown += onShapeMouseDown;
                _shape.PreviewMouseMove += onShapeMouseMove;
                _shape.PreviewMouseUp += onShapeMouseUp;

                bgCanvas.Children.Add ( _shape );

                Canvas.SetLeft ( _shape , _startPoint.X );
                Canvas.SetTop ( _shape , _startPoint.Y );

                ShapeData shapeData = new ShapeData
                {
                    ShapeType = model.ShapeIndex ,
                    Width = _shape.Width ,
                    Height = _shape.Height ,
                    Left = _startPoint.X ,
                    Top = _startPoint.Y,
                    UUID = Guid.NewGuid(),
                    Color = ((SolidColorBrush)_shape.Fill).Color
                };

                _shape.Tag = shapeData.UUID;
                model.Document.Shapes.Add ( shapeData );
            }
            else
            {
                _bDraw = false;
                _iClickCount = 0;
                model.DrawShape = false;
                model.Cursor = Cursors.Arrow;
            }
        }

        private void bgCanvas_PreviewMouseMove ( object sender , MouseEventArgs e )
        {
            if ( _bDraw )   // we can draw something
            {
                Point p = e.GetPosition(bgCanvas);  // this is the current end point that we will use

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
                    var model = getModel();
                    var shape = model.Document.Shapes.FirstOrDefault(x => _shape.Tag?.Equals(x.UUID) ?? false);

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

                var model = getModel();
                var shape = model.Document.Shapes.FirstOrDefault(x => _shape.Tag?.Equals(x.UUID) ?? false);

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
                Point p = e.GetPosition ( bgCanvas );

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

            var model = getModel();
            _shape = sender as Shape;

            if ( model.Cursor.Equals ( Cursors.Hand ) )
            {
                var shapedata = model.Document.Shapes.FirstOrDefault( x => _shape.Tag?.Equals(x.UUID)?? false);

                if ( shapedata != null )
                {
                    model.Document.Shapes.Remove ( shapedata );
                    bgCanvas.Children.Remove ( _shape );
                    _shape = null;
                }

                return;
            }

            _bShapeMove = true;
        }
    }
}