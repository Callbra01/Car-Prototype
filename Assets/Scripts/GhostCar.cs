using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostCar : MonoBehaviour
{
    List<DataFrame> frameDataList = new List<DataFrame>();

    public bool hasStarted = false;
    bool startTimer = false;

    int timer = 0;
    int interval = 6;
    int currentFrame = 0;

    void Start()
    {
        interval = InputRecorder.instance.interval;
        this.gameObject.SetActive(false);
    }


    void Update()
    {
        if (hasStarted)
        {
            frameDataList = InputRecorder.instance.loadedFrames;
            startTimer = true;
            hasStarted = false;
        }

        if (startTimer)
        {
            this.gameObject.SetActive(true);
            IntervalUpdate();
        }
        
    }

    void IntervalUpdate()
    {
        if (currentFrame > frameDataList.Count)
        {
            currentFrame = frameDataList.Count;
        }

        if (timer % interval == 0)
        {
            currentFrame++;
        }
        timer++;

        
        transform.position = frameDataList[currentFrame].position;
        transform.rotation = frameDataList[currentFrame].rotation;
    }
}
