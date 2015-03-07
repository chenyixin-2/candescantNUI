using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CCT.NUI.Recognition;

namespace CCT.NUI.Visual
{
    public class RecognitionLayer : LayerBase
    {
        private IRecognizerDataSource dataSource;
        public RecognitionLayer(IRecognizerDataSource dataSource)
        {
            this.dataSource = dataSource;
            this.dataSource.OnNewCandidatesAvailable += this.datasource_NewCandidateAvailable;
        }
        private void datasource_NewCandidateAvailable(SortedList<double, char> candidates)
        {
            this.OnRequestRefresh();
        }

        public override void Paint(System.Drawing.Graphics g)
        {
            throw new NotImplementedException();
        }
        public override void Dispose()
        {
            base.Dispose();
            this.dataSource.OnNewCandidatesAvailable -= datasource_NewCandidateAvailable;
        }
    }
}
