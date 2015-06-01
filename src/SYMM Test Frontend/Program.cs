﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SYMM_Backend;

namespace SYMM_Test_Frontend
{
    class Program
    {
        static void Main(string[] args)
        {
            Base myBase = new Base();
            myBase.Run("AIzaSyAj82IqIloWupFnhn-hmmUo7iAkcj2xk3g");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }
    }

    class Base
    {
        SYMMHandler backend;

        public void Run(string APIKey)
        {
            backend = new SYMMHandler(APIKey);
            backend.LoadVideosFromChannel("OfficialTrapCity").ForEach(video => 
            {
                Console.WriteLine("Found Video at POS: " + video.PlayListPos);
                Console.WriteLine("Title: " + video.VideoTitle);
                Console.WriteLine("Published at: " + video.PublishDate);
                Console.WriteLine("%-%-%-%-%-%-%-%-%-%-%-%-%-%-%-%-%-%-%-%-%-%-%-%-%-%-%-%");
            });
        }
    }
}