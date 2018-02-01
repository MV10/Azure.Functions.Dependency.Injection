using System;

namespace Library
{
    public interface IClock
    {
        DateTimeOffset Now { get; }
    }
}