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

        public TrajectoryLayer(ITrajectoryDataSource dataSource)
        {
            this.dataSource = dataSource;
            this.dataSource.NewDataAvailable += dataSource_NewDataAvailable;
        }

        public override void Paint(Graphics g)
        {
        }

        private void dataSource_NewDataAvailable(TrajectoryCollection trajectory)
        {
            this.OnRequestRefresh();
        }

        public override void Dispose()
        {
            base.Dispose();
            this.dataSource.NewDataAvailable -= dataSource_NewDataAvailable;
        }
    }
}
