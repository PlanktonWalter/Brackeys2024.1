using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class NotificationUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _label;
    private TimeSince _timeSinceLastNotification;
    private bool _isNotificationShown;

    private void OnEnable()
    {
        _label.gameObject.SetActive(false);
        Notification.Event += OnNotificationSent;
    }

    private void OnDisable()
    {
        Notification.Event -= OnNotificationSent;
    }

    private void OnNotificationSent(string notification)
    {
        _label.text = notification;
        _label.gameObject.SetActive(true);
        _isNotificationShown = true;
        _timeSinceLastNotification = new TimeSince(Time.time);
    }

    private void Update()
    {
        if (_isNotificationShown == false)
            return;

        if (_timeSinceLastNotification > 3f)
        {
            _label.gameObject.SetActive(false);
            _isNotificationShown = false;
        }
    }

}

public static class Notification
{

    public static event Action<string> Event;

    public static void Do(string text)
    {
        Event?.Invoke(text);
    }

}
