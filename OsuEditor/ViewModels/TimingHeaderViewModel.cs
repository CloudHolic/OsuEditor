using OsuEditor.Events;

namespace OsuEditor.ViewModels
{
    public class TimingHeaderViewModel : ViewModelBase, IEvent<TimingChangedEvent>
    {
        public TimingHeaderViewModel()
        {
            EventBus.Instance.RegisterHandler(this);
        }

        public void HandleEvent(TimingChangedEvent e)
        {
            throw new System.NotImplementedException();
        }
    }
}
