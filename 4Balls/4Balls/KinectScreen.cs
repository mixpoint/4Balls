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
        float diff;
        Vector3 worldPosHand = Vector3.Zero;
        Vector3 worldPosshoulder = Vector3.Zero;
        BoxObject box;
        public override void Initialize()
        {
            base.Initialize();
            Scene.Camera = new CameraObject(new Vector3(0, 0, 10), Vector3.Zero);
            box = new BoxObject(Vector3.Zero,Vector3.One,1f);
            Scene.Add(box);
            Scene.InitKinect();
           // Scene.Kinect.ShowCameraImage = Kinect.KinectCameraImage.RGB;
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
                        worldPosHand = skeleton.Joints[JointType.HandRight].WorldPosition;

                    } 
                    if (skeleton.TrackingState == SkeletonTrackingState.Tracked && skeleton.Joints.Count != 0 && skeleton.Joints[JointType.ShoulderRight].TrackingState == JointTrackingState.Tracked)
                    {
                        worldPosshoulder = skeleton.Joints[JointType.ShoulderRight].WorldPosition;

                    }
                }
            }
            diff = worldPosHand.X - worldPosshoulder.X;

            if (diff > 0.1f)
            {
                box.Position = new Vector3(2.0f, 0f, 0f);
            }
            if (diff < 0.1f)
            {
                box.Position = new Vector3(-2.0f, 0f, 0f);
            }


            UI2DRenderer.WriteText(Vector2.Zero, diff.ToString(), Color.Red,null, Vector2.One,UI2DRenderer.HorizontalAlignment.Center,UI2DRenderer.VerticalAlignment.Bottom);
            
            


            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
             



    }
}
