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
        private Pen yellowPen = new Pen(Brushes.Yellow, 3);

        public TrajectoryLayer(ITrajectoryDataSource dataSource)
        {
            this.dataSource = dataSource;
            this.dataSource.NewDataAvailable += dataSource_NewDataAvailable;
        }

        public override void Paint(Graphics g)
        {
            var trajectory = this.dataSource.CurrentValue.Trajecotry;
            if (trajectory.Count >= 2)
            {
                PaintTrajectory(g, trajectory);

                var frontier = this.dataSource.CurrentValue.Frontier;
                var width = 11;
                var height = 11;
                g.FillEllipse(Brushes.Red, (int)frontier.X - width/2, (int)frontier.Y - height/2 , width, height);
            }
        }
        private void PaintTrajectory(Graphics g, IList<FingerPoint> trajectory)
        {
            var points = trajectory.Select(p => new System.Drawing.Point((int)p.X, (int)p.Y)).ToArray();
            g.DrawLines(yellowPen, points);
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
