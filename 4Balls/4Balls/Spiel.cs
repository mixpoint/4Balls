using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NOVA.Components.Kinect;
using NOVA.UI;
using NOVA.ScreenManagement.BaseScreens;
using NOVA.ScreenManagement;
using NOVA.Utilities;
using NOVA.Scenery;
using System.Collections.Generic;
using Microsoft.Kinect;
using System;
using System.Linq;
using System.Text;
using BEPUphysics.Collidables;
using BEPUphysics.Collidables.Events;
using BEPUphysics.Collidables.MobileCollidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using NOVA.Graphics;
using NOVA.Sound;
using System.Timers;
using Microsoft.Xna.Framework.Audio;


// Master
namespace _4Balls
{
    class Spiel : GameplayScreen
    {
        public enum States
        {
            Start,
            Boxeinfuegen,
            Bewegen,
            Wait,
            Gewinn,
            End
        }

        double lastGameTime = 0;
        States currentState;
//        List<BoxObject> boxen = new List<BoxObject>();
        bool timerRunning = false;
        System.Timers.Timer timerObj = new System.Timers.Timer() { Interval = 1000, Enabled = true};
        BoxObject ground;
        int player;
        int x;
        int y;
        int z;
        int zeit;
        int screenhohe;
        int screenbreite;
        bool[,,,] pos = new bool[4,4,4,2];
        double winkel;
        double winkelcam;
        bool markerint;
        bool downset;
        bool soundisplay;
        BoxObject fallingBox;
        BoxObject marker;
        RenderMaterial MarkerRenMat = new RenderMaterial();
        BoxObject PHalter;
        RenderMaterial PHalterMat = new RenderMaterial();
        EventHandler<CollisionArgs> collidedHandler;

        public Spiel(int spielzeit, int hohe, int breite)
        {
            zeit = spielzeit;
            screenhohe = hohe;
            screenbreite = breite;
        }

        void timerHandlerTemp(object source, ElapsedEventArgs e)
        {
            timerRunning = false;
            timerObj.Stop();
            timerObj.Interval = zeit * 1000;
            if ((pos[umrechner(x), 3, umrechner(z), 0] == false) && (pos[umrechner(x), 3, umrechner(z), 1] == false) && downset == false)
            {
                if (downset == false)
                {
                    Console.WriteLine(String.Format("{0}", fallingBox.Physics.LinearVelocity.Length().ToString()));
                    downset = true;
                    Scene.Remove(marker);
                    marker = null;

                    for (int i = 0; i < 4; i++)
                    {
                        if ((pos[umrechner(x), i, umrechner(z), 0] == false) && (pos[umrechner(x), i, umrechner(z), 1] == false))
                        {
                            if (i != 0)
                            {
                                PHalter = new BoxObject(new Vector3(x, ((i - 1) * 9 + 7.6f), z), new Vector3(5f, 4f, 5f), 0f);
                                PHalterMat.Transparency = 1f;
                                PHalter.RenderMaterial = PHalterMat;
                                Scene.Add(PHalter);
                                break;
                            }
                            else
                            {
                                break;
                            }

                        }
                    }
                    currentState = States.Wait;
                }
            } else
            {
                if (downset == false)
                {
                    downset = true;
                    Scene.Remove(fallingBox);
                    switch (player)
                    {
                        case 0:
                            player = 1;
                            break;

                        case 1:
                            player = 0;
                            break;
                    }
                    currentState = States.Boxeinfuegen;
                }

            }

        }
        #region Rechnen - Vier Gewinnt :)
        int umrechner(int x)
        {
            switch (x)
            {
                case -15:
                    return 0;

                case -5:
                    return 1;

                case 5:
                    return 2;

                case 15:
                    return 3;

                default:
                    Console.WriteLine("Fehler");
                    return 5;

            }

        }


