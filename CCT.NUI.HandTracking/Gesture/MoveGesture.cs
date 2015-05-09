using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//
using CCT.NUI.Core;
using CCT.NUI.HandTracking.Mouse;

namespace CCT.NUI.HandTracking.Gesture
{
    public class MoveGesture : IGesture
    {
        private int width, height;
        private Point? lastPointOnScreen;
        private IClickMode clickMode = new TwoFingerClickMode();
        private ICursorMode cursorMode = new FingerCursorMode();

        public MoveGesture(int w, int h)
        {
            this.width = w;
            this.height = h;
        }
        public void process(HandCollection handData, ref IGesture gestureState)
        {

            if ( ! this.cursorMode.HasPoint(handData) ) // not valid handData for MoveGesture
            {
                // 注意： 可以增加延时策略，增加鲁棒性
                gestureState = null;
            }
            else
            {
                var pointOnScreen = this.MapToScreen(this.cursorMode.GetPoint(handData));

                double newX = pointOnScreen.X;
                double newY = pointOnScreen.Y;

                if (lastPointOnScreen.HasValue)
                {
                    var distance = Point.Distance2D(pointOnScreen, lastPointOnScreen.Value);
                    if (distance < 100)
                    {
                        newX = lastPointOnScreen.Value.X + (newX - lastPointOnScreen.Value.X) * (distance / 100);
                        newY = lastPointOnScreen.Value.Y + (newY - lastPointOnScreen.Value.Y) * (distance / 100);
                    }
                    if (distance < 10)
                    {
                        newX = lastPointOnScreen.Value.X;
                        newY = lastPointOnScreen.Value.Y;
                    }
                }

                UserInput.SetCursorPositionAbsolute((int)newX, (int)newY);
                lastPointOnScreen = new Point((float)newX, (float)newY, 0);

                //this.clickMode.Process(handData);

                gestureState = this; // update state
            }
        }

        public void cleanup()
        {

        }

        private Point MapToScreen(Point point)
        {
            var originalSize = new Size(this.width, this.height);
            return new Point(-50 + (float)(point.X / originalSize.Width * (System.Windows.SystemParameters.PrimaryScreenWidth + 100)), -50 + (float)(point.Y / originalSize.Height * (System.Windows.SystemParameters.PrimaryScreenHeight + 100)), point.Z);
        }

    }
}
