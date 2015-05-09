using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CCT.NUI.HandTracking.Gesture
{
    public interface IGesture
    {
        void process(HandCollection handData, ref IGesture gestureState);
        void cleanup();
    }
}
