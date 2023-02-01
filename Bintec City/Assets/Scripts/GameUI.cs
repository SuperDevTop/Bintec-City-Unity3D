using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public GameObject main;
    public GameObject mainPortrait;
    public GameObject mobileController;

    void Start()
    {
        if(Screen.width > Screen.height)
        {
            main.SetActive(true);
            mainPortrait.SetActive(false);
            main.transform.localScale = new Vector3(Screen.width / 1600f, Screen.height / 900f, 1f);
        }
        else
        {
            main.SetActive(false);
            mainPortrait.SetActive(true);
            mainPortrait.transform.localScale = new Vector3(Screen.width / 720f, Screen.height / 1440f, 1f);
        }                        
    }

    void Update()
    {
        if(Screen.width > Screen.height)
        {
            main.SetActive(true);
            mainPortrait.SetActive(false);
            main.transform.localScale = new Vector3(Screen.width / 1600f, Screen.height / 900f, 1f);
        }
        else
        {
            main.SetActive(false);
            mainPortrait.SetActive(true);
            mainPortrait.transform.localScale = new Vector3(Screen.width / 720f, Screen.height / 1440f, 1f);
        }  
    }
}
