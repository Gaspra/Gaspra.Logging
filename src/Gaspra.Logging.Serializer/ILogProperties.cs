using System.Collections.Generic;

namespace Gaspra.Logging.Serializer
{
    public interface ILogProperties
    {
        /*
            Information either retrieved from the
            application settings or derived by the
            application running.
        */
        IDictionary<string, string> Properties { get; }
    }
}
