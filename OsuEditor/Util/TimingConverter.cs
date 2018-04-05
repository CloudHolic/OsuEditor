using System.Collections.Generic;
using OsuEditor.Models;
using OsuParser.Structures;

namespace OsuEditor.Util
{
    public static class TimingConverter
    {
        public static List<TimingPoint> TimingMarkListToTimingPointList(List<TimingMark> mark, out int preview, out List<Bookmark> bookmarks)
        {
            var result = new List<TimingPoint>();
            preview = -1;
            bookmarks = new List<Bookmark>();

            return result;
        }

        public static List<TimingMark> TimingPointListToTimingMarkList(List<TimingPoint> point, int preview, List<Bookmark> bookmarks)
        {
            var result = new List<TimingMark>();
            return result;
        }

        public static Timing TimingMarkListToTiming(List<TimingMark> marks)
        {
            var result = new Timing();
            return result;
        }

        public static Timing TimingPointListToTiming(List<TimingPoint> points, int preview, List<Bookmark> bookmarks)
        {
            var result = new Timing();
            return result;
        }
    }
}
