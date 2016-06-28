
using System.Windows;

namespace Cortex.Util
{
    public class Mouse
    {

        public static Point GetMousePositionWinForms()
        {
            System.Drawing.Point point = System.Windows.Forms.Control.MousePosition;
            return new Point(point.X, point.Y);
        }

        public static Point? GetMousePositionScreenWpf(Window window)
        {
            Point? point = null;
            var presentationSource = PresentationSource.FromVisual(window);
            if (presentationSource != null)
            {
                if (presentationSource.CompositionTarget != null)
                {
                    var transform = presentationSource.CompositionTarget.TransformFromDevice;
                    point = transform.Transform(GetMousePositionWinForms());
                }
            }
            return point;
        }
    }
}
