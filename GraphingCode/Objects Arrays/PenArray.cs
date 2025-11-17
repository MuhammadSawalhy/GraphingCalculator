using System;
using System.Collections.Generic;
using Graphing.Objects;
using System.Drawing;
namespace Graphing.Objects.Arrays
{
        public class PenArray
    {
        public List<DinamicPen> DinamicPenArray = new List<DinamicPen>();
        public List<StaticPen> StaticPenArray = new List<StaticPen>();
        public Bitmap StaticPenBitmap;
        public GraphSetting GraphSetting;

        public PenArray(GraphSetting GraphSetting_)
        {
            GraphSetting = GraphSetting_;
            StaticPenBitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);
        }

        public void AddStaticPen(StaticPen pen)
        {
            StaticPenBitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            StaticPenArray.Add(pen);

            Graphics g = Graphics.FromImage(StaticPenBitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            foreach (StaticPen pen_ in StaticPenArray)
            {
                pen_.Draw();
                pen_.DrawTo(g);
            }
        }

        public void DeleteStaticPen(StaticPen pen)
        {
            StaticPenBitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            StaticPenArray.Remove(pen);
            Graphics g = Graphics.FromImage(StaticPenBitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            foreach (StaticPen pen_ in StaticPenArray)
            {
                pen_.Draw();
                pen_.DrawTo(g);
            }
        }
    }


}
