using System.Collections.Generic;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace xRetry
{
    public class RetryTheoryDiscoverer : TheoryDiscoverer
    {
        public RetryTheoryDiscoverer(IMessageSink diagnosticMessageSink) 
            : base(diagnosticMessageSink) {  }

        protected override IEnumerable<IXunitTestCase> CreateTestCasesForDataRow(
            ITestFrameworkDiscoveryOptions discoveryOptions,
            ITestMethod testMethod,
            IAttributeInfo theoryAttribute,
            object[] dataRow)
        {
            int[] delayBetweenEachRetriesMs = theoryAttribute.GetNamedArgument<int[]>(nameof(RetryTheoryAttribute.DelayBetweenEachRetriesMs));
            if (delayBetweenEachRetriesMs != null)
            {
                return new[]
                {
                    new RetryTestCase(
                        DiagnosticMessageSink, 
                        discoveryOptions.MethodDisplayOrDefault(),
                        discoveryOptions.MethodDisplayOptionsOrDefault(), 
                        testMethod, 
                        delayBetweenEachRetriesMs, 
                        dataRow)
                };
            }
            
            int maxRetries = theoryAttribute.GetNamedArgument<int>(nameof(RetryTheoryAttribute.MaxRetries));
            int delayBetweenRetriesMs =
                theoryAttribute.GetNamedArgument<int>(nameof(RetryTheoryAttribute.DelayBetweenRetriesMs));
            return new[]
            {
                new RetryTestCase(
                    DiagnosticMessageSink, 
                    discoveryOptions.MethodDisplayOrDefault(),
                    discoveryOptions.MethodDisplayOptionsOrDefault(), 
                    testMethod, 
                    maxRetries, 
                    delayBetweenRetriesMs,
                    dataRow)
            };
        }

        protected override IEnumerable<IXunitTestCase> CreateTestCasesForTheory(
            ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod, IAttributeInfo theoryAttribute)
        {
            int[] delayBetweenEachRetriesMs = theoryAttribute.GetNamedArgument<int[]>(nameof(RetryTheoryAttribute.DelayBetweenEachRetriesMs));
            if (delayBetweenEachRetriesMs != null)
            {
                return new[]
                {
                    new RetryTheoryDiscoveryAtRuntimeCase(DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(),
                        discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod, delayBetweenEachRetriesMs)
                };
            }
            
            int maxRetries = theoryAttribute.GetNamedArgument<int>(nameof(RetryTheoryAttribute.MaxRetries));
            int delayBetweenRetriesMs =
                theoryAttribute.GetNamedArgument<int>(nameof(RetryTheoryAttribute.DelayBetweenRetriesMs));

            return new[]
            {
                new RetryTheoryDiscoveryAtRuntimeCase(DiagnosticMessageSink, discoveryOptions.MethodDisplayOrDefault(),
                    discoveryOptions.MethodDisplayOptionsOrDefault(), testMethod, maxRetries, delayBetweenRetriesMs)
            };
        }
    }
}
