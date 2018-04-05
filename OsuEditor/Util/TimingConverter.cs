using System.Collections.Generic;
using OsuEditor.Models;
using OsuParser.Structures;

namespace OsuEditor.Util
{
    public static class TimingConverter
    {
        public static List<TimingPoint> TimingMarkListToTimingPointList(List<TimingMark> mark)
        {
            var result = new List<TimingPoint>();
            return result;
        }

        public static List<TimingMark> TimingPointListToTimingMarkList(List<TimingPoint> point)
        {
            var result = new List<TimingMark>();
            return result;
        }

        public static Timing TimingMarkListToTiming(List<TimingMark> marks)
        {
            var result = new Timing();
            return result;
        }

        public static Timing TimingPointListToTiming(List<TimingPoint> points)
        {
            var result = new Timing();
            return result;
        }
    }
}
