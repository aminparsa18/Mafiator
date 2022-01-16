using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using System;
using Xamarin.Forms.Platform.Android;
using ShapeView = MafiatorApp.UserControls.ShapeView;

namespace MafiatorApp.Droid.Controls
{
    /// <summary>
    /// This is our class responsible for drawing our shapes
    /// </summary>
    public class Shape : View
    {
        private bool isBack = true;
        private const float QuarterTurnCounterClockwise = -90;

        public ShapeView ShapeView { get; set; }

        // Pixel density
        private readonly float density;

        // We need to make sure we account for the padding changes
        public new int Width => base.Width - (int) Resize(ShapeView.Padding.HorizontalThickness);

        public new int Height => base.Height - (int) Resize(ShapeView.Padding.VerticalThickness);

        public Shape(float density, Context context) : base(context)
        {
            this.density = density;
        }

        public Shape(float density, Context context, IAttributeSet attributes) : base(context, attributes)
        {
            this.density = density;
        }

        public Shape(float density, Context context, IAttributeSet attributes, int defStyle) : base(context, attributes,
            defStyle)
        {
            this.density = density;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            HandleShapeDraw(canvas);
        }

        protected virtual void HandleShapeDraw(Canvas canvas)
        {
            // We need to account for offsetting the coordinates based on the padding
            var x = GetX() + Resize(ShapeView.Padding.Left);
            var y = GetY() + Resize(ShapeView.Padding.Top);

            HandleStandardDraw(canvas,
                p => canvas.DrawCircle(5 + x + Width / 2 ,5+ y + Height / 2,5+ (Width - 10) / 2, p), ShapeView.StrokeWidth + 3,
                false);
            HandleStandardDraw(canvas,
                p => canvas.DrawArc(new RectF(x, y, x + Width, y + Height), QuarterTurnCounterClockwise,
                    360 * (ShapeView.IndicatorPercentage / 100), false, p), ShapeView.StrokeWidth + 3, true);
            isBack = false;

        }

        /// <summary>
        /// A simple method that handles drawing our shape with the various colours we need
        /// </summary>
        /// <param name="canvas">Canvas.</param>
        /// <param name="drawShape">Draw shape.</param>
        /// <param name="lineWidth">Line width.</param>
        /// <param name="drawFill">If set to <c>true</c> draw fill.</param>
        protected virtual void HandleStandardDraw(Canvas canvas, Action<Paint> drawShape, float? lineWidth = null,
            bool drawFill = true)
        {
            var strokePaint = new Paint(PaintFlags.AntiAlias);
            strokePaint.SetStyle(Paint.Style.Stroke);
            strokePaint.StrokeWidth = drawFill ? Resize(lineWidth.Value) : Resize(lineWidth.Value-3);

            strokePaint.StrokeCap = Paint.Cap.Round;
            strokePaint.StrokeJoin = Paint.Join.Round;
            strokePaint.Color = ShapeView.StrokeColor.ToAndroid();
            if(!drawFill)
             strokePaint.Alpha = 80;
            drawShape(strokePaint);
        }

        // Helper functions for dealing with pizel density
        private float Resize(float input)
        {
            return input * density;
        }

        private float Resize(double input)
        {
            return Resize((float) input);
        }

        public static Color GetColorWithAlpha(Color color, double ratio)
        {
            var ss = Color.GetAlphaComponent(color) * ratio;
            var alpha = (int) Math.Round(ss);
            var r = Color.GetRedComponent(color);
            var g = Color.GetGreenComponent(color);
            var b = Color.GetBlueComponent(color);
            return Color.Argb(alpha, r, g, b);
        }
    }
}