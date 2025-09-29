namespace SparkplugNet.VersionB.ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class ProtobufPayloadDeath : ProtobufPayloadBase
{
    [global::ProtoBuf.ProtoMember(3, Name = @"seq", IsRequired = false)]
    public override ulong? Seq { get; set; }
}
