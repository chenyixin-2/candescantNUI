﻿using System;
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
                gestureState = this;

                var pointOnScreen = this.MapToScreen(this.cursorMode.GetPoint(handData));

                this.MoveToScreen(pointOnScreen);
            }
        }

        public  override void cleanup()
        {

        }
    }
}
