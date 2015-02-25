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
        private TrajectoryCollection trajectoryCollection;
        public TrajectoryFactory()
        {
            this.trajectoryCollection = new TrajectoryCollection();
        }
        public TrajectoryCollection Create(HandCollection hands)
        {
            var trajectory = this.trajectoryCollection;
            if (hands.HandsDetected)
            {
                foreach (var hand in hands.Hands)
                {
                    if (hand.FingerPoints.Count >= 3)
                    {
                        if (this.trajectoryCollection.Count >= 2)
                        {
                            trajectory.AddTrajectory(trajectory.CurrentTrajectory);
                            trajectory.CurrentTrajectory.Clear();
                        }
                    } else if (hand.FingerPoints.Count < 3 && hand.FingerPoints.Count >= 1 )
                    {
                        var newPoint = hand.FingerPoints[0];
                        trajectory.AddSamplePoint(newPoint);
                    } // finger found
                    break; // only one hand
                }
            }
            return trajectory; 
        }
    }
}
