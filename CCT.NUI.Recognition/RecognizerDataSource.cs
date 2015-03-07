using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CCT.NUI.Core;
using CCT.NUI.HandTracking;
using CCT.NUI.HandTracking.Trajectory;

using NDtw;
using NDtw.FeatureVector;

namespace CCT.NUI.Recognition
{
    public delegate void NewCandidatesAvailableHandler(SortedList<double, char> candidates);

    public interface IRecognizerDataSource : IDataSource<SortedList<double, char>>
    {
        event NewCandidatesAvailableHandler OnNewCandidatesAvailable;
    }
    public class RecognizerDataSource : 
        DataSourceProcessor<SortedList<double, char>, TrajectoryCollection> ,
        IRecognizerDataSource
    {
        public event NewCandidatesAvailableHandler OnNewCandidatesAvailable;

        private char[] _enabledCharset = { 'a', 'b', 'c' } ;
        private Dictionary<char, IList<IList<IFeatureVectorsData>>> _trainingData = new Dictionary<char, IList<IList<IFeatureVectorsData>>>();

        public RecognizerDataSource(ITrajectoryDataSource dataSource) :
            base(dataSource)
        {
            this.CurrentValue = new SortedList<double, char>();
            dataSource.RecognizeNewTrajectory += Training;
            dataSource.RecognizeNewTrajectory += Recognize;
        }
        private IList<IList<IFeatureVectorsData>> FeatureExtraction(IList<FingerPoint> trajectory)
        {
            IList<IList<IFeatureVectorsData>> featuresArray = new List<IList<IFeatureVectorsData>>();
            IList<IFeatureVectorsData> accFeatures = new List<IFeatureVectorsData>(),
                velFeatures = new List<IFeatureVectorsData>();
            for ( int i = 2; i < trajectory.Count; ++i )
            {
                var x1 = trajectory[i - 2];
                var x2 = trajectory[i - 1];
                var x3 = trajectory[i];

                var accFeature = new Vec3(
                    x3.X + x1.X - 2 * x2.X,
                    x3.Y + x1.Y - 2 * x2.Y,
                    x3.Z + x1.Z - 2 * x2.Z
                    );

                var velFeature = new Vec3(
                    x3.X - x2.X,
                    x3.Y - x2.Y,
                    x3.Z - x3.Z
                    );

                accFeatures.Add(accFeature);
                velFeatures.Add(velFeature);
            }
            featuresArray.Add(accFeatures);
            featuresArray.Add(velFeatures);
            return featuresArray;
        }

        protected override SortedList<double, char> Process(TrajectoryCollection sourceData)
        {
            return null;
        }

        public void Training(IList<FingerPoint> trajectory) 
        {
            for ( int i = 0; i < _enabledCharset.Length; ++i )
            {
                var featuresArrayAccVelo = FeatureExtraction(trajectory);
                _trainingData.Add(_enabledCharset[i], featuresArrayAccVelo);
            }
        }
        public void Recognize(IList<FingerPoint> trajectory)
        {
            this.CurrentValue.Clear();

            var featuresArray = FeatureExtraction(trajectory);

            for (int i = 0; i < _enabledCharset.Length; ++i)
            {
                var candidateChar = _enabledCharset[i];
                IList<IList<IFeatureVectorsData>> pivot = new List<IList<IFeatureVectorsData>>();
                _trainingData.TryGetValue(candidateChar, out pivot);
                var seriesVariableAcc = new SeriesVariable(featuresArray[0], pivot[0]);
                var seriesVariableVelo = new SeriesVariable(featuresArray[1], pivot[1]);

                SeriesVariable[] seriesVariableArray = { seriesVariableAcc, seriesVariableVelo };

                var dtw = new Dtw(seriesVariableArray, DistanceMeasure.Cosine);
                var similarity = dtw.GetCost();

                this.CurrentValue.Add(similarity, candidateChar);
            }

            if (this.OnNewCandidatesAvailable != null)
            {
                this.OnNewCandidatesAvailable(this.CurrentValue);
            }
        }
    }
}
