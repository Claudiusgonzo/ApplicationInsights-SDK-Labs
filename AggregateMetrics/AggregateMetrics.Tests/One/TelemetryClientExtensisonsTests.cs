﻿namespace CounterCollection.One.Tests
{
    using Microsoft.ApplicationInsights;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.ApplicationInsights.Extensibility.AggregateMetrics.One;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.DataContracts;

    [TestClass]
    public class TelemetryClientExtensisonsTests
    {
        [TestMethod]
        public void SimpleCounterUsageExample()
        {
            TelemetryConfiguration configuraiton = new TelemetryConfiguration();

            TelemetryClient client = new TelemetryClient(configuraiton);
            client.Context.Properties["a"] = "b";

            var simpleCounter = client.Counter("test");
            var counters = configuraiton.GetCounters();

            Assert.AreEqual(1, counters.Count);

            for (int i = 0; i < 10; i++)
            {
                simpleCounter.Increment();
            }

            MetricTelemetry metric = counters["test"].Value;
            Assert.AreEqual(10, metric.Value);
            Assert.AreEqual(null, metric.Count);
            Assert.AreEqual("test", metric.Name);
            Assert.AreEqual("b", metric.Context.Properties["a"]);
        }

        [TestMethod]
        public void SimpleGaugeUsageExample()
        {
            TelemetryConfiguration configuraiton = new TelemetryConfiguration();

            TelemetryClient client = new TelemetryClient(configuraiton);
            client.Context.Properties["a"] = "b";

            client.Gauge("test", () => { return 10; });
            var counters = configuraiton.GetCounters();

            Assert.AreEqual(1, counters.Count);

            MetricTelemetry metric = counters["test"].Value;
            Assert.AreEqual(10, metric.Value);
            Assert.AreEqual(null, metric.Count);
            Assert.AreEqual("test", metric.Name);
            Assert.AreEqual("b", metric.Context.Properties["a"]);
        }

        [TestMethod]
        public void SimpleMeterUsageExample()
        {
            TelemetryConfiguration configuraiton = new TelemetryConfiguration();

            TelemetryClient client = new TelemetryClient(configuraiton);
            client.Context.Properties["a"] = "b";

            var simpleMeter = client.Meter("test");
            var counters = configuraiton.GetCounters();

            Assert.AreEqual(1, counters.Count);

            for (int i = 0; i < 10; i++)
            {
                simpleMeter.Mark(2);
            }

            MetricTelemetry metric = counters["test"].Value;
            Assert.AreEqual(2, metric.Value);
            Assert.AreEqual(10, metric.Count);
            Assert.AreEqual("test", metric.Name);
            Assert.AreEqual("b", metric.Context.Properties["a"]);
        }

    }
}