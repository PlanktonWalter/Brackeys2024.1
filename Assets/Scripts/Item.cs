using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PickupInteraction))]
public class Item : MonoBehaviour
{
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }

    [SerializeField] public List<ItemTag> Tags;

    public bool HasTag(ItemTag tag)
    {
        return Tags.Contains(tag);
    }

    public void Push(Vector3 force)
    {
        GetComponent<Rigidbody>().AddForce(force);
    }

    public virtual void OnPickedUp()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnDropped()
    {
        gameObject.SetActive(true);
    }

}
