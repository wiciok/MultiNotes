using System;
using Android.Content;
using Android.Support.V4.View;
using Android.Util;
using Android.Widget;
using Android.Views;

namespace MultiNotes.XAndroid
{
    public class SlidingTabScrollView : HorizontalScrollView
    {
        private const int TITLE_OFFSET_DIPS = 24;
        private const int TAB_VIEW_PADDING_DIPS = 16;
        private const int TAB_VIEW_TEXT_SIZE_SP = 12;

        private int mTitleOffset;

        private int mTabViewLayoutID;
        private int mTabViewTextViewID;

        private ViewPager mViewPager;
        private ViewPager.IOnPageChangeListener mViewPagerPageChangeListener;

        private static SlidingTabStrip mTabStrip;

        private int mScrollState;


        public interface ITabColorizer
        {
            int GetIndicatorColor(int position);
            int GetDividerColor(int position);
        }


        public SlidingTabScrollView(Context context)
            : this(context, null) { }

        public SlidingTabScrollView(Context context, IAttributeSet attrs)
            : this(context, attrs, 0) { }

        public SlidingTabScrollView(Context context, IAttributeSet attrs, int defaultStyle)
            : base(context, attrs, defaultStyle)
        {
            // Disable the scrol bar.
            HorizontalScrollBarEnabled = false;

            // Make sure the tab strpis fill the view.
            FillViewport = true;

            this.SetBackgroundColor(Android.Graphics.Color.Rgb(0xF5, 0xF5, 0xF5)); // Gray color.

            mTitleOffset = (int)(TITLE_OFFSET_DIPS * Resources.DisplayMetrics.Density);

            mTabStrip = new SlidingTabStrip(context);
            this.AddView(mTabStrip, LayoutParams.MatchParent, LayoutParams.MatchParent);
        }


        public ITabColorizer CustomTabColorizer
        {
            set { mTabStrip.CustomTabColorizer = value; }
        }

        public int[] SelectedIndicatorColor
        {
            set { mTabStrip.SelectedIndicatorColors = value; }
        }

        public int[] DividerColors
        {
            set { mTabStrip.DividerColors = value; }
        }

        public ViewPager.IOnPageChangeListener OnPageChangeListener
        {
            set { mViewPagerPageChangeListener = value; }
        }

        public ViewPager ViewPager
        {
            set
            {
                mTabStrip.RemoveAllViews();
                mViewPager = value;
                if (value != null)
                {
                    value.PageSelected += value_pageSelected;
                    value.PageScrollStateChanged += value_pageScrollStateChanged;
                    value.PageScrolled += value_pageScrolled;
                }
            }
        }


        private void value_pageScrolled(object sender, ViewPager.PageScrolledEventArgs e)
        {
            int tabCount = mTabStrip.ChildCount;

            if ((tabCount == 0) || (e.Position < 0) || (e.Position >= tabCount))
            {
                // If any of this conditions apply, return = no need to scroll.
                return;
            }

            mTabStrip.OnViewPagerPageChanged(e.Position, e.PositionOffset);

            View selectedTitle = mTabStrip.GetChildAt(e.Position);

            int extraOffset = selectedTitle != null ? (int)(e.Position * selectedTitle.Width) : 0;

            ScrollToTab(e.Position, extraOffset);

            if (mViewPagerPageChangeListener != null)
            {
                mViewPagerPageChangeListener.OnPageScrolled(
                    e.Position, 
                    e.PositionOffset, 
                    e.PositionOffsetPixels
                );
            }
        }

        private void value_pageScrollStateChanged(object sender, ViewPager.PageScrollStateChangedEventArgs e)
        {
            mScrollState = e.State;

            if (mViewPagerPageChangeListener != null)
            {
                mViewPagerPageChangeListener.OnPageScrollStateChanged(e.State);
            }
        }

        private void value_pageSelected(object sender, ViewPager.PageSelectedEventArgs e)
        {
            if (mScrollState == ViewPager.ScrollStateIdle)
            {
                mTabStrip.OnViewPagerPageChanged(e.Position, 0.0f);
                ScrollToTab(e.Position, 0);
            }

            if (mViewPagerPageChangeListener != null)
            {
                mViewPagerPageChangeListener.OnPageSelected(e.Position);
            }
        }

        // TODO: zakończono w minucie 29:31.
        private void PopulateTabStrip()
        {
            PagerAdapter adapter = mViewPager.Adapter;

            for (int i = 0; i < adapter.Count; ++i)
            {
                TextView tabView = CreateDefaultTabView(Context);
                tabView.Text = i.ToString();
            }
        }

        private TextView CreateDefaultTabView(Context context)
        {
            throw new NotImplementedException();
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            if (mViewPager != null)
            {
                ScrollToTab(mViewPager.CurrentItem, 0);
            }
        }

        private void ScrollToTab(int tabIndex, int extraOffset)
        {
            int tabCount = mTabStrip.ChildCount;

            if (tabCount == 0 || tabIndex < 0 || tabIndex >= tabCount)
            {
                // No need to go further - don't scroll.
                return;
            }

            View selectedChild = mTabStrip.GetChildAt(tabIndex);
            if (selectedChild != null)
            {
                int scrollAmountX = selectedChild.Left + extraOffset;

                if (tabIndex > 0 || extraOffset > 0)
                {
                    scrollAmountX -= mTitleOffset;
                }

                this.ScrollTo(scrollAmountX, 0);
            }
        }
    }
}