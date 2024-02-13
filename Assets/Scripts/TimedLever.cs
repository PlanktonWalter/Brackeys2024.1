using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class TimedLever : MonoBehaviour
{

    [SerializeField] private float _stayingActiveDuration = 4f;
    [SerializeField] private UnityEvent _activated;
    [SerializeField] private UnityEvent _deactivated;

    public bool IsActivated { get; private set; }
    public TimeUntil _timeUntilDeactivation;

    public void Activate()
    {
        if (IsActivated == true)
        {
            Debug.LogError("Cannot activate activated lever", this);
            return;
        }

        IsActivated = true;
        _timeUntilDeactivation = new TimeUntil(Time.time + _stayingActiveDuration);
        OnActivated();
    }

    private void Update()
    {
        if (IsActivated == true && _timeUntilDeactivation < 0)
        {
            IsActivated = false;
            OnDeactivated();
        }
    }

    private void OnActivated()
    {
        _activated.Invoke();
    }

    private void OnDeactivated()
    {
        _deactivated.Invoke();
    }

}
