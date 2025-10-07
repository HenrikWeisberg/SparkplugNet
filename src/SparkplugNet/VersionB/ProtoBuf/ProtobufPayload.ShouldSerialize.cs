namespace SparkplugNet.VersionB.ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

internal partial class ProtoBufPayload
{
    public SparkplugMessageType MessageType { get; set; }

    // Conditional serialization: include Seq only for NBIRTH, DBIRTH, DDATA
    public bool ShouldSerializeSeq()
    {
        return this.MessageType != SparkplugMessageType.NodeDeath;
    }
}
