﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unlightvbe_kai_core
{
    public class Dice
    {
        readonly Random rnd = new(DateTime.Now.Millisecond);
        public bool Roll()
        {
            var result = rnd.Next(1, 7);
            if (result == 1 || result == 6)
            {
                return true;
            }
            return false;
        }
    }
}
