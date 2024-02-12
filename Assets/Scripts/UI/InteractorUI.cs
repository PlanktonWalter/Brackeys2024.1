using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class InteractorUI : MonoBehaviour
{

    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private Transform _interactionsPanel;
    [SerializeField] private TextMeshProUGUI _textPrefab;
    [SerializeField] private KeyCode[] _keyCodes;

    private readonly List<TextMeshProUGUI> _activeTexts = new List<TextMeshProUGUI>();

    private void OnEnable()
    {
        _player.Interactor.TargetChanged += OnTargetChanged;
    }

    private void OnDisable()
    {
        _player.Interactor.TargetChanged -= OnTargetChanged;
    }

    private void OnTargetChanged(List<IInteraction> interactions)
    {
        foreach (var text in _activeTexts)
        {
            Destroy(text.gameObject);
        }

        _activeTexts.Clear();

        for (int i = 0; i < Mathf.Min(interactions.Count, _keyCodes.Length); i++)
        {
            var interaction = interactions[i];
            var text = Instantiate(_textPrefab);
            text.transform.SetParent(_interactionsPanel, false);
            _activeTexts.Add(text);

            text.text = $"[{_keyCodes[i]}] {interaction.Text}";
        }
    }

}
