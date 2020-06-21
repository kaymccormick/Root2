namespace KmDevLib
{
    public class MyReplaySubjectImpl : MyReplaySubject<ActivationInfo>, IActivationStream

    {
        public MyReplaySubjectImpl()
        {
            Title = "IOC Activations";
        }
    }
}