using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CCT.NUI.HandTracking.Gesture;
namespace CCT.NUI.HandTracking.Mouse
{
    public class FingerClickMode : ClickModeBase
    {
        private bool mouseDown;
        private DateTime? firstClick;

        public override void Process(HandCollection handData)
        {
            var fingerCount = handData.Hands[0].FingerCount;
            if ( firstClick == null )
            {
                firstClick = DateTime.Now;
            }
            else if ( firstClick.HasValue )
            {
                // fps 30hz, so 1000/30 * frames to wait
                if ( DateTime.Now > firstClick.Value.AddMilliseconds(33 * 5) && !this.mouseDown )
                {
                    UserInput.MouseDown();
                    this.mouseDown = true;
                }
                else if ( DateTime.Now > firstClick.Value.AddMilliseconds(33 * 10) && this.mouseDown )
                {
                    UserInput.MouseUp();
                    this.mouseDown = false;
                }
            }
        }

        public override ClickMode EnumValue
        {
            get { return ClickMode.TwoFinger; }
        }
    }
}
