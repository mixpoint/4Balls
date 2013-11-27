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
            Warten,
            End
        }

        States currentState;
//        List<BoxObject> boxen = new List<BoxObject>();

        BoxObject ground;
        int player;
        int x;
        int y;
        int z;
        bool[,,,] pos1 = new bool[3,3,3,2];
        bool[,,,] pos2 = new bool[3,3,3,2];
        BoxObject fallingBox;
        EventHandler<CollisionArgs> collidedHandler;

        public override void Initialize()
        {
            base.Initialize();
            Scene.Camera = new CameraObject(new Vector3(30, 80, 100), new Vector3(0, 20, 0));
            Scene.Physics.ForceUpdater.Gravity = new Vector3(0, -9.81f, 0);
            //Scene.Physics.Solver.
            currentState = States.Start;

            collidedHandler = new EventHandler<CollisionArgs>(BoxCollidedHandler);
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
                    fallingBox = new BoxObject(new Vector3(x, y, z), new Vector3(9.7f, 9.7f, 9.7f), 10f);
                    fallingBox.Physics.IsAffectedByGravity = false;
                    float speed = fallingBox.Physics.LinearVelocity.Length();
                    Console.WriteLine("Box eingefügt");
                    Scene.Add(fallingBox);
//                  boxen.Add(fallingBox);

                    currentState = States.Bewegen;


                    break;

                case States.Bewegen:
                    UI2DRenderer.WriteText(Vector2.Zero, fallingBox.Physics.LinearVelocity.Length().ToString(), Color.Blue, null, Vector2.One, UI2DRenderer.HorizontalAlignment.Center, UI2DRenderer.VerticalAlignment.Bottom);
                    break;

                case States.Warten:

                    break;

                case States.End:

                    break;
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

        }

        void BoxCollidedHandler(object sender, CollisionArgs e)
        {
            Console.WriteLine(" Collision ");
            currentState = States.Boxeinfuegen;
            fallingBox.Collided -= collidedHandler;
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
                case States.Warten:
                    text = " warten ";
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
                        x = x - 10;
                        fallingBox.MoveToPosition(new Vector3(x, y, z));
                        //Console.WriteLine(String.Format("{0} {1}", fallingBox.Position.X, fallingBox.Position.Y));
                    }
                    if (input.WasKeyPressed(Keys.Right, PlayerIndex.One))
                    {
                        x = x + 10;
                        fallingBox.MoveToPosition(new Vector3(x, y, z));
                    }
                    if (input.WasKeyPressed(Keys.Up, PlayerIndex.One))
                    {
                        z = z - 10;
                        fallingBox.MoveToPosition(new Vector3(x, y, z));
                    }
                    if (input.WasKeyPressed(Keys.Down, PlayerIndex.One))
                    {
                        z = z + 10;
                        fallingBox.MoveToPosition(new Vector3(x, y, z));
                    }
                    if (input.WasKeyPressed(Keys.Space, PlayerIndex.One) && fallingBox.Physics.LinearVelocity.Length() <= 0.01f)
                    {
                            fallingBox.Collided += collidedHandler;
                            fallingBox.Physics.IsAffectedByGravity = true;
                    }
                    break;


                case States.Warten:
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
