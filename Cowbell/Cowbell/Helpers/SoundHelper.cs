using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;


namespace Centapp.Cowbell.Helpers
{
    class SoundHelper
    {
        public static void PlaySound(SoundEffect soundEffect)
        {
            FrameworkDispatcher.Update();
            soundEffect.Play();
        }
    }
}
