using System;
using System.Collections.Generic;
using System.Text;

namespace Gaspra.Logging.Sample.Models
{
    public class ComplexLoggingObject
    {
        public int Id { get; set; }

        public Guid Uid { get; set; }

        public DateTimeOffset Date { get; set; }

        public Dictionary<string, object> Dict { get; set; }

        public ComplexLoggingObject(int level)
        {
            var rnd = new Random();
            Id = rnd.Next();

            Uid = Guid.NewGuid();

            Date = DateTimeOffset.UtcNow;

            level--;

            if (level > 0)
            {
                Dict = new Dictionary<string, object>()
                {
                    { "ComplexLoggingObject", new ComplexLoggingObject(level) }
                };
            }
            else
            {
                Dict = new Dictionary<string, object>()
                {
                    { "ComplexLoggingObject", "The End" }
                };
            }
        }
    }
}
