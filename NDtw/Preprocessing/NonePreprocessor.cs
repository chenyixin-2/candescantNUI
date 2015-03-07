using System.Linq;
using System.Collections.Generic;

namespace NDtw.Preprocessing
{
    public class NonePreprocessor : IPreprocessor
    {
        public IList<double> Preprocess(IList<double> data)
        {
            return data;
        }

        public override string ToString()
        {
            return "None";
        }
    }
}
