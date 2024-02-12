using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerInteraction : MonoBehaviour
{

    public event Action<List<IInteraction>> TargetChanged;

    [SerializeField] private PlayerCharacter _player;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _interactableLayer;

    private GameObject _currentTarget;
    private readonly List<IInteraction> _avaliableInteractions = new List<IInteraction>();

    public void TryPerform(int index)
    {
        if (_avaliableInteractions.Count <= index)
            return;

        _avaliableInteractions[index].Perform(_player);
    }

    private void Update()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 3f, _interactableLayer))
        {
            if (hit.transform.gameObject != _currentTarget)
            {
                OnTargetChanged(hit.transform.gameObject);
            }
        }
        else
        {
            if (_currentTarget != null)
            {
                OnTargetChanged(null);
            }
        }
    }

    private void OnTargetChanged(GameObject target)
    {
        _avaliableInteractions.Clear();
        _currentTarget = target;

        if (target != null)
        {
            CreateInteractions(_currentTarget, _avaliableInteractions);
        }

        TargetChanged?.Invoke(_avaliableInteractions);
    }

    private void CreateInteractions(GameObject target, List<IInteraction> interactions)
    {
        var door = target.GetComponentInParent<Door>();

        if (door != null)
        {
            interactions.Add(new ToggleDoorInteraction(door));
            interactions.Add(new KnockDoorInteraction(door));
        }

        var item = target.GetComponent<Item>();

        if (item != null)
        {
            interactions.Add(new GenericInteraction($"Pickup {item.DisplayName}", (player) => player.Inventory.AddItem(item)));
        }
    }

}

public interface IInteraction
{
    public string Text { get; }
    public void Perform(PlayerCharacter player);

}

public sealed class GenericInteraction : IInteraction
{

    private readonly string _text;
    private readonly Action<PlayerCharacter> _perform;

    public GenericInteraction(string text, Action<PlayerCharacter> perform)
    {
        _text = text;
        _perform = perform;
    }

    public string Text => _text;

    public void Perform(PlayerCharacter player)
    {
        _perform(player);
    }
}

public sealed class ToggleDoorInteraction: IInteraction
{

    private readonly Door _door;

    public ToggleDoorInteraction(Door door)
    {
        _door = door;
    }

    public string Text => _door.IsOpen ? "Close" : "Open";

    public void Perform(PlayerCharacter player)
    {
        if (_door.IsOpen == true)
        {
            _door.Close();
            return;
        }

        if (_door.IsLocked == false)
        {
            _door.Open();
            return;
        }

        foreach (var item in player.Inventory.Content)
        {
            if (item.HasTag(_door.KeyTag) == true)
            {
                _door.Open();
                return;
            }
        }

        Notification.Do("Locked!");
    }

}

public sealed class KnockDoorInteraction : IInteraction
{

    private readonly Door _door;

    public KnockDoorInteraction(Door door)
    {
        _door = door;
    }

    public string Text => "Knock";

    public void Perform(PlayerCharacter player)
    {
        Notification.Do("Knock knock");
    }

}