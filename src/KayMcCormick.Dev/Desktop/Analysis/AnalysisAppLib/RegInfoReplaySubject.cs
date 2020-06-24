using KmDevLib;

namespace AnalysisAppLib
{
    internal class RegInfoReplaySubject : MyReplaySubject<RegInfo>
    {
        public RegInfoReplaySubject()
        {
            ListView = false;
            Title = "IOC Registrations";
        }

    }
}