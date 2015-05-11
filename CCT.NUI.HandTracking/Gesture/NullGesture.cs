using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.HandTracking.Gesture
{
    public class NullGesture : GestureBase
    {
        public NullGesture(int w = 0, int h = 0):
            base("Null++++++++++++++", w, h)
        {

        }

        public override void process(HandCollection handData, ref IGesture gestureState)
        {
            if ( handData.Hands.Count != (int)Gestures.Null )
            {
                if (!this.InAbnormal())
                {
                    this.BeginAbnormal();
                }
                else // in abnormal
                {
                    if (this.TimeToQuitGesture())
                        gestureState = null;
                }
            }
            else
            {
                if (this.InAbnormal())
                {
                    this.LeaveAbnormal();
                }

                gestureState = this;
            }
        }

        public override void cleanup()
        {
            if (this.InAbnormal())
                this.LeaveAbnormal();
        }
    }
}
