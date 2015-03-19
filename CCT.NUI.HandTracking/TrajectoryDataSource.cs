using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CCT.NUI.Core;
using CCT.NUI.HandTracking.Trajectory;
using CCT.NUI.HandTracking.TrajectoryCollector;

namespace CCT.NUI.HandTracking
{
    public class MockTrajectoryDataSource : DataSourceProcessor<TrajectoryCollection, HandCollection>, ITrajectoryDataSource
    {
        //private ITrajectoryCollector _collector;
        private IList<Trajectory.Trajectory> _trajectorySet;
        public MockTrajectoryDataSource(ITrajectoryCollector collector)
            : base(null)
        {
            _trajectorySet = collector.Provide();
        }
        protected override unsafe TrajectoryCollection Process(HandCollection hands)
        {
            return CurrentValue;
        }
        public void StartMocking()
        {
            int candidateIdx = 0;
            while ( this.NewTrajectoryAvailable != null )
            {
                this.NewTrajectoryAvailable(_trajectorySet[candidateIdx]);
                candidateIdx++;
            }
        }
        public event NewTrajectoryHandler NewTrajectoryAvailable;
    }

    public class TrajectoryDataSource : DataSourceProcessor<TrajectoryCollection, HandCollection>, ITrajectoryDataSource
    {
        private ITrajectoryFactory factory;

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
