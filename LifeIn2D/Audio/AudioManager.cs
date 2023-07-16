using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace LifeIn2D.Audio
{
    public class AudioManager
    {
        private SoundEffect _clickSoundEffect;
        public AudioManager(ContentManager content)
        {
            _clickSoundEffect = content.Load<SoundEffect>("Audio/ClickTile");
        }

        public void PlayClickSound()
        {
            _clickSoundEffect.Play();
        }
    }
}