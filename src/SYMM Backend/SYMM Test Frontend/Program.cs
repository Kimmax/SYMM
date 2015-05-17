using SYMM_Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SYMM_Test_Frontend
{
    class Program
    {
        static void Main(string[] args)
        {
            Base myBase = new Base();
            myBase.Run("AIzaSyAj82IqIloWupFnhn-hmmUo7iAkcj2xk3g");
        }
    }

    class Base
    {
        SYMMHandler backend;

        public void Run(string APIKey)
        {
            backend = new SYMMHandler(APIKey);

        }
    }
}
