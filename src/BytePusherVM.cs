
public class BytePusherVM
{
    byte[] memory = new byte[0xFFFFFF];
    // BytePusherIODriver ioDriver;

    // public BytePusherVM(BytePusherIODriver ioDriver)
    // {
    //     this.ioDriver = ioDriver;
    // }

    public void Load(string rom)
    {
        memory = new byte[0xFFFFFF];
        using (FileStream fs = new FileStream(rom, FileMode.Open))
        {
            int pc = 0;
            int i = 0;
            while ((i = fs.ReadByte()) != -1)
            {
                memory[pc++] = (byte)i;
            }
        }
    }

    public void Run()
    {
        // var s = ioDriver.GetKeyPress();
        // memory[0] = (byte)((s & 0xFF00) >> 8);
        // memory[1] = (byte)(s & 0xFF);
        var i = 0x10000;
        int pc = GetValue(2, 3);
        while (i-- != 0)
        {
            memory[GetValue(pc + 3, 3)] = memory[GetValue(pc, 3)];
            pc = GetValue(pc + 6, 3);
        }
        // ioDriver.RenderAudioFrame(Copy(GetValue(6, 2) << 8, 256));
        // ioDriver.RenderDisplayFrame(Copy(GetValue(5, 1) << 16, 256 * 256));
    }

    private int GetValue(int pc, int length)
    {
        var v = 0;
        for (var i = 0; i < length; i++)
        {
            v = (v << 8) + memory[pc++];
        }
        
        return v;
    }

    private byte[] Copy(int start, int length)
    {
        var b = new byte[length];
        Array.Copy(memory, start, b, 0, length);
        return b;
    }
}