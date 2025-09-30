// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SparkplugMessageGeneratorTestVersion22.cs" company="Hämmer Electronics">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   A class to test the <see cref="SparkplugMessageGenerator"/> class with specification version 2.2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SparkplugNet.Tests.Messages;

/// <summary>
/// A class to test the <see cref="SparkplugMessageGenerator"/> class with specification version 2.2.
/// </summary>
[TestClass]
public sealed class SparkplugMessageGeneratorTestVersion22
{
    /// <summary>
    /// The metrics for namespace A.
    /// </summary>
    private readonly List<VersionAData.KuraMetric> metricsA =
    [
        new VersionAData.KuraMetric("Test", VersionAData.DataType.Boolean, true)
    ];

    /// <summary>
    /// The metrics for namespace B.
    /// </summary>
    private readonly List<VersionBData.Metric> metricsB =
    [
        new VersionBData.Metric("Test", VersionBData.DataType.Int32, 20)
    ];

    /// <summary>
    /// The SEQ metric for namespace A.
    /// </summary>
    private readonly VersionAData.KuraMetric seqMetricA = new(Constants.SessionNumberMetricName, VersionAData.DataType.Int64, 1);

    /// <summary>
    /// The SEQ metric for namespace B.
    /// </summary>
    private readonly VersionBData.Metric seqMetricB = new(Constants.SessionNumberMetricName, VersionBData.DataType.Int64, 1);

    //Begin HEWA: rebirth metric for namespace B
    /// <summary>
    /// The SEQ metric for namespace B.
    /// </summary>
    private readonly VersionBData.Metric rebirthMetricB = new(Constants.NodeControlRebirthName, VersionBData.DataType.Boolean, false);
    // End HEWA

    /// <summary>
    /// The message generator.
    /// </summary>
    private readonly SparkplugMessageGenerator messageGenerator = new(SparkplugSpecificationVersion.Version22);

    /// <summary>
    /// Tests the Sparkplug message generator with a message with a version A namespace and a online state.
    /// </summary>
    [TestMethod]
    public void TestStateMessageNamespaceAOnline()
    {
        var message = this.messageGenerator.GetSparkplugStateMessage(SparkplugNamespace.VersionA, "scada1", true);

        Assert.AreEqual("STATE/scada1", message.Topic);
        Assert.AreEqual("ONLINE", message.ConvertPayloadToString());
    }

    /// <summary>
    /// Tests the Sparkplug message generator with a message with a version A namespace and a offline state.
    /// </summary>
    [TestMethod]
    public void TestStateMessageNamespaceAOffline()
    {
        var message = this.messageGenerator.GetSparkplugStateMessage(SparkplugNamespace.VersionA, "scada1", false);

        Assert.AreEqual("STATE/scada1", message.Topic);
        Assert.AreEqual("OFFLINE", message.ConvertPayloadToString());
    }

    /// <summary>
    /// Tests the Sparkplug message generator with a message with a version B namespace and a online state.
    /// </summary>
    [TestMethod]
    public void TestStateMessageNamespaceBOnline()
    {
        var message = this.messageGenerator.GetSparkplugStateMessage(SparkplugNamespace.VersionB, "scada1", true);

        Assert.AreEqual("STATE/scada1", message.Topic);
        Assert.AreEqual("ONLINE", message.ConvertPayloadToString());
    }

    /// <summary>
    /// Tests the Sparkplug message generator with a message with a version B namespace and a offline state.
    /// </summary>
    [TestMethod]
    public void TestStateMessageNamespaceBOffline()
    {
        var message = this.messageGenerator.GetSparkplugStateMessage(SparkplugNamespace.VersionB, "scada1", false);

        Assert.AreEqual("STATE/scada1", message.Topic);
        Assert.AreEqual("OFFLINE", message.ConvertPayloadToString());
    }

