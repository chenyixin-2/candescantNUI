using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CCT.NUI.HandTracking;
namespace CCT.NUI.HandTracking.Trajectory
{
    public class TrajectoryCollection
    {
        private FingerPoint frontier;
        private List<FingerPoint> trajectory;
        private List<List<FingerPoint>> trajectorySet;
        private List<FingerPoint> newTrajectory;

        public TrajectoryCollection()
        {
            this.newTrajectory = null;
            this.frontier = null;
            this.trajectory = new List<FingerPoint>();
            this.trajectorySet = new List<List<FingerPoint>>();
        }
        public FingerPoint Frontier
        {
            get { return this.frontier; }
            set { this.frontier = value; }
        }
        public List<FingerPoint> CurrentTrajectory
        {
            get { return this.trajectory; }
        }
        public List<List<FingerPoint>> TrajectorySet
        {
            get { return this.trajectorySet; }
        }
        public List<FingerPoint> NewTrajecotry
        {
            get { return this.newTrajectory; }
            set { this.newTrajectory = value; }
        }   
        public void AddTrajectory(List<FingerPoint> trajectory)
        {
            this.trajectorySet.Add(trajectory);
            this.NewTrajecotry = trajectory;
        }
        public void AddSamplePoint(FingerPoint pt)
        {
            this.CurrentTrajectory.Add(pt);
            this.Frontier = pt;
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
