using System.Collections.Generic;
using NDtw.FeatureVector;
using NDtw;

namespace NDtw.Preprocessing
{
    public interface IPreprocessor
    {
        IList<double> Preprocess(IList<double> data);
        string ToString();
    }

    public interface IPreprocessorGeneric
    {
        IList<FeatureVector<IFeatureVectorsData>> Preprocess(IList<FeatureVector<IFeatureVectorsData>> data);
        string ToString();
    }
}