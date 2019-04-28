using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.Events;

public class CanListener
{
    WebSocketSharp.WebSocket m_socket;

    private String IPAddress = "localhost";
    private Boolean m_isConnected = false;

    private static readonly Lazy<CanListener> lazy =
        new Lazy<CanListener>(() => new CanListener());

    public static CanListener Instance { get { return lazy.Value; } }


    private CanListener()
    {

    }
     
    public void setIPAdress(String ipAddress)
    {
        IPAddress = ipAddress;
        m_socket = new WebSocketSharp.WebSocket("ws://" + IPAddress + ":8765");
        var nf = new Notifier();

        m_socket.OnMessage += (sender, e) =>
        {
            if (e.Data.Length > 4)
            {
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
        m_socket.ConnectAsync();
    }

    public Boolean isConnected()
    {
        return m_isConnected;
    }

    public void sendResetMessage()
    {
        // RZL
        byte[] bytes = { 82, 90, 76 };
        m_socket.Send(bytes);
    }

    public void setSlopeLevel(String slope)
    {
        if (slope.Length < 10) {
            byte[] keyBytes = { 83, 76, 79, 58 }; // SLO:
            byte[] slopeBytes = System.Text.Encoding.ASCII.GetBytes(slope);

            var bytes = new byte[keyBytes.Length + slopeBytes.Length];
            keyBytes.CopyTo(bytes, 0);
            slopeBytes.CopyTo(bytes, keyBytes.Length);

            m_socket.Send(bytes);
        }
    }

    public void stop()
    {
        m_socket.Close();
    }
}
