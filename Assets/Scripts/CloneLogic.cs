using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public struct CloneFrame
{
    public Vector3 position;
    public Quaternion rotation;
    public bool action1;
    public bool action2;
}

public enum CloneState
{
    DELAY, RECORDING, PLAYBACK
}

public class CloneLogic : MonoBehaviour
{
    Transform mTransform;
    List<CloneFrame> mCloneFrames;
    int mCloneFrameIndex;
    CloneState mState;

    IEnumerator Start()
    {
        mCloneFrames = new List<CloneFrame>();
        mState = CloneState.RECORDING;
        mTransform = this.transform;
        yield return new WaitForSeconds(5.0f);
        mState = CloneState.PLAYBACK;
        mCloneFrameIndex = 0;
        yield return null;
    }

    void Update()
    {
        if (mState == CloneState.RECORDING)
        {
            CloneFrame frame;
            frame.position = mTransform.position;
            frame.rotation = mTransform.rotation;
            frame.action1 = false;
            frame.action2 = false;
            //frame.action1 = Input.GetKey("action1");
            //frame.action2 = Input.GetKey("action2");
            mCloneFrames.Add(frame);
        }
        else if (mState == CloneState.PLAYBACK)
        {
            CloneFrame frame = mCloneFrames[mCloneFrameIndex++];
            mTransform.position = frame.position;
            mTransform.rotation = frame.rotation;
            // action1 thing
            // action2 thing
        }
    }
}