using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputRecorder : MonoBehaviour
{
    public static InputRecorder instance { get; private set; }

    int currentFrame = 0;
    bool isRecording = true;
    bool isPlaying = false;

    DataFrameSet frameSet = new DataFrameSet();

    List<DataFrame> frameDataList = new List<DataFrame>();

    int timer = 0;
    public int interval = 6;

    private void Awake()
    {
        // Create instance if instance doesnt exist
        // Else replace instance with this
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Reset current frame on start
        currentFrame = 0;
    }

    private void Update()
    {
        // Every 6 frames, record frame data, update timer every frame
        if (timer%interval == 0)
        {
            RecordFrames(currentFrame);
        }
        timer++;


        // Debug code for manually stopping recording
        // TODO - REFACTOR THIS TO ALLOW FOR TRIGGER IN GAME
        if (Input.GetKeyDown(KeyCode.P))
        {
            isRecording = false;
            frameSet.frameDataList = frameDataList;
            SaveDataToFile();
        }

        //Debug.Log(frameDataList.Count);
    }

    private void RecordFrames(int frame)
    {
        // If not recording, or playing back an existing recording, stop recording frames
        if (!isRecording || isPlaying)
            return;

        // Create new data frame, set frame number, position, and rotation
        // Then add data frame to list of frames, and advance to next frame
        DataFrame currentDataFrame = new DataFrame();
        currentDataFrame.frameNumber = frame;
        currentDataFrame.position = transform.position;
        currentDataFrame.rotation = transform.rotation;
        frameDataList.Add(currentDataFrame);
        currentFrame++;
        return;
    }

    // TODO - REFACTOR FOR CUSTOM SAVE AND LOAD, JSON DOESNT SUPPORT LISTS
    private void SaveDataToFile()
    {
        //DataFrameSet dataFrameSet = new DataFrameSet();
        string json = JsonUtility.ToJson(frameDataList);

        File.WriteAllText(Application.dataPath + "/framedata.txt", json);

    }

    private void LoadDataFromFile()
    {

    }
}

// This is probably unneccessary
[Serializable]
public class DataFrameSet
{
    public List<DataFrame> frameDataList = new List<DataFrame>();

}

// DataFrame class, holds number, as well as 
public class DataFrame
{
    public int frameNumber;
    public int frameInterval;
    public Vector3 position = new Vector3();
    public Quaternion rotation = new Quaternion();

    public DataFrame()
    {
        frameInterval = InputRecorder.instance.interval;
    }
}

