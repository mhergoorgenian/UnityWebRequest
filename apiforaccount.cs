using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class apiforaccount : MonoBehaviour
{
    string url= "http://82.99.219.170:8888/app";
    string yourAPIKey;
    public InputField loginusername;
    public InputField loginpassword;
    public InputField registerusername;
    public InputField registerpassword;
    public InputField confirmpassword;
    public InputField email;
    public GameObject login;
    public GameObject register;

    // Start is called before the first frame update
    void Start()
    {
       
    }


    
    
     IEnumerator loginRequest(string url)
    {
        
        WWWForm form = new WWWForm();
        form.AddField("username", loginusername.text);
        form.AddField("password", loginpassword.text);

        UnityWebRequest uwr = UnityWebRequest.Post(url, form);

        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            users userdata = JsonUtility.FromJson<users>(uwr.downloadHandler.text);
            var token = userdata.token;
            if (token!=null)
            {
                Debug.Log(userdata.token);
                PlayerPrefs.SetString("usertoken", userdata.token);
                SceneManager.LoadScene("arscene");

            }
            else
            {
                Debug.Log("user or password problem");
            }  
        }
    }




    IEnumerator registerRequest(string url)
    {

        
        WWWForm form = new WWWForm();
        form.AddField("email", email.text);
        form.AddField("username", registerusername.text);
        form.AddField("password", registerpassword.text);
        form.AddField("password2", confirmpassword.text);

        if (registerusername.text.Length >= 4 && registerpassword.text.Length >= 6 && confirmpassword.text.Length >= 6 && registerpassword.text == confirmpassword.text && IsValidEmail(email.text))
        {
            UnityWebRequest uwr = UnityWebRequest.Post(url, form);

            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
            else
            {
                if (uwr.responseCode == 200)
                {
                    Debug.Log("Received: " + uwr.downloadHandler.text);
                    Debug.Log("Received: " + uwr.responseCode);
                    login.SetActive(true);
                    register.SetActive(false);
                }
                else
                {
                    Debug.Log("email or username already registered");
                }
            }
        }
        else
        {
            Debug.Log("input problem");
        }
    }

    public void onlogin()
    {
       StartCoroutine( loginRequest(url+ "/login/"));
    }
    public void onregister()
    {
        StartCoroutine(registerRequest(url + "/register/"));
    }

    bool IsValidEmail(string email)
    {
        var trimmedEmail = email.Trim();

        if (trimmedEmail.EndsWith("."))
        {
            return false; // suggested by @TK-421
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }
}
