using System.Linq;
using OsuEditor.CustomExceptions;
using OsuEditor.Events;

namespace OsuEditor.ViewModels
{
    public class ComposeHeaderViewModel : ViewModelBase, IEvent<BeatSnapEvent>
    {
        public string BeatSnapText
        {
            get { return Get(() => BeatSnapText); }
            set { Set(() => BeatSnapText, value); }
        }

        public double BeatValue
        {
            get { return Get(() => BeatValue); }
            set { Set(() => BeatValue, value); }
        }

        public ComposeHeaderViewModel(int snap)
        {
            BeatValue = BeatSnapToSlider(snap);
            BeatSnapText = $"1/{snap}";
            EventBus.Instance.RegisterHandler(this);
        }

        public void HandleEvent(BeatSnapEvent e)
        {
            BeatSnapText = $"1/{e.Snap}";
        }

        private static double BeatSnapToSlider(int beatSnap)
        {
            var beatSnapList = new[] { 1, 2, 3, 4, 6, 8, 12, 16, 24, 32 }.ToList();
            var sliderValueList = new[] { 0, 2, 3, 4, 6, 8, 10, 12, 14, 16 };

            var index = beatSnapList.FindIndex(x => x == beatSnap);
            if (index > -1 && index < 10)
                return sliderValueList[index];

            throw new InvalidValueException();
        }
    }
}
