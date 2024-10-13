using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BytePusherCsharp.src;

public class MgBytepusherIODriver : IBytePusherIODriver
{
    public Texture2D? Texture { get; set; }

    private GraphicsDevice gd;
    private DynamicSoundEffectInstance soundEffect;

    public MgBytepusherIODriver(GraphicsDevice gd)
    {
        this.gd = gd;
        soundEffect = new DynamicSoundEffectInstance(15360, AudioChannels.Mono);
        soundEffect.Play();
    }

    public ushort GetKeyPress()
    {
        ushort key = 0;
        foreach (var k in Keyboard.GetState().GetPressedKeys())
        {
            switch (k)
            {
                case Keys.D0: key += 1; break;
                case Keys.D1: key += 2; break;
                case Keys.D2: key += 4; break;
                case Keys.D3: key += 8; break;
                case Keys.D4: key += 16; break;
                case Keys.D5: key += 32; break;
                case Keys.D6: key += 64; break;
                case Keys.D7: key += 128; break;
                case Keys.D8: key += 256; break;
                case Keys.D9: key += 512; break;
                case Keys.A: key += 1024; break;
                case Keys.B: key += 2048; break;
                case Keys.C: key += 4096; break;
                case Keys.D: key += 8192; break;
                case Keys.E: key += 16384; break;
                case Keys.F: key += 32768; break;
            }
        }

        return key;
    }

    public void RenderAudioFrame(byte[] data)
    {
        // Convert to 16 bit data
        var audio = new byte[data.Length * 2];
        var z = 0;
        foreach (var d in data)
        {
            audio[z++] = 0;
            audio[z++] = d;
        }
        // Submit audio buffer
        soundEffect.SubmitBuffer(audio);
    }

    public void RenderDisplayFrame(byte[] data)
    {
        // Convert 8 bit data to 24 bit RGB
        var rgbuffer = new Color[256*256];
        var z = 0;

        foreach (var c in data)
        {
            if (c > 215)
            {
                z++;
            }
            else
            {
                int blue = c % 6;
                int green = ((c-blue) / 6) % 6;
                int red = ((c-blue - (6*green)) / 36) % 6;

                rgbuffer[z++] = new Color(red*0x33, green*0x33, blue*0x33);
            }
        }

        Texture = new Texture2D(gd, 256, 256);
        Texture.SetData(rgbuffer);
    }
}