    /// <summary>
    /// Tests the Sparkplug message generator with a device birth message with a version A namespace.
    /// </summary>
    [TestMethod]
    public void TestDeviceBirthMessageNamespaceA()
    {
        var dateTime = DateTimeOffset.Now;
        var message = this.messageGenerator.GetSparkplugDeviceBirthMessage(SparkplugNamespace.VersionA, "group1", "edge1", "device1", this.metricsA, 0, 1, dateTime);
        var payloadVersionA = PayloadHelper.Deserialize<VersionAProtoBufPayload>(message.Payload);

        Assert.AreEqual("spAv1.0/group1/DBIRTH/edge1/device1", message.Topic);
        Assert.IsNotNull(payloadVersionA);
        Assert.AreEqual(dateTime.ToUnixTimeMilliseconds(), payloadVersionA.Timestamp);
        Assert.AreEqual(2, payloadVersionA.Metrics.Count);

        Assert.AreEqual(this.metricsA.First().Name, payloadVersionA.Metrics.ElementAt(0).Name);
        Assert.AreEqual((bool?)this.metricsA.First().Value, payloadVersionA.Metrics.ElementAt(0).BooleanValue);
        Assert.AreEqual(this.metricsA.First().DataType, VersionAMain.PayloadConverter.ConvertVersionADataType(payloadVersionA.Metrics.ElementAt(0).DataType));

        Assert.AreEqual(this.seqMetricA.Name, payloadVersionA.Metrics.ElementAt(1).Name);
        Assert.AreEqual((long?)this.seqMetricA.Value, payloadVersionA.Metrics.ElementAt(1).LongValue);
        Assert.AreEqual(this.seqMetricA.DataType, VersionAMain.PayloadConverter.ConvertVersionADataType(payloadVersionA.Metrics.ElementAt(1).DataType));
    }

    /// <summary>
    /// Tests the Sparkplug message generator with a device birth message with a version B namespace.
    /// </summary>
    [TestMethod]
    public void TestDeviceBirthMessageNamespaceB()
    {
        var dateTime = DateTimeOffset.UtcNow;
        var message = this.messageGenerator.GetSparkplugDeviceBirthMessage(SparkplugNamespace.VersionB, "group1", "edge1", "device1", this.metricsB, 0, 1, dateTime);
        var payloadVersionB = PayloadHelper.Deserialize<VersionBProtoBufPayload>(message.Payload);

        Assert.AreEqual("spBv1.0/group1/DBIRTH/edge1/device1", message.Topic);
        Assert.IsNotNull(payloadVersionB);
        Assert.AreEqual((ulong)dateTime.ToUnixTimeMilliseconds(), payloadVersionB.Timestamp);
        Assert.AreEqual(2, payloadVersionB.Metrics.Count);

        Assert.AreEqual(this.metricsB.First().Name, payloadVersionB.Metrics.ElementAt(0).Name);
        Assert.AreEqual(Convert.ToUInt32(this.metricsB.First().Value), payloadVersionB.Metrics.ElementAt(0).IntValue);
        Assert.AreEqual((uint?)this.metricsB.First().DataType, payloadVersionB.Metrics.ElementAt(0).DataType);

        Assert.AreEqual(this.seqMetricB.Name, payloadVersionB.Metrics.ElementAt(1).Name);
        Assert.AreEqual(Convert.ToUInt64(this.seqMetricB.Value), payloadVersionB.Metrics.ElementAt(1).LongValue);
        Assert.AreEqual((uint?)this.seqMetricB.DataType, payloadVersionB.Metrics.ElementAt(1).DataType);
    }

