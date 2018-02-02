using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    /// <summary>
    /// A class which is "external" to the IGreeter services but depends upon them. Used
    /// to demonstrate scoped lifetime services.
    /// </summary>
    public class GreeterConsumer : IGreeterConsumer
    {
        private readonly IGreeter greeter;
        public GreeterConsumer(IGreeter greeter) 
            => this.greeter = greeter;

        public string Greeting() 
            => greeter.Greet();
    }
}
