using UnityEngine;

public sealed class PickupInteraction : Interaction
{

    private Item _item;

    private void Awake()
    {
        _item = GetComponent<Item>();
    }

    public override string Text => $"Pickup {_item.DisplayName}";

    public override void Perform(PlayerCharacter player)
    {
        player.Inventory.AddItem(_item);
    }

}
