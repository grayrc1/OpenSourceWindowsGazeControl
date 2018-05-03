using Karna.Magnification;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GazeToolBar
{
    public class ZoomMagnifierCentered : ZoomMagnifier
    {
        private int FORM_WIDTH { get; set; }
        private int FORM_HEIGHT { get; set;}

        public ZoomMagnifierCentered(Form displayform, Point fixationPoint) : base(displayform, fixationPoint)
        {
            FORM_WIDTH = Program.readSettings.zoomWindowSize * 100;
            FORM_HEIGHT = FORM_WIDTH / 2;
        }

        public override void UpdatePosition(Point fixationPoint)
        {
            this.FixationPoint = fixationPoint;
            Rectangle screenBounds = Screen.FromControl(form).Bounds;
            int offsetX = 0;
            offsetX = screenBounds.Left;
            MessageBox.Show(offsetX.ToString());
            form.Width = FORM_WIDTH;
            form.Height = FORM_HEIGHT;

            form.Left = offsetX + (screenBounds.Right / 2) - (form.Width / 2);
            form.Top = (screenBounds.Bottom / 2) - (form.Height / 2);

            int dX = fixationPoint.X - (form.Left + FORM_WIDTH / 2);
            int dY = fixationPoint.Y - (form.Top + FORM_HEIGHT / 2);

            Offset = new Point(dX, dY);
        }

        public override void UpdateMagnifier()
        {
            //If the magnifier is not setup correctly (will crash otherwise)
            if ((!hasInitialized) || (hwndMag == IntPtr.Zero))
            {
                return;
            }

            sourceRect = new RECT();
            Point zoomPosition = Utils.SubtractPoints(GetZoomPosition(), Offset);
            Rectangle screenBounds = Screen.FromControl(form).Bounds;

            //difference
            int offsetX = 0;
            offsetX = screenBounds.Left;
            form.Width = FORM_WIDTH;
            form.Height = FORM_HEIGHT;

            form.Left = Math.Abs(offsetX) + (screenBounds.Right / 2) - (form.Width / 2);
            form.Top =  (screenBounds.Bottom / 2) - (form.Height / 2);

            int dX = FixationPoint.X - (form.Left + FORM_WIDTH / 2);
            int dY = FixationPoint.Y - (form.Top + FORM_HEIGHT / 2);

            Offset = new Point(dX, dY);

            //

            //Magnified width and height
            int width = (int)(form.Width / Magnification);
            int height = (int)(form.Height / Magnification);

            //Zoom rectangle positionpop
            sourceRect.left = zoomPosition.X - (width / 2);
            sourceRect.top = zoomPosition.Y - (height / 2);

            int inLeft = sourceRect.left;
            int inTop = sourceRect.top;
            
            sourceRect.left = Clamp(sourceRect.left, 0, screenBounds.Width - width);
            sourceRect.top = Clamp(sourceRect.top, 0, screenBounds.Height - height);
            
            int fnLeft = sourceRect.left - inLeft;
            int fnTop = sourceRect.top - inTop;

            if (SecondaryOffset.X == 0 && SecondaryOffset.Y == 0)
            {
           //     MessageBox.Show(SecondaryOffset.X + " " + SecondaryOffset.Y + " " + inLeft + " " + fnLeft);

                SecondaryOffset = new Point(fnLeft, fnTop);
            }

            Utils.Print("SecOffset- " + SecondaryOffset);

            NativeMethods.MagSetWindowSource(hwndMag, sourceRect);  //Sets the source of the zoom
            NativeMethods.InvalidateRect(hwndMag, IntPtr.Zero, true); // Force redraw.
        }

        public override int MagnifierDivAmount()
        {
            return 1;
        }
    }
}
