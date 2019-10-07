using System.Collections.Generic;

namespace Gaspra.Logging.ApplicationInformation
{
    public interface IApplicationInformation
    {
        /*
            Information either retrieved from the
            application settings or derived by the
            application running.
        */
        IDictionary<string, string> Information { get; }
    }
}
