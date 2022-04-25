using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Global
{
    static class Globals
    {
        /// <summary>
        /// Random number generator
        /// </summary>
        public static Random ran = new Random();

        public static float RadToDeg = 180 / Mathf.PI;
        public static float DegToRad = Mathf.PI / 180;
    }
}
