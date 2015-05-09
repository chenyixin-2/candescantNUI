using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            
            if ( fingerCount != (int)Gestures.Drag_On || fingerCount != (int)Gestures.Drag_Ready )
            {
                gestureState = null;
            }
            else // 
            {
                if ( fingerCount == (int)Gestures.Drag_Ready ) // drag ready
                {

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
