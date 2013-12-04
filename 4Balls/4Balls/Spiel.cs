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

        States currentState;
//        List<BoxObject> boxen = new List<BoxObject>();

        BoxObject ground;
        int player;
        int x;
        int y;
        int z;
        bool[,,,] pos = new bool[4,4,4,2];
        BoxObject fallingBox;
        EventHandler<CollisionArgs> collidedHandler;

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

            for (int i = -3; i <= s; i++)
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
            for (int i = -3; i <= s; i++)
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
            for (int i = -3; i <= s; i++)
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
            for (int i = -3; i <= s; i++)
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
            for (int i = -3; i <= s; i++)
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
            for (int i = -3; i <= s; i++)
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
            for (int i = -3; i <= s; i++)
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
            for (int i = -3; i <= s; i++)
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
            for (int i = -3; i <= s; i++)
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
            for (int i = -3; i <= s; i++)
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
            for (int i = -3; i <= s; i++)
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

            return false;

        }


        public override void Initialize()
        {
            base.Initialize();
            Scene.Camera = new CameraObject(new Vector3(30, 80, 100), new Vector3(0, 20, 0));
            Scene.Physics.ForceUpdater.Gravity = new Vector3(0, -9.81f, 0);
            //Scene.Physics.Solver.
            currentState = States.Start;

            collidedHandler = new EventHandler<CollisionArgs>(BoxCollidedHandler);

            player = 0;
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            switch (currentState)
            {
                case States.Start:

                    ground = new BoxObject(new Vector3(0, 0, 0), new Vector3(50f, 1f, 50f), 0f);
                    Scene.Add(ground);

                    currentState = States.Boxeinfuegen;
                    break;

                case States.Boxeinfuegen:
                    x = 5;
                    y = 50;
                    z = 5;
                    fallingBox = new BoxObject(new Vector3(x, y, z), new Vector3(9f, 9f, 9f), 10f);
                    fallingBox.Physics.IsAffectedByGravity = false;
                    
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

                    fallingBox.RenderMaterial = FallingBoxRenMat;

                    Console.WriteLine("Box eingefügt");
                    Scene.Add(fallingBox);
//                  boxen.Add(fallingBox);

                    currentState = States.Bewegen;


                    break;

                case States.Bewegen:
                    UI2DRenderer.WriteText(Vector2.Zero, fallingBox.Physics.LinearVelocity.Length().ToString(), Color.Blue, null, Vector2.One, UI2DRenderer.HorizontalAlignment.Center, UI2DRenderer.VerticalAlignment.Bottom);
                    break;

                case States.Wait:
                    if (fallingBox.Physics.LinearVelocity.Length() <= 0f)
                    {
                        fallingBox.Collided += collidedHandler;
                        fallingBox.MoveToPosition(new Vector3(x, (y - 0.1f), z));
                        fallingBox.Physics.IsAffectedByGravity = true;
                    }
                    break;

                case States.Gewinn:

                    break;

                case States.End:

                    break;
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

        }



        void BoxCollidedHandler(object sender, CollisionArgs e)
        {
            Console.WriteLine(" Collision ");


            fallingBox.Collided -= collidedHandler;
            int a;

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
            }
            else
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

        }

        public override void Draw(GameTime gameTime)
        {
            string text = "";
            Color color = new Color();
            switch (currentState)
            {
                case States.Start:
                case States.Boxeinfuegen:
                    text = " einfuegen";
                    color = Color.Yellow;
                    break;
                case States.Bewegen:
                    break;
                case States.Wait:
                    break;
                case States.Gewinn:
                    text = " Gewonnen! ";
                    color = Color.Yellow;
                    break;
                case States.End:
                    text = " Press Space to restart !";
                    color = Color.Blue;
                    break;
            }
            UI2DRenderer.WriteText(Vector2.Zero, text, color, null, Vector2.One, UI2DRenderer.HorizontalAlignment.Center, UI2DRenderer.VerticalAlignment.Bottom);
            base.Draw(gameTime);
        }

        public override void HandleInput(InputState input)
        {
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
                        if (x > -15)
                        {
                            x = x - 10;
                            fallingBox.MoveToPosition(new Vector3(x, y, z));
                            //Console.WriteLine(String.Format("{0} {1}", fallingBox.Position.X, fallingBox.Position.Y));
                        }
                    }
                    if (input.WasKeyPressed(Keys.Right, PlayerIndex.One))
                    {
                        if (x < 15)
                        {
                            x = x + 10;
                            fallingBox.MoveToPosition(new Vector3(x, y, z));
                        }
                    }
                    if (input.WasKeyPressed(Keys.Up, PlayerIndex.One))
                    {
                        if (z > -15)
                        {
                            z = z - 10;
                            fallingBox.MoveToPosition(new Vector3(x, y, z));
                        }
                    }
                    if (input.WasKeyPressed(Keys.Down, PlayerIndex.One))
                    {
                        if (z < 15)
                        {
                            z = z + 10;
                            fallingBox.MoveToPosition(new Vector3(x, y, z));
                        }
                    }
                    if (input.WasKeyPressed(Keys.Space, PlayerIndex.One) && (pos[umrechner(x), 3, umrechner(z), 0] == false) && (pos[umrechner(x), 3, umrechner(z), 1] == false))
                    {
                        Console.WriteLine(String.Format("{0}", fallingBox.Physics.LinearVelocity.Length().ToString()));
                        currentState = States.Wait;
                    }
                    break;

                case States.Wait:
                    break;

                case States.Gewinn:
                    if (input.WasKeyPressed(Keys.Space, PlayerIndex.One))
                    {
 //                       Scene.PausePhysics = false;
 //                       currentState = States.End;

                    }
                    break;
                case States.End:
                    if (input.WasKeyPressed(Keys.Space, PlayerIndex.One))
                    {
  //                      Scene.RemoveAllSceneObjects();
  //                      currentState = States.Start;
                    }
                    break;
            }
            base.HandleInput(input);
        }

    }



}
