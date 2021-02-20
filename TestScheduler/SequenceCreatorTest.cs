using Scheduler;
using Scheduler.Interfaces;
using System.Collections.Generic;
using Xunit;

namespace TestScheduler
{
    public class SequenceCreatorTest
    {
        [Fact]
        public void GenerateSequenceTest()
        {
            ISequence sequenceCreator = new SequenceCreator();
            var result = sequenceCreator.GenerateSequence("02,3-14/3", 0, 30);
            var expected = new List<int>() { 2, 3, 6, 9, 12 };
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }

            result = sequenceCreator.GenerateSequence("*/2", 1, 7);
            expected = new List<int>() { 1, 3, 5, 7 };
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }
    }
}