    /// <summary>
    /// Tests the Sparkplug message generator with a node birth message with a version A namespace.
    /// </summary>
    [TestMethod]
    public void TestNodeBirthMessageNamespaceA()
    {
        var dateTime = DateTimeOffset.Now;
        var message = this.messageGenerator.GetSparkplugNodeBirthMessage(SparkplugNamespace.VersionA, "group1", "edge1", this.metricsA, 0, 1, dateTime);
        var payloadVersionA = PayloadHelper.Deserialize<VersionAProtoBufPayload>(message.Payload);

        Assert.AreEqual("spAv1.0/group1/NBIRTH/edge1", message.Topic);
        Assert.IsNotNull(payloadVersionA);
        Assert.AreEqual(dateTime.ToUnixTimeMilliseconds(), payloadVersionA.Timestamp);
        Assert.AreEqual(2, payloadVersionA.Metrics.Count);

        Assert.AreEqual(this.metricsA.First().Name, payloadVersionA.Metrics.ElementAt(0).Name);
        Assert.AreEqual((bool?)this.metricsA.First().Value, payloadVersionA.Metrics.ElementAt(0).BooleanValue);
        Assert.AreEqual(this.metricsA.First().DataType, VersionAMain.PayloadConverter.ConvertVersionADataType(payloadVersionA.Metrics.ElementAt(0).DataType));

        Assert.AreEqual(this.seqMetricA.Name, payloadVersionA.Metrics.ElementAt(1).Name);
        Assert.AreEqual((long?)this.seqMetricA.Value, payloadVersionA.Metrics.ElementAt(1).LongValue);
        Assert.AreEqual(this.seqMetricA.DataType, VersionAMain.PayloadConverter.ConvertVersionADataType(payloadVersionA.Metrics.ElementAt(1).DataType));
    }

    /// <summary>
    /// Tests the Sparkplug message generator with a node birth message with a version B namespace.
    /// </summary>
    [TestMethod]
    public void TestNodeBirthMessageNamespaceB()
    {
        var dateTime = DateTimeOffset.UtcNow;
        var message = this.messageGenerator.GetSparkplugNodeBirthMessage(SparkplugNamespace.VersionB, "group1", "edge1", this.metricsB, 0, 1, dateTime);
        var payloadVersionB = PayloadHelper.Deserialize<VersionBProtoBufPayload>(message.Payload);

        Assert.AreEqual("spBv1.0/group1/NBIRTH/edge1", message.Topic);
        Assert.IsNotNull(payloadVersionB);
        Assert.AreEqual((ulong)dateTime.ToUnixTimeMilliseconds(), payloadVersionB.Timestamp);
        // Begin HEWA: Node rebirth added
//        Assert.AreEqual(2, payloadVersionB.Metrics.Count);
        Assert.AreEqual(3, payloadVersionB.Metrics.Count);
        // End HEWA

        Assert.AreEqual(this.metricsB.First().Name, payloadVersionB.Metrics.ElementAt(0).Name);
        Assert.AreEqual(Convert.ToUInt32(this.metricsB.First().Value), payloadVersionB.Metrics.ElementAt(0).IntValue);
        Assert.AreEqual((uint?)this.metricsB.First().DataType, payloadVersionB.Metrics.ElementAt(0).DataType);

        Assert.AreEqual(this.seqMetricB.Name, payloadVersionB.Metrics.ElementAt(1).Name);
        Assert.AreEqual(Convert.ToUInt64(this.seqMetricB.Value), payloadVersionB.Metrics.ElementAt(1).LongValue);
        Assert.AreEqual((uint?)this.seqMetricB.DataType, payloadVersionB.Metrics.ElementAt(1).DataType);

        // Begin HEWA: Node rebirth added
        Assert.AreEqual(this.rebirthMetricB.Name, payloadVersionB.Metrics.ElementAt(2).Name);
        Assert.AreEqual(Convert.ToBoolean(this.rebirthMetricB.Value), payloadVersionB.Metrics.ElementAt(2).BooleanValue);
        Assert.AreEqual((uint?)this.rebirthMetricB.DataType, payloadVersionB.Metrics.ElementAt(2).DataType);
        // End HEWA
    }

