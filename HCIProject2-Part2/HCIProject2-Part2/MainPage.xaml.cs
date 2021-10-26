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

namespace HCIProject2_Part2
{
    public partial class MainPage : ContentPage
    {
        // https://www.c-sharpcorner.com/article/getting-started-with-skiasharp-with-xamarin-forms/
        // https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/graphics/skiasharp/transforms/translate

        public static readonly SKPath HendecagramPath;
        const double cycleTime = 2000;      // in milliseconds

        SKCanvasView canvasView;
        System.Diagnostics.Stopwatch stopwatch = new Stopwatch();
        bool pageIsActive;
        float angle = -90;
        float previousangle = 0;
        Vector3 acceleration;

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
            angle = (float)(0 - (90 * acceleration.X));
            Accelerometer.Start(SensorSpeed.Fastest);
           
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
        static MainPage()
        {
            // Create 11-pointed star
            HendecagramPath = new SKPath();
            for (int i = 0; i < 11; i++)
            {
                double angle = 5 * i * 2 * Math.PI / 11;
                SKPoint pt = new SKPoint(100 * (float)Math.Sin(angle),
                                        -100 * (float)Math.Cos(angle));
                if (i == 0)
                {
                    HendecagramPath.MoveTo(pt);
                }
                else
                {
                    HendecagramPath.LineTo(pt);
                }
            }
            HendecagramPath.Close();
        }
        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();
            canvas.Translate(info.Width / 2, info.Height / 2);
            float radius = (float)Math.Min(info.Width, info.Height) / 2 - 100;

            using (SKPaint paint = new SKPaint())
            {
                paint.Style = SKPaintStyle.Fill;
                paint.Color = SKColor.FromHsl(angle, 100, 50);

                float x = radius * (float)Math.Sin(Math.PI * angle / 180);
                float y = -radius * (float)Math.Cos(Math.PI * angle / 180);
                canvas.DrawCircle(x, y, 100, paint);
                canvas.DrawPath(MainPage.HendecagramPath, paint);
            }
        }
        /**
        public MainPage()
        {
            InitializeComponent();
            if (Accelerometer.IsMonitoring)
                return;

            // Register and Staart Accelermeter
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            Accelerometer.Start(SensorSpeed.UI);
            

            //get the canvas & info
            

        }
        **/



        async void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            acceleration = (0.5f * e.Reading.Acceleration) + 0.95f * acceleration;
           
            float angleChoice = (float)(Math.Truncate(acceleration.X*100)/100);
            Console.WriteLine(angleChoice);

            if (previousangle - angleChoice > 0.05 ^ previousangle - angleChoice < -0.05)
            {
                angle = (float)(0 - (90 * angleChoice));
                previousangle = angleChoice;
            }
            
            //Console.WriteLine($"Reading: X: {data.Acceleration.X}, Y: {data.Acceleration.Y}, Z: {data.Acceleration.Z}");
            //await ballEllipse.TranslateTo(e.Reading.Acceleration.X * -200, e.Reading.Acceleration.Y * 200, 200);
        }

       

    /**
    void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
        //Initialize the Canvas  
        SKSurface vSurface = args.Surface;
        SKCanvas vCanvas = vSurface.Canvas;
        int surfaceWidth = args.Info.Width;
        int surfaceHeight = args.Info.Height;
        float radius = (Math.Min(surfaceHeight, surfaceWidth) * 0.5f) - 25;
        //Clear the Canvas  
        vCanvas.Clear();
        //Creating the Paint object to color the Items  
        SKPaint vBlackPaint = new SKPaint
        {
            Color = SKColors.Black,
            StrokeWidth = 5,
            StrokeCap = SKStrokeCap.Round,
            TextSize = 60
        };
        SKPaint vWhitePaint = new SKPaint
        {
            Color = SKColors.White
        };
        var outerPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke, //stroke so that it traces the outline
            Color = Color.DarkBlue.ToSKColor(), //make it the color red
            StrokeWidth = 25
        };
        var innerPaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            Color = Color.LightBlue.ToSKColor(),
        };


        //vCanvas.DrawCircle(surfaceWidth / 2, surfaceHeight / 2, radius, outerPaint);
        //vCanvas.DrawCircle(surfaceWidth / 2, surfaceHeight / 2, radius, innerPaint);

        
        var angle = Math.PI * (startAngle + positionOfMarker) / 180.0;

        //calculate the radius and the center point of the circle
        var radius = (originalRect.Right - originalRect.Left) / 2;
        var middlePoint = new SKPoint();
        middlePoint.X = (originalRect.Left + radius);
        middlePoint.Y = originalRect.Top + radius; //top of current circle plus radius

        surface.Canvas.DrawCircle(middlePoint.X + (float)(radius * Math.Cos(angle)),
        middlePoint.Y + (float)(radius * Math.Sin(angle)), 20, circlePaint);
        




    }

    **/

}
    
}
