using OsuEditor.Events;

namespace OsuEditor.ViewModels
{
    public class MetronomeViewModel : ViewModelBase, IEvent<TimingChangedEvent>
    {
        public MetronomeViewModel()
        {
            EventBus.Instance.RegisterHandler(this);
        }

        public void HandleEvent(TimingChangedEvent e)
        {
            throw new System.NotImplementedException();
        }
    }
}
