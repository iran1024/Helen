using Google.Protobuf;

namespace HelenServer.Core;

public interface IEventMessageConverter
{
    TMessage ToMessageModel<TMessage>(ByteString? request);
}