namespace OsuEditor.Models
{
    public class Period
    {
        public int StartTime { get; set; }

        public int EndTime { get; set; }

        public Period()
        {
            StartTime = EndTime = 0;
        }

        public Period(int startTime, int endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
