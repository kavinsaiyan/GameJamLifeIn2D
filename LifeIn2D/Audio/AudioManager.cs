using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace LifeIn2D.Audio
{
    public class AudioManager
    {
        public static AudioManager instance;
        public SoundEffect clickSoundEffect;
        public AudioManager(ContentManager content)
        {
            clickSoundEffect = content.Load<SoundEffect>("ClickTile");
            instance = this;
        }
    }
}