using UnityEngine;
using System.Collections;

public class BasicCharacterController : MonoBehaviour
{
    public enum MovementAction {
        NONE, LEFT, RIGHT, UP, DOWN, ATTACK
    }
    MovementAction mNextMovementAction;
    Transform mTransform;
    bool bUserInput = true;

    public MovementAction GetCurrentAction()
    {
        return mNextMovementAction;
    }

    void Awake()
    {
        mTransform = this.transform;
        mNextMovementAction = MovementAction.NONE;
    }

    void Start()
    {
        GameMode.Instance.RegisterGameTickMethod(OnGameTick);
    }

    void CreateClone()
    {
        GameObject clone = Instantiate(this.gameObject, mTransform.position, mTransform.rotation);
        CloneLogic logic = clone.AddComponent<CloneLogic>();
        logic.SetOriginalCharacter(this);
        clone.SetActive(false);
    }

    void OnGameTick()
    {
        switch(mNextMovementAction) {
            case MovementAction.LEFT:
                mTransform.Translate(-1, 0, 0, Space.World);
                mTransform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case MovementAction.RIGHT:
                mTransform.Translate(1, 0, 0, Space.World);
                mTransform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case MovementAction.UP:
                mTransform.Translate(0, 0, 1, Space.World);
                mTransform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case MovementAction.DOWN:
                mTransform.Translate(0, 0, -1, Space.World);
                mTransform.rotation = Quaternion.Euler(0, 270, 0);
                break;
            case MovementAction.ATTACK:
                StartCoroutine(Attack());
                break;
        }
        mNextMovementAction = MovementAction.NONE;
    }

    IEnumerator Attack()
    {
        Transform fist = mTransform.Find("Fist");
        fist.Translate(-0.8f, 0, 0, Space.Self);
        yield return new WaitForSeconds(GameMode.Instance.GameTickInterval * 0.8f);
        fist.Translate(0.8f, 0, 0, Space.Self);
    }

    void Update()
    {
        if (bUserInput)
        {
            bool goLeft = Input.GetButtonDown("Left");
            bool goRight = Input.GetButtonDown("Right");
            bool goUp = Input.GetButtonDown("Up");
            bool goDown = Input.GetButtonDown("Down");
            bool doAttack = Input.GetButtonDown("Attack");
            if (goLeft)
            {
                mNextMovementAction = MovementAction.LEFT;
            }
            else if (goRight)
            {
                    mNextMovementAction = MovementAction.RIGHT;
            }
            else if (goUp)
            {
                mNextMovementAction = MovementAction.UP;
            }
            else if (goDown)
            {
                mNextMovementAction = MovementAction.DOWN;
            }
            else if (doAttack)
            {
                mNextMovementAction = MovementAction.ATTACK;
            }
        }
    }
}