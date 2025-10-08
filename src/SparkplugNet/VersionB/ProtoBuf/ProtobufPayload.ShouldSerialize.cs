namespace SparkplugNet.VersionB.ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// This partial class implements ShoudSerializeSeq which is call when serializing.
/// It determines whether or not the Seq property shourl be included in the serialized payload
/// For NDEATH messages the Seq property should be omitted accordign to the 3.0 specification.
/// For all other message types the Seq property should be included. Especially seq = 0 for NBIRTH messages. Required for compliance with Ignition.
/// </summary>
internal partial class ProtoBufPayload
{
    public SparkplugMessageType MessageType { get; set; }

    // Conditional serialization: include Seq only for NBIRTH, DBIRTH, DDATA
    public bool ShouldSerializeSeq()
    {
        return this.MessageType != SparkplugMessageType.NodeDeath;
    }
}
