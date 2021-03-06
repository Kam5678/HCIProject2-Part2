using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using System.Threading;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.IO;
using System.Drawing;


namespace HCIProject2_Part2
{
    public partial class MainPage : ContentPage
    {
        // https://www.c-sharpcorner.com/article/getting-started-with-skiasharp-with-xamarin-forms/
        // https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/graphics/skiasharp/transforms/translate
        // https://stackoverflow.com/questions/62143553/xamarin-import-skbitmap-from-local-file-not-resource
        // https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/graphics/skiasharp/basics/bitmaps
        // https://docs.microsoft.com/en-us/dotnet/api/skiasharp.skcanvas.drawimage?view=skiasharp-2.80.2#SkiaSharp_SKCanvas_DrawImage_SkiaSharp_SKImage_SkiaSharp_SKPoint_SkiaSharp_SKPaint_
        // https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/graphics/skiasharp/bitmaps/displaying
        // https://devblogs.microsoft.com/xamarin/adding-sound-xamarin-forms-app/
        // https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/graphics/skiasharp/transforms/rotate


        // Initalizing Variables 
        public static readonly SKPath HendecagramPath;
        const double cycleTime = 9000;      // in milliseconds
        bool check = true;
        SKCanvasView canvasView;
        System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
        System.Diagnostics.Stopwatch stopwatch2 = new Stopwatch();
        bool pageIsActive;
        float angle = -90;
        float previousangle = 0;
        Vector3 acceleration;
        float degrees = 0;
        bool startOnce = false;
        bool startOnce1 = false;
        float loginAngle = 90;
        float loginAngle1 = -90;
        bool check1 = true;
        bool secondPinDone = false;
        int translatePin1 = 0;
        int translatePin2 = 0;

        private SKBitmap resourceBitmap;
        private SKBitmap resourceBitmap2;
        private SKBitmap resourceBitmap3;
        private SKBitmap resourceBitmap4;
        private SKBitmap resourceBitmap5;







        public MainPage()
        {
      


            Title = "Hedecagram Animation";

            canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            Content = canvasView;

            if (Accelerometer.IsMonitoring)
                return;

            // Register and Staart Accelermeter
            //Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            Accelerometer.ReadingChanged += (sender, args) =>
            {
                // Smooth the reading by averaging with prior values
                acceleration = 0.5f * args.Reading.Acceleration + 0.5f * acceleration;
 
            };
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            //angle = (float)(0 - (90 * acceleration.X));
            Accelerometer.Start(SensorSpeed.Fastest);

            


            //InitializeComponent();
        }

            

         protected override void OnAppearing()
        {
            base.OnAppearing();
            pageIsActive = true;
            stopwatch.Start();

            Device.StartTimer(TimeSpan.FromMilliseconds(80), () =>
            {
                //var data = e.Reading;
                //double t = stopwatch.Elapsed.TotalMilliseconds % cycleTime / cycleTime;
                //double t = data.Acceleration.X;
                //angle = (float)(-90+(180 * t));
                //Console.WriteLine(angle);
               
                //Console.WriteLine(angle);
                canvasView.InvalidateSurface();

                if (!pageIsActive)
                {
                    stopwatch.Stop();
                }

                return pageIsActive;
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            pageIsActive = false;
        }
        

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            var assemblys = typeof(App).GetTypeInfo().Assembly;
            Stream audioStream = assemblys.GetManifestResourceStream("HCIProject2-Part2.AnimationSound.mp3");


            var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;

            string resourceID = "HCIProject2-Part2.KeyholeOnly.png";
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(resourceID))
            {
                resourceBitmap = SKBitmap.Decode(stream);
            }

            

            string resourceID3 = "HCIProject2-Part2.LockInside.png";
            Assembly assembly3 = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly3.GetManifestResourceStream(resourceID3))
            {
                resourceBitmap3 = SKBitmap.Decode(stream);
            }

            string resourceID4 = "HCIProject2-Part2.Pin.png";
            Assembly assembly4 = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly3.GetManifestResourceStream(resourceID4))
            {
                resourceBitmap4 = SKBitmap.Decode(stream);
            }

