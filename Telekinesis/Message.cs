namespace Telekinesis;
public class Message
{
    public const int MSG_UNKNOWN = -1;
    public const int MSG_ERROR = 0;
    public const int MSG_CONNECT = 1;
    public const int MSG_DISCONNECT = 2;
    public const int MSG_PLAYER = 3;

    public int ID { get; set; }
    public string Data { get; set; }
    public string? Error { get; set; }


    public Message()
    {
        ID = -1;
        Data = "";
    }
}

