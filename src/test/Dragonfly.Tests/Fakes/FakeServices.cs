﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Firefly.Utils;

namespace Firefly.Tests.Fakes
{
    public class FakeServices : IFireflyService
    {
        public FakeServices()
        {
            Trace = new FakeTrace();
            Memory = new FakeMemoryPool();
        }
        public IServerTrace Trace { get; private set; }
        public IMemoryPool Memory { get; private set; }
    }
}
