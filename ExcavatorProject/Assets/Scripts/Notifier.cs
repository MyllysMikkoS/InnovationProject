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
                        Thread.Sleep(500);
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
            //lock (_sync)
                return _queue.Count;
        }
    }

    private NotificationMessage dequeue()
    {
        //lock (_sync)
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
        //lock (_sync)
        {
            if (_enabled)
                _queue.Enqueue(message);
        }
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
    int temp = 0;
    private float[] bucketAngleGround = new float[2] { 0, 0 };
    private float[] bucketAngle = new float[2] { 0, 0 };
    private float[] boomAngle = new float[2] { 0, 0 };
    private float[] armAngle = new float[2] { 0, 0 };
    private float[] headingAngle = new float[2] { 0, 0 };
    private float[] heightFromZero = new float[4] { 0, 0, 0, 0 };
    private float[] distanceFromZero = new float[4] { 0, 0, 0, 0 };
    private float[] heightToSlopeFromZero = new float[4] { 0, 0, 0, 0 };
    private float[] framePitch = new float[2] { 0, 0 };
    private float[] frameRoll = new float[2] { 0, 0 };

    public void parseMessage(string header, string message)
    {
        /*
        if (header.StartsWith("386"))
        {
            if (message != null)
                parse386(message);
        }
        */
        if (header.StartsWith("388"))
        {
            if (message != null)
                parse388(message);
        }
        else if (header.StartsWith("389"))
        {
            if (message != null)
                parse389(message);
        }

        else if (header.StartsWith("392"))
        {
            if (message != null)
                parse392(message);
        }
        /*
        else if (header.StartsWith("393"))
        {
            if (message != null)
                parse393(message);
        }
    */  
    }

    private void parse386(string message)
    {
        string[] values = message.Split('.');
        int length = values.Length;
        if (length > 1)
            bucketAngleGround[0] = convertToDegrees(Convert.ToInt32(values[1]));
        if (length > 2)
            bucketAngleGround[1] = convertToDegrees(Convert.ToInt32(values[2]));
        ExcavatorData386.Instance.setData(bucketAngleGround);
    }

    private void parse388(string message)
    {

        //Debug.Log("388: " + message);
        string[] values = message.Split('.');
        int length = values.Length;
        if(length == 8)
        {
            ExcavatorData388.Instance.setData(turnToFloat(new string[4] { values[0], values[1], values[2], values[3] }, "388 1"),
            turnToFloat(new string[4] { values[4], values[5], values[6], values[7] }, "388 2"));
        }
        /*
        if (length > 0)
            heightFromZero[0] = convertToDegrees(Convert.ToInt32(values[0]));
        if (length > 1)
            heightFromZero[1] = convertToDegrees(Convert.ToInt32(values[1]));
        if (length > 2)
            heightFromZero[2] = convertToDegrees(Convert.ToInt32(values[2]));
        if (length > 3)
            heightFromZero[3] = convertToDegrees(Convert.ToInt32(values[3]));
        if (length > 4)
            distanceFromZero[0] = convertToDegrees(Convert.ToInt32(values[4]));
        if (length > 5)
            distanceFromZero[1] = convertToDegrees(Convert.ToInt32(values[5]));
        if (length > 6)
            distanceFromZero[2] = convertToDegrees(Convert.ToInt32(values[6]));
        if (length > 7)
            distanceFromZero[3] = convertToDegrees(Convert.ToInt32(values[7]));

        ExcavatorData388.Instance.setData(heightFromZero, distanceFromZero);
        */
        //Debug.Log("heightFromZero: " + heightFromZero[0] + ", " + heightFromZero[1] + ", " + heightFromZero[2] + ", " + heightFromZero[3] +
        //    "\ndistanceFromZero" + distanceFromZero[0] + ", " + distanceFromZero[1] + ", " + distanceFromZero[2] + ", " + distanceFromZero[3]);
    }

    private void parse389(string message)
    {
        //Debug.Log("389: " + message);
        string[] values = message.Split('.');
        int length = values.Length;
        if (length == 4)
        {
            ExcavatorData389.Instance.setData(turnToFloat(new string[4] { values[0], values[1], values[2], values[3] }, "389"));
        }
        /*
        if (length > 0)
            heightToSlopeFromZero[0] = convertToDegrees(Convert.ToInt32(values[0]));
        if (length > 1)
            heightToSlopeFromZero[1] = convertToDegrees(Convert.ToInt32(values[1]));
        if (length > 2)
            heightToSlopeFromZero[2] = convertToDegrees(Convert.ToInt32(values[2]));
        if (length > 3)
            heightToSlopeFromZero[3] = convertToDegrees(Convert.ToInt32(values[3]));
        
        ExcavatorData389.Instance.setData(heightToSlopeFromZero);
        */
        //Debug.Log("heightToSlopeFromZero: " + heightToSlopeFromZero[0] + ", " + heightToSlopeFromZero[1] + ", " + heightToSlopeFromZero[2] + ", " + heightToSlopeFromZero[3]);

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
        //Debug.Log(debugMsg + ": " + f);
        return f;
    }

    private void parse392(string message)
    {
        string[] values = message.Split('.');
        int length = values.Length;
        temp++;
        //Debug.Log(temp + "----------------------");
        //Debug.Log(message);
        /*
        if (length > 0)
            boomAngle[0] = convertToDegrees(Convert.ToInt32(values[0]));
        if (length > 1)
            boomAngle[1] = convertToDegrees(Convert.ToInt32(values[1]));
        if (length > 2)
            armAngle[0] = convertToDegrees(Convert.ToInt32(values[2]));
        if (length > 3)
            armAngle[1] = convertToDegrees(Convert.ToInt32(values[3]));
        if (length > 4)
            bucketAngle[0] = convertToDegrees(Convert.ToInt32(values[4]));
        if (length > 5)
            bucketAngle[1] = convertToDegrees(Convert.ToInt32(values[5]));
        if (length > 6)
            headingAngle[0] = convertToDegrees(Convert.ToInt32(values[6]));
        if (length > 7)
            headingAngle[1] = convertToDegrees(Convert.ToInt32(values[7]));
        */
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
        //Debug.Log("boomAngle: " + (int)boomAngle[0] + ", " + (int)boomAngle[1] + "\narmAngle " + (int)armAngle[0] + ", " + (int)armAngle[1] +
        //    "\nbucketAngle" + (int)bucketAngle[0] + ", " + (int)bucketAngle[1] + "\nheadingAngle " + (int)headingAngle[0] + ", " + (int)headingAngle[1]);

        ExcavatorController.UpdateAngleData(ExcavatorData392.Instance.getBoom(), ExcavatorData392.Instance.getArm(), ExcavatorData392.Instance.getBucket());
        //Debug.Log(ExcavatorData392.Instance.getBoom() + " - " + ExcavatorData392.Instance.getArm() + " - " + ExcavatorData392.Instance.getBucket());

    }

    float convertBoom(float boomOne, float boomTwo)
    {
        //Debug.Log("BOOMONE: " + boomOne + " BOOMTWO: " + boomTwo);
        return (float)((256 * boomTwo) + boomOne) / 10 - 5;
    }

    float convertArm(float armOne, float armTwo)
    {
        //Debug.Log("ARMONE: " + armOne + " ARMTWO: " + armTwo);
        return (float)((((256 * armTwo) + armOne) - 64058) / 10) - 138f;
    }

    float convertBucket(float bucketOne, float bucketTwo)
    {
        //Debug.Log("BUCKETONE: " + bucketOne + " BUCKETTWO: " + bucketTwo);
        if (bucketTwo > 2)
            return (float)(((256 * bucketTwo) + bucketOne) - 65536) / 10 - 10;
        else
            return (float)((256 * bucketTwo) + bucketOne) / 10 -10;
    }

    private void parse393(string message)
    {
        string[] values = message.Split('.');
        int length = values.Length;

        if (length > 0)
            framePitch[0] = convertToDegrees(Convert.ToInt32(values[0]));
        if (length > 1)
            framePitch[1] = convertToDegrees(Convert.ToInt32(values[1]));
        if (length > 2)
            frameRoll[0] = convertToDegrees(Convert.ToInt32(values[2]));
        if (length > 3)
            frameRoll[1] = convertToDegrees(Convert.ToInt32(values[3]));
        ExcavatorData393.Instance.setData(framePitch, frameRoll);
        //Debug.Log("framePitch: " + framePitch[0] + ", " + framePitch[1] + "\nframeRoll " + frameRoll[0] + ", " + frameRoll[1]);

    }

    private float convertToDegrees(int convertable)
    {
        float con = (float)convertable;
        return (con / 255) * 180;
    }
}

