using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CCT.NUI.HandTracking.Mouse;

namespace CCT.NUI.HandTracking.Gesture
{
    public class ClickGesture : GestureBase
    {
        public ClickGesture(int w , int h):
            base("Click", w, h)
        {
        }

        public override void process(HandCollection handData, ref IGesture gestureState)
        {
            var fingerCount = handData.Hands.First().FingerCount;
            
            if ( fingerCount != (int)Gestures.Click )
            {
                gestureState = null;
            }
            else if ( fingerCount == (int)Gestures.Click )
            {
                gestureState = this;

                this.clickMode.Process(handData);
            }
        }

        public override void cleanup()
        {
            this.clickMode = new FingerClickMode();
        }
    }
}
