using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ExcavatorData386
{
    private volatile float[] bucketAngleGround = new float[2] { 0, 0 };

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
    private volatile float heightFromZero = 0;
    private volatile float heightToSlopeFromZero = 0;

    private static readonly Lazy<ExcavatorData388> lazy =
    new Lazy<ExcavatorData388>(() => new ExcavatorData388());

    public static ExcavatorData388 Instance { get { return lazy.Value; } }


    public void setData(float height, float slopeHeigtht)
    {
        heightFromZero = height;
        heightToSlopeFromZero = slopeHeigtht;
    }

    public float getHeight()
    {
        return heightFromZero;
    }

    public float getSlopeHeight()
    {
        return heightToSlopeFromZero;
    }
}

class ExcavatorData389
{
    private volatile float distanceFromZero = 0;

    private static readonly Lazy<ExcavatorData389> lazy =
    new Lazy<ExcavatorData389>(() => new ExcavatorData389());

    public static ExcavatorData389 Instance { get { return lazy.Value; } }


    public void setData(float distance)
    {
        distanceFromZero = distance;
    }

    public float getDistance()
    {
        return distanceFromZero;
    }
}


class ExcavatorData392
{
    private volatile float bucketAngle = 0;
    private volatile float boomAngle = 0;
    private volatile float armAngle = 0;
    private volatile float[] headingAngle = new float[2] { 0, 0 };

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
    private volatile float[] framePitch = new float[2] { 0, 0 };
    private volatile float[] frameRoll = new float[2] { 0, 0 };

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

class ExcavatorZeroPoint
{
    private volatile bool zeroPointBool = false;

    private static readonly Lazy<ExcavatorZeroPoint> lazy =
        new Lazy<ExcavatorZeroPoint>(() => new ExcavatorZeroPoint());

    public static ExcavatorZeroPoint Instance { get { return lazy.Value; } }

    public void newZeroPoint()
    {
        zeroPointBool = true;
    }

    public bool checkZeroPoint()
    {
        return zeroPointBool;
    }

    public void zeroPointSet()
    {
        zeroPointBool = false;
    }
}

/* TODO
class ExcavatorSlope
{
    private volatile bool slopeBool = false;

    private static readonly Lazy<ExcavatorSlope> lazy =
        new Lazy<ExcavatorSlope>(() => new ExcavatorSlope());

    public static ExcavatorSlope Instance { get { return lazy.Value; } }

    public void newSlope()
    {
        slopeBool = true;
    }

    public bool checkSlope()
    {
        return slopeBool;
    }

    public void slopeSet()
    {
        slopeBool = false;
    }
}
*/