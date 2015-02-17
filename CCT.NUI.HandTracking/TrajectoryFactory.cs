using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CCT.NUI.Core;
using CCT.NUI.HandTracking;
using CCT.NUI.HandTracking.Trajectory;

namespace CCT.NUI.HandTracking
{
    internal class TrajectoryFactory : ITrajectoryFactory
    {
        public TrajectoryFactory()
        { }
        public TrajectoryCollection Create(HandCollection hands)
        {
            return new TrajectoryCollection(); 
        }
    }
}
