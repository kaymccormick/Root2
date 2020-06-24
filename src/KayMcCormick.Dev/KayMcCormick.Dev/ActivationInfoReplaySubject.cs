namespace KmDevLib
{
    public class ActivationInfoReplaySubject : MyReplaySubject<ActivationInfo>, IActivationStream
            
    {
        public ActivationInfoReplaySubject()
        {
            Title = "IOC Activations";
        }
    }
}