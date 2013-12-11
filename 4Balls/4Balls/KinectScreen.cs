using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NOVA.Components.Kinect;
using NOVA.UI;
using NOVA.ScreenManagement.BaseScreens;
using NOVA.Utilities;
using NOVA.Scenery;

namespace _4Balls
{
    public class KinectScreen : GameplayScreen
    {
        float[] diffx = new float[2];
        float[] diffy = new float[2];
        float[] diffz = new float[2];
        float testx = 0.0f;
        float testz = 0.0f;

        float[] cam = new float[3] { 5f, 5f, 5f };
        int twohand = 0;
        int sethight = 0;
        int [] handalt = new int[2];
        Vector3 worldPosHandr = Vector3.Zero;
        Vector3 worldPosHandl = Vector3.Zero;
        Vector3 worldPosshoulderl = Vector3.Zero;
        Vector3 worldPosshoulderr = Vector3.Zero;
        BoxObject box;
        BoxObject box1;
        BoxObject box2;
        BoxObject box3;
        


        public override void Initialize()
        {
            base.Initialize();
            Scene.Camera = new CameraObject(new Vector3(cam[0], cam[1], cam[2]), Vector3.Zero);
            box = new BoxObject(Vector3.Zero,Vector3.One,1f);
            box1 = new BoxObject(new Vector3(0,-1,0), new Vector3(10, 1, 10), 1f);
            box2 = new BoxObject(new Vector3(0, -1, 0), new Vector3(0.1f, 100, 0.1f), 0f);
            box3 = new BoxObject(new Vector3(0, 0, 1), Vector3.One, 0f);
            Scene.PausePhysics = true;
            Scene.Add(box);
            Scene.Add(box1);
            Scene.Add(box2);
            Scene.Add(box3);
            Scene.InitKinect();
            //Scene.Kinect.ShowCameraImage = Kinect.KinectCameraImage.RGB;
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            
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
            }

            
            diffx[0] = worldPosHandr.X - worldPosshoulderr.X;
            diffx[1] = worldPosshoulderl.X - worldPosHandl.X;

            diffy[0] = worldPosHandr.Y - worldPosshoulderr.Y;
            diffy[1] = worldPosshoulderl.Y - worldPosHandl.Y;
            
            diffz[0] = worldPosshoulderr.Z - worldPosHandr.Z;
            diffz[1] = worldPosshoulderl.Z - worldPosHandl.Z;

            // Check right hand
           if (checkHand(diffx[0], diffz[0], 0  /*left*/))
            {
                Scene.Camera = new CameraObject(new Vector3(5f, 5f, 0f), Vector3.Zero);
                testx = testx + 0.1f;
           }
           if (checkHand(diffx[1], diffz[1], 1  /*right*/))
            {
                testx = testx - 0.1f;
            }
            testz = testz + checkdepth(diffz[0], diffx[0], diffx[1]);

            


            checkrotate(diffy[0]);

            UI2DRenderer.WriteText(Vector2.Zero, cam[0].ToString(), Color.Red, null, Vector2.One, UI2DRenderer.HorizontalAlignment.Center, UI2DRenderer.VerticalAlignment.Bottom);
            UI2DRenderer.WriteText(Vector2.Zero, cam[2].ToString(), Color.Red, null, Vector2.One, UI2DRenderer.HorizontalAlignment.Center, UI2DRenderer.VerticalAlignment.Top);

            Scene.Camera = new CameraObject(new Vector3(cam[0], cam[1], cam[2]), Vector3.Zero);
                 


            box.Position = new Vector3(testx, 0f, testz);
            
            

            /*
            if (diffx[0] > 0.4f ^ diffx[1] > 0.4f)
            {
                if (diffx[0] > 0.4f && i == 0)
                {
                    i = 1;
                    test = test + 0.1f;
                    box.Position = new Vector3(test, 0f, 0f);
                }

                if (diffx[1] > 0.4f && j == 0)
                {
                    j = 1;
                    test = test - 0.1f;
                    box.Position = new Vector3(test, 0f, 0f);
                }
            }
            else 
            {
                box.Position = new Vector3(test, 0f, 0f);
            }
            if (diffx[0] < 0.4f && i == 1)
            {
                i = 0;
            }
            if (diffx[1] < 0.4f && j == 1)
            {
                j = 0;
            }
             */
            //UI2DRenderer.WriteText(Vector2.Zero, cam[0].ToString(), Color.Red, null, Vector2.One, UI2DRenderer.HorizontalAlignment.Center, UI2DRenderer.VerticalAlignment.Bottom);
            //UI2DRenderer.WriteText(Vector2.Zero, cam[2].ToString(), Color.Red, null, Vector2.One, UI2DRenderer.HorizontalAlignment.Center, UI2DRenderer.VerticalAlignment.Top);
           /* UI2DRenderer.WriteText(Vector2.Zero, diffz[0].ToString(), Color.Red, null, Vector2.One, UI2DRenderer.HorizontalAlignment.Center, UI2DRenderer.VerticalAlignment.Bottom);
            UI2DRenderer.WriteText(Vector2.Zero, diffz[1].ToString(), Color.Red, null, Vector2.One, UI2DRenderer.HorizontalAlignment.Center, UI2DRenderer.VerticalAlignment.Top);
      //      UI2DRenderer.WriteText(Vector2.Zero, i.ToString(), Color.Red, null, Vector2.One, UI2DRenderer.HorizontalAlignment.Center, UI2DRenderer.VerticalAlignment.Center);
            */

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
        
        bool checkHand(float x,float z,int hand)
        {
           
            if ((x > 0.4f && handalt[hand] == 0) && z <= 0.3f)
            {
                handalt[hand] = 1;
                return true;
            }
            else if (x <= 0.4f)
            {
                handalt[hand] = 0;
                return false;
            }
            else
            {
                return false;
            }
        
        }
        float checkdepth(float z, float x1, float x2) 
        {   
            if ((z > 0.3f && twohand == 1)&& (x1 <= 0.4f && x2 <=0.4f))
            {
                twohand = 2;
                return (+0.5f);
            }
            else if ((z < 0.1f && twohand == 1) && (x1 <= 0.4f && x2 <= 0.4f))
            {
                twohand = 0;
                return (-0.5f);
            }
            else if (z<=0.3f && z>=0.1f)
            {
                twohand = 1;
                return (0f);
            }
            else{
                return (0f);
            }    
        }

        void checkrotate(float y)
        {
            if (y > 0.3 && sethight == 0)
            {
                if ((cam[0] < 0 && cam[2] < 0) || (cam[0] > 0 && cam[2] > 0))
                {
                    cam[0] = -cam[0];
                }else if ((cam[0] > 0 && cam[2] < 0)||(cam[0] < 0 && cam[2] > 0))
                { 
                    cam[2] = -cam[2];
                }
                sethight = 1;
            } else if (y <= 0.3 && y >= -0.3)
            {
                sethight = 0;
            }
            else if (y < -0.3 && sethight == 0) 
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
        
        
    }
}
