// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SparkplugMessageGeneratorTestVersion30.cs" company="Hämmer Electronics">
// The project is licensed under the MIT license.
// </copyright>
// <summary>
//   A class to test the <see cref="SparkplugMessageGenerator"/> class with specification version 3.0.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SparkplugNet.Tests.Messages;

/// <summary>
/// A class to test the <see cref="SparkplugMessageGenerator"/> class with specification version 3.0.
/// </summary>
[TestClass]
public sealed class SparkplugSerializerTestVersion30
{
    /// <summary>
    /// The metrics for namespace B.
    /// </summary>
    private readonly List<VersionBData.Metric> metricsB =
    [
        new VersionBData.Metric("Test", VersionBData.DataType.Int32, 20)
    ];

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
    private readonly SparkplugMessageGenerator messageGenerator = new(SparkplugSpecificationVersion.Version30);

    /// <summary>
    /// Tests the Sparkplug message generator with a node birth message with a version B namespace.
    /// </summary>
    [TestMethod]
    public void TestNodeBirthMessageNamespaceB()
    {
        var dateTime = DateTimeOffset.UtcNow;
        var messageNodeBirth = this.messageGenerator.GetSparkplugNodeBirthMessage(SparkplugNamespace.VersionB, "group1", "edge1", this.metricsB, 0, 1, dateTime);
        var payloadVersionNodeBirth = PayloadHelper.Deserialize<VersionBProtoBufPayload>(messageNodeBirth.Payload);
        Assert.IsNotNull(payloadVersionNodeBirth);
        Assert.IsNotNull(payloadVersionNodeBirth.Seq);
        Assert.IsTrue(payloadVersionNodeBirth.Seq.HasValue);
        Assert.AreEqual<ulong>(0, payloadVersionNodeBirth.Seq.Value);

        var messageNodeDeath = this.messageGenerator.GetSparkplugNodeDeathMessage(SparkplugNamespace.VersionB, "group1", "edge1", 1);
        var payloadVersionNodeDeath = PayloadHelper.Deserialize<VersionBProtoBufPayload>(messageNodeDeath.Payload);
        Assert.IsNotNull(payloadVersionNodeDeath);
        Assert.IsNull(payloadVersionNodeDeath.Seq);
        Assert.IsFalse(payloadVersionNodeDeath.Seq.HasValue);

        var messageDeviceBirth = this.messageGenerator.GetSparkplugDeviceBirthMessage(SparkplugNamespace.VersionB, "group1", "edge1", "device1", this.metricsB, 0, 1, dateTime);
        var payloadVersionDeviceBirth = PayloadHelper.Deserialize<VersionBProtoBufPayload>(messageDeviceBirth.Payload);
        Assert.IsNotNull(payloadVersionDeviceBirth);
        Assert.IsNotNull(payloadVersionDeviceBirth.Seq);
        Assert.IsTrue(payloadVersionDeviceBirth.Seq.HasValue);
        Assert.AreEqual<ulong>(0, payloadVersionDeviceBirth.Seq.Value);

        var messageDeviceDeath = this.messageGenerator.GetSparkplugDeviceDeathMessage(SparkplugNamespace.VersionB, "group1", "edge1", "device1", 0, 1, dateTime);
        var payloadVersionDeviceDeath = PayloadHelper.Deserialize<VersionBProtoBufPayload>(messageDeviceBirth.Payload);
        Assert.IsNotNull(payloadVersionDeviceDeath);
        Assert.IsNotNull(payloadVersionDeviceDeath.Seq);
        Assert.IsTrue(payloadVersionDeviceDeath.Seq.HasValue);
        Assert.AreEqual<ulong>(0, payloadVersionDeviceDeath.Seq.Value);

        var messageDeviceData = this.messageGenerator.GetSparkplugDeviceDataMessage(SparkplugNamespace.VersionB, "group1", "edge1", "device1", this.metricsB, 0, 1, dateTime);
        var payloadVersionDeviceData = PayloadHelper.Deserialize<VersionBProtoBufPayload>(messageDeviceData.Payload);
        Assert.IsNotNull(payloadVersionDeviceData);
        Assert.IsNotNull(payloadVersionDeviceData.Seq);
        Assert.IsTrue(payloadVersionDeviceData.Seq.HasValue);
        Assert.AreEqual<ulong>(0, payloadVersionDeviceData.Seq.Value);
    }
}
