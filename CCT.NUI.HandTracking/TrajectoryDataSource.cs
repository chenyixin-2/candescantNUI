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

        public event NewTrajectoryHandler NewTrajectoryAvailable;
        protected override unsafe TrajectoryCollection Process(HandCollection hands)
        {
            //var ret = null;
            if ( hands.Count > 0 )
            {
                var newTrajectoryCollection = this.factory.Create(hands);
                var newTraj = newTrajectoryCollection.NewTrajectory;
                if (newTraj != null && this.NewTrajectoryAvailable != null)
                {
                    this.NewTrajectoryAvailable(newTraj);
                    newTrajectoryCollection.NewTrajectory = null;
                }
                return newTrajectoryCollection;
            }
            else
            {
                return null;
            }
        }
    }
}
