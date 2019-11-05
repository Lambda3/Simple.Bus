namespace Simple.Bus.Core.Receivers
{
    public abstract class ReceiverConfiguration<T>
    {
        public bool AutoCompleteMessage { get; private set; }

        public ReceiverConfiguration(bool autoComplete)
        {
            AutoCompleteMessage = autoComplete;
        }
    }
}
