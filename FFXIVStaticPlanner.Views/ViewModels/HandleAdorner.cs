using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace FFXIVStaticPlanner.Views.ViewModels
{
    internal class HandleAdorner : Adorner
    {
        const double _dOpacity = .2;
        const double _dPenThick = 1.5;
        const double _dCircleRadius = 10;

        private EllipseGeometry[] _objCorners = new EllipseGeometry[4];
        private int selectedIndex = -1;

        public HandleAdorner ( UIElement adornedElement ) : base ( adornedElement )
        {
            
        }

        protected override void OnMouseEnter ( MouseEventArgs e )
        {
            base.OnMouseEnter ( e );

            int iIndex = -1;

            for ( var i = 0; i < _objCorners.Length; i++ )
            {
                var corner = _objCorners[i];

                if ( corner.Bounds.Contains ( e.GetPosition ( AdornedElement ) ) )
                {
                    iIndex = i;
                    break;
                }
            }

            Cursor = iIndex % 2 == 0 ? Cursors.SizeNWSE : Cursors.SizeNESW;
        }

        protected override void OnPreviewMouseDown ( MouseButtonEventArgs e )
        {
            base.OnPreviewMouseDown ( e );

            for ( int i = 0; i < 4; i++ )
            {
                var corner = _objCorners[i];

                if ( corner.Bounds.Contains ( e.GetPosition ( AdornedElement ) ) )
                {
                    selectedIndex = i;
                    break;
                }
            }
        }

        protected override void OnPreviewMouseUp ( MouseButtonEventArgs e )
        {
            base.OnPreviewMouseUp ( e );
            selectedIndex = -1;
        }

        protected override void OnMouseLeave ( MouseEventArgs e )
        {
            base.OnMouseLeave ( e );
            selectedIndex = -1;
        }

        protected override void OnPreviewMouseMove ( MouseEventArgs e )
        {
            base.OnPreviewMouseMove ( e );

            if ( selectedIndex == -1 )
            {
                return;
            }

            var corner = _objCorners[selectedIndex];
            var element = (AdornedElement as FrameworkElement);
            var canvas = VisualTreeHelper.GetParent(AdornedElement) as Canvas;
            Point p = e.GetPosition(canvas);
            Point c = canvas.TranslatePoint(corner.Center, AdornedElement);
            var parent = element.Parent as Canvas;

            double left = Canvas.GetLeft(AdornedElement);
            double top = Canvas.GetTop(AdornedElement);
            double width = element.Width;
            double height = element.Height;

            double topAdj = p.Y + c.Y;
            double leftAdj = p.X + c.X;

            Vector v = new Vector(p.X - c.X, p.Y - c.Y);

            switch ( selectedIndex )
            {
                case 0:
                    top += topAdj;
                    height -= topAdj;
                    left += leftAdj;
                    width -= leftAdj;
                    break;
                case 1:
                    top += topAdj;
                    height -= topAdj;
                    width -= leftAdj;
                    break;
                case 2:
                    height -= topAdj;
                    width -= leftAdj;
                    break;
                default:
                    left += leftAdj;
                    width -= leftAdj;
                    height -= topAdj;
                    break;
            }

            Canvas.SetLeft ( AdornedElement , Math.Abs(left) );
            Canvas.SetTop ( AdornedElement , Math.Abs(top) );
            element.Width = Math.Abs(width);
            element.Height = Math.Abs(height);
        }

        protected override void OnRender ( DrawingContext drawingContext )
        {
            var adornedElementRect = new Rect(AdornedElement.DesiredSize);

            // Some arbitrary drawing implements.
            var renderBrush = new SolidColorBrush ( Colors.Green )
            {
                Opacity = _dOpacity
            };
            var renderPen = new Pen(new SolidColorBrush(Colors.Navy), _dPenThick);

            // create the four ellipses
            _objCorners[0] = new EllipseGeometry ( adornedElementRect.TopLeft , _dCircleRadius , _dCircleRadius );
            _objCorners[1] = new EllipseGeometry ( adornedElementRect.TopRight , _dCircleRadius , _dCircleRadius );
            _objCorners[2] = new EllipseGeometry ( adornedElementRect.BottomRight , _dCircleRadius , _dCircleRadius );
            _objCorners[3] = new EllipseGeometry ( adornedElementRect.BottomLeft , _dCircleRadius , _dCircleRadius );

            // Draw a circle at each corner.
            drawingContext.DrawEllipse ( renderBrush , renderPen , _objCorners[0].Center , _dCircleRadius, _dCircleRadius );
            drawingContext.DrawEllipse ( renderBrush , renderPen , _objCorners[1].Center , _dCircleRadius, _dCircleRadius );
            drawingContext.DrawEllipse ( renderBrush , renderPen , _objCorners[2].Center , _dCircleRadius, _dCircleRadius );
            drawingContext.DrawEllipse ( renderBrush , renderPen , _objCorners[3].Center , _dCircleRadius, _dCircleRadius );
        }
    }
}
