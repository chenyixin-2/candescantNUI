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
        public TrajectoryFactory(TrajectoryCollection data)
        {
            this.trajectoryCollection = data;
        }
        public TrajectoryCollection Create(HandCollection hands)
        {
            var trajectoryCollection = this.trajectoryCollection;
            if (hands.HandsDetected)
            {
                foreach (var hand in hands.Hands)
                {
                    if (hand.FingerPoints.Count >= 3)  // Gesture Delimeter encounts
                    {
                        if ( trajectoryCollection.CurrentTrajectoryLength >= 3 )
                        {
                            trajectoryCollection.AddNewTrajectory();
                        }
                    } 
                    else if (hand.FingerPoints.Count < 3 && hand.FingerPoints.Count >= 1 )
                    {
                        trajectoryCollection.AddSamplePoint(hand.FingerPoints[0]);
                    } // finger found
                    break; // only one hand
                }
            }
            return trajectoryCollection; 
        }
    }
}
