using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SYMM_Backend;

namespace SYMM_Remote.Commands
{
    abstract class Command
    {
        private string _commandString;
        internal SYMMHandler APIHandler;
        internal APIClient owner;

        public string CommandString
        {
            get { return _commandString; }
            private set { _commandString = value; }
        }

        public Command(SYMMHandler APIHandler, APIClient owner)
        {
            this.CommandString = this.GetType().Name;
            this.APIHandler = APIHandler;
            this.owner = owner;
        }

        public abstract void Run()
        {

        }
    }
}
