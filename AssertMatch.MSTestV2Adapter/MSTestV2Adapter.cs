﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AssertMatch.MSTestV2Adapter
{
    public class MSTestV2Adapter : ITestFrameworkAdapter
    {
        public void Fail(string message)
        {
            Assert.Fail(message);
        }

        public void Ok()
        {
            Assert.IsTrue(true);
        }
    }
}
