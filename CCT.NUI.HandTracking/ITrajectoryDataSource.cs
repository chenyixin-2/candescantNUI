using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CCT.NUI.Core;
using CCT.NUI.HandTracking.Trajectory;

namespace CCT.NUI.HandTracking
{
    public delegate void NewTrajectoryHandler(List<FingerPoint> data);

    public interface ITrajectoryDataSource : IDataSource<TrajectoryCollection>
    {
        event NewTrajectoryHandler RecognizeNewTrajectory;
    }
}
