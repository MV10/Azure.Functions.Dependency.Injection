using System;

namespace Library
{
    /// <summary>
    /// A basic implementation of IGreeter. The constructed property's Millisecond value
    /// can be used to compare instances.
    /// </summary>
    public class AGreeter : IGreeter
    {
        public readonly DateTimeOffset constructed;
        public AGreeter()
            => constructed = DateTimeOffset.UtcNow;

        public string Greet()
            => $"Hello World, no counter, ms:{constructed.Millisecond}";
    }
}
