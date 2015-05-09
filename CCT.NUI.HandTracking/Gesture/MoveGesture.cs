using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//
using CCT.NUI.Core;
using CCT.NUI.HandTracking.Mouse;

namespace CCT.NUI.HandTracking.Gesture
{
    public class MoveGesture : GestureBase
    {

        public MoveGesture(int w, int h):
            base("Move", w, h)
        {
        }

        public override void process(HandCollection handData, ref IGesture gestureState)
        {
            var fingerCount = handData.Hands.First().FingerCount;

            if ( fingerCount != (int)Gestures.Move_Write ) // not valid handData for MoveGesture
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

                gestureState = this;
            }
        }

        public  override void cleanup()
        {

        }
    }
}
