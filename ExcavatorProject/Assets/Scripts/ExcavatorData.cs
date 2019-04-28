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
        bucketAngleGround = bucket;
    }

    public float[] getBucketAngle()
    {
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
        heightFromZero = height;
        distanceFromZero = distance;
    }

    public float getHeight()
    {
        return heightFromZero;
    }

    public float getDistance()
    {
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
        heightToSlopeFromZero = height;
    }

    public float getHeight()
    {
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
        boomAngle = boom;
    }

    public void setArm(float arm)
    {
        armAngle = arm;
    }

    public void setBucket(float bucket)
    {
        bucketAngle = bucket;
    }

    public float getBoom()
    {
        return boomAngle;
    }

    public float getArm()
    {
            return armAngle;
    }

    public float getBucket()
    {
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
        framePitch = pitch;
        frameRoll = roll;
    }

    public float[] getPitch()
    {
        return framePitch;
    }

    public float[] getRoll()
    {
        return frameRoll;
    }
}
