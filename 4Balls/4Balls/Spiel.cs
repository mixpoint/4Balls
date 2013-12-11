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
        Vector3 worldPosHandr = Vector3.Zero;
        Vector3 worldPosHandl = Vector3.Zero;
        Vector3 worldPosshoulderl = Vector3.Zero;
        Vector3 worldPosshoulderr = Vector3.Zero;
        float[] diffx = new float[2];
        float[] diffy = new float[2];
        float[] diffz = new float[2];
        int[] handalt = new int[2];
        int twohand = 0;
        int sethight = 0;
        float[] cam = new float[3] { 30, 80, 100 };
       
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

            return false;

        }


        public override void Initialize()
        {
            base.Initialize();
//            Scene.RenderType = RenderType.ForwardRenderer;
            Scene.Camera = new CameraObject(new Vector3(cam[0], cam[1], cam[2]), new Vector3(0, 20, 0));
            Scene.Physics.ForceUpdater.Gravity = new Vector3(0, -9.81f, 0);
            Scene.InitKinect();
            
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
                    ground.RenderMaterial.Diffuse = new Microsoft.Xna.Framework.Vector4(1, 1, 1, 1);
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

                    PointLightObject point = new PointLightObject (
                    new Vector3 (5, 1, 5), // Position
                    100f, // Intensit¨at
                    Color .Green , // Farbe des Lichts
                    Color .White ); // Farbe der spiegelnden Anteile
                    Scene.Add(point);

                    Console.WriteLine("Box eingefügt");
                    Scene.Add(fallingBox);
//                  boxen.Add(fallingBox);

                    currentState = States.Bewegen;


                    break;

                case States.Bewegen:
                    
                    if (Scene.Kinect.SkeletonDataReady)
                    {
                        List<NOVA.Components.Kinect.Skeleton> skeletons = new List<NOVA.Components.Kinect.Skeleton>(Scene.Kinect.Skeletons);
                        foreach (NOVA.Components.Kinect.Skeleton skeleton in skeletons)
                        {
                            if (skeleton.TrackingState == SkeletonTrackingState.Tracked && skeleton.Joints.Count != 0 && skeleton.Joints[JointType.HandRight].TrackingState == JointTrackingState.Tracked)
                            {
                                worldPosHandr = skeleton.Joints[JointType.HandRight].WorldPosition;
                            }
                            if (skeleton.TrackingState == SkeletonTrackingState.Tracked && skeleton.Joints.Count != 0 && skeleton.Joints[JointType.HandLeft].TrackingState == JointTrackingState.Tracked)
                            {
                                worldPosHandl = skeleton.Joints[JointType.HandLeft].WorldPosition;
                            }
                            if (skeleton.TrackingState == SkeletonTrackingState.Tracked && skeleton.Joints.Count != 0 && skeleton.Joints[JointType.ShoulderRight].TrackingState == JointTrackingState.Tracked)
                            {
                                worldPosshoulderr = skeleton.Joints[JointType.ShoulderRight].WorldPosition;
                            }
                            if (skeleton.TrackingState == SkeletonTrackingState.Tracked && skeleton.Joints.Count != 0 && skeleton.Joints[JointType.ShoulderLeft].TrackingState == JointTrackingState.Tracked)
                            {
                                worldPosshoulderl = skeleton.Joints[JointType.ShoulderLeft].WorldPosition;
                            }

                        }
                        diffx[0] = worldPosHandr.X - worldPosshoulderr.X;
                        diffx[1] = worldPosshoulderl.X - worldPosHandl.X;

                        diffy[0] = worldPosHandr.Y + worldPosshoulderr.Y;
                        diffy[1] = worldPosHandl.Y + worldPosshoulderl.Y;

                        diffz[0] = worldPosshoulderr.Z - worldPosHandr.Z;
                        diffz[1] = worldPosshoulderl.Z - worldPosHandl.Z;



                        if (checkHand(diffx[0], diffz[0], 0, diffy[0]  /*left*/))
                        {
                            if (x < 15)
                            {
                                x = x + 10;
                            }
                        }
                        if (checkHand(diffx[1], diffz[1], 1, diffy[1]  /*right*/))
                        {
                            if (x > -15)
                            {
                                x = x - 10;
                            }
                        }

                 //       UI2DRenderer.WriteText(Vector2.Zero, cam[2].ToString(), Color.Red, null, Vector2.One, UI2DRenderer.HorizontalAlignment.Center, UI2DRenderer.VerticalAlignment.Top);
                        checkdepth(diffz[0], diffx[0], diffx[1], diffy[0]);
                        checkfallingbox(diffy[0], diffy[1]);
                        checkrotate(diffy[0], diffy[1]);

                        
                        Scene.Camera = new CameraObject(new Vector3(cam[0], cam[1], cam[2]), new Vector3(0, 20, 0));

                        Matrix vm = Scene.Camera.ViewMatrix;
                        Matrix vmi = Matrix.Invert(vm);
                        

                       fallingBox.MoveToPosition(new Vector3(x, y, z));
                    }

                                    


                   // UI2DRenderer.WriteText(Vector2.Zero, fallingBox.Physics.LinearVelocity.Length().ToString(), Color.Blue, null, Vector2.One, UI2DRenderer.HorizontalAlignment.Center, UI2DRenderer.VerticalAlignment.Bottom);
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

        
        bool checkHand(float x1, float z1, int hand, float y2)
        {

            if ((x1 > 0.4f && handalt[hand] == 0) && z1 <= 0.3f && y2 >= -0.3f)
            {
                handalt[hand] = 1;
                return true;
            }
            else if (x1 <= 0.4f)
            {
                handalt[hand] = 0;
                return false;
            }
            else
            {
                return false;
            }

        }

        int checkdepth(float z1, float x2, float x3, float y3)
        {
            if ((z1 > 0.4f && twohand == 1) && (x2 <= 0.4f && x3 <= 0.4f) && y3 >= -0.3f)
            {
                twohand = 2;
                if (z > -15)
                {
                    z = z - 10;
                }
                return (z);
            }
            else if ((z1 < 0.1f && twohand == 1) && (x2 <= 0.4f && x3 <= 0.4f) && y3 >= -0.3f)
            {
                twohand = 0;
                if (z < 15)
                {
                    z = z + 10;
                }
                return (z);
            }
            else if (z1 <= 0.4f && z1 >= 0.1f)
            {
                twohand = 1;
                return (z);
            }
            else
            {

                return (z);
            }
        }

        void checkfallingbox(float rechtehand, float linkehand)
        {
            if (rechtehand < -0.2 && linkehand < -0.2)
            {
                currentState = States.Wait;
            }
            else
            {
            }
        }

        void checkrotate(float rechtehand1, float linkehand1)
        {



            UI2DRenderer.WriteText(Vector2.Zero, rechtehand1.ToString(), Color.Red, null, Vector2.One, UI2DRenderer.HorizontalAlignment.Center, UI2DRenderer.VerticalAlignment.Top);
            UI2DRenderer.WriteText(Vector2.Zero, linkehand1.ToString(), Color.Red, null, Vector2.One, UI2DRenderer.HorizontalAlignment.Center, UI2DRenderer.VerticalAlignment.Bottom);
            
   
            if ((linkehand1 < -0.2 && rechtehand1 > 0.2) && sethight == 0)
            {
                if ((cam[0] < 0 && cam[2] < 0) || (cam[0] > 0 && cam[2] > 0))
                {
                    cam[0] = -cam[0];
                }
                else if ((cam[0] > 0 && cam[2] < 0) || (cam[0] < 0 && cam[2] > 0))
                {
                    cam[2] = -cam[2];
                }
                sethight = 1;
            }
            else if ((linkehand1 <= 0.2 && linkehand1 >= -0.2) || (rechtehand1 <= 0.2 && rechtehand1 >= -0.2))
            {

                sethight = 0;
            }
           
            if ((rechtehand1 < -0.2 && linkehand1 > 0.2) && sethight == 0 )
            {
                if ((cam[0] < 0 && cam[2] < 0) || (cam[0] > 0 && cam[2] > 0))
                {
                    cam[2] = -cam[2];
                }
                else if ((cam[0] > 0 && cam[2] < 0) || (cam[0] < 0 && cam[2] > 0))
                {
                    cam[0] = -cam[0];
                }
                sethight = 1;
            }


        }

        void BoxCollidedHandler(object sender, CollisionArgs e)
        {
            Console.WriteLine(" Collision ");


            fallingBox.Collided -= collidedHandler;

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
