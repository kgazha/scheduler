using Scheduler;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestScheduler
{
    public class SequenceCreatorTest
    {
        [Fact]
        public void GenerateSequenceTest()
        {
            SequenceCreator sequenceCreator = new SequenceCreator("02,3-14/3");
            var result = sequenceCreator.GenerateSequence(0, 30);
            var expected = new List<int>() { 2, 3, 6, 9, 12 };
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i], result[i]);
            }
        }
    }
}
