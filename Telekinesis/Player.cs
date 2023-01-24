namespace Telekinesis;
public class Player
{
    public string username { get; set; }
    public int health { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    public float Speed { get; set; }

    public Player()
    {
        
    }

    public byte[] Serialize()
    {
        var data = new byte[sizeof(int) * 3];

        //foreach (var field in this.GetType().GetFields())
        //{
        //	var value = (field.GetType())field.GetValue(this)!;
        //         }

        Array.Copy(BitConverter.GetBytes(health), data, sizeof(int)); // might need htonl (big endian)
                                                                      //data.Concat(BitConverter.GetBytes(x));
                                                                      //data.Concat(BitConverter.GetBytes(y));
        Console.WriteLine(string.Join(" ", data));
        return data;
    }
}

