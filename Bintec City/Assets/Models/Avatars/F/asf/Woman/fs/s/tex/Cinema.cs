using UnityEngine;

public class Cinema : MonoBehaviour
{
    public GameObject cam1;
    public GameObject Player1;
    public AudioSource Musica;
    public AudioClip ClipMusica;
    void Start()
    {
        //Player1.SetActive(true);
       
    }

    public void PlayerOne()
    {
        //Player1.SetActive(true);
        cam1.SetActive(true);
        Musica.PlayOneShot(ClipMusica);
    }
}
