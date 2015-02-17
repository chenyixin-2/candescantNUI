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
            this.factory = new TrajectoryFactory();
            this.CurrentValue = new TrajectoryCollection();
        }

        protected override unsafe TrajectoryCollection Process(HandCollection shapeData)
        {
            return this.factory.Create(shapeData);
        }
    }
}
