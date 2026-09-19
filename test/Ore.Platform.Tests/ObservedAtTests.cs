using System;
using Ore.Platform.Time;
using Xunit;

namespace Ore.Platform.Tests
{
    public sealed class ObservedAtTests
    {
        [Fact]
        public void From_WithOffset_ConvertsToUtc()
        {
            var moscow = new DateTimeOffset(2026, 9, 19, 9, 30, 0, TimeSpan.FromHours(3));

            var observed = ObservedAt.From(moscow);

            Assert.Equal(TimeSpan.Zero, observed.Value.Offset);
            Assert.Equal(moscow.UtcDateTime, observed.Value.UtcDateTime);
        }

        [Fact]
        public void From_Default_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ObservedAt.From(default));
        }

        [Fact]
        public void Value_OnDefaultStruct_Throws()
        {
            var bypassed = default(ObservedAt);

            Assert.Throws<InvalidOperationException>(() => bypassed.Value);
        }

        [Fact]
        public void SameInstant_AreEqual()
        {
            var moscow = new DateTimeOffset(2026, 9, 19, 9, 30, 0, TimeSpan.FromHours(3));
            var utc = new DateTimeOffset(2026, 9, 19, 6, 30, 0, TimeSpan.Zero);

            Assert.Equal(ObservedAt.From(moscow), ObservedAt.From(utc));
        }
    }
}
