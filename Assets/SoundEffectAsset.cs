using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using NVorbis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.Assets
{
    public class SoundEffectAsset : AssetBase
    {
        public SoundEffect SoundEffect { get; }

        internal SoundEffectAsset(AssetID<SoundEffectAsset> assetID, SoundEffect soundEffect) : base(assetID)
        {
            SoundEffect = soundEffect;
        }
    }

    internal class OwnedSoundEffectAsset : OwnedAsset
    {
        public SoundEffect? SoundEffect { get; private set; }
        public AssetID<SoundEffectAsset> AssetID { get; }

        private OwnedSoundEffectAsset(SoundEffect soundEffect, AssetID<SoundEffectAsset> assetID)
        {
            SoundEffect = soundEffect;
            AssetID = assetID;
        }

        internal static OwnedSoundEffectAsset Load(AssetID<SoundEffectAsset> assetID)
        {
            try
            {
                SoundEffect soundEffect;
                if (assetID.Path.EndsWith(".ogg"))
                {
                    soundEffect = ReadOgg.ReadSoundEffect(assetID.Path);
                }
                else if (assetID.Path.EndsWith(".wav"))
                {
                    soundEffect = SoundEffect.FromFile(assetID.Path);
                }
                else
                {
                    throw new InvalidFileExtensionException("Sound effects must be .ogg or .wav");
                }
                return new OwnedSoundEffectAsset(soundEffect, assetID);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AssetNotFoundException(assetID);
            }
        }

        internal override AssetBase GetAsset()
        {
            if (SoundEffect is null)
                throw new AssetAlreadyUnloadedException(AssetID);
            return new SoundEffectAsset(AssetID, SoundEffect);
        }

        internal override void Unload()
        {
            if (SoundEffect is null)
                throw new AssetAlreadyUnloadedException(AssetID);
            SoundEffect.Dispose();
            SoundEffect = null;
        }
    }

    // REF: https://www.notfruit.net/2023/11/12/read-ogg-monogame.html
    internal static class ReadOgg
    {
        private static void ConvertFloatBufferToShortBuffer(
            float[] inBuffer, short[] outBuffer, int length)
        {
            // The float[] we get from NVorbis has the range [-1f, 1f],
            // we need to convert that to a short[] with a range of [-32768, 32767]
            for (var i = 0; i < length; i++)
            {
                var temp = (int)(short.MaxValue * inBuffer[i]);
                temp = Math.Clamp(temp, short.MinValue, short.MaxValue);
                outBuffer[i] = (short)temp;
            }
        }

        public static SoundEffect ReadSoundEffect(string fullFileName)
        {
            // VorbisReader comes from NVorbis.
            using var vorbis = new VorbisReader(fullFileName);

            // TotalSamples is actually in Frames, 
            // so we need to multiply it by channels to get Samples.
            var frames = new float[vorbis.TotalSamples * vorbis.Channels];

            // Read all frames (again, NVorbis calls them Samples), 
            // starting at index 0 and reading to the end.
            var length = vorbis.ReadSamples(frames, 0, frames.Length);

            // frames is a float[], we need a short[].
            var castBuffer = new short[length];
            ConvertFloatBufferToShortBuffer(frames, castBuffer, castBuffer.Length);

            // Now that we have the sound represented as a short[], 
            // we need to convert that to bytes. 
            // Each short is 2 bytes long, so we need 2X as many bytes
            // as we have shorts.
            var bytes = new byte[castBuffer.Length * 2];

            for (var i = 0; i < castBuffer.Length; i++)
            {
                var b = BitConverter.GetBytes(castBuffer[i]);
                bytes[i * 2] = b[0];
                bytes[i * 2 + 1] = b[1];
            }

            // Finally, we convert the vorbis.Channels count to the AudioChannels enum. 
            // Casting like this: `(AudioChannels) vorbis.Channels` would also work.
            var channels = vorbis.Channels == 2 ? AudioChannels.Stereo : AudioChannels.Mono;

            // Put it all together!
            return new SoundEffect(bytes, vorbis.SampleRate, channels);
        }
    }
}
