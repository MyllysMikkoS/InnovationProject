using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{

    CanListener canListener;
    private Button webSoccetButton;
    float updateInterval = 1f;
    float nextDataUpdate;
    private Boolean isConnectedToWebSocket = false;

    void Start()
    {
        canListener = CanListener.Instance;
        webSoccetButton = GameObject.Find("WebSoccetButton").GetComponent<Button>();
        webSoccetButton.GetComponent<Image>().color = Color.red;
    }

    void Update()
    {
        if (Time.time >= nextDataUpdate)
        {
            if(isConnectedToWebSocket != canListener.isConnected())
            {
                isConnectedToWebSocket = canListener.isConnected();
                webSoccetButton.GetComponent<Image>().color = canListener.isConnected() ? Color.green : Color.red;
            }
            nextDataUpdate = Time.time + updateInterval;
        }
    }

        public void ChangeToExcavatorView()
    {
        if(canListener.isConnected())
            SceneManager.LoadScene(1);
    }

    public void ConnectToWebSoccet()
    {
        if(!canListener.isConnected())
            canListener.connect();
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("PAUSE MAIN");
            canListener.stop();
        }

    }

    private void OnApplicationQuit()
    {
        Debug.Log("QUIT MAIN");
        canListener.stop();
    }
}
