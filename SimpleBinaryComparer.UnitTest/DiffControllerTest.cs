using SimpleBinaryComparer.API.Controllers;
using System;
using Xunit;

namespace SimpleBinaryComparer.UnitTest
{
    public class DiffControllerTest
    {
        [Fact]
        public void Ctor_Test()
        {
            Assert.Throws<ArgumentNullException>(() => { new DiffController(null); });
        }
    }
}