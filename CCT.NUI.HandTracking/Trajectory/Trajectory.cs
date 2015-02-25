using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CCT.NUI.HandTracking;
namespace CCT.NUI.HandTracking.Trajectory
{
    public class TrajectoryCollection
    {
        private bool bNewTrajectoryAvailabe;

        public bool NewTrajectoryToProcess
        {
            get { return this.bNewTrajectoryAvailabe; }
            set { this.bNewTrajectoryAvailabe = value; }
        }

        private FingerPoint frontier;
        private IList<FingerPoint> trajectory;
        public TrajectoryCollection()
        {
            this.frontier = null;
            this.trajectory = new List<FingerPoint>();
        }
        public TrajectoryCollection(IList<FingerPoint> trajectory)
        {
            this.trajectory = trajectory;
        }
        public FingerPoint Frontier
        {
            get { return this.frontier; }
            set { this.frontier = value; }
        }
        public IList<FingerPoint> Trajecotry
        {
            get { return this.trajectory; }
        }
        public int Count
        {
            get { return this.trajectory.Count; }
        }

        public bool TrajectoryExists
        {
            get { return this.Count > 0; }
        }
        public bool IsEmpty
        {
            get { return this.Count == 0; }
        }
    }
}
