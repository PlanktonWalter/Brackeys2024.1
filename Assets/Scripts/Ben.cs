using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Door))]
public sealed class Ben : MonoBehaviour
{

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Sound _hungrySound;
    [SerializeField] private Sound _happySound;
    [SerializeField] private ItemTag _foodTag;
    [SerializeField] private Code _codeReward;
    public ItemTag FoodTag => _foodTag;
    public bool IsHungry { get; private set; } = true;

    private void OnEnable()
    {
        GetComponent<Door>().SomeoneKnocked += OnDoorKnocked;
    }

    private void OnDisable()
    {
        GetComponent<Door>().SomeoneKnocked -= OnDoorKnocked;
    }

    public bool TryFeed(PlayerCharacter character)
    {
        if (IsHungry == false)
            return false;

        foreach (var item in character.Inventory.Content)
        {
            if (item.HasTag(_foodTag) == false)
                continue;

            character.Inventory.RemoveItem(item);
            IsHungry = false;
            SayCode();
            return true;
        }

        _hungrySound.Play(_audioSource);
        Notification.Do("I don't have any food...");
        return false;
    }

    private void OnDoorKnocked()
    {
        if (IsHungry == true)
        {
            _hungrySound.Play(_audioSource);
            Notification.Do("Is he... hungry?");
        }
        else
        {
            SayCode();
        }
    }

    private void SayCode()
    {
        _happySound.Play(_audioSource);
        Notification.Do($"Is he saying... {_codeReward.Value}?");
    }

}