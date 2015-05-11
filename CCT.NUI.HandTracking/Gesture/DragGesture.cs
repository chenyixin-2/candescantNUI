using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CCT.NUI.Core;
using CCT.NUI.HandTracking.Mouse; // class UserInput

namespace CCT.NUI.HandTracking.Gesture
{
    internal enum DragState : int { None = 0, Ready = 1, Dragging = 2 } ;
    public class DragGesture : GestureBase
    {
        private DragState state;
        private bool mouseDown;
        public DragGesture(int w, int h):
            base("Drag", w, h)
        {
            this.ResetStateVariables();
        }

        public override void process(HandCollection handData, ref IGesture gestureState)
        {
            var fingerCount = handData.Hands.First().FingerCount;
            
            if ( fingerCount != (int)Gestures.Drag_On && fingerCount != (int)Gestures.Drag_Ready )
            {
                if ( !this.InAbnormal() )
                {
                    this.BeginAbnormal();
                }
                else // in abnormal
                {
                    if (this.TimeToQuitGesture())
                        gestureState = null;
                }
            }
            else // normal(valid) input
            {
                if ( this.InAbnormal() )
                {
                    this.LeaveAbnormal();
                }

                gestureState = this;
                var t = Point.Zero;

                if ( fingerCount == (int)Gestures.Drag_Ready ) // drag ready
                {
                    if (this.mouseDown && this.state == DragState.Dragging) // dragging -> dragging ready
                    {
                        this.Name = "Dragging... ... ...MouseUp";

                        UserInput.MouseUp();
                        this.mouseDown = false;
                    }
                    else
                    {
                        this.Name = "Drag... ... ...Ready";

                        var fingers = handData.Hands.First().FingerPoints;
                        var f1 = fingers[0].Location;
                        var f2 = fingers[1].Location;
                        t = Point.Center(f1, f2);
                    }

                    this.state = DragState.Ready;
                }
                else if ( fingerCount == (int)Gestures.Drag_On ) // dragging
                {
                    if ( this.state == DragState.Ready )
                    {
                        this.Name = "Drag... ... ...";
                        if (!this.mouseDown)
                        {
                            UserInput.MouseDown();
                            this.mouseDown = true;
                        }

                        this.state = DragState.Dragging;

                    }
                    if ( this.state == DragState.Dragging )
                    {
                        var fingers = handData.Hands.First().FingerPoints;
                        t = fingers[0].Location; // use first finger
                    }
                }

                if ( ! Point.IsZero(t) )
                {
                    var pointOnScreen = this.MapToScreen(t);
                    this.MoveToScreen(pointOnScreen);
                }
            }
        }
          
        public override void cleanup()
        {
            if (this.mouseDown)   //  critical 
                UserInput.MouseUp();

            if (this.InAbnormal())
                this.LeaveAbnormal();

            this.ResetStateVariables();
        }
        private void ResetStateVariables()
        {
            this.mouseDown = false;
            this.state = DragState.None;
        }
    }
}
