namespace OsuEditor.Models
{
    public class Period
    {
        public int StartTime { get; set; }

        public int EndTime { get; set; }

        public Period()
        {
            StartTime = EndTime = -1;
        }

        public Period(int startTime, int endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public Period(Period prevPreriod)
        {
            StartTime = prevPreriod.StartTime;
            EndTime = prevPreriod.EndTime;
        }
    }
}
