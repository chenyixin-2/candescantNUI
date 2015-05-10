using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CCT.NUI.Core;
using CCT.NUI.HandTracking.Mouse;

namespace CCT.NUI.HandTracking.Gesture
{
    public enum Gestures : int
    {
        Move_Write = 1,
        Drag_Ready = 2,
        Drag_On    = 4,
        Click      = 3,
        Stop       = 5   
    };

    public interface IGesture
    {
        void process(HandCollection handData, ref IGesture gestureState);
        void cleanup();

        String Name { get; set; }
    }

    public abstract class GestureBase : IGesture
    {
        private int width, height;

        protected Point? lastPointOnScreen;
        protected IClickMode clickMode = new FingerClickMode();
        protected ICursorMode cursorMode = new FingerCursorMode();

        public GestureBase(String name, int w, int h)
        {
            this.width = w;
            this.height = h;
            this.name = name;
        }
        private String name ;
        public String Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public abstract void process(HandCollection handData, ref IGesture gestureState);
        public abstract void cleanup();

        protected Point MapToScreen(Point point)
        {
            var originalSize = new Size(this.width, this.height);
            return new Point(-50 + (float)(point.X / originalSize.Width * (System.Windows.SystemParameters.PrimaryScreenWidth + 100)), -50 + (float)(point.Y / originalSize.Height * (System.Windows.SystemParameters.PrimaryScreenHeight + 100)), point.Z);
        }
        protected void MoveToScreen(Point pointOnScreen)
        {
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
        }
    }
}
