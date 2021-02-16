using System;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace xRetry
{
    /// <summary>
    /// Attribute that is applied to a method to indicate that it is a fact that should be run
    /// by the test runner up to MaxRetries times, until it succeeds.
    /// </summary>
    [XunitTestCaseDiscoverer("xRetry.RetryFactDiscoverer", "xRetry")]
    [AttributeUsage(AttributeTargets.Method)]
    public class RetryFactAttribute : FactAttribute
    {
        public readonly int MaxRetries;
        public readonly int DelayBetweenRetriesMs;
        public int[] DelayBetweenEachRetriesMs { get; set; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="maxRetries">The number of times to run a test for until it succeeds</param>
        /// <param name="delayBetweenRetriesMs">The amount of time (in ms) to wait between each test run attempt</param>
        public RetryFactAttribute(int maxRetries = 3, int delayBetweenRetriesMs = 0)
        {
            if (maxRetries < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRetries) + " must be >= 1");
            }
            if (delayBetweenRetriesMs < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(delayBetweenRetriesMs) + " must be >= 0");
            }

            MaxRetries = maxRetries;
            DelayBetweenRetriesMs = delayBetweenRetriesMs;
        } 
        
        public RetryFactAttribute(int[] delayBetweenEachRetriesMs)
        {
            if (delayBetweenEachRetriesMs.Length < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(delayBetweenEachRetriesMs), "The array must contains at least 1 element");
            }
            
            if (delayBetweenEachRetriesMs.Any(i => i < 100))
            {
                throw new ArgumentOutOfRangeException(nameof(delayBetweenEachRetriesMs), "Any value in the table must be >= 100 miliseconds");
            }

            MaxRetries = delayBetweenEachRetriesMs.Length;
            DelayBetweenRetriesMs = delayBetweenEachRetriesMs[0];
        }  
    }
}
