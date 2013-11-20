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
    class 
        Spiel : GameplayScreen
    {
        public enum States
        {
            Start,
            WaitForCollision,
            Collided,
            End
        }

        States currentState;
        BoxObject box;
        SphereObject sphere;

        public override void Initialize()
        {
            base.Initialize();
            Scene.Camera = new CameraObject(new Vector3(0, 15, 35),
            new Vector3(0, 0, 0));
            Scene.Physics.ForceUpdater.Gravity = new Vector3(0, -9.81f, 0);
            currentState = States.Start;
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            switch (currentState)
            {
                case States.Start:
                    box = new BoxObject(new Vector3(0, 0, 0), new Vector3(10f, 1f, 10f), 0f);
                    sphere = new SphereObject(new Vector3(0, 10, 0), 2f, 10, 10, 0f);
                    sphere.Collided += new EventHandler<CollisionArgs>(SphereCollidedHandler);
                    Scene.Add(box);
                    Scene.Add(sphere);

                    currentState = States.WaitForCollision;
                    break;

                case States.WaitForCollision:

                    break;

                case States.Collided:
                    Scene.PausePhysics = true;
                    break;

                case States.End:

                    break;
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

        }

        void SphereCollidedHandler(object sender, CollisionArgs e)
        {
            Vector3 position = sphere.Position + new Vector3(0, 3, 0);
            Text3D text = new Text3D(position,
            Text3D.Type.Fill,
            "Hit!",
            Color.Red,
            "");
            text.LinearVelocity = Vector3.Up;
            text.Lifetime = new TimeSpan(0, 0, 2);
            Scene.Add(text);
            currentState = States.Collided;
        }

        public override void Draw(GameTime gameTime)
        {
            string text = "";
            Color color = new Color();
            switch (currentState)
            {
                case States.Start:
                case States.WaitForCollision:
                    text = " Press Space to start !";
                    color = Color.Yellow;
                    break;
                case States.Collided:
                    text = " Press Space to see physical effect of collision ";
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
                case States.WaitForCollision:
                    if (input.WasKeyPressed(Keys.Space, PlayerIndex.One))
                    {
                        sphere.Mass = 1f;
                        sphere.Physics.Material.Bounciness = 1.8f;
                    }
                    break;
                case States.Collided:
                    if (input.WasKeyPressed(Keys.Space, PlayerIndex.One))
                    {
                        Scene.PausePhysics = false;
                        currentState = States.End;

                    }
                    break;
                case States.End:
                    if (input.WasKeyPressed(Keys.Space, PlayerIndex.One))
                    {
                        Scene.RemoveAllSceneObjects();
                        currentState = States.Start;
                    }
                    break;
            }
            base.HandleInput(input);
        }

    }



}
