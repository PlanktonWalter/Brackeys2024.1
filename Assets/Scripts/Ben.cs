using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Door))]
public sealed class Ben : MonoBehaviour
{

    private const float answerDelay = 1.0f;
    private const float textDelay = 1.84f;
    private const float feedDelay = 0.5f;

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

        Delayed.Do(() => _hungrySound.Play(_audioSource), feedDelay);
        return false;
    }

    private void OnDoorKnocked()
    {
        if (IsHungry == true)
        {
            Delayed.Do(() => _hungrySound.Play(_audioSource), answerDelay);
            Delayed.Do(() => Notification.Do("Is he... hungry?"), textDelay);
        }
        else
        {
            SayCode();
        }
    }

    private void SayCode()
    {
        Delayed.Do(() => _happySound.Play(_audioSource), answerDelay);
        Delayed.Do(() => Notification.Do($"Is he saying... {_codeReward.Value}?"), textDelay);
    }

}