    /// <summary>
    /// Tests the Sparkplug message generator with a device death message with a version A namespace.
    /// </summary>
    [TestMethod]
    public void TestDeviceDeathMessageNamespaceA()
    {
        var dateTime = DateTimeOffset.Now;
        var message = this.messageGenerator.GetSparkplugDeviceDeathMessage(SparkplugNamespace.VersionA, "group1", "edge1", "device1", 0, 1, dateTime);
        var payloadVersionA = PayloadHelper.Deserialize<VersionAProtoBufPayload>(message.Payload);

        Assert.AreEqual("spAv1.0/group1/DDEATH/edge1/device1", message.Topic);
        Assert.IsNotNull(payloadVersionA);
        Assert.AreEqual(dateTime.ToUnixTimeMilliseconds(), payloadVersionA.Timestamp);
        Assert.AreEqual(1, payloadVersionA.Metrics.Count);

        Assert.AreEqual(this.seqMetricA.Name, payloadVersionA.Metrics.ElementAt(0).Name);
        Assert.AreEqual((long?)this.seqMetricA.Value, payloadVersionA.Metrics.ElementAt(0).LongValue);
        Assert.AreEqual(this.seqMetricA.DataType, VersionAMain.PayloadConverter.ConvertVersionADataType(payloadVersionA.Metrics.ElementAt(0).DataType));
    }

    /// <summary>
    /// Tests the Sparkplug message generator with a device death message with a version B namespace.
    /// </summary>
    [TestMethod]
    public void TestDeviceDeathMessageNamespaceB()
    {
        var dateTime = DateTimeOffset.UtcNow;
        var message = this.messageGenerator.GetSparkplugDeviceDeathMessage(SparkplugNamespace.VersionB, "group1", "edge1", "device1", 0, 1, dateTime);
        var payloadVersionB = PayloadHelper.Deserialize<VersionBProtoBuf.ProtobufPayloadDeath>(message.Payload);

        Assert.AreEqual("spBv1.0/group1/DDEATH/edge1/device1", message.Topic);
        Assert.IsNotNull(payloadVersionB);
        Assert.AreEqual((ulong)dateTime.ToUnixTimeMilliseconds(), payloadVersionB.Timestamp);
        Assert.AreEqual(1, payloadVersionB.Metrics.Count);

        Assert.AreEqual(this.seqMetricB.Name, payloadVersionB.Metrics.ElementAt(0).Name);
        Assert.AreEqual(Convert.ToUInt64(this.seqMetricB.Value), payloadVersionB.Metrics.ElementAt(0).LongValue);
        Assert.AreEqual((uint?)this.seqMetricB.DataType, payloadVersionB.Metrics.ElementAt(0).DataType);
    }

    /// <summary>
    /// Tests the Sparkplug message generator with a node death message with a version A namespace.
    /// </summary>
    [TestMethod]
    public void TestNodeDeathMessageNamespaceA()
    {
        var message = this.messageGenerator.GetSparkplugNodeDeathMessage(SparkplugNamespace.VersionA, "group1", "edge1", 1);
        var payloadVersionA = PayloadHelper.Deserialize<VersionAProtoBufPayload>(message.Payload);

        Assert.AreEqual("spAv1.0/group1/NDEATH/edge1", message.Topic);
        Assert.IsNotNull(payloadVersionA);
        Assert.AreEqual(1, payloadVersionA.Metrics.Count);

        Assert.AreEqual(this.seqMetricA.Name, payloadVersionA.Metrics.ElementAt(0).Name);
        Assert.AreEqual((long?)this.seqMetricA.Value, payloadVersionA.Metrics.ElementAt(0).LongValue);
        Assert.AreEqual(this.seqMetricA.DataType, VersionAMain.PayloadConverter.ConvertVersionADataType(payloadVersionA.Metrics.ElementAt(0).DataType));
    }

