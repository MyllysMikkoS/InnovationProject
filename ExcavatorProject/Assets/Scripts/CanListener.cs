using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.Events;

public class CanListener
{
    // Start is called before the first frame update
    WebSocketSharp.WebSocket m_socket;

    private Boolean m_isConnected = false;

    private static readonly Lazy<CanListener> lazy =
        new Lazy<CanListener>(() => new CanListener());


    public static CanListener Instance { get { return lazy.Value; } }

    private CanListener()
    {
        Debug.Log("CanListener initialized");
        var nf = new Notifier();
        m_socket = new WebSocketSharp.WebSocket("ws://localhost:8765");
        m_socket.OnMessage += (sender, e) =>
        {
            //Debug.Log("CanListener OnMessage sender: " + sender + "\nCanListener OnMessage e: " + e.Data);
            if (e.Data.Length > 4)
            {
                //string[] message = e.Data.Split(':');
                //parseMessage(message[0], message[1]);
                nf.Notify(
                    new NotificationMessage
                    {
                        Summary = "WebSocket Message",
                        Body = e.Data,
                        Parse = true
                    });
            }
        };
        m_socket.OnOpen += (sender, e) =>
        {
            Debug.Log("CanListener OnOpen sender: " + e.ToString());
            m_isConnected = true;
            nf.Notify(
                    new NotificationMessage
                    {
                        Summary = "WebSocket OnOpen",
                        Body = e.ToString(),
                        Parse = false
                    });
        };
        m_socket.OnError += (sender, e) =>
        {
            Debug.Log("CanListener OnError sender: " + e.Message + e.GetType());
            nf.Notify(
                new NotificationMessage
                {
                    Summary = "WebSocket OnError",
                    Body = e.Message,
                    Parse = false
                });

        };
        m_socket.OnClose += (sender, e) =>
        {
            Debug.Log("CanListener OnClose sender: " + e.Reason + e.Code);
            m_isConnected = false;
            nf.Notify(
                new NotificationMessage
                {
                    Summary = "WebSocket OnClose",
                    Body = e.Reason + " " + e.Code
                });

        };

    }


    public void connect()
    {
        //if(!m_isConnected)
        m_socket.ConnectAsync();
    }

    public Boolean isConnected()
    {
        return m_isConnected;
    }

    public void stop()
    {
        //if(m_isConnected)
            m_socket.Close();
    }
}
