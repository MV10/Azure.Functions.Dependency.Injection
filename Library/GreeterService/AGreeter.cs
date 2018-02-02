using System;

namespace Library
{
    public class AGreeter : IGreeter
    {
        public readonly DateTimeOffset constructed;
        public AGreeter()
        {
            constructed = DateTimeOffset.UtcNow;
        }

        public string Greet()
        {
            return $"Hello World, no counter, ms:{constructed.Millisecond}";
        }
    }
}
