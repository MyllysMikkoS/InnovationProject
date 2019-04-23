using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExcavatorController : MonoBehaviour
{
    public GameObject Body;
    public GameObject Boom;
    public GameObject Stick;
    public GameObject Bucket;
    public GameObject TipPoint;
    public GameObject ZeroPoint;
    public Text Height;
    public Text Distance;
    public Canvas HomePageCanvas;

    public float BoomAngleZero;
    public float StickAngleZero;
    public float BucketAngleZero;

    static float CurrentBoomAngle;
    static float CurrentStickAngle;
    static float CurrentBucketAngle;

    public static bool angleLock;


    void Start()
    {
        if (HomePageCanvas.isActiveAndEnabled == false)
        {
            UpdateAngleData(BoomAngleZero, StickAngleZero, BucketAngleZero);

            SetExcavatorAngles();
        }
    }

    void Update()
    {
        // Update Excavator model on UI every frame
        SetExcavatorAngles();

        // Update distance texts
        SetDistanceTexts();
    }

    public void SetExcavatorAngles()
    {    
        Boom.transform.localEulerAngles = new Vector3(0, 0, 360 - CurrentBoomAngle);
        Stick.transform.localEulerAngles = new Vector3(0, 0, 360 - CurrentStickAngle);
        Bucket.transform.localEulerAngles = new Vector3(0, 0, 360 - CurrentBucketAngle);

        angleLock = false;
    }

    /// <summary>
    /// Call this function with joint angles as parameters and the current joint angles will be updated automatically to UI on Update().
    /// </summary>
    /// <param name="BoomAngle"></param>
    /// <param name="StickAngle"></param>
    /// <param name="BucketAngle"></param>
    public static void UpdateAngleData(float BoomAngle, float StickAngle, float BucketAngle) // For example BoomAngle = 30, StickAngle = -15 and BucketAngle = -45
    {
        CurrentBoomAngle = BoomAngle;
        CurrentStickAngle = StickAngle;
        CurrentBucketAngle = BucketAngle;

        angleLock = true;
    }

    public void SetZeroPoint()
    {
        CanListener.Instance.sendResetMessage();
        ZeroPoint.transform.position = TipPoint.transform.position;
    }

    public void SetDistanceTexts()
    {
        Vector3 position = TipPoint.transform.position;
        position.y -= 1.3f;
        Height.transform.position = position;
        position.x -= 2f;
        position.y += 1.3f;
        Distance.transform.position = position;
        float HeightValue = ExcavatorData389.Instance.getHeight();
        float DistanceValue = ExcavatorData388.Instance.getDistance();
        Height.text = HeightValue.ToString("0.00") + " m";
        Distance.text = DistanceValue.ToString("0.00") + " m";
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            CanListener.Instance.stop();
        }
    }

    private void OnApplicationQuit()
    {
        CanListener.Instance.stop();
    }
}
