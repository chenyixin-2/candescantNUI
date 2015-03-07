using System.Linq;
using System.Collections.Generic;

namespace NDtw.Preprocessing
{
    public class CentralizationPreprocessor : IPreprocessor
    {
        public IList<double> Preprocess(IList<double> data)
        {
            var avg = data.Average();
            return data.Select(x => x - avg).ToArray();
        }

        public override string ToString()
        {
            return "Centralization";
        }
    }
}
