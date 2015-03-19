using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CCT.NUI.HandTracking.Trajectory;
namespace CCT.NUI.HandTracking.TrajectoryCollector
{
    public interface ITrajectoryCollector
    {
        void Collecct(Trajectory.Trajectory trajectory);
        IList<Trajectory.Trajectory> Provide();
        void Serialize();
    }
}
