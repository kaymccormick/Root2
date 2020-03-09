using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AnalysisServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class AnalysisService1 : IAnalysisService1
    {
        #region Implementation of IService1
        public string RestorePackages ( RestorePackagesRequest request ) { return "" ; }
        #endregion
    }
}
