using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class Clock : IClock
    {
        public DateTimeOffset Now { get => DateTimeOffset.UtcNow; }
    }
}
