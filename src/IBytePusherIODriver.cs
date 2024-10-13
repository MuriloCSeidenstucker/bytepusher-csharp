namespace BytePusherCsharp.src;

public interface IBytePusherIODriver
{
    // Get the current pressed key (0-9 A-F)
    ushort GetKeyPress();

    // Render 256 bytes of audio
    void RenderAudioFrame(byte[] data);
    // Render 256*256 pixels
    void RenderDisplayFrame(byte[] data);
}