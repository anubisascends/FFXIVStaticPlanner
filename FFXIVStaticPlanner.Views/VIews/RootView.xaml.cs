using FFXIVStaticPlanner.Core;
using FFXIVStaticPlanner.Data;
using FFXIVStaticPlanner.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
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

        public RootView ( ) => InitializeComponent ( );

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
            var model = DataContext as RootViewModel;
            var dropItem = e.Data.GetData(typeof(ImageData)) as ImageData;
            var image = new ImageIcon
            {
                Display = dropItem.Display,
                Id = dropItem.ID,
                Location = p,
                Scale = new Size(100,100)
            };

            model.Document.Images.Add ( image );
            var imageSize = 96;
            var displayImage = new Image
            {
                Source = dropItem.Source,
                Width = imageSize,
                Height = imageSize
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
            Canvas.SetZIndex ( displayImage , model.Document.Images.Count );

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
                _objCurrentImage = null;
            }
        }

        private void onImageMouseMove ( object sender , MouseEventArgs e )
        {
            if ( _bImageMove )
            {
                var p = new Point(0,0);
                var model = DataContext as RootViewModel;

                if ( model.EnablePlayers )
                {
                    p = e.GetPosition ( playerCanvas );
                }
                else if ( model.EnableBackground )
                {
                    p = e.GetPosition ( bgCanvas );
                }

                var image = _objCurrentImage;

                Canvas.SetLeft ( image , p.X - (image.Width / 2) );
                Canvas.SetTop ( image , p.Y - (image.Height / 2) );
            }
        }

        private void onImageMouseDown ( object sender , MouseButtonEventArgs e )
        {
            var parent = _objCurrentImage.Parent as Canvas;

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

    }
}