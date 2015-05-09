using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CCT.NUI.Core;
using CCT.NUI.Core.OpenNI;

using CCT.NUI.HandTracking.Gesture;

namespace CCT.NUI.HandTracking.Mouse
{
    public enum ClickMode { TwoFinger = 0, SecondHand = 1, Hand = 2 }
    public enum CursorMode { Finger = 0, CenterOfHand = 1, CenterOfCluster = 2, HandTracking = 3}

    public class MouseController : IDisposable
    {
        private IList<IGesture> gestureList;
        private IGesture gestureState;

        private IHandDataSource handSource;
        //private Point? lastPointOnScreen;
        private IClickMode clickMode = new TwoFingerClickMode();
        private ICursorMode cursorMode = new FingerCursorMode();
        private TrackingClusterDataSource trackingClusterDataSource;

        public MouseController(IHandDataSource handSource)
        {
            this.handSource = handSource;
            this.handSource.NewDataAvailable += new NewDataHandler<HandCollection>(handSource_NewDataAvailable);
            this.gestureState = null;
            this.gestureList = null;
        }

        public MouseController(IHandDataSource handSource, bool enabled,
            IList<IGesture> gestList)
            : this(handSource)
        {
            this.Enabled = enabled;
            this.gestureState = null;
            this.gestureList = gestList;
        }

        public MouseController(IHandDataSource handSource, TrackingClusterDataSource trackingClusterDataSource)
            : this(handSource)
        {
            this.trackingClusterDataSource = trackingClusterDataSource;
        }

        public bool Enabled { get; set; }

        public void Dispose()
        {
            this.Enabled = false;
            this.handSource.NewDataAvailable -= new NewDataHandler<HandCollection>(handSource_NewDataAvailable);
        }

        public CursorMode CursorMode
        {
            get { return this.cursorMode.EnumValue; }
        }

        public ClickMode ClickMode
        {
            get { return this.clickMode.EnumValue; }
        }

        public void SetCursorMode(CursorMode mode)
        {
            switch (mode)
            {
                case CursorMode.Finger:
                    this.cursorMode = new FingerCursorMode();
                    break;
                case CursorMode.CenterOfHand:
                    this.cursorMode = new CenterOfHandCursorMode();
                    break;
                case CursorMode.CenterOfCluster:
                    this.cursorMode = new CenterOfClusterCursorMode();
                    break;
                case CursorMode.HandTracking:
                    this.cursorMode = new HandTrackingCursorMode(this.trackingClusterDataSource);
                    break;
            }
        }

        public void SetClickMode(ClickMode mode)
        {
            switch (mode)
            {
                case ClickMode.TwoFinger:
                    this.clickMode = new TwoFingerClickMode();
                    break;
                case ClickMode.SecondHand:
                    this.clickMode = new SecondHandClickMode();
                    break;
                case ClickMode.Hand:
                    this.clickMode = new HandClickMode();
                    break;
            }
        }

        void handSource_NewDataAvailable(HandCollection handData)
        {
            if (!this.Enabled || handData.Count == 0) // 判断 手势 的 数量
            {
                return;
            }

            var g = this.gestureState;
            if ( g != null )  // operating some gestures
            {
                this.gestureState.process(handData, ref g);
            }
            else // g == null
            {
                foreach ( var gest in this.gestureList )
                {
                    gest.process(handData, ref g);
                    if (g != null)
                        break;
                }
            }

            this.gestureState = g; // update new state

            if ( g != null )
            {
                foreach ( var gest in this.gestureList )
                {
                    gest.cleanup();
                }
            }
        }
    }
}
