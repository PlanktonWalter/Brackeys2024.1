using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ShotgunTrap : MonoBehaviour
{

    [SerializeField] private Door _door;
    [SerializeField] private Sound _shootSound;
    [SerializeField] private AudioSource _shootAudioSource;
    [SerializeField] private PlayerCharacter _playerCharacter;

    private bool _isActivated;
    private TimeSince _timeSinceLastActivation;

    private void OnEnable()
    {
        _door.Opened += OnDoorOpened;
    }

    private void OnDisable()
    {
        _door.Opened -= OnDoorOpened;
    }

    private void OnDoorOpened()
    {
        _isActivated = true;
        _timeSinceLastActivation = new TimeSince(Time.time);
    }

    private void Update()
    {
        if (_isActivated == false)
            return;

        if (_timeSinceLastActivation > 0.2f)
        {
            _playerCharacter.Kill();
            _isActivated = false;
            _shootSound.Play(_shootAudioSource);
            Delayed.Do(() => _door.Close(), 0.7f);
        }
    }

}
