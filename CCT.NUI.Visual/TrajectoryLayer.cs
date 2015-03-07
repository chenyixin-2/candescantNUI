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
            this.dataSource.NewDataAvailable += dataSource_NewDataAvailable;
        }

        public override void Paint(Graphics g)
        {

            var realtimeTrajectory = this.dataSource.CurrentValue.CurrentTrajectory;
            if ( realtimeTrajectory != null && realtimeTrajectory.Count >= 2 )
            {
                PaintTrajectory(g, yellowPen, realtimeTrajectory);

                var frontier = this.dataSource.CurrentValue.Frontier;
                var width = 11;
                var height = 11;
                g.FillEllipse(Brushes.Red, (int)frontier.X - width/2, (int)frontier.Y - height/2 , width, height);
            }
            var completeTrajectory = this.dataSource.CurrentValue.NewTrajecotry;
            if (completeTrajectory != null && completeTrajectory.Count >= 2)
            {
                PaintTrajectory(g, redPen, completeTrajectory);
            }
        }
        private void PaintTrajectory(Graphics g, Pen pen, IList<FingerPoint> trajectory)
        {
            var points = trajectory.Select(p => new System.Drawing.Point((int)p.X, (int)p.Y)).ToArray();
            g.DrawLines(pen, points);
        }

        private void dataSource_NewDataAvailable(TrajectoryCollection trajectory)
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