    /// <summary>
    /// Tests the Sparkplug message generator with a node death message with a version B namespace.
    /// </summary>
    [TestMethod]
    public void TestNodeDeathMessageNamespaceB()
    {
        var message = this.messageGenerator.GetSparkplugNodeDeathMessage(SparkplugNamespace.VersionB, "group1", "edge1", 1);
        var payloadVersionB = PayloadHelper.Deserialize<VersionBProtoBuf.ProtobufPayloadDeath>(message.Payload);

        Assert.AreEqual("spBv1.0/group1/NDEATH/edge1", message.Topic);
        Assert.IsNotNull(payloadVersionB);
        Assert.AreEqual(1, payloadVersionB.Metrics.Count);

        Assert.AreEqual(this.seqMetricB.Name, payloadVersionB.Metrics.ElementAt(0).Name);
        Assert.AreEqual(Convert.ToUInt64(this.seqMetricB.Value), payloadVersionB.Metrics.ElementAt(0).LongValue);
        Assert.AreEqual((uint?)this.seqMetricB.DataType, payloadVersionB.Metrics.ElementAt(0).DataType);
    }

    /// <summary>
    /// Tests the Sparkplug message generator with a device data message with a version A namespace.
    /// </summary>
    [TestMethod]
    public void TestDeviceDataMessageNamespaceA()
    {
        var dateTime = DateTimeOffset.Now;
        var message = this.messageGenerator.GetSparkplugDeviceDataMessage(SparkplugNamespace.VersionA, "group1", "edge1", "device1", this.metricsA, 0, 1, dateTime);
        var payloadVersionA = PayloadHelper.Deserialize<VersionAProtoBufPayload>(message.Payload);

        Assert.AreEqual("spAv1.0/group1/DDATA/edge1/device1", message.Topic);
        Assert.IsNotNull(payloadVersionA);
        Assert.AreEqual(dateTime.ToUnixTimeMilliseconds(), payloadVersionA.Timestamp);
        Assert.AreEqual(2, payloadVersionA.Metrics.Count);

        Assert.AreEqual(this.metricsA.First().Name, payloadVersionA.Metrics.ElementAt(0).Name);
        Assert.AreEqual((bool?)this.metricsA.First().Value, payloadVersionA.Metrics.ElementAt(0).BooleanValue);
        Assert.AreEqual(this.metricsA.First().DataType, VersionAMain.PayloadConverter.ConvertVersionADataType(payloadVersionA.Metrics.ElementAt(0).DataType));

        Assert.AreEqual(this.seqMetricA.Name, payloadVersionA.Metrics.ElementAt(1).Name);
        Assert.AreEqual((long?)this.seqMetricA.Value, payloadVersionA.Metrics.ElementAt(1).LongValue);
        Assert.AreEqual(this.seqMetricA.DataType, VersionAMain.PayloadConverter.ConvertVersionADataType(payloadVersionA.Metrics.ElementAt(1).DataType));
    }

