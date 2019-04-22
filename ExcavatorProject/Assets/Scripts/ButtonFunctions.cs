using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    public Canvas HomePageCanvas;
    public InputField IPAddressInputField;

    CanListener canListener;
    private Button webSoccetButton;
    float updateInterval = 1f;
    float nextDataUpdate;
    private Boolean isConnectedToWebSocket = false;


    void Start()
    {
        canListener = CanListener.Instance;
        webSoccetButton = GameObject.Find("WebSocketButton").GetComponent<Button>();
        webSoccetButton.GetComponent<Image>().color = Color.red;
    }

    void Update()
    {
        if (Time.time >= nextDataUpdate)
        {
            if(canListener != null)
            {
            if(isConnectedToWebSocket != canListener.isConnected())
            {
                isConnectedToWebSocket = canListener.isConnected();
                webSoccetButton.GetComponent<Image>().color = canListener.isConnected() ? Color.green : Color.red;
            }
            }
            nextDataUpdate = Time.time + updateInterval;
        }
    }

    public void ChangeToExcavatorView()
    {
        if (canListener.isConnected())
            HomePageCanvas.gameObject.SetActive(false);
    }

    public void ConnectToWebSoccet()
    {
        if(canListener != null)
        {
        if (!canListener.isConnected())
        {
            canListener.setIPAdress(IPAddressInputField.text);
            canListener.connect();
        }
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
