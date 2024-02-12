using UnityEngine;

public sealed class ToggleDoorInteraction: Interaction
{

    [SerializeField] private Door _door;

    public override string Text => _door.IsOpen ? "Close" : "Open";

    public override void Perform(PlayerCharacter player)
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
