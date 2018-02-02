using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    /// <summary>
    /// A simple container for an integer so we can demonstrate library-level injection.
    /// </summary>
    public class Counter : ICounter
    {
        public int count { get; set; } = 0;
    }
}
