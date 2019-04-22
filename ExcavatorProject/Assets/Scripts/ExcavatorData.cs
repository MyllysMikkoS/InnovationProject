using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ExcavatorData386
{
    private float[] bucketAngleGround = new float[2] { 0, 0 };
    private readonly object lock386 = new object();

    private static readonly Lazy<ExcavatorData386> lazy =
        new Lazy<ExcavatorData386>(() => new ExcavatorData386());

    public static ExcavatorData386 Instance { get { return lazy.Value; } }


    public void setData(float[] bucket)
    {
        lock (lock386)
        {
            bucketAngleGround = bucket;
        }
    }

    public float[] getBucketAngle()
    {
        lock (lock386)
            return bucketAngleGround;
    }
}

class ExcavatorData388
{
    private float heightFromZero = 0;
    private float distanceFromZero = 0;
    private readonly object lock388 = new object();

    private static readonly Lazy<ExcavatorData388> lazy =
    new Lazy<ExcavatorData388>(() => new ExcavatorData388());

    public static ExcavatorData388 Instance { get { return lazy.Value; } }


    public void setData(float height, float distance)
    {
        lock (lock388)
        {
            heightFromZero = height;
            distanceFromZero = distance;
        }
    }

    public float getHeight()
    {
        lock (lock388)
            return heightFromZero;
    }

    public float getDistance()
    {
        lock (lock388)
            return distanceFromZero;
    }
}

class ExcavatorData389
{
    private float heightToSlopeFromZero = 0;
    private readonly object lock389 = new object();

    private static readonly Lazy<ExcavatorData389> lazy =
    new Lazy<ExcavatorData389>(() => new ExcavatorData389());

    public static ExcavatorData389 Instance { get { return lazy.Value; } }


    public void setData(float height)
    {
        lock (lock389)
        {
            heightToSlopeFromZero = height;
        }
    }

    public float getHeight()
    {
        lock (lock389)
            return heightToSlopeFromZero;
    }
}


class ExcavatorData392
{
    private float bucketAngle = 0;
    private float boomAngle = 0;
    private float armAngle = 0;
    private float[] headingAngle = new float[2] { 0, 0 };
    private readonly object lock392 = new object();

    private static readonly Lazy<ExcavatorData392> lazy =
    new Lazy<ExcavatorData392>(() => new ExcavatorData392());

    public static ExcavatorData392 Instance { get { return lazy.Value; } }


    public void setBoom(float boom)
    {
        lock (lock392)
        {
            boomAngle = boom;
        }
    }

    public void setArm(float arm)
    {
        lock (lock392)
        {
            armAngle = arm;
        }
    }

    public void setBucket(float bucket)
    {
        lock (lock392)
        {
            bucketAngle = bucket;
        }
    }

    public float getBoom()
    {
        lock (lock392)
            return boomAngle;
    }

    public float getArm()
    {
        lock (lock392)
            return armAngle;
    }

    public float getBucket()
    {
        lock (lock392)
            return bucketAngle;
    }
}

class ExcavatorData393
{
    private float[] framePitch = new float[2] { 0, 0 };
    private float[] frameRoll = new float[2] { 0, 0 };
    private readonly object lock393 = new object();

    private static readonly Lazy<ExcavatorData393> lazy =
    new Lazy<ExcavatorData393>(() => new ExcavatorData393());

    public static ExcavatorData393 Instance { get { return lazy.Value; } }


    public void setData(float[] pitch, float[] roll)
    {
        lock (lock393)
        {
            framePitch = pitch;
            frameRoll = roll;
        }
    }

    public float[] getPitch()
    {
        lock (lock393)
            return framePitch;
    }

    public float[] getRoll()
    {
        lock (lock393)
            return frameRoll;
    }
}
