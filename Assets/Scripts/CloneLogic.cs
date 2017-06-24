using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public struct CloneFrame
{
    public BasicCharacterController.MovementAction action;
}

public enum CloneState
{
    NONE, RECORDING, PLAYBACK
}

public class CloneLogic : MonoBehaviour
{
    Transform mTransform;
    List<CloneFrame> mCloneFrames;
    int mCloneFrameIndex;
    CloneState mState;
    BasicCharacterController mOriginalCharacter;

    void Start()
    {
        mState = CloneState.NONE;
    }

    public void SetOriginalCharacter(BasicCharacterController character)
    {
        mOriginalCharacter = character;
        mCloneFrames = new List<CloneFrame>();
        mState = CloneState.RECORDING;
        mCloneFrameIndex = 0;
        GameMode.Instance.RegisterPreGameTickMethod(OnPreGameTick);
    }

    void OnPreGameTick()
    {
        if (mState == CloneState.RECORDING)
        {
            CloneFrame frame;
            frame.action = mOriginalCharacter.GetCurrentAction();
            mCloneFrames.Add(frame);
            if (mCloneFrames.Count >= 10) {
                mState = CloneState.PLAYBACK;
                this.gameObject.SetActive(true);
            }
        }
        else if (mState == CloneState.PLAYBACK)
        {
            CloneFrame frame = mCloneFrames[mCloneFrameIndex++];
            GetComponent<BasicCharacterController>().SetCurrentAction(frame.action);
            if (mCloneFrameIndex == mCloneFrames.Count) {
                mState = CloneState.NONE;
            }
        }
    }
}