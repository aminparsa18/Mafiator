using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using AndroidX.AppCompat.Widget;
using MafiatorApp.Droid.Helpers;

namespace MafiatorApp.Droid.Controls
{
    public class ShowCaseImageView : AppCompatImageView
    {
        #region CLASS LEVEL VARIABLES

        private Bitmap mBitmap;
        private Paint backgroundPaint, mErasePaint, mCircleBorderPaint;
        private Color backgroundColor = Color.Transparent;
        private readonly Color focusBorderColor = Color.Transparent;
        private int focusBorderSize, animCounter = 20;
        private const int FocusAnimationMaxValue = 20;
        private const int FocusAnimationStep = 1;
        private int mStep;
        private Calculator mCalculator;
        private RectF rectF;
		private Path mPath;

		#endregion

		#region CONSTRUCTORS

		public ShowCaseImageView(Context context) : base(context)
        {
            Init();
        }

        #endregion


        /// <summary>
        /// Initializations for background and paints
        /// </summary>
        private void Init()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb)
            {
                SetLayerType(LayerType.Hardware, null);
            }

            SetWillNotDraw(false);
            SetBackgroundColor(Color.Transparent);
            backgroundPaint = new Paint
            {
                AntiAlias = true,
                Color = backgroundColor,
                Alpha = 0xFF
            };

            mErasePaint = new Paint();
            mErasePaint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.Clear));
            mErasePaint.Alpha = 0xFF;
            mErasePaint.AntiAlias = true;

			mPath = new Path();
            mCircleBorderPaint = new Paint
            {
                AntiAlias = true,
                Color = focusBorderColor,
                StrokeWidth = focusBorderSize
            };
            mCircleBorderPaint.SetStyle(Paint.Style.Stroke);

			rectF = new RectF();
        }

        /// <summary>
        /// Setting parameters for background an animation
        /// </summary>
        /// <param name="backgroundColor"></param>
        /// <param name="calculator"></param>
        public void SetParameters(Color backgroundColor, Calculator calculator)
        {
            this.backgroundColor = backgroundColor;
            mCalculator = calculator;
        }

		public void SetBorderParameters(Color focusBorderColor, int focusBorderSize)
		{
			this.focusBorderSize = focusBorderSize;
			mCircleBorderPaint.Color = focusBorderColor;
		}

		/// <summary>
		/// Draws background and moving focus area
		/// </summary>
		/// <param name="canvas"></param>
		public override void Draw(Canvas canvas)
		{
			base.Draw(canvas);

			if (mBitmap == null)
			{
				mBitmap = Bitmap.CreateBitmap(Width, Height, Bitmap.Config.Argb8888);
				mBitmap.EraseColor(backgroundColor);
			}
			canvas.DrawBitmap(mBitmap, 0, 0, backgroundPaint);
			DrawRectangle(canvas);
			if (animCounter == FocusAnimationMaxValue)
			{
				mStep = -1 * FocusAnimationStep;
			}
			else if (animCounter == 0)
			{
				mStep = FocusAnimationStep;
			}
			animCounter += mStep;
			PostInvalidate();
		}

        /// <summary>
        /// Draws focus rounded rectangle
        /// </summary>
        /// <param name="canvas"></param>
        private void DrawRectangle(Canvas canvas)
        {
            var left = mCalculator.RoundRectLeft(animCounter, 1);
            var top = mCalculator.RoundRectTop(animCounter, 1);
            var right = mCalculator.RoundRectRight(animCounter, 1);
            var bottom = mCalculator.RoundRectBottom(animCounter, 1);
            rectF.Set(left, top, right, bottom);
            canvas.DrawRect(rectF, mErasePaint);

			if (focusBorderSize > 0)
			{
				mPath.Reset();
				mPath.MoveTo((float)mCalculator.CircleCenterX, (float)mCalculator.CircleCenterY);
				mPath.AddRect(rectF, Path.Direction.Cw);
				canvas.DrawPath(mPath, mCircleBorderPaint);
			}
		}
    }
}