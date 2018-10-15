using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSecrets : MonoBehaviour {

    public InputField clientIdField;
    public InputField tokenField;
    public InputField usernameField;
    public InputField channelField;
    public Button linkBotButton;
    public Text infoText;
    public static SetSecrets Instance { get; private set; }
    public string CLIENT_ID;
    public string OAUTH_TOKEN;
    public string USERNAME_FROM_OAUTH_TOKEN;
    public string CHANNEL_ID_FROM_OAUTH_TOKEN;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        linkBotButton = GameObject.Find("Link Bot Button").GetComponent<Button>();

        clientIdField = GameObject.Find("Client ID Field").GetComponent<InputField>();
        tokenField = GameObject.Find("Token Field").GetComponent<InputField>();
        usernameField = GameObject.Find("Username Field").GetComponent<InputField>();
        channelField = GameObject.Find("Channel Field").GetComponent<InputField>();

        infoText = GameObject.Find("Info Text").GetComponent<Text>();
        infoText.enabled = false;
    }

    // Use this for initialization
    void Start()
    {

        if (PlayerPrefs.HasKey("CHANNEL_ID"))
        {
            GetPlayerPrefs();
        }
        else
        {
            //Do nothing, because the player hasn't set up their stream credentials yet.
        }
    }

    public void LinkBot()
    {
        //Make our data in secrets equal to whatever was put in the text field
        CLIENT_ID = clientIdField.GetComponent<InputField>().text;
        OAUTH_TOKEN = tokenField.GetComponent<InputField>().text;
        USERNAME_FROM_OAUTH_TOKEN = usernameField.GetComponent<InputField>().text;
        CHANNEL_ID_FROM_OAUTH_TOKEN = channelField.GetComponent<InputField>().text;
        
        //Create a playerprefs entry with each field's data
        SetPlayerPrefs();

        StartCoroutine(ShowInfoText());
    }

    public void SetPlayerPrefs()
    {

        PlayerPrefs.SetString("CLIENT_ID", CLIENT_ID);
        PlayerPrefs.SetString("OAUTH_TOKEN", OAUTH_TOKEN);
        PlayerPrefs.SetString("CHANNEL_NAME", USERNAME_FROM_OAUTH_TOKEN);
        PlayerPrefs.SetString("CHANNEL_ID", CHANNEL_ID_FROM_OAUTH_TOKEN);
        PlayerPrefs.Save();
    }

    public void GetPlayerPrefs()
    {
        CLIENT_ID = PlayerPrefs.GetString("CLIENT_ID");
        clientIdField.GetComponent<InputField>().text = CLIENT_ID;

        OAUTH_TOKEN = PlayerPrefs.GetString("OAUTH_TOKEN");
        clientIdField.GetComponent<InputField>().text = OAUTH_TOKEN;

        USERNAME_FROM_OAUTH_TOKEN = PlayerPrefs.GetString("CHANNEL_NAME");
        clientIdField.GetComponent<InputField>().text = USERNAME_FROM_OAUTH_TOKEN;

        CHANNEL_ID_FROM_OAUTH_TOKEN = PlayerPrefs.GetString("CHANNEL_ID");
        clientIdField.GetComponent<InputField>().text = CHANNEL_ID_FROM_OAUTH_TOKEN;
    }

    public IEnumerator ShowInfoText()
    {
        infoText.enabled = true;
        yield return new WaitForSeconds(30);
        infoText.enabled = false;
        yield return null;
    }
}
