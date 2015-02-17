using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CCT.NUI.HandTracking.Trajectory;

namespace CCT.NUI.HandTracking
{
    public interface ITrajectoryFactory
    {
        TrajectoryCollection Create(HandCollection hands);
    }
}
