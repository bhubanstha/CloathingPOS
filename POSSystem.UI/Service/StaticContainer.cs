﻿using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSSystem.UI.Service
{
    public static class StaticContainer
    {
        public static App ThisApp { get; set; }
        public static IContainer Container { get; set; }

    }
}
