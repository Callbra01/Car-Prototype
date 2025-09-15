using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Written by Brandon Callaway
// To be added on whatever game object that represents the ghost car
// TODO: CREATE BEHAVIOR FOR INTERACTING WITH THE PLAYER - WHILST MAINTAINING THE CORRECT FRAME INTERVAL POS/ROT DATA
// ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^ HARD PART
public class GhostCar : MonoBehaviour
{
    // Frame Data list
    List<DataFrame> frameDataList = new List<DataFrame>();

    public bool hasStarted = false;
    bool startTimer = false;

    // Timer related vars
    int timer = 0;
    int interval = 6;
    int currentFrame = 0;

    // Set interval to InputRecorders interval
    void Start()
    {
        interval = InputRecorder.instance.interval;
    }


    void Update()
    {
        // Load frame data, start timer if bool is checked - TODO: TOGGLE "hasStarted" WHEN PLAYER COMPLETES A LAP
        if (hasStarted)
        {
            frameDataList = InputRecorder.instance.loadedFrames;
            startTimer = true;
            hasStarted = false;
        }

        // If timer has started, update ghost car pos/rot data per interval
        if (startTimer)
        {
            IntervalUpdate();
        }
    }

    void IntervalUpdate()
    {
        // Prevent index error
        if (currentFrame > frameDataList.Count -1)
        {
            currentFrame = frameDataList.Count -1;
        }

        // Every interval frame, update currentFrame to change pos/rot data for ghost car
        // Update "timer" every frame
        if (timer % interval == 0)
        {
            currentFrame++;
        }
        timer++;

        // Update pos/rot data for ghost car
        transform.position = frameDataList[currentFrame].position;
        transform.rotation = frameDataList[currentFrame].rotation;
    }
}
