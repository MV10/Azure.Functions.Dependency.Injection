using System;
using System.Collections.Generic;
using System.Text;

namespace Library
{
    public class GreeterConsumer : IGreeterConsumer
    {
        private readonly IGreeter greeter;
        public GreeterConsumer(IGreeter greeter) 
            => this.greeter = greeter;

        public string Greeting() 
            => greeter.Greet();
    }
}
