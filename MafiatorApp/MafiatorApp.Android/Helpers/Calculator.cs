using Android.Views;

namespace MafiatorApp.Droid.Helpers
{
	public class Calculator
    {
        #region Class Variables

        private double mBitmapWidth, mBitmapHeight;
        private double mFocusWidth, mFocusHeight;
        private bool hasFocus;

		#endregion

		public Calculator(View view, double radiusFactor)
        {
            var deviceWidth = Xamarin.Forms.Application.Current.MainPage.Bounds.Width;
            var deviceHeight = Xamarin.Forms.Application.Current.MainPage.Bounds.Height;
            mBitmapWidth = deviceWidth;
            mBitmapHeight = deviceHeight;
            if (view != null)
            {
                var adjustHeight = 0;
                var viewPoint = new int[2];
                view.GetLocationInWindow(viewPoint);
                mFocusWidth = view.Width;
                mFocusHeight = view.Height;
                CircleCenterX = viewPoint[0] + mFocusWidth / 2;
                CircleCenterY = viewPoint[1] + mFocusHeight / 2 - adjustHeight;
                hasFocus = true;
            }
            else
            {
                hasFocus = false;
            }
        }

        public void SetRectPosition(int positionX, int positionY, int rectWidth, int rectHeight)
        {
            CircleCenterX = positionX;
            CircleCenterY = positionY;
            mFocusWidth = rectWidth;
            mFocusHeight = rectHeight;
            hasFocus = true;
        }

		/**
		* @return X coordinate of focus circle
		*/
		public double CircleCenterX { get; private set; }

        /**
         * @return Y coordinate of focus circle
         */
		public double CircleCenterY { get; private set; }

        /// <summary>
		/// Return Bottom position of round rect
		/// </summary>
		/// <param name="animCounter"></param>
		/// <param name="animMoveFactor"></param>
		/// <returns></returns>
		public float RoundRectLeft(int animCounter, double animMoveFactor)
        {
            return (float)(CircleCenterX - mFocusWidth / 2 - animCounter * animMoveFactor);
        }

        /// <summary>
        /// Return Top position of focus round rect
        /// </summary>
        /// <param name="animCounter"></param>
        /// <param name="animMoveFactor"></param>
        /// <returns></returns>
        public float RoundRectTop(int animCounter, double animMoveFactor)
        {
            return (float)(CircleCenterY - mFocusHeight / 2 - animCounter * animMoveFactor);
        }

        /// <summary>
        /// Return Bottom position of round rect
        /// </summary>
        /// <param name="animCounter"></param>
        /// <param name="animMoveFactor"></param>
        /// <returns></returns>
        public float RoundRectRight(int animCounter, double animMoveFactor)
        {
            return (float)(CircleCenterX + mFocusWidth / 2 + animCounter * animMoveFactor);
        }

        /// <summary>
        /// Return Bottom position of round rect
        /// </summary>
        /// <param name="animCounter"></param>
        /// <param name="animMoveFactor"></param>
        /// <returns></returns>
        public float RoundRectBottom(int animCounter, double animMoveFactor)
        {
            return (float)(CircleCenterY + mFocusHeight / 2 + animCounter * animMoveFactor);
        }
    }
}