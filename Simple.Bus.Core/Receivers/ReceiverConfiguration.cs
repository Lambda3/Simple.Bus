namespace Simple.Bus.Core.Receivers
{
    public abstract class ReceiverConfiguration
    {
        public bool AutoCompleteMessage { get; private set; }

        public ReceiverConfiguration(bool autoComplete)
        {
            AutoCompleteMessage = autoComplete;
        }
    }
}
