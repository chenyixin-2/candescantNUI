using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CCT.NUI.Core;
using CCT.NUI.HandTracking.Trajectory;

namespace CCT.NUI.HandTracking
{
    public class TrajectoryDataSource : DataSourceProcessor<TrajectoryCollection, HandCollection>, ITrajectoryDataSource
    {
        private TrajectoryFactory factory;

        public TrajectoryDataSource(IHandDataSource handDataSource)
            :base(handDataSource)
        {
            this.CurrentValue = new TrajectoryCollection();
            this.factory = new TrajectoryFactory(this.CurrentValue);
        }

        public event NewTrajectoryHandler NewTrajectoryAvailable;
        protected override unsafe TrajectoryCollection Process(HandCollection hands)
        {
            var processedData = this.factory.Create(hands);
            var newTraj = processedData.NewTrajectory;
            if (newTraj != null && this.NewTrajectoryAvailable != null)
            {
                this.NewTrajectoryAvailable(newTraj);
            }
            return processedData;
        }
    }
}
