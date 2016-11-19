using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    // PUBLICS
    public int choicesListSize;
    public SoundToPlay[] soundsList;

    // PRIVATES
    public SoundToPlay initialSound;

    public SoundToPlay[] similarSounds;
    public SoundToPlay[] differentSounds;
    public SoundToPlay[] choicesSounds;

    private int similarSoundsCounter = 0;
    private int differentSoundsCounter = 0;
    private int randSoundIndex;

    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        choicesSounds = new SoundToPlay[choicesListSize];

        audioSource = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioSource>();

        FindNewInitialSound(-1);

        UpdateSoundsLists();

        InvokeRepeating("GameLoop", 0, 2f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GameLoop()
    {
        FindNewInitialSound(randSoundIndex);
        UpdateSoundsLists();
        UpdateChoicesList();
    }

    void UpdateChoicesList()
    {
        // Set next similar sound in choices
        int similarSoundIndex = Random.Range(0, choicesSounds.Length);
        int randSimiliarSoundIndex = Random.Range(0, similarSounds.Length);

        choicesSounds[similarSoundIndex] = similarSounds[randSimiliarSoundIndex];

        // Set different sounds used indices in array (init to -1)
        int[] usedIndices = new int[choicesSounds.Length - 1];
        for (int i = 0; i < usedIndices.Length; i++)
        {
            usedIndices[i] = -1;
        }

        int usedIndicesCounter = 0;
        for (int i = 0; i < choicesListSize; i++)
        {
            if (i != similarSoundIndex)
            {
                int randDifferentIndex = FindAvailableDifferentIndex(ref usedIndices);

                usedIndices[usedIndicesCounter] = randDifferentIndex;
                usedIndicesCounter++;

                choicesSounds[i] = differentSounds[randDifferentIndex];
            }
        }
    }

    void FindNewInitialSound(int lastRandIndex)
    {
        randSoundIndex = Random.Range(0, soundsList.Length);
        if (randSoundIndex == lastRandIndex)
        {
            FindNewInitialSound(lastRandIndex);
        }

        initialSound = soundsList[randSoundIndex];
    }

    // Find similar and different sounds than initial sound
    void UpdateSoundsLists()
    {
        similarSoundsCounter = 0;
        differentSoundsCounter = 0;

        for (int i = 0; i < soundsList.Length; i++)
        {
            if ((initialSound.rhyme_id == soundsList[i].rhyme_id) && (initialSound.id != soundsList[i].id))
            {
                similarSoundsCounter++;
            }
            else if ((initialSound.rhyme_id != soundsList[i].rhyme_id))
            {
                differentSoundsCounter++;
            }
        }

        similarSounds = new SoundToPlay[similarSoundsCounter];
        differentSounds = new SoundToPlay[differentSoundsCounter];

        similarSoundsCounter = 0;
        differentSoundsCounter = 0;

        for (int i = 0; i < soundsList.Length; i++)
        {
            if ((initialSound.rhyme_id == soundsList[i].rhyme_id) && (initialSound.id != soundsList[i].id))
            {
                similarSounds[similarSoundsCounter] = soundsList[i];
                similarSoundsCounter++;
            }
            else if ((initialSound.rhyme_id != soundsList[i].rhyme_id))
            {
                differentSounds[differentSoundsCounter] = soundsList[i];
                differentSoundsCounter++;
            }
        }
    }

    int FindAvailableDifferentIndex(ref int[] usedIndices)
    {
        int randIndex = Random.Range(0, differentSounds.Length);
        for (int j = 0; j < usedIndices.Length; j++)
        {
            if (randIndex == usedIndices[j])
            {
                randIndex = FindAvailableDifferentIndex(ref usedIndices);
            }
        }
        return randIndex;
    }
}
