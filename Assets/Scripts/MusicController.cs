using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static MusicController mC;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip MenuMusic;
    [SerializeField] private AudioClip levelMusic;

    // Update is called once per frame
    private void Awake()
    {
        if (mC == null)
        {
            mC = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Detecta mudança de cena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Escolhe a música com base no nome da cena
        switch (scene.name)
        {
            case "Menu":
                PlayMusic(MenuMusic);
                break;
            case "Fase 1":
                PlayMusic(levelMusic);
                break;
            case "Fase 2":
                PlayMusic(levelMusic);
                break;
            case "GameOver":
                PlayMusic(MenuMusic);
                break;
            default:
                StopMusic();
                break;
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip && musicSource.isPlaying)
            return; // já está tocando, não faz nada

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();

        Debug.Log("Tocando música: " + clip.name);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayUISound(AudioClip clip)
    {
        if (clip != null)
            musicSource.PlayOneShot(clip);
    }
}
