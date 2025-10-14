using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("AudioSettings")]
    [SerializeField] private AudioSource[] SFX;
    [SerializeField] private AudioSource[] BGM;
    private int BGMIndex;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)   
            instance = this;    
        else
            Destroy(this.gameObject);

        if (BGM.Length <= 0)
            return;

        InvokeRepeating(nameof(PlayMusicIfNeeded), 0, 2);
    }

    public void PlaySFX(int SFXToPlay, bool pitch = true)
    {
        if (SFXToPlay > SFX.Length)
            return;

        if(pitch == true)
            SFX[SFXToPlay].pitch = Random.Range(.9f, 1.1f); 
        SFX[SFXToPlay].Play(); 
    }

    public void StopAudio(int SFXToStop)
    {
        SFX[SFXToStop].Stop();
    }

    public void PlayMusicIfNeeded()
    {
        if (BGM[BGMIndex].isPlaying == false)
            RandomBGM();
    }
    public void RandomBGM()
    {
        BGMIndex = Random.Range(0, BGM.Length);
        BGM[BGMIndex].Play();
    }

    public void PlayBGM(int BGMToPlay)
    {
        if(BGM.Length <= 0)
        {
            Debug.LogWarning("You have no music on audio Manager");
            return;
        }

        foreach(var bgm in BGM)
        {
            bgm.Stop();
        }

        BGM[BGMToPlay].Play();
        BGMIndex = BGMToPlay;
    }
}
