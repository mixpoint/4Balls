using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NOVA.Components.Kinect;
using NOVA.UI;
using NOVA.ScreenManagement.BaseScreens;
using NOVA.Utilities;
using NOVA.Scenery;
using System.Collections.Generic;
using Microsoft.Kinect;

namespace _4Balls
{
    class 
        Spiel : GameplayScreen
    {
        SphereObject sphere;
        public override void Initialize()
        {
            base.Initialize();
            // Kamera erstellen
            Scene.Camera = new CameraObject(new Vector3(0, 0, 10), Vector3.Zero);
            // Kugel erstellen
            sphere = new SphereObject(Vector3.Zero, 1f, 10, 10, 1f);
            Scene.Add(sphere);

            // Kinect initialisieren
            Scene.InitKinect();
            // RGB - Kamerabild als Szenenhintergrund verwenden
            Scene.Kinect.ShowCameraImage = Kinect.KinectCameraImage.RGB;
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            // Kugel transformieren , wenn die Kinect bereit ist
            if (Scene.Kinect.SkeletonDataReady)
            {
                List<NOVA.Components.Kinect.Skeleton> skeletons = new List<NOVA.Components.Kinect.Skeleton>(Scene.Kinect.Skeletons);
                // Aktives Skelett finden
                foreach (NOVA.Components.Kinect.Skeleton skeleton in skeletons)
                {
                    if (skeleton.TrackingState == SkeletonTrackingState.Tracked && skeleton.Joints.Count != 0 && skeleton.Joints[JointType.HandRight].TrackingState == JointTrackingState.Tracked)
                    {
                        // Position der rechten Hand des Spielers in Bildschirmkoordinaten
                        Vector2 screenPos = skeleton.Joints[JointType.HandLeft].ScreenPosition;
                        screenPos.X *= Scene.Game.Window.ClientBounds.Width;
                        screenPos.Y *= Scene.Game.Window.ClientBounds.Height;
                        // Eine Ebene parallel zur Bildschirmebene erzeugen in die die Kugel
                        // transformiert wird.
                        Plane plane = new Plane(Vector3.Forward, -10f);
                        // Weltkoordinatenpunkt finden
                        Vector3 worldPos = Helpers.Unproject(screenPos, plane);
                        // Position der Kugel setzen
                        sphere.Position = worldPos;
                    }
                }
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}
