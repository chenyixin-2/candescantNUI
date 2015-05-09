using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CCT.NUI.Core;

namespace CCT.NUI.HandTracking.Gesture
{
    public class DragGesture : GestureBase
    {
        public DragGesture(int w, int h):
            base("Drag", w, h)
        {
            
        }

        public override void process(HandCollection handData, ref IGesture gestureState)
        {
            var fingerCount = handData.Hands.First().FingerCount;
            
            if ( fingerCount != (int)Gestures.Drag_On && fingerCount != (int)Gestures.Drag_Ready )
            {
                gestureState = null;
            }
            else // 
            {
                gestureState = this;

                if ( fingerCount == (int)Gestures.Drag_Ready ) // drag ready
                {
                    var fingers = handData.Hands.First().FingerPoints;
                    var f1 = fingers[0].Location;
                    var f2 = fingers[1].Location;
                    var t = Point.Center(f1, f2);

                    var pointOnScreen = this.MapToScreen(t);
                    this.MoveToScreen(pointOnScreen);
                }
                else if ( fingerCount == (int)Gestures.Drag_On ) // dragging
                {

                }
                else
                {
                    gestureState = null;
                }
            }
        }
          
        public override void cleanup()
        {

        }
    }
}
