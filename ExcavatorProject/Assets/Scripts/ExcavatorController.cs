using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExcavatorController : MonoBehaviour
{
    public GameObject Body;
    public GameObject Boom;
    public GameObject Stick;
    public GameObject Bucket;

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
        CurrentBoomAngle = BoomAngleZero;
        CurrentStickAngle = StickAngleZero;
        CurrentBucketAngle = BucketAngleZero;

        SetExcavatorAngles();
    }

    // Update is called once per frame
    void Update()
    {
        // for testing update angle every 50ms => 20 times per second
        if (Time.time >= nextDataUpdate)
        {
            UpdateAngleData(50 * BoomSlider.GetComponent<Slider>().value, 110 * StickSlider.GetComponent<Slider>().value, 165 * BucketSlider.GetComponent<Slider>().value);
            nextDataUpdate = Time.time + updateInterval;
        }

        SetExcavatorAngles();
    }

    public void SetExcavatorAngles()
    {
        Boom.transform.eulerAngles = new Vector3(0, 0, CurrentBoomAngle);
        Stick.transform.eulerAngles = new Vector3(0, 0, CurrentStickAngle);
        Bucket.transform.eulerAngles = new Vector3(0, 0, CurrentBucketAngle);
    }

    public void UpdateAngleData(float BoomAngle, float StickAngle, float BucketAngle)
    {
        CurrentBoomAngle = BoomAngleZero + BoomAngle;
        CurrentStickAngle = StickAngleZero + CurrentBoomAngle + StickAngle;
        CurrentBucketAngle = BucketAngleZero + CurrentStickAngle + BucketAngle;
    }
}
