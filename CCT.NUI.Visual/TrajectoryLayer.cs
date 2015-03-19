using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using CCT.NUI.HandTracking;

using CCT.NUI.HandTracking.Trajectory;

namespace CCT.NUI.Visual
{
    public class TrajectoryLayer : LayerBase
    {
        private ITrajectoryDataSource dataSource;
        private Pen yellowPen = new Pen(Brushes.Yellow, 3),
            redPen = new Pen(Brushes.Red, 3);

        public TrajectoryLayer(ITrajectoryDataSource dataSource)
        {
            this.dataSource = dataSource;
            //this.dataSource.NewDataAvailable += dataSource_NewDataAvailable;
            this.dataSource.NewTrajectoryAvailable += dataSource_NewTrajectoryAvailable;
        }

        public override void Paint(Graphics g)
        {
            if (this.dataSource.CurrentValue.NewTrajectory != null && this.dataSource.CurrentValue.NewTrajectoryLength >= 2)
            {
                var realtimeTrajectory = this.dataSource.CurrentValue.NewTrajectory;
                PaintTrajectory(g, yellowPen, realtimeTrajectory);

                var frontier = realtimeTrajectory.Frontier;
                var width = 11;
                var height = 11;
                g.FillEllipse(Brushes.Red, (int)frontier.X - width/2, (int)frontier.Y - height/2 , width, height);

                this.dataSource.CurrentValue.ResetNewTrajectory();
            }
            //var completeTrajectory = this.dataSource.CurrentValue.NewTrajecotry;
            //if (completeTrajectory != null && completeTrajectory.Count >= 2)
            //{
            //    PaintTrajectory(g, redPen, completeTrajectory);
            //}
        }
        private void PaintTrajectory(Graphics g, Pen pen, Trajectory trajectory)
        {
            g.DrawLines(pen, trajectory.Points);
        }

        private void dataSource_NewDataAvailable(TrajectoryCollection trajectoryCollection)
        {
            this.OnRequestRefresh();
        }
        private void dataSource_NewTrajectoryAvailable(Trajectory trajectory)
        {
            this.OnRequestRefresh();
        }
        public override void Dispose()
        {
            base.Dispose();
            this.dataSource.NewDataAvailable -= dataSource_NewDataAvailable;

            yellowPen.Dispose();
        }
    }
}
