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
        public int Length
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
        public void Reset()
        {
            this.trajectory.Clear();
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
        private Trajectory currentTrajectory;
        private bool bNewTrajectory = false;

        public TrajectoryCollection()
        {
            this.currentTrajectory = new Trajectory();
            this.trajectorySet = new List<Trajectory>();
        }

        public Trajectory NewTrajectory
        {
            get 
            {
                if ( bNewTrajectory )
                {
                    return this.trajectorySet[trajectorySet.Count - 1]; 
                }
                else
                {
                    return null;
                }
            }
        }
        public IList<Trajectory> TrajectorySet
        {
            get { return this.trajectorySet; }
        }
        public void ResetNewTrajectory()
        {
            this.bNewTrajectory = false;
        }
        public void AddNewTrajectory()
        {
            this.trajectorySet.Add(this.currentTrajectory);
            this.bNewTrajectory = true;
            this.currentTrajectory = new Trajectory();
        }
        public void AddSamplePoint(FingerPoint pt)
        {
            this.currentTrajectory.Add(pt);
            this.currentTrajectory.Frontier = pt;
        }
        public int NewTrajectoryLength
        {
            get 
            {
                var newtraj = this.NewTrajectory;
                if (newtraj != null)
                    return newtraj.Length;
                else
                    return 0;
            }
        }
        public int CurrentTrajectoryLength
        {
            get { return this.currentTrajectory.Length; }
        }
    }
}
