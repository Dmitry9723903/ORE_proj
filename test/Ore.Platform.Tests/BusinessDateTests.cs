using System;
using Ore.Platform.Time;
using Xunit;

namespace Ore.Platform.Tests
{
    public sealed class BusinessDateTests
    {
        [Fact]
        public void From_SameDay_AreEqual()
        {
            var day = new DateOnly(2026, 9, 19);

            Assert.Equal(BusinessDate.From(day), BusinessDate.From(day));
        }

        [Fact]
        public void From_DifferentDay_AreNotEqual()
        {
            Assert.NotEqual(
                BusinessDate.From(new DateOnly(2026, 9, 19)),
                BusinessDate.From(new DateOnly(2026, 9, 20)));
        }

        [Fact]
        public void From_Default_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => BusinessDate.From(default));
        }

        [Fact]
        public void Value_OnDefaultStruct_Throws()
        {
            var bypassed = default(BusinessDate);

            Assert.Throws<InvalidOperationException>(() => bypassed.Value);
        }
    }
}
