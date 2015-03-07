using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDtw.Preprocessing;
using NDtw.FeatureVector;

namespace NDtw
{
    public class SeriesVariable
    {
        private readonly IList<FeatureVector<IFeatureVectorsData>> _x;
        private readonly IList<FeatureVector<IFeatureVectorsData>> _y;
        private readonly string _variableName;
        private readonly IPreprocessorGeneric _preprocessor;
        private readonly double _weight;

        public SeriesVariable(IList<IFeatureVectorsData> x, IList<IFeatureVectorsData> y, string variableName = null, IPreprocessorGeneric preprocessor = null, double weight = 1)
        {
            _x = new List<FeatureVector<IFeatureVectorsData>>();
            _y = new List<FeatureVector<IFeatureVectorsData>>();
            foreach (var item in x)
            {
                var feature = new FeatureVector<IFeatureVectorsData>(item);
                _x.Add(feature);
            }
            foreach (var item in y)
            {
                var feature = new FeatureVector<IFeatureVectorsData>(item);
                _y.Add(feature);
            }
            _variableName = variableName;
            _preprocessor = preprocessor;
            _weight = weight;
        }

        public string VariableName
        {
            get { return _variableName; }
        }

        public double Weight
        {
            get { return _weight; }
        }

        public IList<FeatureVector<IFeatureVectorsData>> OriginalXSeries
        {
            get { return _x; }
        }

        public IList<FeatureVector<IFeatureVectorsData>> OriginalYSeries
        {
            get { return _y; }
        }

        public IList<FeatureVector<IFeatureVectorsData>> GetPreprocessedXSeries()
        {
            if (_preprocessor == null)
                return _x;

            return _preprocessor.Preprocess(_x);
        }

        public IList<FeatureVector<IFeatureVectorsData>> GetPreprocessedYSeries()
        {
            if (_preprocessor == null)
                return _y;

            return _preprocessor.Preprocess(_y);
        }
    }
}
