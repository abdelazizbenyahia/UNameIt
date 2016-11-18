using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Create Sound", menuName = "SoundsToPlay/Sound", order = 1)]
public class SoundToPlay : ScriptableObject {

    public int id;
    public int rhyme_id;
    public string sound_name;
    public bool isUsed;

    public AudioClip audioClip;
}
