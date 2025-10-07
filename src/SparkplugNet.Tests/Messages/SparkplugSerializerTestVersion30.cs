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
    /// Test that seq numbers are compatible with Ignition and Eclipse Sparkplug B implementation.
    /// </summary>
    [TestMethod]
    public void TestSparkplugBSeqSerialization()
    {
        var dateTime = DateTimeOffset.UtcNow;

        // ---------- Node Messages ----------

        // NBIRTH
        var messageNodeBirth = this.messageGenerator.GetSparkplugNodeBirthMessage(
            SparkplugNamespace.VersionB, "group1", "edge1", this.metricsB, 0, 1, dateTime);
        var payloadNodeBirth = PayloadHelper.Deserialize<VersionBProtoBufPayload>(messageNodeBirth.Payload);
        Assert.IsNotNull(payloadNodeBirth);
        Assert.IsNotNull(payloadNodeBirth.Seq);
        Assert.AreEqual<ulong>(0, payloadNodeBirth.Seq.Value); // Seq = 0

        // NDEATH
        var messageNodeDeath = this.messageGenerator.GetSparkplugNodeDeathMessage(
            SparkplugNamespace.VersionB, "group1", "edge1", 1);
        var payloadNodeDeath = PayloadHelper.Deserialize<VersionBProtoBufPayload>(messageNodeDeath.Payload);
        Assert.IsNotNull(payloadNodeDeath);
        Assert.IsNull(payloadNodeDeath.Seq); // Seq omitted

        // ---------- Device Messages ----------

        // DBIRTH
        var messageDeviceBirth = this.messageGenerator.GetSparkplugDeviceBirthMessage(
            SparkplugNamespace.VersionB, "group1", "edge1", "device1", this.metricsB, 0, 1, dateTime);
        var payloadDeviceBirth = PayloadHelper.Deserialize<VersionBProtoBufPayload>(messageDeviceBirth.Payload);
        Assert.IsNotNull(payloadDeviceBirth);
        Assert.IsNotNull(payloadDeviceBirth.Seq);
        Assert.AreEqual<ulong>(0, payloadDeviceBirth.Seq.Value); // Seq = 0

        // DDEATH
        var messageDeviceDeath = this.messageGenerator.GetSparkplugDeviceDeathMessage(
            SparkplugNamespace.VersionB, "group1", "edge1", "device1", 1, 1, dateTime);
        var payloadDeviceDeath = PayloadHelper.Deserialize<VersionBProtoBufPayload>(messageDeviceDeath.Payload);
        Assert.IsNotNull(payloadDeviceDeath);
        Assert.IsNotNull(payloadDeviceDeath.Seq);
        Assert.AreEqual<ulong>(1, payloadDeviceDeath.Seq.Value); // Seq incremented from previous

        // DDATA
        var messageDeviceData = this.messageGenerator.GetSparkplugDeviceDataMessage(
            SparkplugNamespace.VersionB, "group1", "edge1", "device1", this.metricsB, 2, 1, dateTime);
        var payloadDeviceData = PayloadHelper.Deserialize<VersionBProtoBufPayload>(messageDeviceData.Payload);
        Assert.IsNotNull(payloadDeviceData);
        Assert.IsNotNull(payloadDeviceData.Seq);
        Assert.AreEqual<ulong>(2, payloadDeviceData.Seq.Value); // Seq incremented
    }
}