            string resourceID5 = "HCIProject2-Part2.Pin.png";
            Assembly assembly5 = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly3.GetManifestResourceStream(resourceID5))
            {
                resourceBitmap5 = SKBitmap.Decode(stream);
            }

            canvas.Clear();
            canvas.Translate(info.Width / 2, info.Height / 2);
            float radius = (float)Math.Min(info.Width, info.Height) / 2 -20;

            SkiaSharp.SKPaint thickLinePaint = new SkiaSharp.SKPaint
            {
                Style = SkiaSharp.SKPaintStyle.Stroke,
                Color = SkiaSharp.SKColors.Red,
                StrokeWidth = 30
            };

            SkiaSharp.SKPaint lockPickPaint = new SkiaSharp.SKPaint
            {
                Style = SkiaSharp.SKPaintStyle.Stroke,
                Color = SkiaSharp.SKColors.LightGray,
                StrokeWidth = 15
            };
            SkiaSharp.SKPaint lockPickPaintHandleBlack = new SkiaSharp.SKPaint
            {
                Style = SkiaSharp.SKPaintStyle.Stroke,
                Color = SkiaSharp.SKColors.Black,
                StrokeWidth = 45
            };

            SkiaSharp.SKPaint lockPickPaintHandleRed = new SkiaSharp.SKPaint
            {
                Style = SkiaSharp.SKPaintStyle.Stroke,
                Color = SkiaSharp.SKColors.Red,
                StrokeWidth = 45
            };

            SkiaSharp.SKPaint circle = new SkiaSharp.SKPaint
            {
                Style = SkiaSharp.SKPaintStyle.Stroke,
                Color = SkiaSharp.SKColors.Black,
                IsAntialias = true,
                StrokeWidth = 15
            };

            SkiaSharp.SKPaint paint = new SkiaSharp.SKPaint
            {
                Style = SkiaSharp.SKPaintStyle.Fill,
                Color = SkiaSharp.SKColor.FromHsl(angle, 100, 50)
            };
           

            float x = radius * (float)Math.Sin(Math.PI * angle / 180);
            float y = -radius * (float)Math.Cos(Math.PI * angle / 180);
            //canvas.DrawPath(MainPage.HendecagramPath, paint);

            float prevPointx = radius * (float)Math.Sin(Math.PI * 90 / 180);
            float prevPointy = -radius * (float)Math.Cos(Math.PI * 90 / 180);

            canvas.Translate(0, -250);

            for (double i = 90; i >= -90; i -= 0.1)
            {
                SkiaSharp.SKPaint arcpaint = new SkiaSharp.SKPaint
                {
                    Style = SkiaSharp.SKPaintStyle.Fill,
                    Color = SkiaSharp.SKColor.FromHsl((float)(i / 2), 100, 50),
                    StrokeWidth = 30
                };

                float nextPointX = radius * (float)Math.Sin(Math.PI * i / 180);
                float nextPointY = -radius * (float)Math.Cos(Math.PI * i / 180);
                canvas.DrawLine(prevPointx, prevPointy, nextPointX, nextPointY, arcpaint);
                prevPointx = nextPointX;
                prevPointy = nextPointY;
            }


            
            //canvas.Translate(x, y);
            if (resourceBitmap != null) {
                //canvas.DrawCircle(x, y, 100, paint);
                SKRect rect = new SKRect((float)-1.2*resourceBitmap.Width / 2, (float)-1.0*resourceBitmap.Height / 2, (float)1.2*resourceBitmap.Width / 2, (float)1.0*resourceBitmap.Height);
                SKRect rect1 = new SKRect((float)-2.25*resourceBitmap3.Width / 2, (float)-1.75 * resourceBitmap3.Height / 2, (float)2.25 * resourceBitmap3.Width / 2, (float)1.75 * resourceBitmap3.Height);
                SKRect rect2 = new SKRect((float)-0.65 * resourceBitmap4.Width / 2, (float)-0.65 * resourceBitmap4.Height / 2, (float)0.65 * resourceBitmap4.Width / 2, (float)0.65 * resourceBitmap4.Height);
                canvas.DrawCircle(0, 10, 400, circle);
                canvas.DrawCircle(0, 10, 418, circle);
                
                if (check == false && check1==false && secondPinDone)
                {
                    
                    if (degrees < 90)
                    {

                        if (!startOnce1)
                        {
                           
                            player.Load(audioStream);

                            player.Play();

                            startOnce1 = true;
                        }
                       
                        canvas.Save();
                        canvas.RotateDegrees(degrees);

                        canvas.DrawBitmap(resourceBitmap, rect);
                        canvas.DrawLine(x / 10, y / 10, x / 2, y / 2, lockPickPaint);
                        canvas.DrawLine(x / 2, y / 2, (float)1.1 * x, (float)1.1 * y, lockPickPaintHandleBlack);

                        canvas.DrawLine((-x / 10), (-y / 10) + 170, (-x / 2), (-y / 2) + 170, lockPickPaint);
                        canvas.DrawLine((-x / 2), (-y / 2) + 170, (float)((-2.1 * x / 2)), (float)((-2.1 * y / 2) + 170), lockPickPaintHandleRed);
                        canvas.Restore();
                        degrees += (float)3;
                        
                    }
                    else
                    {
                        player.Stop();
                        canvas.Save();
                        canvas.RotateDegrees(degrees);

                        canvas.DrawBitmap(resourceBitmap, rect);
                        //canvas.DrawLine(x / 10, y / 10, x / 2, y / 2, lockPickPaint);
                        //canvas.DrawLine(x / 2, y / 2, (float)1.1 * x, (float)1.1 * y, lockPickPaintHandleBlack);

                        //canvas.DrawLine((-x / 10), (-y / 10) + 170, (-x / 2), (-y / 2) + 170, lockPickPaint);
                        //canvas.DrawLine((-x / 2), (-y / 2) + 170, (float)((-2.1 * x / 2)), (float)((-2.1 * y / 2) + 170), lockPickPaintHandleRed);
                        canvas.Restore();
                        if (!startOnce)
                        {
                            Accelerometer.Start(SensorSpeed.Fastest);
                            startOnce = true;
                        }

                       
                        

                    }
                }
                else
                {
                    canvas.DrawBitmap(resourceBitmap, rect);

                   

                    canvas.DrawLine(x / 10, y / 10, x / 2, y / 2, lockPickPaint);
                    canvas.DrawLine(x / 2, y / 2, (float)1.1 * x, (float)1.1 * y, lockPickPaintHandleBlack);

                    //canvas.DrawLine((-x / 10), (-y / 10) + 170, (-x / 2), (-y / 2) + 170, lockPickPaint);
                    //canvas.DrawLine((-x / 2), (-y / 2) + 170, (float)((-2.1 * x / 2)), (float)((-2.1 * y / 2) + 170), lockPickPaintHandleRed);

                    canvas.DrawLine((-x/10), (-y/10) + 170, (-x / 2), (-y / 2) + 170, lockPickPaint);
                    canvas.DrawLine((-x / 2), (-y / 2) + 170, (float)((-2.1 * x / 2.5)), (float)((-2.1 * y / 2.5) + 170), lockPickPaintHandleRed);

                    //canvas.DrawLine((-x / 20), (-y / 0) + 170, (-x / 2), (-y / 2) + 170, lockPickPaint);
                    //canvas.DrawLine((-x / 5), (-y / 5) + 170, (float)((-2.1 * x / 2)), (float)((-2.1 * y / 2) + 170), lockPickPaintHandleRed);
                    //canvas.DrawLine(x / 2, y / 2, (float)1.1 * x, (float)1.1 * y, lockPickPaintHandleBlack);
                }
              
                canvas.Save();
                canvas.Translate(0,820);
                canvas.DrawBitmap(resourceBitmap3,rect1);
                canvas.Restore();

                

               

                if (check == false)
                {
                    if (translatePin1 < 100)
                    {
                        canvas.Save();
                        canvas.Translate(-52, 795 - translatePin1);
                        canvas.DrawBitmap(resourceBitmap4, rect2);
                        canvas.Restore();
                        translatePin1 += 20;

                    }
                    else
                    {
                        canvas.Save();
                        canvas.Translate(-52, 795 - translatePin1);
                        canvas.DrawBitmap(resourceBitmap4, rect2);
                        canvas.Restore();
                    }
                }
                else
                {
                    canvas.Save();
                    canvas.Translate(-52, 795);
                    canvas.DrawBitmap(resourceBitmap4, rect2);
                    canvas.Restore();

                }


                if (check1 == false)
                {
                    if (translatePin2 < 100) {
                        canvas.Save();
                        canvas.Translate(52, 795 - translatePin2);
                        canvas.DrawBitmap(resourceBitmap5, rect2);
                        canvas.Restore();
                        translatePin2 += 20;

                    }
                    else
                    {
                        
                        canvas.Save();
                        canvas.Translate(52, 795 - translatePin2);
                        canvas.DrawBitmap(resourceBitmap5, rect2);
                        canvas.Restore();
                        secondPinDone = true;
                    }
                }
                else
                {
                    canvas.Save();
                    canvas.Translate(52, 795);
                    canvas.DrawBitmap(resourceBitmap5, rect2);
                    canvas.Restore();

                }


            }
            
        }


   
        public void Todo(Stopwatch stopwatch2)
        {
            

            if (stopwatch2.Elapsed.TotalSeconds >= 2)
            {
                check = false;
                //Thread.Sleep(1000);
                var assemblys = typeof(App).GetTypeInfo().Assembly;
                Stream audioStream = assemblys.GetManifestResourceStream("HCIProject2-Part2.LockClick.mp3");


                var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                player.Load(audioStream);
                player.Play();

                Console.WriteLine("Position Secure");
                stopwatch2.Restart();
                
                

            }
        }
        public void Todo2(Stopwatch stopwatch2)
        {


            if (stopwatch2.Elapsed.TotalSeconds >= 2)
            {
                check1 = false;
                //Thread.Sleep(1000);
                var assemblys = typeof(App).GetTypeInfo().Assembly;
                Stream audioStream = assemblys.GetManifestResourceStream("HCIProject2-Part2.LockClick.mp3");


                var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                player.Load(audioStream);
                player.Play();
                //Thread.Sleep(500);

                Console.WriteLine("Position Secure");
                stopwatch2.Restart();
                Accelerometer.Stop();
                

            }
        }

        async void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            acceleration = (0.5f * e.Reading.Acceleration) + 0.95f * acceleration;

            float angleChoice = (float)(Math.Truncate(acceleration.X * 100) / 100);


            if (previousangle - angleChoice > 0.1 ^ previousangle - angleChoice < -0.1)
            {
                angle = (float)(0 - (90 * angleChoice));
                previousangle = angleChoice;
            }
            if (acceleration.Z > 3 && check == false && check1 == false)
            {
                Console.WriteLine("Thrust");
                var assemblys = typeof(App).GetTypeInfo().Assembly;
                Stream audioStream = assemblys.GetManifestResourceStream("HCIProject2-Part2.lock.mp3");


                var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                player.Load(audioStream);

                player.Play();
                Thread.Sleep(1000);
                Accelerometer.Stop();
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                //Application.Current.MainPage.Navigation.PopAsync();
            }
            if (angle >= 90)
            {
                angle = 90;
              
                    stopwatch2.Start();
                    Todo(stopwatch2);
                
            }
            else if (angle < -90)
            {
                angle = -90;
            }
            Console.WriteLine(angle);
            if (angle>=loginAngle-5 && angle<=loginAngle+5)
            {
                stopwatch2.Start();
                Todo(stopwatch2);
                Console.WriteLine("here");
            }
            else if (angle >= loginAngle1 - 5 && angle <= loginAngle1 + 5 && check==false)
            {
                stopwatch2.Start();
                Todo2(stopwatch2);
                Console.WriteLine("here1");
            }
            else
            {
                stopwatch2.Restart();

            }
           


        }



        

    }
    
}
