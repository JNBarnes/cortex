
using System.Windows;

namespace Cortex.Util
{
    public class Mouse
    {

        public static System.Windows.Point GetMousePositionWinForms()
        {
            System.Drawing.Point point = System.Windows.Forms.Control.MousePosition;
            return new System.Windows.Point(point.X, point.Y);
        }

        public static System.Windows.Point? GetMousePositionScreenWpf(Window window)
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
