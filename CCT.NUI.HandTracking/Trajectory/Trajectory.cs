using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

using CCT.NUI.HandTracking;

namespace CCT.NUI.HandTracking.Trajectory
{
    public class Trajectory
    {
        private List<FingerPoint> trajectory;
        private FingerPoint frontier;

        public Trajectory()
        {
            this.frontier = null;
            this.trajectory = new List<FingerPoint>();
        }
        public int Count
        {
            get { return trajectory.Count; }
        }
        public Point[] Points
        {
            get { return this.trajectory.Select(p => new Point((int)p.X, (int)p.Y)).ToArray(); }
        }
        public FingerPoint Frontier
        {
            get { return this.frontier; }
            set { this.frontier = value; }
        }
        public void Add(FingerPoint pt)
        {
            this.trajectory.Add(pt);
        }

        // we make a indexer here
        public FingerPoint this [int idx]
        {
            get
            {
                if ( idx >= this.trajectory.Count || idx < 0 )
                {
                    return null;
                }
                else 
                    return this.trajectory[idx];
            }
            set
            {
                if ( !(idx >= this.trajectory.Count || idx < 0) )
                    this.trajectory[idx] = value;
            }

        }
    }
    public class TrajectoryCollection
    {
        private IList<Trajectory> trajectorySet;
        private Trajectory newTrajectory;

        public TrajectoryCollection()
        {
            this.newTrajectory = null;
            this.trajectorySet = new List<Trajectory>();
        }

        public Trajectory NewTrajectory
        {
            get { return this.newTrajectory; }
            set { this.newTrajectory = value; }
        }
        public IList<Trajectory> TrajectorySet
        {
            get { return this.trajectorySet; }
        }
        public void AddTrajectory(Trajectory trajectory)
        {
            this.trajectorySet.Add(trajectory);
        }
        public void AddSamplePoint(FingerPoint pt)
        {
            this.NewTrajectory.Add(pt);
            this.NewTrajectory.Frontier = pt;
        }
        public int NewTrajectoryLength
        {
            get { return this.newTrajectory.Count; }
        }
    }
}
