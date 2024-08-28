namespace Assets.DigitalSoftware.zFPS.Scripts
{
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UI;

    public class zFPS : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField, Tooltip("The interval for calculating the fps"), Range(0.0f, 2.0f)] private float _samplingIntervalInSeconds = 0.25f;

        // fps
        private int _countFrames = 0;
        private float _lastMeasureAt = 0.0f;
        const string displayFPS = "{0} FPS";
        [Header("Internal references")]
        [SerializeField] private Text _FPSMeasureText;

        // ticks
        const string displayTicks = "Ticks: ";
        [SerializeField] private Text _TicksMeasureText;
        private int _ticksAccumulator = 0;

        // local cache
        private string _cachedCurrentDisplayStringFps;
        private int _cachedCurrentTicks;

        private void Start()
        {
            _lastMeasureAt = Time.realtimeSinceStartup;
        }

        // once per frame
        private void Update()
        {
            UpdateTicks();

            // grab time
            var currentTime = Time.realtimeSinceStartup;

            _countFrames++;

            float fpsMeasurePeriod = currentTime - _lastMeasureAt;

            // see if time has passed (less fluctuation) to show short-period average - while still be flexible with regards to Tick 
            if (fpsMeasurePeriod > _samplingIntervalInSeconds)
            {
                var currentFps = _countFrames / fpsMeasurePeriod; // time since last compared, using frames done since, get a nice average

                string localCachedCurrentDisplayString = string.Format(displayFPS, (int)currentFps);

                // if info differs from previous, push it to Text object
                if (localCachedCurrentDisplayString != _cachedCurrentDisplayStringFps)
                {
                    _cachedCurrentDisplayStringFps = localCachedCurrentDisplayString;
                    _FPSMeasureText.text = _cachedCurrentDisplayStringFps;
                    // Debug.Log("Now: " + Time.realtimeSinceStartup + ", fps: " + _cachedCurrentDisplayStringFps + " -- period: " + fpsMeasurePeriod);
                }

                _countFrames = 0;
                _lastMeasureAt = currentTime;
            }
            
            _ticksAccumulator = 0; // reset every time we hit a frame-sum
        }

        // once per tick
        private void FixedUpdate()
        {
            _ticksAccumulator++;
        }

        private void UpdateTicks()
        {
            // prevent over-draw, not sure if Unity has this internally
            if (_ticksAccumulator != _cachedCurrentTicks)
            {
                var localCachedCurrentDisplayStringTicks = displayTicks;

                // draw tick-count
                for (int i = 0; i < _ticksAccumulator; i++)
                    localCachedCurrentDisplayStringTicks += "X";

                // push to control
                _TicksMeasureText.text = localCachedCurrentDisplayStringTicks;

                // store for future compare
                _cachedCurrentTicks = _ticksAccumulator;
            }
        }
    }
}
