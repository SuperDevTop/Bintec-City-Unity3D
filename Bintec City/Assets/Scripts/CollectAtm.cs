using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Runtime.InteropServices;

public class CollectAtm : MonoBehaviour
{
    [System.Serializable]
    public class AtmInfos
    {
        public string[] atms_found;
        public string user_id;
    }

    [DllImport("__Internal")]
    private static extern string GetURLFromPage();

    public string ReadURL()
    {
        return GetURLFromPage();
    }

    public GameObject rockets;    
    public Text countText;
    public GameObject exitCard;      
    public Material matAtm;

    int countATM = 0;

    public string userId = "";

    string gameURL = "";    
    string getURL = "https://v15fl682ie.execute-api.us-east-1.amazonaws.com/prod/getatms?user_id=";
    string postURL = "https://v15fl682ie.execute-api.us-east-1.amazonaws.com/prod/pushatms";
    
    void Start()
    {       
        gameURL = ReadURL();                
        userId = GetUserId(gameURL);

        // gameURL = "https://bintec.xalvia.co/game/?user_id=D1563bd6e2b27";
        // userId = GetUserId(gameURL);        
        
        StartCoroutine(GetRequest(getURL + userId));
    }

    // Find ATMs
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "ATM")
        {
            countATM++;
            
            foreach(Transform foundAtm in other.transform.GetComponentsInChildren<Transform>())
            {
                foundAtm.GetComponent<MeshRenderer>().material = matAtm;
            }

            other.tag = "Found";
            other.GetComponent<BoxCollider>().isTrigger = false;

            // Request to post when the player found new ATM.
            StartCoroutine(PostRequest(postURL, VectorToJson(userId, int.Parse(other.gameObject.name))));
        }        
    }

    void Update()
    {
        // Finish game when the player finds all ATMs.
        if(countATM == 25)
        {
            rockets.SetActive(true);
            exitCard.SetActive(true);
        }

        // Output Atms the player found.
        countText.text = countATM.ToString();
    }

    IEnumerator GetRequest(string uri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(uri);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);

            AtmInfos loadData = JsonUtility.FromJson<AtmInfos>(uwr.downloadHandler.text);
            countATM = loadData.atms_found.Length;

            for (int i = 0; i < loadData.atms_found.Length; i++)
            {
                int atmNum = int.Parse(loadData.atms_found[i]);
                GameObject atmFound = GameObject.Find(atmNum.ToString());

                foreach (Transform foundAtm in atmFound.transform.GetComponentsInChildren<Transform>())
                {
                    foundAtm.GetComponent<MeshRenderer>().material = matAtm;
                }

                atmFound.tag = "Found";
                atmFound.GetComponent<BoxCollider>().isTrigger = false;
            }
        }
    }

    IEnumerator PostRequest(string url, string json)
    {
        var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);            
        }
    }

    public static string VectorToJson(string userId, int atmFound)
    {
		return string.Format(@"{{""user_id"":""{0}"", ""atm_found"":{1}}}", userId, atmFound);
	}

    // Get user_id from gameURL.
    public string GetUserId(string gameUrl)
    {
        string temp = "";

        for(int i = 0; i < gameUrl.Length; i++)
        {
            temp += gameUrl[i];
            
            if(gameUrl[i] == '=')
            {
                temp = "";
            }
        }

        return temp;
    }
}