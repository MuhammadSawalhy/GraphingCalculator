using System;
using System.Collections.Generic;
using Graphing.Objects;


namespace Graphing.Objects.Arrays
{
    public class SliderArray:ObjectArray<Slider>
    {
        public override void Clear()
        {
            Items.Clear();
        }
        public override int Count => Items.Count;
        public override Slider[] ToArray()
        {
            return Items.ToArray();
        }
        public override List<Slider> ToList()
        {
            return Items;
        }
        public override void AddRange(Slider[] Item)
        {
            Items.AddRange(Item);
        }
        public override Slider Item(int index)
        {
            return Items[index];
        }



        public override void Add(Slider Slider)
        {
            Items.Add(Slider);
        }

        public override void Delete(Slider Slider, bool RemoveControl)
        {
            Slider.GraphSetting.UpdateSliderTimerState(Slider);
            Slider.GraphSetting.RemoveByName(Slider.Name, RemoveControl);
            Items.Remove(Slider);
            Slider.SliderControl.Parent.Controls.Remove(Slider.SliderControl);
            Slider.GraphSetting.Sketch.Draw();
            Slider.GraphSetting.Sketch.SketchControl.Draw();
        }
    }

}
