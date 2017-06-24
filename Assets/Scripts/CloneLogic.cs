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
    GameObject explosion;

    void OnDestroy()
    {
        GameMode.Instance.UnregisterPreGameTickMethod(OnPreGameTick);
    }

    public void SetExplosion(GameObject explosion) {
        this.explosion = explosion;
    }

    public void SetOriginalCharacter(BasicCharacterController character)
    {
        mOriginalCharacter = character;
        mCloneFrames = new List<CloneFrame>();
        mState = CloneState.RECORDING;
        mCloneFrameIndex = 0;
        GameMode.Instance.RegisterPreGameTickMethod(OnPreGameTick);
    }

    public void OnPreGameTick()
    {
        if (mState == CloneState.RECORDING)
        {
            CloneFrame frame;
            frame.action = mOriginalCharacter.GetCurrentAction();
            mCloneFrames.Add(frame);
            if (mCloneFrames.Count >= GameMode.Instance.CloneDelayTicks) {
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
                Destroy(this.gameObject);

                GameObject drop = Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(drop, 1);
            }
        }
    }
}