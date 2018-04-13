using OsuEditor.Models;
using OsuEditor.Models.Timings;

namespace OsuEditor.Events
{
    public class TimingChangedEvent
    {
        public Timing NewTiming { get; set; }
    }
}
