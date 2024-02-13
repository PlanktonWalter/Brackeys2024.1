using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public sealed class ScreenFader : MonoBehaviour
{

    private readonly int _id = Shader.PropertyToID("_Fade");

    [SerializeField] private PlayerCharacter _playerCharacter;
    [SerializeField] private Material _mat;
    [SerializeField] private float _fadeTime = 0.7f;

    private Tween _currentTween;

    private void OnEnable()
    {
        _playerCharacter.Died += OnCharacterDied;
        _playerCharacter.Respawned += OnCharacterRespawned;
        _mat.SetFloat(_id, 1f);
    }

    private void OnDisable()
    {
        _playerCharacter.Died -= OnCharacterDied;
        _mat.SetFloat(_id, 0f);
    }

    private void Start()
    {
        FadeIn();
    }

    private void OnCharacterDied()
    {
        Delayed.Do(FadeOut, Mathf.Max(_playerCharacter.RespawnTime - 0.25f, 0.5f));
    }

    private void OnCharacterRespawned()
    {
        FadeIn();
    }

    private void FadeOut()
    {
        _currentTween?.Kill(true);
        _currentTween = _mat.DOFloat(1f, _id, _fadeTime).From(0f);
    }

    private void FadeIn()
    {
        _currentTween?.Kill(true);
        _currentTween = _mat.DOFloat(0f, _id, _fadeTime).From(1f);
    }

}
