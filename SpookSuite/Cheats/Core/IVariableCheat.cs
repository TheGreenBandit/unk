﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SpookSuite.Cheats.Core
{
    internal interface IVariableCheat<T>
    {
       T Value { get; set; }


    }
}
