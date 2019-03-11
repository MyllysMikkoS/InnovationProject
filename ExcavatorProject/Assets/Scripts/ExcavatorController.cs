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

    // FOR TESTING
    public GameObject BoomSlider;
    public GameObject StickSlider;
    public GameObject BucketSlider;

    public float BoomAngleZero;
    public float StickAngleZero;
    public float BucketAngleZero;

    float CurrentBoomAngle;
    float CurrentStickAngle;
    float CurrentBucketAngle;

    float updateInterval = 0.05f; // test interval in seconds for 20 data updates per second
    float nextDataUpdate; // test variable
    // Start is called before the first frame update
    void Start()
    {
        /*CurrentBoomAngle = BoomAngleZero;
        CurrentStickAngle = StickAngleZero;
        CurrentBucketAngle = BucketAngleZero;*/

        UpdateAngleData(BoomAngleZero, StickAngleZero, BucketAngleZero);

        SetExcavatorAngles();
        Debug.Log("ExcavatorController scene");
    }

    // Update is called once per frame
    void Update()
    {
        // for testing update angle every 50ms => 20 times per second
        if (Time.time >= nextDataUpdate)
        {
            /* Get new joint angle values from ExcavatorData classes 
            // For testing purposes the datas are taken from sliders, on real situation the parameters would look something like this:
            // UpdateAngleData(ExcavatorData392.Instance.getBoom(), ExcavatorData392.Instance.getArm(), ExcavatorData392.Instance.getBucket());
            */

            //if(CanListener.Instance.isConnected())
                UpdateAngleData(BoomSlider.GetComponent<Slider>().value, StickSlider.GetComponent<Slider>().value, BucketSlider.GetComponent<Slider>().value);
            //else
            //    SceneManager.LoadScene(0);
            nextDataUpdate = Time.time + updateInterval;
        }

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
    }

    /// <summary>
    /// Call this function with joint angles as parameters and the current joint angles will be updated automatically to UI on Update().
    /// </summary>
    /// <param name="BoomAngle"></param>
    /// <param name="StickAngle"></param>
    /// <param name="BucketAngle"></param>
    public void UpdateAngleData(float BoomAngle, float StickAngle, float BucketAngle) // For example BoomAngle = 30, StickAngle = -15 and BucketAngle = -45
    {
        CurrentBoomAngle = BoomAngle;
        CurrentStickAngle = StickAngle;
        CurrentBucketAngle = BucketAngle;
    }

    public void SetZeroPoint()
    {
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
        float HeightValue = (TipPoint.transform.position.y - ZeroPoint.transform.position.y) * 1.25f;
        float DistanceValue = (TipPoint.transform.position.x - ZeroPoint.transform.position.x) * -1.25f;
        Height.text = HeightValue.ToString("0.00") + " m";
        Distance.text = DistanceValue.ToString("0.00") + " m";
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("PAUSE MAIN");
            CanListener.Instance.stop();
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("QUIT MAIN");
        CanListener.Instance.stop();
    }
}
