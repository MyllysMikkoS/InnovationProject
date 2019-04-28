using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class ExcavatorController : MonoBehaviour
{
    public Camera BucketCamera, MainCamera;
    public GameObject Body;
    public GameObject Boom;
    public GameObject Stick;
    public GameObject Bucket;
    public GameObject TipPoint;
    public GameObject ZeroPoint;
    public Text Height, HeightZoom;
    public Text Distance, DistanceZoom;
    public Canvas HomePageCanvas;
    public Canvas ButtonCanvas;
    public Canvas MainCanvas;
    public InputField SlopeAngle;

    public float BoomAngleZero;
    public float StickAngleZero;
    public float BucketAngleZero;

    static float CurrentBoomAngle;
    static float CurrentStickAngle;
    static float CurrentBucketAngle;
    

    private bool cameraSwitch = false;

    public float lerpValue = 0.1f;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (HomePageCanvas.isActiveAndEnabled == false)
            {
                HomePageCanvas.gameObject.SetActive(true);
                MainCanvas.gameObject.SetActive(false);
                CanListener.Instance.stop();
            }
        }

        // Update BucketCamera location every frame
        SetBucketCamera();

        // Update Excavator model on UI every frame
        SetExcavatorAngles();

        // Update distance texts
        SetDistanceTexts();
    }

    public void SetExcavatorAngles()
    {
        float boomAngle = Mathf.LerpAngle(Boom.transform.localEulerAngles.z, 360 - CurrentBoomAngle, Time.time * lerpValue);
        Boom.transform.localEulerAngles = new Vector3(0, 0, boomAngle);
        float stickAngle = Mathf.LerpAngle(Stick.transform.localEulerAngles.z, 360 - CurrentStickAngle, Time.time * lerpValue);
        Stick.transform.localEulerAngles = new Vector3(0, 0, stickAngle);
        float bucketAngle = Mathf.LerpAngle(Bucket.transform.localEulerAngles.z, 360 - CurrentBucketAngle, Time.time * lerpValue);
        Bucket.transform.localEulerAngles = new Vector3(0, 0, bucketAngle);
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
    }

    public void SetBucketCamera()
    {
        Vector3 offset = transform.position;
        offset.z = -30f;

        float targetX = Mathf.Lerp(BucketCamera.transform.position.x, TipPoint.transform.position.x, 1 / 0.1f * Time.deltaTime);
        float targetY = Mathf.Lerp(BucketCamera.transform.position.y, TipPoint.transform.position.y, 1 / 0.1f * Time.deltaTime);
        BucketCamera.transform.position = new Vector3(targetX, targetY, offset.z);

    }

    public void SetZeroPoint()
    {
        CanListener.Instance.sendResetMessage();
        ZeroPoint.transform.position = TipPoint.transform.position;
    }

    public void SetSlope()
    {
        string angle = SlopeAngle.text;
        if (isValidSlope(angle))
        {
            ZeroPoint.transform.rotation = Quaternion.Euler(0, 0, float.Parse(SlopeAngle.text));
            CanListener.Instance.setSlopeLevel((float.Parse(SlopeAngle.text) / 0.45f).ToString());
        }
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
        HeightZoom.text = HeightValue.ToString("Height:\n" + "0.00") + " m";
        DistanceZoom.text = DistanceValue.ToString("Distance:\n" + "0.00") + " m";
    }

    public void ChangeCamera()
    {
        ButtonCanvas.gameObject.SetActive(cameraSwitch);
        cameraSwitch = !cameraSwitch;
        MainCamera.gameObject.SetActive(!cameraSwitch);
        BucketCamera.gameObject.SetActive(cameraSwitch);
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

    private bool isValidSlope(string slope)
    {
        if (slope == null || slope == "")
        {
            return false;
        }
        double val;
        double.TryParse(slope, out val);
        if (val > -46 && val < 46)
        {
            return true;
        }
        return false;
    }

}