    /// <summary>
    /// Tests the Sparkplug message generator with a device data message with a version B namespace.
    /// </summary>
    [TestMethod]
    public void TestDeviceDataMessageNamespaceB()
    {
        var dateTime = DateTimeOffset.UtcNow;
        var message = this.messageGenerator.GetSparkplugDeviceDataMessage(SparkplugNamespace.VersionB, "group1", "edge1", "device1", this.metricsB, 0, 1, dateTime);
        var payloadVersionB = PayloadHelper.Deserialize<VersionBProtoBufPayload>(message.Payload);

        Assert.AreEqual("spBv1.0/group1/DDATA/edge1/device1", message.Topic);
        Assert.IsNotNull(payloadVersionB);
        Assert.AreEqual((ulong)dateTime.ToUnixTimeMilliseconds(), payloadVersionB.Timestamp);
        // Begin HEWA: No session number added
        // Assert.AreEqual(2, payloadVersionB.Metrics.Count);
        Assert.AreEqual(1, payloadVersionB.Metrics.Count);
        // End HEWA

        Assert.AreEqual(this.metricsB.First().Name, payloadVersionB.Metrics.ElementAt(0).Name);
        Assert.AreEqual(Convert.ToUInt32(this.metricsB.First().Value), payloadVersionB.Metrics.ElementAt(0).IntValue);
        Assert.AreEqual((uint?)this.metricsB.First().DataType, payloadVersionB.Metrics.ElementAt(0).DataType);

        // Begin HEWA: No session number added
        //Assert.AreEqual(this.seqMetricB.Name, payloadVersionB.Metrics.ElementAt(1).Name);
        //Assert.AreEqual(Convert.ToUInt64(this.seqMetricB.Value), payloadVersionB.Metrics.ElementAt(1).LongValue);
        //Assert.AreEqual((uint?)this.seqMetricB.DataType, payloadVersionB.Metrics.ElementAt(1).DataType);
        // End HEWA
    }

    /// <summary>
    /// Tests the Sparkplug message generator with a node data message with a version A namespace.
    /// </summary>
    [TestMethod]
    public void TestNodeDataMessageNamespaceA()
    {
        var dateTime = DateTimeOffset.Now;
        var message = this.messageGenerator.GetSparkplugNodeDataMessage(SparkplugNamespace.VersionA, "group1", "edge1", this.metricsA, 0, 1, dateTime);
        var payloadVersionA = PayloadHelper.Deserialize<VersionAProtoBufPayload>(message.Payload);

        Assert.AreEqual("spAv1.0/group1/NDATA/edge1", message.Topic);
        Assert.IsNotNull(payloadVersionA);
        Assert.AreEqual(dateTime.ToUnixTimeMilliseconds(), payloadVersionA.Timestamp);
        Assert.AreEqual(2, payloadVersionA.Metrics.Count);

        Assert.AreEqual(this.metricsA.First().Name, payloadVersionA.Metrics.ElementAt(0).Name);
        Assert.AreEqual((bool?)this.metricsA.First().Value, payloadVersionA.Metrics.ElementAt(0).BooleanValue);
        Assert.AreEqual(this.metricsA.First().DataType, VersionAMain.PayloadConverter.ConvertVersionADataType(payloadVersionA.Metrics.ElementAt(0).DataType));

        Assert.AreEqual(this.seqMetricA.Name, payloadVersionA.Metrics.ElementAt(1).Name);
        Assert.AreEqual((long?)this.seqMetricA.Value, payloadVersionA.Metrics.ElementAt(1).LongValue);
        Assert.AreEqual(this.seqMetricA.DataType, VersionAMain.PayloadConverter.ConvertVersionADataType(payloadVersionA.Metrics.ElementAt(1).DataType));
    }

    /// <summary>
    /// Tests the Sparkplug message generator with a node data message with a version B namespace.
    /// </summary>
    [TestMethod]
    public void TestNodeDataMessageNamespaceB()
    {
        var dateTime = DateTimeOffset.UtcNow;
        var message = this.messageGenerator.GetSparkplugNodeDataMessage(SparkplugNamespace.VersionB, "group1", "edge1", this.metricsB, 0, 1, dateTime);
        var payloadVersionB = PayloadHelper.Deserialize<VersionBProtoBufPayload>(message.Payload);

        Assert.AreEqual("spBv1.0/group1/NDATA/edge1", message.Topic);
        Assert.IsNotNull(payloadVersionB);
        Assert.AreEqual((ulong)dateTime.ToUnixTimeMilliseconds(), payloadVersionB.Timestamp);
        Assert.AreEqual(2, payloadVersionB.Metrics.Count);

        Assert.AreEqual(this.metricsB.First().Name, payloadVersionB.Metrics.ElementAt(0).Name);
        Assert.AreEqual(Convert.ToUInt32(this.metricsB.First().Value), payloadVersionB.Metrics.ElementAt(0).IntValue);
        Assert.AreEqual((uint?)this.metricsB.First().DataType, payloadVersionB.Metrics.ElementAt(0).DataType);

        Assert.AreEqual(this.seqMetricB.Name, payloadVersionB.Metrics.ElementAt(1).Name);
        Assert.AreEqual(Convert.ToUInt64(this.seqMetricB.Value), payloadVersionB.Metrics.ElementAt(1).LongValue);
        Assert.AreEqual((uint?)this.seqMetricB.DataType, payloadVersionB.Metrics.ElementAt(1).DataType);
    }

