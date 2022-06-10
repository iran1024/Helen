using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using ProtoBuf;

namespace HelenServer.Dapr;

internal static class GrpcUtilities
{
    internal static Any ToAny<T>(T model)
    {
        using var resultStream = new MemoryStream();

        Serializer.Serialize(resultStream, model);

        resultStream.Seek(0, 0);

        var data = ByteString.FromStream(resultStream);

        return new Any { Value = data };
    }

    internal static T ToModel<T>(Any any)
    {
        var inputBytes = any.Value.ToByteArray();

        using var inputStream = new MemoryStream(inputBytes);

        var model = Serializer.Deserialize<T>(inputStream);

        return model;
    }
}
