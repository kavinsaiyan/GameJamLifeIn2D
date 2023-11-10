using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace LifeIn2D.Audio
{
    public class AudioManager
    {
        private SoundEffect _clickSoundEffect;
        private SoundEffect _bgMusicEffect;
        private SoundEffectInstance _bgSoundEffectInstance;
        public AudioManager(ContentManager content)
        {
            _clickSoundEffect = content.Load<SoundEffect>("Audio/ClickTile");
            _bgMusicEffect = content.Load<SoundEffect>("Audio/BGTrack");
            _bgSoundEffectInstance = _bgMusicEffect.CreateInstance();
            _bgSoundEffectInstance.IsLooped = true;
            _bgSoundEffectInstance.Play();
        }

        public void PlayClickSound()
        {
            _clickSoundEffect.Play();
        }
    }
}