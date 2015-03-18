using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
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
        private void datasource_NewCandidateAvailable()
        {
            this.OnRequestRefresh();
        }

        public override void Paint(System.Drawing.Graphics g)
        {
            var candidates = dataSource.CurrentValue;
            var displayString = "";
            foreach (var item in candidates)
            {
                displayString += item.Value.ToString() + " : " + item.Key.ToString() + "\n";
            }
            Font font1 = new Font("Arial", 20);
            Brush yellowBrush = Brushes.Yellow;
            g.DrawString(displayString, font1, yellowBrush, new PointF(0, 0));
        }
        public override void Dispose()
        {
            base.Dispose();
            this.dataSource.OnNewCandidatesAvailable -= datasource_NewCandidateAvailable;
        }
    }
}
