using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using NOVA;
using NOVA.ScreenManagement;
using NOVA.ScreenManagement.BaseScreens;

namespace _4Balls
{
    public class MainMenuScreen : MenuScreen
    {
        int hohe;
        int breite;

        public MainMenuScreen(int hohe1, int breite1) : base("4 Balls")
            {    
                MenuEntry einfachEntry = new MenuEntry("Einfach");
                MenuEntry mittelEntry = new MenuEntry("Mittel");
                MenuEntry schwerEntry = new MenuEntry("Schwer");
                einfachEntry.Selected += einfachSelected;
                mittelEntry.Selected += mittelSelected;
                schwerEntry.Selected += schwerSelected;
                MenuEntries.Add(einfachEntry);
                MenuEntries.Add(mittelEntry);
                MenuEntries.Add(schwerEntry);

                hohe = hohe1;
                breite = breite1;
            }
        void einfachSelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(Core.ScreenManager, true, e.PlayerIndex, new Spiel(31, hohe, breite));
        }
        void mittelSelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(Core.ScreenManager, true, e.PlayerIndex, new Spiel(16, hohe, breite));
        }
        void schwerSelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(Core.ScreenManager, true, e.PlayerIndex, new Spiel(6, hohe, breite));
        }
    }
}
