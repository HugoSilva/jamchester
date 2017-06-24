using UnityEngine;

public class BasicCharacterController : MonoBehaviour
{
    Transform mTransform;

    void Awake()
    {
        mTransform = this.transform;    
    }

    void Update()
    {
       bool goLeft = Input.GetButtonDown("Left");
       bool goRight = Input.GetButtonDown("Right");
       bool goUp = Input.GetButtonDown("Up");
       bool goDown = Input.GetButtonDown("Down");
       if (goLeft)
       {
            mTransform.Translate(-1, 0, 0);
       }
       else if (goRight)
       {
            mTransform.Translate(1, 0, 0);
       }
       else if (goUp)
       {
           mTransform.Translate(0, 0, 1);
       }
       else if (goDown)
       {
           mTransform.Translate(0, 0, -1);
       }
    }
}