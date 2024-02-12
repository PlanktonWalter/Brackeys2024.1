using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound")]
public sealed class Sound : ScriptableObject
{

    [SerializeField] private AudioClip[] _clips;

    public void Play(AudioSource source)
    {
        var clip = _clips[Random.Range(0, _clips.Length)];
        source.PlayOneShot(clip);
    }

}
