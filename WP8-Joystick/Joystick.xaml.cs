//
// Written in 2012 by Sascha Corti.
//
// Licensed under the Microsoft Public License (Ms-PL).
// You may se this file in compliance with the License.
// Obtain a copy of the License at:
//
//    http://opensource.org/licenses/Ms-PL.html
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Media.Animation;
using System.Windows.Media;

namespace WP8_Joystick
{
    public partial class Joystick : UserControl
    {
        public static readonly DependencyProperty TimerMilliSecondsProperty =
            DependencyProperty.Register("TimerMilliSeconds", typeof(double), typeof(Joystick),
                new PropertyMetadata(Convert.ToDouble(250), new PropertyChangedCallback(Joystick.OnTimerMilliSecondsChanged)));

        DispatcherTimer timer;

        int direction = 360;
        int speed = 0;
        int lastDirection = 0;
        int lastSpeed = 0;

        double lastX = 0;
        double lastY = 0;
        double newX = 0;
        double newY = 0;

        bool moveJoystick = false;

        public Joystick()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(TimerMilliSeconds);
            timer.Tick += timer_Tick;

        }

        public double TimerMilliSeconds
        {
            get
            {
                return Convert.ToDouble(GetValue(TimerMilliSecondsProperty));
            }
            set
            {
                SetValue(TimerMilliSecondsProperty, value);
            }
        }

        private static void OnTimerMilliSecondsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Joystick).timer.Interval = TimeSpan.FromMilliseconds((d as Joystick).TimerMilliSeconds);
        }

        public void StartJoystick()
        {
            Touch.FrameReported += Touch_FrameReported;
        }

        public void StopJoystick()
        {
            Touch.FrameReported -= Touch_FrameReported;
        }

        void Touch_FrameReported(object sender, TouchFrameEventArgs e)
        {
            try
            {
                int pointsNumber = e.GetTouchPoints(ellipseSense).Count;
                TouchPointCollection pointCollection = e.GetTouchPoints(ellipseSense);


                for (int i = 0; i < pointsNumber; i++)
                {
                    if (pointCollection[i].Position.X > 0 && pointCollection[i].Position.X < ellipseSense.ActualWidth)
                    {
                        if (pointCollection[i].Position.Y > 0 && pointCollection[i].Position.Y < ellipseSense.ActualHeight)
                        {
                            // Update Shpero speed and direction
                            Point p = pointCollection[i].Position;
                            Point center = new Point(ellipseSense.ActualWidth / 2, ellipseSense.ActualHeight / 2);

                            double distance = Math.Sqrt(Math.Pow((p.X - center.X), 2) + Math.Pow((p.Y - center.Y), 2));

                            double distanceRel = distance * 255 / (ellipseSense.ActualWidth / 2);
                            if (distanceRel > 255)
                            {
                                distanceRel = 255;
                            }

                            double angle = Math.Atan2(p.Y - center.Y, p.X - center.X) * 180 / Math.PI;
                            if (angle > 0)
                            {
                                angle += 90;
                            }
                            else
                            {
                                angle = 270 + (180 + angle);
                                if (angle >= 360)
                                {
                                    angle -= 360;
                                }
                            }
                            direction = Convert.ToInt16(angle);
                            speed = Convert.ToInt16(distanceRel);

                            // Set Joystick Pos
                            newX = p.X - (ellipseSense.ActualWidth / 2);
                            newY = p.Y - (ellipseSense.ActualWidth / 2);
                            if (moveJoystick) MoveJoystick(newX, newY);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void MoveJoystick(double moveX, double moveY)
        {
            Storyboard sb = new Storyboard();
            KeyTime ktStart = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0));
            KeyTime ktEnd = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200));

            DoubleAnimationUsingKeyFrames animationFirstX = new DoubleAnimationUsingKeyFrames();
            DoubleAnimationUsingKeyFrames animationFirstY = new DoubleAnimationUsingKeyFrames();

            ellipseButton.RenderTransform = new CompositeTransform();

            Storyboard.SetTargetProperty(animationFirstX, new PropertyPath(CompositeTransform.TranslateXProperty));
            Storyboard.SetTarget(animationFirstX, ellipseButton.RenderTransform);
            animationFirstX.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = ktStart, Value = lastX });
            animationFirstX.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = ktEnd, Value = moveX });


            Storyboard.SetTargetProperty(animationFirstY, new PropertyPath(CompositeTransform.TranslateYProperty));
            Storyboard.SetTarget(animationFirstY, ellipseButton.RenderTransform);
            animationFirstY.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = ktStart, Value = lastY });
            animationFirstY.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = ktEnd, Value = moveY });

            sb.Children.Add(animationFirstX);
            sb.Children.Add(animationFirstY);
            sb.Begin();

            lastX = moveX;
            lastY = moveY;

            //Debug.WriteLine("Move Joystick to: " + moveX + ", " + moveY);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (((direction - lastDirection) > 5 || (direction - lastDirection) < -5) || ((speed - lastSpeed) > 5 || (speed - lastSpeed) < -5))
            {
                lastDirection = direction;
                lastSpeed = speed;

                OnNewCoordinates();

                Debug.WriteLine("Event fired: " + speed + ", " + direction);
            }
        }

        private void ellipseSense_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            timer.Start();
            moveJoystick = true;

            Debug.WriteLine("Manipulation Started");
        }

        private void ellipseSense_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            timer.Stop();

            // Fire event
            OnStop();

            // Move Joystick to Center
            MoveJoystick(0, 0);
            moveJoystick = false;

            Debug.WriteLine("Manipulation Completed");
        }

        public event EventHandler NewCoordinates;
        protected virtual void OnNewCoordinates()
        {
            var myCoordinates = new MyCoordinates();
            myCoordinates.Direction = direction;
            myCoordinates.Speed = speed;
            if (NewCoordinates != null)
                NewCoordinates(this, myCoordinates);
        }

        public event EventHandler Stop;
        protected virtual void OnStop()
        {
            var myStop = new MyStop();
            myStop.Stopped = true;
            if (Stop != null)
                Stop(this, myStop);
        }

    }

    public class MyCoordinates : EventArgs
    {
        public int Direction;
        public int Speed;
    }

    public class MyStop : EventArgs
    {
        public bool Stopped;
    }
}
