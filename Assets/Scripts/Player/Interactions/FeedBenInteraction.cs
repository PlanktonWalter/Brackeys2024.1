using UnityEngine;

public sealed class FeedBenInteraction : Interaction
{

    [SerializeField] private Ben _ben;

    public override string Text => "Give food";

    public override bool IsAvaliable(PlayerCharacter player)
    {
        return _ben.IsHungry == true && player.Inventory.HasItemWithTag(_ben.FoodTag);
    }

    public override void Perform(PlayerCharacter player)
    {
        _ben.TryFeed(player);
    }
}

