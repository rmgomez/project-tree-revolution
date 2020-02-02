using UnityEngine;

public class Duration
{
    public bool IsDone => (_useRealTime ? Time.unscaledTime : Time.time) >= _timeEnd;
    
    public float Progress01 => ((_useRealTime ? Time.unscaledTime : Time.time) - _timeStart) / (_timeEnd - _timeStart);
    
    private float _timeStart = 0f;
    private float _timeEnd = 1f;
    private bool _useRealTime;
    
    public Duration(float duration, bool useRealTime = false)
    {
        Setup(duration, useRealTime);
    }

    private void Setup(float duration, bool useRealTime = false)
    {
        _useRealTime = useRealTime;

        if (duration != 0f)
        {
            _timeStart = _useRealTime ? Time.unscaledTime : Time.time;
            _timeEnd = _timeStart + duration;
        }
    }
}
