namespace BytePusherCsharp.src;

public class BytePusherVM
{
    byte[] memory = new byte[0xFFFFFF];
    IBytePusherIODriver ioDriver;

    public BytePusherVM(IBytePusherIODriver ioDriver)
    {
        this.ioDriver = ioDriver;
    }

    // TODO: Implement stream
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
        var s = ioDriver.GetKeyPress();
        memory[0] = (byte)((s & 0xFF00) >> 8);
        memory[1] = (byte)(s & 0xFF);
        var instructionCounter = 0x10000; // 65536
        int pc = GetAddress(2, 3);
        while (instructionCounter-- != 0)
        {
            memory[GetAddress(pc + 3, 3)] = memory[GetAddress(pc, 3)];
            pc = GetAddress(pc + 6, 3);
        }
        ioDriver.RenderAudioFrame(Copy(GetAddress(6, 2) << 8, 256));
        ioDriver.RenderDisplayFrame(Copy(GetAddress(5, 1) << 16, 256 * 256));
    }

    private int GetAddress(int pc, int length)
    {
        var address = 0;
        for (var i = 0; i < length; i++)
        {
            address = (address << 8) + memory[pc++];
        }
        
        return address;
    }

    private byte[] Copy(int start, int length)
    {
        var b = new byte[length];
        Array.Copy(memory, start, b, 0, length);
        return b;
    }
}