﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using OsuEditor.Models;
using OsuEditor.Models.Timings;
using OsuParser.Structures;

namespace OsuEditor.Util
{
    public static class TimingConverter
    {
        public static List<TimingPoint> TimingMarkListToTimingPointList(ObservableCollection<TimingMark> mark, out int preview, out List<Bookmark> bookmarks)
        {
            var result = new List<TimingPoint>();
            bookmarks = new List<Bookmark>();
            preview = -1;

            TimingMark prevMark = null;

            foreach (var cur in mark)
            {
                if (cur.Preview)
                    preview = cur.Offset;
                if (cur.BookMarkChange)
                    bookmarks.Add(new Bookmark(cur.Offset, cur.Bookmark));

                if (cur.MeasureChange || cur.SpeedChange || cur.HitSoundChange)
                {
                    if (prevMark == null || Math.Abs(cur.Bpm - prevMark.Bpm) > 0.01)
                        result.Add(new TimingPoint
                        {
                            Offset = cur.Offset,
                            MsPerBeat = BpmConverter.BpmToMilliseconds(cur.Bpm, 1),
                            Meter = cur.Measure,
                            SampleSet = (int) cur.HitSound + 1,
                            SampleIndex = 0,
                            Volume = cur.Volume,
                            Inherited = true,
                            Kiai = cur.Kiai
                        });
                    if (cur.SpeedRate != 100 || !cur.SpeedChange)
                        result.Add(new TimingPoint
                        {
                            Offset = cur.Offset,
                            MsPerBeat = (double) -10000 / cur.SpeedRate,
                            Meter = cur.Measure,
                            SampleSet = (int) cur.HitSound + 1,
                            SampleIndex = 0,
                            Volume = cur.Volume,
                            Inherited = false,
                            Kiai = cur.Kiai
                        });
                }

                prevMark = cur;
            }

            return result;
        }

        public static ObservableCollection<TimingMark> TimingPointListToTimingMarkList(List<TimingPoint> point, int preview, List<Bookmark> bookmarks)
        {
            var result = new ObservableCollection<TimingMark>();

            TimingPoint prevPoint = null;
            TimingPoint secondPrevPoint = null;
            double prevBpm = -1;
            foreach (var cur in point)
            {
                if (cur.Inherited)
                {
                    result.Add(new TimingMark
                    {
                        Offset = (int)Math.Round(cur.Offset),
                        SpeedChange = true,
                        NewBase = true,
                        Bpm = Math.Round(BpmConverter.MillisecondsToBpm(cur.MsPerBeat, 1), 2),
                        SpeedRate = 100,
                        MeasureChange = prevPoint == null || prevPoint.Meter != cur.Meter,
                        Measure = cur.Meter,
                        HitSoundChange = prevPoint == null || prevPoint.SampleSet != cur.SampleSet || prevPoint.Volume != cur.Volume,
                        HitSound = (DefaultHitSound)(cur.SampleSet - 1),
                        Volume = cur.Volume,
                        Kiai = cur.Kiai,
                        Preview = false,
                        BookMarkChange = false
                    });

                    prevBpm = Math.Round(BpmConverter.MillisecondsToBpm(cur.MsPerBeat, 1), 2);
                }
                else
                {
                    Debug.Assert(prevPoint != null, nameof(prevPoint) + " != null");
                    Debug.Assert(Math.Abs(prevBpm + 1) > 0.001, nameof(prevBpm) + " != -1");
                    if (Math.Abs(prevPoint.Offset - cur.Offset) < 0.001)
                    {
                        var lastPoint = result.Last();
                        lastPoint.SpeedRate = (int) Math.Round(-10000 / cur.MsPerBeat);
                        lastPoint.MeasureChange = secondPrevPoint == null || secondPrevPoint.Meter != cur.Meter;
                        lastPoint.HitSoundChange = secondPrevPoint == null ||
                                                   secondPrevPoint.SampleSet != cur.SampleSet ||
                                                   secondPrevPoint.Volume != cur.Volume;
                        lastPoint.HitSound = (DefaultHitSound) (cur.SampleSet - 1);
                        lastPoint.Volume = cur.Volume;
                        lastPoint.Kiai = cur.Kiai;
                    }
                    else
                    {
                        result.Add(new TimingMark
                        {
                            Offset = (int) Math.Round(cur.Offset),
                            SpeedChange = true,
                            NewBase = false,
                            Bpm = prevBpm,
                            SpeedRate = (int) Math.Round(-10000 / cur.MsPerBeat),
                            MeasureChange = prevPoint.Meter != cur.Meter,
                            Measure = cur.Meter,
                            HitSoundChange = prevPoint.SampleSet != cur.SampleSet || prevPoint.Volume != cur.Volume,
                            HitSound = (DefaultHitSound) (cur.SampleSet - 1),
                            Volume = cur.Volume,
                            Kiai = cur.Kiai,
                            Preview = false,
                            BookMarkChange = false
                        });
                    }
                }

                secondPrevPoint = prevPoint;
                prevPoint = cur;
            }

            //  Add preview point
            if (preview >= 0)
            {
                for (var i = 0; i < result.Count; i++)
                {
                    if (result[i].Offset < preview)
                        continue;

                    if (result[i].Offset == preview)
                    {
                        result[i].Preview = true;
                        break;
                    }

                    result.Insert(i, new TimingMark(result[i - 1])
                    {
                        Offset = preview,
                        SpeedChange = false,
                        NewBase = false,
                        HitSoundChange = false,
                        MeasureChange = false,
                        Preview = true,
                        BookMarkChange = false
                    });
                    break;
                }
            }

            //  Add bookmark points
            foreach (var cur in bookmarks)
            {
                for (var i = 0; i < result.Count; i++)
                {
                    if (result[i].Offset < cur.Offset)
                        continue;

                    if (result[i].Offset == cur.Offset)
                    {
                        result[i].BookMarkChange = true;
                        result[i].Bookmark = cur.Memo;
                        break;
                    }

                    result.Insert(i, new TimingMark(result[i - 1])
                    {
                        Offset = cur.Offset,
                        SpeedChange = false,
                        NewBase = false,
                        HitSoundChange = false,
                        MeasureChange = false,
                        Preview = false,
                        BookMarkChange = true,
                        Bookmark = cur.Memo
                    });
                    break;
                }
            }

            return result;
        }

        public static Timing TimingMarkListToTiming(ObservableCollection<TimingMark> marks)
        {
            var result = new Timing();
            Period curKiai = null;

            foreach (var cur in marks)
            {
                if (cur.Preview)
                    result.PreviewPoint = cur.Offset;
                if (cur.BookMarkChange)
                    result.Bookmarks.Add(new Bookmark(cur.Offset, cur.Bookmark));
                if (cur.SpeedChange)
                    result.SvPoints.Add(new SvPoint(cur.Offset, cur.Bpm, cur.SpeedRate));

                if (cur.NewBase || cur.MeasureChange)
                {
                    result.Offset.Add(cur.Offset);
                    result.BeatLength.Add(BpmConverter.BpmToBeat(cur.Bpm));
                    result.BeatsPerMeasure.Add(cur.Measure);
                }

                if (cur.Kiai)
                    if (curKiai == null)
                        curKiai = new Period {StartTime = cur.Offset};
                if(!cur.Kiai)
                    if (curKiai != null && curKiai.EndTime == -1)
                    {
                        curKiai.EndTime = cur.Offset;
                        result.KiaiPeriods.Add(new Period(curKiai));
                        curKiai = null;
                    }
            }

            return result;
        }
    }
}