    /// <summary>
    /// Tests the Sparkplug message generator with a device command message with a version A namespace.
    /// </summary>
    [TestMethod]
    public void TestDeviceCommandMessageNamespaceA()
    {
        var dateTime = DateTimeOffset.Now;
        var message = this.messageGenerator.GetSparkplugDeviceCommandMessage(SparkplugNamespace.VersionA, "group1", "edge1", "device1", this.metricsA, 0, 1, dateTime);
        var payloadVersionA = PayloadHelper.Deserialize<VersionAProtoBufPayload>(message.Payload);

        Assert.AreEqual("spAv1.0/group1/DCMD/edge1/device1", message.Topic);
        Assert.IsNotNull(payloadVersionA);
        Assert.AreEqual(dateTime.ToUnixTimeMilliseconds(), payloadVersionA.Timestamp);
        Assert.AreEqual(2, payloadVersionA.Metrics.Count);

        Assert.AreEqual(this.metricsA.First().Name, payloadVersionA.Metrics.ElementAt(0).Name);
        Assert.AreEqual((bool?)this.metricsA.First().Value, payloadVersionA.Metrics.ElementAt(0).BooleanValue);
        Assert.AreEqual(this.metricsA.First().DataType, VersionAMain.PayloadConverter.ConvertVersionADataType(payloadVersionA.Metrics.ElementAt(0).DataType));

        Assert.AreEqual(this.seqMetricA.Name, payloadVersionA.Metrics.ElementAt(1).Name);
        Assert.AreEqual((long?)this.seqMetricA.Value, payloadVersionA.Metrics.ElementAt(1).LongValue);
        Assert.AreEqual(this.seqMetricA.DataType, VersionAMain.PayloadConverter.ConvertVersionADataType(payloadVersionA.Metrics.ElementAt(1).DataType));
    }

    /// <summary>
    /// Tests the Sparkplug message generator with a device command message with a version B namespace.
    /// </summary>
    [TestMethod]
    public void TestDeviceCommandMessageNamespaceB()
    {
        var dateTime = DateTimeOffset.UtcNow;
        var message = this.messageGenerator.GetSparkplugDeviceCommandMessage(SparkplugNamespace.VersionB, "group1", "edge1", "device1", this.metricsB, 0, 1, dateTime);
        var payloadVersionB = PayloadHelper.Deserialize<VersionBProtoBufPayload>(message.Payload);

        Assert.AreEqual("spBv1.0/group1/DCMD/edge1/device1", message.Topic);
        Assert.IsNotNull(payloadVersionB);
        Assert.AreEqual((ulong)dateTime.ToUnixTimeMilliseconds(), payloadVersionB.Timestamp);
        Assert.AreEqual(2, payloadVersionB.Metrics.Count);

        Assert.AreEqual(this.metricsB.First().Name, payloadVersionB.Metrics.ElementAt(0).Name);
        Assert.AreEqual(Convert.ToUInt32(this.metricsB.First().Value), payloadVersionB.Metrics.ElementAt(0).IntValue);
        Assert.AreEqual((uint?)this.metricsB.First().DataType, payloadVersionB.Metrics.ElementAt(0).DataType);

        Assert.AreEqual(this.seqMetricB.Name, payloadVersionB.Metrics.ElementAt(1).Name);
        Assert.AreEqual(Convert.ToUInt64(this.seqMetricB.Value), payloadVersionB.Metrics.ElementAt(1).LongValue);
        Assert.AreEqual((uint?)this.seqMetricB.DataType, payloadVersionB.Metrics.ElementAt(1).DataType);
    }

