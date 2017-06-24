using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public struct CloneFrame
{
    public BasicCharacterController.MovementAction action;
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
    BasicCharacterController mOriginalCharacter;

    public void SetOriginalCharacter(BasicCharacterController character)
    {
        mOriginalCharacter = character;
        mCloneFrames = new List<CloneFrame>();
        mState = CloneState.RECORDING;
        mCloneFrameIndex = 0;
    }

    void OnGameTick()
    {
        if (mState == CloneState.RECORDING)
        {
            CloneFrame frame;
            frame.action = mOriginalCharacter.GetCurrentAction();
            mCloneFrames.Add(frame);
            if (mCloneFrames.Count >= 20) {
                mState = CloneState.PLAYBACK;
                this.gameObject.SetActive(true);
            }
        }
        else if (mState == CloneState.PLAYBACK)
        {
            
        }
    }

}