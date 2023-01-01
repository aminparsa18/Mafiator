using Android.Animation;
using Android.App;
using Android.Views;
using Android.Views.Animations;
using AndroidX.Core.Content;
using Java.Lang;
using static Android.Animation.Animator;

namespace MauiSpotlight.Platforms.Android;

public class Spotlight
{
    private int _currentIndex;
    private SpotlightView _spotlight;
    private readonly Target[] _targets;
    private long _duration;
    private ITimeInterpolator _interpolator;
    private ViewGroup _container;
    private Action _onSpotlightStart;
    private Action _onSpotlightEnd;
    private static int NO_POSITION = -1;

    private Spotlight(SpotlightView spotlight, Target[] targets, long duration, ITimeInterpolator interpolator, ViewGroup container,
        Action onSpotlightStart, Action onSpotlightEnd)
    {
        _spotlight = spotlight;
        _targets = targets;
        _duration = duration;
        _interpolator = interpolator;
        _container = container;
        _onSpotlightStart = onSpotlightStart;
        _onSpotlightEnd = onSpotlightEnd;
        _currentIndex = -1;
        _container.AddView(_spotlight, -1, -1);
    }

    public void Start() => StartSpotlight();

    public void Show(int index) => ShowTarget(index);

    public void Next() => ShowTarget(_currentIndex + 1);

    public void Previous() => ShowTarget(_currentIndex - 1);

    public void Finish() => finishSpotlight();

    private void StartSpotlight() => _spotlight.StartSpotlight(_duration, _interpolator, new SpotlightStartListenet(this));

    private void ShowTarget(int index)
    {
        if (_currentIndex == NO_POSITION)
        {
            Target target = _targets[index];
            _currentIndex = index;
            _spotlight.StartTarget(target);
            target.OnTargetStart?.Invoke();
        }
        else
        {
            _spotlight.FinishTarget(new TargetFinishListener(this, index));
        }
    }

    private void finishSpotlight() => _spotlight.FinishSpotlight(_duration, _interpolator, new SpotlightFinishListener(this));

    public class SpotlightStartListenet : Java.Lang.Object, IAnimatorListener
    {
        private Spotlight _spotlight;

        public SpotlightStartListenet(Spotlight spotlight)
        {
            _spotlight = spotlight;
        }

        public void OnAnimationCancel(Animator animation) { }
        public void OnAnimationEnd(Animator animation) => _spotlight.ShowTarget(0);
        public void OnAnimationRepeat(Animator animation) { }
        public void OnAnimationStart(Animator animation) => _spotlight._onSpotlightStart?.Invoke();
    }

    public class TargetFinishListener : Java.Lang.Object, IAnimatorListener
    {
        private Spotlight _spotlight;
        private int _index;
        public TargetFinishListener(Spotlight spotlight, int index)
        {
            _spotlight = spotlight;
            _index = index;
        }

        public void OnAnimationCancel(Animator animation) { }
        public void OnAnimationEnd(Animator animation)
        {
            int previousIndex = _spotlight._currentIndex;
            Target previousTarget = _spotlight._targets[previousIndex];
            previousTarget.OnTargetEnd?.Invoke();
            if (_index < _spotlight._targets.Length)
            {
                Target target = _spotlight._targets[_index];
                _spotlight._currentIndex = _index;
                _spotlight._spotlight.StartTarget(target);
                target.OnTargetStart?.Invoke();
            }
            else
            {
                _spotlight.finishSpotlight();
            }
        }
        public void OnAnimationRepeat(Animator animation) { }
        public void OnAnimationStart(Animator animation) { }
    }

    public class SpotlightFinishListener : Java.Lang.Object, IAnimatorListener
    {
        private Spotlight _spotlight;

        public SpotlightFinishListener(Spotlight spotlight)
        {
            _spotlight = spotlight;
        }

        public void OnAnimationCancel(Animator animation) { }
        public void OnAnimationEnd(Animator animation)
        {
            _spotlight._spotlight.Cleanup();
            _spotlight._container.RemoveView(_spotlight._spotlight);
            _spotlight._onSpotlightEnd?.Invoke();
        }
        public void OnAnimationRepeat(Animator animation) { }
        public void OnAnimationStart(Animator animation) { }
    }

    public class Builder
    {
        private Target[] targets;
        private long duration;
        private ITimeInterpolator interpolator;
        private int backgroundColor;
        private ViewGroup container;
        private Action _onSpotlightStart;
        private Action _onSpotlightEnd;
        private Activity activity;
        private static long DEFAULT_DURATION;
        private static DecelerateInterpolator DEFAULT_ANIMATION;
        private static int DEFAULT_OVERLAY_COLOR;

        public Builder(Activity activity) : base()
        {
            this.activity = activity;
            duration = DEFAULT_DURATION;
            interpolator = DEFAULT_ANIMATION;
            backgroundColor = DEFAULT_OVERLAY_COLOR;
        }

        public Builder SetTargets(params Target[] targets)
        {
            if (!targets.Any())
            {
                throw new IllegalArgumentException("targets should not be empty. ");
            }
            this.targets = targets;
            return this;
        }

        public Builder SetTargets(List<Target> targets)
        {
            if (!targets.Any())
            {
                throw new IllegalArgumentException("targets should not be empty. ");
            }
            this.targets = targets.ToArray();
            return this;
        }

        public Builder SetDuration(long duration)
        {
            this.duration = duration;
            return this;
        }

        public Builder SetBackgroundColorRes(int backgroundColorRes)
        {
            backgroundColor = ContextCompat.GetColor(activity, backgroundColorRes);
            return this;
        }

        public Builder SetBackgroundColor(int backgroundColor)
        {
            this.backgroundColor = backgroundColor;
            return this;
        }

        public Builder SetAnimation(ITimeInterpolator interpolator)
        {
            this.interpolator = interpolator;
            return this;
        }

        public Builder SetContainer(ViewGroup container)
        {
            this.container = container;
            return this;
        }

        public Builder SetOnSpotlightStart(Action onSpotlightStart)
        {
            _onSpotlightStart = onSpotlightStart;
            return this;
        }

        public Builder SetOnSpotlightEnd(Action onSpotlightEnd)
        {
            _onSpotlightEnd = onSpotlightEnd;
            return this;
        }

        public Spotlight Build()
        {
            SpotlightView spotlight = new SpotlightView(activity, null, 0, backgroundColor);
            if (targets == null)
            {
                throw new IllegalArgumentException("targets should not be null. ");
            }

            container ??= (ViewGroup)activity.Window.DecorView;
            return new Spotlight(spotlight, targets, duration, interpolator, container, _onSpotlightStart, _onSpotlightEnd);
        }
    }
}