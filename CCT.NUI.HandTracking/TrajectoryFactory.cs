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
        private TrajectoryCollection currentTrajectory;
        public TrajectoryFactory()
        {
            this.currentTrajectory = new TrajectoryCollection();
        }
        public TrajectoryCollection Create(HandCollection hands)
        {
            var trajectory = this.currentTrajectory;
            if (hands.HandsDetected)
            {
                foreach (var hand in hands.Hands)
                {
                    if (hand.FingerPoints.Count >= 1)
                    {
                        if (hand.FingerPoints.Count >= 3)
                        {
                            trajectory.Trajecotry.Clear();
                        }
                        var newPoint = hand.FingerPoints[0];
                        trajectory.Trajecotry.Add(newPoint);
                        trajectory.Frontier = newPoint;
                    } // finger found
                    break; // only one hand
                }
            }
            return trajectory; 
        }
    }
}
