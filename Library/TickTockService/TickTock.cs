using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class TickTock : ITickTock
    {
        private readonly IClock clock;
        public TickTock(IClock clock) => this.clock = clock;

        public string TickOrTock()
        {
            int seconds = clock.Now.Second;
            return (seconds % 2 == 0) ? $"EVEN {seconds}" : $"ODD {seconds}";
        }
    }
}
