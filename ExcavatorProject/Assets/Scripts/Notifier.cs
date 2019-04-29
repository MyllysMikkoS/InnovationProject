using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;


internal class Notifier : IDisposable
{
    private volatile bool _enabled;
    private ManualResetEvent _exited;
    private Queue<NotificationMessage> _queue;
    private object _sync;
    private Data _data;


    public Notifier()
    {
        _enabled = true;
        _exited = new ManualResetEvent(false);
        _queue = new Queue<NotificationMessage>();
        _sync = ((ICollection)_queue).SyncRoot;
        _data = new Data();

        ThreadPool.QueueUserWorkItem(
            state => 
            {
                while (_enabled || Count > 0)
                {
                    var msg = dequeue();
                    if (msg != null)
                    {
                        if (msg.Parse)
                        {
                            string[] message = msg.Body.Split(':');
                            _data.parseMessage(message[0], message[1]);
                        }
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
                _exited.Set();
            }
        );
    }

    public int Count
    {
        get
        {
            return _queue.Count;
        }
    }

    private NotificationMessage dequeue()
    {
        return _queue.Count > 0 ? _queue.Dequeue() : null;
    }

    public void Close()
    {
        _enabled = false;
        _exited.WaitOne();
        _exited.Close();
    }

    public void Notify(NotificationMessage message)
    {
        if (_enabled)
            _queue.Enqueue(message);
    }

    void IDisposable.Dispose()
    {
        Close();
    }
}

internal class NotificationMessage
{
    public string Body
    {
        get; 
        set;
    }

    public string Summary
    {
        get; set;
    }

    public Boolean Parse
    {
        get; set;
    }

    public override string ToString()
    {
        return String.Format("{0}: {1}", Summary, Body);
    }
}

internal class Data
{
    private float[] bucketAngle = new float[2] { 0, 0 };
    private float[] boomAngle = new float[2] { 0, 0 };
    private float[] armAngle = new float[2] { 0, 0 };

    public void parseMessage(string header, string message)
    {
        if (header.StartsWith("392"))
        {
            if (message != null)
                parse392(message);
        }
        else if (header.StartsWith("388"))
        {
            if (message != null)
                parse388(message);
        }
        else if (header.StartsWith("389"))
        {
            if (message != null)
                parse389(message);
        }
        else if (header.StartsWith("1418"))
        {
            if (message != null)
                parse1418(message);
        }
        /*To do if needed
        else if (header.StartsWith("386"))
        {
            if (message != null)
                parse386(message);
        }
        else if (header.StartsWith("393"))
        {
            if (message != null)
                parse393(message);
        }
        */
    }

    /*To do if needed
    private void parse386(string message)
    {
    
    }
    */

    private void parse388(string message)
    {
        string[] values = message.Split('.');
        int length = values.Length;

        if(length == 8)
        {
            ExcavatorData388.Instance.setData(turnToFloat(new string[4] { values[0], values[1], values[2], values[3] }, "388 1"),
            turnToFloat(new string[4] { values[4], values[5], values[6], values[7] }, "388 2"));
        }
    }

    private void parse389(string message)
    {
        string[] values = message.Split('.');
        int length = values.Length;

        if (length == 4)
        {
            ExcavatorData389.Instance.setData(turnToFloat(new string[4] { values[0], values[1], values[2], values[3] }, "389"));
        }
    }

    private float turnToFloat(string[] msg, string debugMsg)
    {
        string hexValue1 = (Convert.ToInt32(msg[0])).ToString("X");
        string hexValue2 = (Convert.ToInt32(msg[1])).ToString("X");
        string hexValue3 = (Convert.ToInt32(msg[2])).ToString("X");
        string hexValue4 = (Convert.ToInt32(msg[3])).ToString("X");

        uint num = uint.Parse(hexValue4 + hexValue3 + hexValue2 + hexValue1, System.Globalization.NumberStyles.AllowHexSpecifier);
        byte[] floatVals = BitConverter.GetBytes(num);
        float f = BitConverter.ToSingle(floatVals, 0);

        return f;
    }

    private void parse392(string message)
    {
        string[] values = message.Split('.');
        int length = values.Length;

        if (length > 1)
        {
            boomAngle[0] = Convert.ToInt32(values[0]);
            boomAngle[1] = Convert.ToInt32(values[1]);
            ExcavatorData392.Instance.setBoom(convertBoom(boomAngle[0], boomAngle[1]));
        }
        if (length > 3)
        {
            armAngle[0] = Convert.ToInt32(values[2]);
            armAngle[1] = Convert.ToInt32(values[3]);
            ExcavatorData392.Instance.setArm(convertArm(armAngle[0], armAngle[1]));
        }
        if (length > 5)
        {
            bucketAngle[0] = Convert.ToInt32(values[4]);
            bucketAngle[1] = Convert.ToInt32(values[5]);

            ExcavatorData392.Instance.setBucket(convertBucket(bucketAngle[0], bucketAngle[1]));
        }

        ExcavatorController.UpdateAngleData(ExcavatorData392.Instance.getBoom(), ExcavatorData392.Instance.getArm(), ExcavatorData392.Instance.getBucket());
    }

    private void parse1418(string message)
    {
        string[] values = message.Split('.');
        int length = values.Length;
        if (length > 3)
        {
            if ("96".Equals(values[0]))
            {
                if ("32".Equals(values[1]))
                {
                    if ("32".Equals(values[2]))
                    {
                        if ("2".Equals(values[3]))
                        {
                            ExcavatorZeroPoint.Instance.newZeroPoint();
                        }
                        else if ("1".Equals(values[3]))
                        {
                            //TODO Slope
                        }
                    }
                }
            }
        }
    }

    float convertBoom(float boomOne, float boomTwo)
    {
        return (float)((256 * boomTwo) + boomOne) / 10 - 5;
    }

    float convertArm(float armOne, float armTwo)
    {
        return (float)((((256 * armTwo) + armOne) - 64058) / 10) - 138f;
    }

    float convertBucket(float bucketOne, float bucketTwo)
    {
        if (bucketTwo > 2)
            return (float)(((256 * bucketTwo) + bucketOne) - 65536) / 10 - 10;
        else
            return (float)((256 * bucketTwo) + bucketOne) / 10 -10;
    }

    /*To do if needed
    private void parse393(string message)
    {
      
    }
    */
}

