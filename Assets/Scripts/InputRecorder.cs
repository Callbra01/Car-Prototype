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
    public List<DataFrame> loadedFrames = new List<DataFrame>();

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

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            Debug.Log(loadedFrames.Count);
        }

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            loadedFrames = LoadDataFromFile();
            
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
        //StreamWriter streamWriter = new StreamWriter(Application.dataPath + "/frameData.txt");
        SaveDataTestFunc("NUM", 0);
        SaveDataTestFunc("POS", 1);
        SaveDataTestFunc("POS", 2);
        SaveDataTestFunc("POS", 3);
        SaveDataTestFunc("ROT", 4);
        SaveDataTestFunc("ROT", 5);
        SaveDataTestFunc("ROT", 6);
        SaveDataTestFunc("ROT", 7);

        /*
        for (int frame = 0; frame < frameDataList.Count; frame++)
        {
            DataFrame tempFrame = frameDataList[frame];
            //streamWriter.Write($"_{tempFrame.frameNumber}-{tempFrame.position}={tempFrame.rotation}");
            //Debug.Log($"N:{tempFrame.frameNumber}, P:{tempFrame.position}, R:{tempFrame.rotation}");
        }
        */
    }

    private void SaveDataTestFunc(string mod, int caseIndex)
    {
        StreamWriter streamWriter = new StreamWriter(Application.dataPath + $"/frameData{mod}{caseIndex}.txt");

        if (caseIndex < 0 || caseIndex > 7)
        {
            Debug.Log("INVALID CASE INDEX, 0=FRAME NUMBER, 1=POSX, 2=POSY, 3=POSZ, 4=ROTX, 5=ROTY, 6=ROTZ, 7=ROTW");
            return;
        }

        for (int frame = 0; frame < frameDataList.Count; frame++)
        {
            DataFrame tempFrame = frameDataList[frame];
   
            if (caseIndex == 0)
            {
                streamWriter.Write($"{tempFrame.frameNumber}_");
            }
            else if (caseIndex == 1)
            {
                streamWriter.Write($"{tempFrame.position.x}_");
            }
            else if (caseIndex == 2)
            {
                streamWriter.Write($"{tempFrame.position.y}_");
            }
            else if (caseIndex == 3)
            {
                streamWriter.Write($"{tempFrame.position.z}_");
            }
            else if (caseIndex == 4)
            {
                streamWriter.Write($"{tempFrame.rotation.x}_");
            }
            else if (caseIndex == 5)
            {
                streamWriter.Write($"{tempFrame.rotation.y}_");
            }
            else if (caseIndex == 6)
            {
                streamWriter.Write($"{tempFrame.rotation.z}_");
            }
            else if (caseIndex == 7)
            {
                streamWriter.Write($"{tempFrame.rotation.w}_");
            }
        }
        streamWriter.Close();
    }

    private float[] LoadDataTestFunc(string mod, int caseIndex)
    {
        StreamReader streamReader = new StreamReader(Application.dataPath + $"/frameData{mod}{caseIndex}.txt");

        string[] tempInputStringArray = streamReader.ReadToEnd().Split('_');
        float[] tempFloatArray = new float[tempInputStringArray.Length];
        int[] tempIntArray = new int[tempInputStringArray.Length];

        for (int index = 0; index < tempFloatArray.Length; index++)
        {
            float currentVal;
            string currentString = tempInputStringArray[index];
            if (float.TryParse(currentString, out currentVal))
            {
                tempFloatArray[index] = currentVal;
            }
        }
        streamReader.Close();

        return tempFloatArray;
    }

    private List<DataFrame> LoadDataFromFile()
    {
        float[] dataFrameNumbers = LoadDataTestFunc("NUM", 0);

        float[] dataFramePOSX = LoadDataTestFunc("POS", 1);
        float[] dataFramePOSY = LoadDataTestFunc("POS", 2);
        float[] dataFramePOSZ = LoadDataTestFunc("POS", 3);

        float[] dataFrameROTX = LoadDataTestFunc("ROT", 4);
        float[] dataFrameROTY = LoadDataTestFunc("ROT", 5);
        float[] dataFrameROTZ = LoadDataTestFunc("ROT", 6);
        float[] dataFrameROTW = LoadDataTestFunc("ROT", 7);

        List<DataFrame> loadedDataFrames = new List<DataFrame>();

        for (int i = 0; i < dataFrameNumbers.Length; i++)
        {
            DataFrame newDataFrame = new DataFrame();
            newDataFrame.frameInterval = 6;
            newDataFrame.frameNumber = (int)dataFrameNumbers[i];
            newDataFrame.position = new Vector3(dataFramePOSX[i], dataFramePOSY[i], dataFramePOSZ[i]);
            newDataFrame.rotation = new Quaternion(dataFrameROTX[i], dataFrameROTY[i], dataFrameROTZ[i], dataFrameROTW[i]);
            loadedDataFrames.Add(newDataFrame);
        }

        return loadedDataFrames;
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