    /// <summary>
    /// Tests the Sparkplug message generator with a node command message with a version A namespace.
    /// </summary>
    [TestMethod]
    public void TestNodeCommandMessageNamespaceA()
    {
        var dateTime = DateTimeOffset.Now;
        var message = this.messageGenerator.GetSparkplugNodeCommandMessage(SparkplugNamespace.VersionA, "group1", "edge1", this.metricsA, 0, 1, dateTime);
        var payloadVersionA = PayloadHelper.Deserialize<VersionAProtoBufPayload>(message.Payload);

        Assert.AreEqual("spAv1.0/group1/NCMD/edge1", message.Topic);
        Assert.IsNotNull(payloadVersionA);
        Assert.AreEqual(dateTime.ToUnixTimeMilliseconds(), payloadVersionA.Timestamp);
        Assert.AreEqual(2, payloadVersionA.Metrics.Count);

        Assert.AreEqual(this.metricsA.First().Name, payloadVersionA.Metrics.ElementAt(0).Name);
        Assert.AreEqual((bool?)this.metricsA.First().Value, payloadVersionA.Metrics.ElementAt(0).BooleanValue);
        Assert.AreEqual(this.metricsA.First().DataType, VersionAMain.PayloadConverter.ConvertVersionADataType(payloadVersionA.Metrics.ElementAt(0).DataType));

        Assert.AreEqual(this.seqMetricA.Name, payloadVersionA.Metrics.ElementAt(1).Name);
        Assert.AreEqual((long?)this.seqMetricA.Value, payloadVersionA.Metrics.ElementAt(1).LongValue);
        Assert.AreEqual(this.seqMetricA.DataType, VersionAMain.PayloadConverter.ConvertVersionADataType(payloadVersionA.Metrics.ElementAt(1).DataType));
    }

    /// <summary>
    /// Tests the Sparkplug message generator with a node command message with a version B namespace.
    /// </summary>
    [TestMethod]
    public void TestNodeCommandMessageNamespaceB()
    {
        var dateTime = DateTimeOffset.UtcNow;
        var message = this.messageGenerator.GetSparkplugNodeCommandMessage(SparkplugNamespace.VersionB, "group1", "edge1", this.metricsB, 0, 1, dateTime);
        var payloadVersionB = PayloadHelper.Deserialize<VersionBProtoBufPayload>(message.Payload);

        Assert.AreEqual("spBv1.0/group1/NCMD/edge1", message.Topic);
        Assert.IsNotNull(payloadVersionB);
        Assert.AreEqual((ulong)dateTime.ToUnixTimeMilliseconds(), payloadVersionB.Timestamp);
        Assert.AreEqual(2, payloadVersionB.Metrics.Count);

        Assert.AreEqual(this.metricsB.First().Name, payloadVersionB.Metrics.ElementAt(0).Name);
        Assert.AreEqual(Convert.ToUInt32(this.metricsB.First().Value), payloadVersionB.Metrics.ElementAt(0).IntValue);
        Assert.AreEqual((uint?)this.metricsB.First().DataType, payloadVersionB.Metrics.ElementAt(0).DataType);

        Assert.AreEqual(this.seqMetricB.Name, payloadVersionB.Metrics.ElementAt(1).Name);
        Assert.AreEqual(Convert.ToUInt64(this.seqMetricB.Value), payloadVersionB.Metrics.ElementAt(1).LongValue);
        Assert.AreEqual((uint?)this.seqMetricB.DataType, payloadVersionB.Metrics.ElementAt(1).DataType);
    }
}