        bool win(int s)
        {
            int matches = 0;

            for (int i = -3; i <= 3; i++)
            {
                if ((x + i >= 0) && (x + i <= s))
                {
                    if (pos[(x + i), y, z, player] == true)
                    {
                        matches++;
                        if (matches == 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        matches = 0;
                    }

                }
            }

            matches = 0;
            for (int i = -3; i <= 3; i++)
            {
                if ((y + i >= 0) && (y + i <= s))
                {
                    if (pos[x, (y + i), z, player] == true)
                    {
                        matches++;
                        if (matches == 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        matches = 0;
                    }

                }
            }


            matches = 0;
            for (int i = -3; i <= 3; i++)
            {
                if ((z + i >= 0) && (z + i <= s))
                {
                    if (pos[x, y, (z + i), player] == true)
                    {
                        matches++;
                        if (matches == 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        matches = 0;
                    }

                }
            }

            matches = 0;
            for (int i = -3; i <= 3; i++)
            {
                if ((x + i >= 0) && (x + i <= s) && (y + i >= 0) && (y + i <= s))
                {
                    if (pos[(x + i), (y + i), z, player] == true)
                    {
                        matches++;
                        if (matches == 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        matches = 0;
                    }

                }
            }


            matches = 0;
            for (int i = -3; i <= 3; i++)
            {
                if ((x + i >= 0) && (x + i <= s) && (y - i <= s) && (y - i >= 0))
                {
                    if (pos[(x + i), (y - i), z, player] == true)
                    {
                        matches++;
                        if (matches == 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        matches = 0;
                    }

                }
            }

            matches = 0;
            for (int i = -3; i <= 3; i++)
            {
                if ((z + i >= 0) && (z + i <= s) && (y + i >= 0) && (y + i <= s))
                {
                    if (pos[x, (y + i), (z + i), player] == true)
                    {
                        matches++;
                        if (matches == 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        matches = 0;
                    }

                }
            }


            matches = 0;
            for (int i = -3; i <= 3; i++)
            {
                if ((z + i >= 0) && (z + i <= s) && (y - i <= s) && (y - i >= 0))
                {
                    if (pos[x, (y - i), (z + i), player] == true)
                    {
                        matches++;
                        if (matches == 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        matches = 0;
                    }

                }
            }

            matches = 0;
            for (int i = -3; i <= 3; i++)
            {
                if ((x + i >= 0) && (x + i <= s) && (y + i >= 0) && (y + i <= s) && (z + i >= 0) && (z + i <= s))
                {
                    if (pos[(x + i), (y + i), (z + i), player] == true)
                    {
                        matches++;
                        if (matches == 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        matches = 0;
                    }

                }
            }


            matches = 0;
            for (int i = -3; i <= 3; i++)
            {
                if ((x + i >= 0) && (x + i <= s) && (y + i >= 0) && (y + i <= s) && (z - i <= s) && (z - i >= 0))
                {
                    if (pos[(x + i), (y + i), (z - i), player] == true)
                    {
                        matches++;
                        if (matches == 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        matches = 0;
                    }

                }
            }

            matches = 0;
            for (int i = -3; i <= 3; i++)
            {
                if ((x + i >= 0) && (x + i <= s) && (y - i <= s) && (y - i >= 0) && (z + i >= 0) && (z + i <= s))
                {
                    if (pos[(x + i), (y - i), (z + i), player] == true)
                    {
                        matches++;
                        if (matches == 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        matches = 0;
                    }

                }
            }

            matches = 0;
            for (int i = -3; i <= 3; i++)
            {
                if ((x + i >= 0) && (x + i <= s) && (y - i <= s) && (y - i >= 0) && (z - i <= s) && (z - i >= 0))
                {
                    if (pos[(x + i), (y - i), (z - i), player] == true)
                    {
                        matches++;
                        if (matches == 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        matches = 0;
                    }

                }
            }

            matches = 0;
            for (int i = -3; i <= 3; i++)
            {
                if ((x + i >= 0) && (x + i <= s) && (z + i >= 0) && (z + i <= s))
                {
                    if (pos[(x + i), y, (z + i), player] == true)
                    {
                        matches++;
                        if (matches == 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        matches = 0;
                    }

                }
            }

            matches = 0;
            for (int i = -3; i <= 3; i++)
            {
                if ((x + i >= 0) && (x + i <= s) && (z - i <= s) && (z - i >= 0))
                {
                    if (pos[(x + i), y, (z - i), player] == true)
                    {
                        matches++;
                        if (matches == 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        matches = 0;
                    }

                }
            }

            return false;

        }

        void markerupate()
        {
            
            for (int i = 0; i < 4; i++)
            {
                if (markerint == false)
                {
                    markerint = true;
                }
                else
                {

                    Scene.Remove(marker);
                }
                marker = null;
                if ((pos[umrechner(x), i, umrechner(z), 0] == false) && (pos[umrechner(x), i, umrechner(z), 1] == false))
                {
                    if (i == 0)
                    {
                        marker = new BoxObject(new Vector3(x, (0.6f), z), new Vector3(5f, 0.1f, 5f), 0f);
                    }
                    else
                    {
                        marker = new BoxObject(new Vector3(x, ((i - 1) * 9f + 5.6f), z), new Vector3(5f, 0.1f, 5f), 0f);
                    }
                    marker.RenderMaterial = MarkerRenMat;
                    Scene.Add(marker);


                    if (soundisplay == false)
                    {
                        soundisplay = false;
                        SoundEffect sound = Scene.Game.Content.Load<SoundEffect>("move");
                        SoundObject downSound = new SoundObject(new Vector3(x, ((i - 1) * 9f + 5.6f), z), sound, false);
                        Scene.Add(sound);
                        sound.Play();
                    }

  

                    break;
                }
            }
        }
#endregion
        #region Nova functions
        public override void Initialize()
        {
            base.Initialize();
//            Scene.RenderType = RenderType.ForwardRenderer;
            Scene.Camera = new CameraObject(new Vector3(100 * (float)Math.Cos(winkelcam), 80, 100 * (float)Math.Sin(winkelcam)), new Vector3(0, 20, 0));
            Scene.Physics.ForceUpdater.Gravity = new Vector3(0, -900.81f, 0);
            currentState = States.Start;
            Scene.ShowCollisionMeshes = false;
            collidedHandler = new EventHandler<CollisionArgs>(BoxCollidedHandler);

            BEPUphysics.Settings.MotionSettings.DefaultPositionUpdateMode = BEPUphysics.PositionUpdating.PositionUpdateMode.Continuous;
            BEPUphysics.Settings.CollisionDetectionSettings.AllowedPenetration = 0.4f;
//            BEPUphysics.Settings.CollisionResponseSettings.PenetrationRecoveryStiffness = 10f;
            BEPUphysics.Settings.CollisionDetectionSettings.DefaultMargin = 0.4f;
            BEPUphysics.Settings.CollisionResponseSettings.MaximumPenetrationCorrectionSpeed = 100000f;
//            Scene.ShowTriangleCount = true;
            soundisplay = true;

            winkel = Math.PI / 4;
            winkelcam = 0;

            markerint = false;
            
            player = 0;
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            switch (currentState)
            {
                case States.Start:

                    timerObj.Interval = zeit * 1000;
                    player = 0;
                    ground = new BoxObject(new Vector3(0, 0, 0), new Vector3(50f, 1f, 50f), 0f);
                    ground.RenderMaterial.Diffuse = new Microsoft.Xna.Framework.Vector4(1, 1, 1, 1);
                    ground.PhysicsMaterial.Bounciness = 0f;
                    Scene.Add(ground);

                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            BoxObject groundPlane = new BoxObject(new Vector3(-15 + i * 10, 0.55f, -15 + j * 10), new Vector3(9f, 0.1f, 9f), 0f);
                            groundPlane.PhysicsMaterial.Bounciness = 0f;
                            groundPlane.RenderMaterial.Diffuse = new Microsoft.Xna.Framework.Vector4(10, 1, 1, 1);
                            Scene.Add(groundPlane);
                        }
                    }


                    
                    currentState = States.Boxeinfuegen;
                    break;

                case States.Boxeinfuegen:

                    downset = false;
                    x = 5;
                    y = 50;
                    z = 5;
                    BoxObject fallingBox2 = new BoxObject(new Vector3(x, y, z), new Vector3(5f, 5f, 5f), 1f);
                    fallingBox2.Physics.IsAffectedByGravity = false;
                    fallingBox2.PhysicsMaterial.Bounciness = 0f;
                    fallingBox2.Collided += collidedHandler;

                    RenderMaterial FallingBoxRenMat = new RenderMaterial();

                    switch (player) 
                    {
                        case 0:
                            FallingBoxRenMat.Diffuse = Color.Red.ToVector4();
                            break;

                        case 1:
                            FallingBoxRenMat.Diffuse = Color.Green.ToVector4();
                            break;
                    }


                    fallingBox2.RenderMaterial = FallingBoxRenMat;


                    Console.WriteLine("Box eingefügt");
                    Scene.Add(fallingBox2);
                    fallingBox = fallingBox2;

                    MarkerRenMat.Diffuse = Color.Blue.ToVector4();

                    markerupate();

                    soundisplay = false;

                    currentState = States.Bewegen;


                    break;

                case States.Bewegen:
                    //UI2DRenderer.WriteText(Vector2.Zero, fallingBox.Physics.LinearVelocity.Length().ToString(), Color.Blue, null, Vector2.One, UI2DRenderer.HorizontalAlignment.Center, UI2DRenderer.VerticalAlignment.Bottom);
                    if (!timerRunning)
                    {
                        timerObj.Start();
                        timerRunning = true;
                        timerObj.Elapsed += new System.Timers.ElapsedEventHandler(timerHandlerTemp);
                        lastGameTime = gameTime.TotalGameTime.TotalSeconds;
                    }
                    else
                    {
                        UI2DRenderer.WriteText(Vector2.Zero, "Spieler: " + (player + 1), Color.Red, null, Vector2.One * (NOVA.Core.Graphics.PreferredBackBufferWidth / screenbreite), UI2DRenderer.HorizontalAlignment.Left, UI2DRenderer.VerticalAlignment.Bottom);
                        UI2DRenderer.WriteText(Vector2.Zero, "Verbleibende Zeit: " + ((int)(zeit - (gameTime.TotalGameTime.TotalSeconds - lastGameTime))).ToString() + " Sekunden", Color.Red, null, Vector2.One * (NOVA.Core.Graphics.PreferredBackBufferWidth / screenbreite), UI2DRenderer.HorizontalAlignment.Right, UI2DRenderer.VerticalAlignment.Bottom);
                            
                    }

                    break;

                case States.Wait:
                    if (fallingBox.Physics.LinearVelocity.Length() <= 0f)
                    {
                        
                        //fallingBox.MoveToPosition(new Vector3(x, (y - 0.1f), z));                        
                        fallingBox.Physics.IsAffectedByGravity = true;
                        fallingBox.Physics.LinearVelocity = -Vector3.UnitY*1;
                    }
                    break;

                case States.Gewinn:
                    if (soundisplay == false)
                    {
                        soundisplay = true;
                        SoundEffect sound = Scene.Game.Content.Load<SoundEffect>("win");
                        SoundObject winSound = new SoundObject(new Vector3(0, 10, 0), sound, false);
                        Scene.Add(sound);
                        sound.Play();
                    }
                    break;

                case States.End:

                    break;
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

        }


        void BoxCollidedHandler(object sender, CollisionArgs e)
        {
            Console.WriteLine(" Collision ");

            BoxObject box = sender as BoxObject;
            box.Collided -= collidedHandler;            
            box.Physics.LinearVelocity = Vector3.Zero;
           
            for (int i = 0; i < 4; i++)
            {
                if ((pos[umrechner(x), i, umrechner(z), 0] == false) && (pos[umrechner(x), i, umrechner(z), 1] == false))
                {
                    if (soundisplay == false)
                    {
                        soundisplay = true;
                        SoundEffect sound = Scene.Game.Content.Load<SoundEffect>("down");
                        SoundObject downSound = new SoundObject(new Vector3(x, ((i - 1) * 9f + 5.6f), z), sound, false);
                        Scene.Add(sound);
                        sound.Play();
                    }
                    break;
                }
            }
            

            x = umrechner(x);
            z = umrechner(z);

            for (int i = 0; i < 4; i++)
            {
                if ((pos[x, i, z, 0] == false) && (pos[x, i, z, 1] == false))
                {
                    pos[x, i, z, player] = true;
                    y = i;
                    break;
                }
            }





            if (win(3) == true)
            {
                currentState = States.Gewinn;
                soundisplay = false;
            }
            else
            {
                int voll = 0;
                for (int i = 0; i < 4; i++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        for (int l = 0; l < 2; l++)
                        {
                            if (pos[i, 3, k, l] == true)
                            {
                                voll = voll + 1;

                            }
                        }
                                
                    }
                }


                if (voll != 16)
                {
                    switch (player)
                    {
                        case 0:
                            player = 1;
                            break;

                        case 1:
                            player = 0;
                            break;
                    }
                    currentState = States.Boxeinfuegen;
                }
                else
                {
                    currentState = States.End;
                }


            }

        }

        public override void Draw(GameTime gameTime)
        {
            string text = "";
            Color color = new Color();
            switch (currentState)
            {
                case States.Start:
                    break;
                case States.Boxeinfuegen:
                    break;
                case States.Bewegen:
                    break;
                case States.Wait:
                    break;
                case States.Gewinn:
                    text = " Spieler " + (player + 1) + " hat gewonnen!";
                    color = Color.Red;
                    break;
                case States.End:
                    text = "Kein Spieler hat gewonnen!";
                    color = Color.Red;
                    break;
            }
            UI2DRenderer.WriteText(Vector2.Zero, text, color, null, Vector2.One * (NOVA.Core.Graphics.PreferredBackBufferWidth / screenbreite), UI2DRenderer.HorizontalAlignment.Center, UI2DRenderer.VerticalAlignment.Bottom);
            base.Draw(gameTime);
        }

        public override void HandleInput(InputState input)
        {
           if (input.WasKeyPressed(Keys.A, PlayerIndex.One))
                    {
                         //NOVA.Core.Graphics.ToggleFullScreen();
                        //NOVA.Core.Graphics.IsFullScreen = ...;
                        //NOVA.Core.Graphics.ApplyChanges();
                        if (NOVA.Core.Graphics.IsFullScreen == true)
                        {
                            NOVA.Core.Graphics.PreferredBackBufferWidth = screenbreite;
                            NOVA.Core.Graphics.PreferredBackBufferHeight = screenhohe;
                            NOVA.Core.Graphics.ToggleFullScreen();
                        }
                        else
                        {
                            NOVA.Core.Graphics.PreferredBackBufferHeight = NOVA.Core.Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
                            NOVA.Core.Graphics.PreferredBackBufferWidth = NOVA.Core.Graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
                            NOVA.Core.Graphics.ToggleFullScreen();
                        }

                    }

           if (input.WasKeyPressed(Keys.K, PlayerIndex.One))
           {

               Console.WriteLine(Scene.Camera.ViewMatrix.ToString());
               winkel -= 0.1;
               winkelcam -= 0.1;
               Scene.Camera = new CameraObject(new Vector3(100 * (float)Math.Cos(winkelcam), 80, 100 * (float)Math.Sin(winkelcam)), new Vector3(0, 20, 0));
           }

           if (input.WasKeyPressed(Keys.L, PlayerIndex.One))
           {

               Console.WriteLine(Scene.Camera.ViewMatrix.ToString());
               winkel += 0.1;
               winkelcam += 0.1;
               Scene.Camera = new CameraObject(new Vector3(100 * (float)Math.Cos(winkelcam), 80, 100 * (float)Math.Sin(winkelcam)), new Vector3(0, 20, 0));
           }


            switch (currentState)
            {
                case States.Start:
                    break;
                case States.Boxeinfuegen:
                    if (input.WasKeyPressed(Keys.Space, PlayerIndex.One))
                    {
                        //box.Mass = 1f;
                        //box2.Mass = 100f;
                        //box.Physics.Material.Bounciness = 1.8f;
                    }
                    break;

                case States.Bewegen:
                    if (input.WasKeyPressed(Keys.Left, PlayerIndex.One))
                    {

                        if (Math.Sin(winkel) >= 0 && Math.Cos(winkel) >= 0)
                        {
                            if (z < 15)
                            {
                                z = z + 10;
                                fallingBox.MoveToPosition(new Vector3(x, y, z));
                                markerupate();
                            }
                        }

                        if (Math.Sin(winkel) >= 0 && Math.Cos(winkel) <= 0)
                        {
                            if (x > -15)
                            {
                                x = x - 10;
                                fallingBox.MoveToPosition(new Vector3(x, y, z));
                                markerupate();
                            }
                        }

                        if (Math.Sin(winkel) <= 0 && Math.Cos(winkel) <= 0)
                        {
                            if (z > -15)
                            {
                                z = z - 10;
                                fallingBox.MoveToPosition(new Vector3(x, y, z));
                                markerupate();
                            }
                        }

                        if (Math.Sin(winkel) <= 0 && Math.Cos(winkel) >= 0)
                        {
                            if (x < 15)
                            {
                                x = x + 10;
                                fallingBox.MoveToPosition(new Vector3(x, y, z));
                                markerupate();
                            }
                        }





                    }
                    if (input.WasKeyPressed(Keys.Right, PlayerIndex.One))
                    {
                        if (Math.Sin(winkel) >= 0 && Math.Cos(winkel) >= 0)
                        {
                            if (z > -15)
                            {
                                z = z - 10;
                                fallingBox.MoveToPosition(new Vector3(x, y, z));
                                markerupate();
                            }
                        }

                        if (Math.Sin(winkel) >= 0 && Math.Cos(winkel) <= 0)
                        {
                            if (x < 15)
                            {
                                x = x + 10;
                                fallingBox.MoveToPosition(new Vector3(x, y, z));
                                markerupate();
                            }
                        }

                        if (Math.Sin(winkel) <= 0 && Math.Cos(winkel) <= 0)
                        {
                            if (z < 15)
                            {
                                z = z + 10;
                                fallingBox.MoveToPosition(new Vector3(x, y, z));
                                markerupate();
                            }
                        }

                        if (Math.Sin(winkel) <= 0 && Math.Cos(winkel) >= 0)
                        {
                            if (x > -15)
                            {
                                x = x - 10;
                                fallingBox.MoveToPosition(new Vector3(x, y, z));
                                markerupate();
                            }
                        }



                    }
                    if (input.WasKeyPressed(Keys.Up, PlayerIndex.One))
                    {
                        if (Math.Sin(winkel) >= 0 && Math.Cos(winkel) >= 0)
                        {
                            if (x > -15)
                            {
                                x = x - 10;
                                fallingBox.MoveToPosition(new Vector3(x, y, z));
                                markerupate();
                            }
                        }


                        if (Math.Sin(winkel) >= 0 && Math.Cos(winkel) <= 0)
                        {
                            if (z > -15)
                            {
                                z = z - 10;
                                fallingBox.MoveToPosition(new Vector3(x, y, z));
                                markerupate();
                            }
                        }

                        if (Math.Sin(winkel) <= 0 && Math.Cos(winkel) <= 0)
                        {
                            if (x < 15)
                            {
                                x = x + 10;
                                fallingBox.MoveToPosition(new Vector3(x, y, z));
                                markerupate();
                            }
                        }

                        if (Math.Sin(winkel) <= 0 && Math.Cos(winkel) >= 0)
                        {
                            if (z < 15)
                            {
                                z = z + 10;
                                fallingBox.MoveToPosition(new Vector3(x, y, z));
                                markerupate();
                            }
                        }


                    }
                    if (input.WasKeyPressed(Keys.Down, PlayerIndex.One))
                    {
                        if (Math.Sin(winkel) >= 0 && Math.Cos(winkel) >= 0)
                        {
                            if (x < 15)
                            {
                                x = x + 10;
                                fallingBox.MoveToPosition(new Vector3(x, y, z));
                                markerupate();
                            }
                        }

                        if (Math.Sin(winkel) >= 0 && Math.Cos(winkel) <= 0)
                        {
                            if (z < 15)
                            {
                                z = z + 10;
                                fallingBox.MoveToPosition(new Vector3(x, y, z));
                                markerupate();
                            }
                        }

                        if (Math.Sin(winkel) <= 0 && Math.Cos(winkel) <= 0)
                        {
                            if (x > -15)
                            {
                                x = x - 10;
                                fallingBox.MoveToPosition(new Vector3(x, y, z));
                                markerupate();
                            }
                        }

                        if (Math.Sin(winkel) <= 0 && Math.Cos(winkel) >= 0)
                        {
                            if (z > -15)
                            {
                                z = z - 10;
                                fallingBox.MoveToPosition(new Vector3(x, y, z));
                                markerupate();
                            }
                        }
 
                    }
                    if (input.WasKeyPressed(Keys.Space, PlayerIndex.One) && (pos[umrechner(x), 3, umrechner(z), 0] == false) && (pos[umrechner(x), 3, umrechner(z), 1] == false))
                    {
                        Console.WriteLine(String.Format("{0}", fallingBox.Physics.LinearVelocity.Length().ToString()));
                        Scene.Remove(marker);
                        marker = null;
                        soundisplay = false;

                        for (int i = 0; i < 4; i++)
                        {
                            if ((pos[umrechner(x), i, umrechner(z), 0] == false) && (pos[umrechner(x), i, umrechner(z), 1] == false))
                            {
                                if (i != 0)
                                {
                                    PHalter = new BoxObject(new Vector3(x, ((i - 1) * 9 + 7.6f), z), new Vector3(5f, 4f, 5f), 0f);
                                    PHalterMat.Transparency = 1f;
                                    PHalter.RenderMaterial = PHalterMat;
                                    Scene.Add(PHalter);
                                    break;
                                }
                                else
                                {
                                    break;
                                }

                            }
                        }

                        timerRunning = false;
                        timerObj.Stop();
                        timerObj.Interval = zeit * 1000;

                        currentState = States.Wait;
                    }


                    break;

                case States.Wait:
                    break;

                case States.Gewinn:
                    if (input.WasKeyPressed(Keys.Space, PlayerIndex.One))
                    {
                        Scene.Clear();
                        for (int i = 0; i < 4; i++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                for (int k = 0; k < 4; k++)
                                {
                                    for (int l = 0; l < 2; l++)
                                    {
                                        pos[i, j, k, l] = false;
                                        
                                    }
                                }
                            }
                        }
                        markerint = false;
                        currentState = States.Start;

                    }
                    break;
                case States.End:
                    if (input.WasKeyPressed(Keys.Space, PlayerIndex.One))
                    {
                        Scene.Clear();
                        for (int i = 0; i < 4; i++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                for (int k = 0; k < 4; k++)
                                {
                                    for (int l = 0; l < 2; l++)
                                    {
                                        pos[i, j, k, l] = false;

                                    }
                                }
                            }
                        }
                        markerint = false;
                        currentState = States.Start;

                    }
                    break;
            }
            base.HandleInput(input);
        }
    }

        #endregion
}
