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

        double rot = 0;
        int twohand = 0;
        int [] handalt = new int[2];
        Vector3 worldPosHandr = Vector3.Zero;
        Vector3 worldPosHandl = Vector3.Zero;
        Vector3 worldPosshoulderl = Vector3.Zero;
        Vector3 worldPosshoulderr = Vector3.Zero;
        BoxObject box;
        double[] cam = new double[3]{0,5,5};


        public override void Initialize()
        {
            base.Initialize();
            Scene.Camera = new CameraObject(new Vector3((float)(cam[0]), (float)(cam[1]), (float)(cam[2])), Vector3.Zero);
            box = new BoxObject(Vector3.Zero,Vector3.One,1f);
            box = new BoxObject(new Vector3(1,0,1), Vector3.One, 1f);
            Scene.Add(box);
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
                testx = testx + 0.1f;
            }
            if (checkHand(diffx[1], diffz[1], 1  /*right*/))
            {
                testx = testx - 0.1f;
            }
            testz = testz + checkdepth(diffz[0]);

            /* if (checkrotate(0)) {
               
             }
             if (checkrotate(1)) {
                
             }
                */

            box.Position = new Vector3(testx, 0f, testz);
            //Scene.Camera = new CameraObject(new Vector3((float)(cam[0]), (float)(cam[1]), (float)(cam[2])), Vector3.Zero);
            

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
           
            if (x > 0.4f && handalt[hand] == 0)
            {
                handalt[hand] = 1;
                return true;
            }
            else if (x < 0.4f)
            {
                handalt[hand] = 0;
                return false;
            }
            else
            {
                return false;
            }
        
        }
        float checkdepth(float z) 
        {
            if (z > 0.3f && twohand == 1) 
            {
                twohand = 2;
                return (+0.5f);
            }
            else if (z < 0.1f && twohand == 1)
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
        
        bool checkrotate(int hand)
        {   
            

            return(true);
        }
        
        
    }
}
