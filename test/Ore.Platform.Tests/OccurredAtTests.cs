using System;
using Ore.Platform.Time;
using Xunit;

namespace Ore.Platform.Tests
{
    public sealed class OccurredAtTests
    {
        private static readonly DateTimeOffset Moscow =
            new(2026, 9, 19, 9, 30, 0, TimeSpan.FromHours(3));

        private static readonly DateTimeOffset Utc =
            new(2026, 9, 19, 6, 30, 0, TimeSpan.Zero);

        [Fact]
        public void From_WithOffset_PreservesOffset()
        {
            var occurred = OccurredAt.From(Moscow);

            Assert.Equal(TimeSpan.FromHours(3), occurred.Offset);
            Assert.Equal(TimeSpan.FromHours(3), occurred.Value.Offset);
        }

        [Fact]
        public void SameInstantDifferentOffset_AreNotEqual()
        {
            Assert.NotEqual(OccurredAt.From(Moscow), OccurredAt.From(Utc));
        }

        [Fact]
        public void SameInstantDifferentOffset_HaveSameInstantUtc()
        {
            Assert.Equal(
                OccurredAt.From(Moscow).InstantUtc,
                OccurredAt.From(Utc).InstantUtc);
        }

        [Fact]
        public void From_Default_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => OccurredAt.From(default));
        }

        [Fact]
        public void Value_OnDefaultStruct_Throws()
        {
            var bypassed = default(OccurredAt);

            Assert.Throws<InvalidOperationException>(() => bypassed.Value);
        }
    }
}
