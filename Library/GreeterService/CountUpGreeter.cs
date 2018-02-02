using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class CountUpGreeter : AGreeter, IGreeter
    {
        private readonly ICounter counter;
        public CountUpGreeter(ICounter counter) : base()
        {
            this.counter = counter;
        }

        public new string Greet()
        {
            return $"Hello World, counter {counter.count++}, ms:{constructed.Millisecond}";
        }
    }
}
