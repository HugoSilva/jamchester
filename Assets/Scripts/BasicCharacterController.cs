using UnityEngine;
using System.Collections;

public class BasicCharacterController : MonoBehaviour
{
    public int MyID;
    
    public enum MovementAction {
        NONE, RIGHT, UP, LEFT, DOWN, ATTACK
    }
    MovementAction mNextMovementAction;
    Transform mTransform;
    public bool bUserInput = true;

    public MovementAction GetCurrentAction()
    {
        return mNextMovementAction;
    }

    public void SetCurrentAction(MovementAction action)
    {
        mNextMovementAction = action;
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

    public void CreateClone()
    {
        GameMode.Instance.UnregisterGameTickMethod(OnGameTick);
    }

    void CreateClone(Vector3 pos)
    {
        GameObject clone = Instantiate(this.gameObject, pos, mTransform.rotation);
        CloneLogic logic = clone.AddComponent<CloneLogic>();
        clone.GetComponent<BasicCharacterController>().bUserInput = false;
        logic.SetOriginalCharacter(this);
        clone.SetActive(false);
    }

    void OnGameTick()
    {
        float tickInterval = GameMode.Instance.GetTimeUntilNextTick();
        Vector3 nextPosition = mTransform.position;
        switch(mNextMovementAction) {
            case MovementAction.LEFT:
                nextPosition += new Vector3(-1, 0, 0);
                mTransform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case MovementAction.RIGHT:
                nextPosition += new Vector3(1, 0, 0);
                mTransform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case MovementAction.UP:
                nextPosition += new Vector3(0, 0, 1);
                mTransform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case MovementAction.DOWN:
                nextPosition += new Vector3(0, 0, -1);
                mTransform.rotation = Quaternion.Euler(0, 270, 0);
                break;
            case MovementAction.ATTACK:
                StartCoroutine(Attack());
                break;
        }
        if (mNextMovementAction != MovementAction.NONE) {
            LeanTween.move(this.gameObject, nextPosition, tickInterval).setEase(LeanTweenType.easeOutCubic);
        }

        bool didLandOnConveyor = false;

        RaycastHit hit;
        if (Physics.Raycast(nextPosition, Vector3.down, out hit)) {
            FloorTile tile = hit.collider.GetComponent<FloorTile>();
            if (tile != null) {
                switch(tile.MyType) {
                    case FloorTileType.TIMEPAD:
                        if (bUserInput && tile.MyCooldown == 0) {
                            CreateClone(nextPosition);
                            tile.MyCooldown = 5;
                        }
                        break;
                    case FloorTileType.CONVEYOR:
                        int direction = (int)Mathf.Floor(tile.transform.eulerAngles.y / 90.0f);
                        mNextMovementAction = (MovementAction)(direction + 1);
                        didLandOnConveyor = true;
                        break;
                    case FloorTileType.TOGGLE:
                        tile.MyTarget.MyState = !tile.MyTarget.MyState;
                        break;
                    case FloorTileType.TRAPDOOR:
                        if (tile.MyState) {
                            Explode();
                        }
                        break;
                }
            }
        }


        if (!didLandOnConveyor) {
            mNextMovementAction = MovementAction.NONE;
        }
    }

    IEnumerator Attack()
    {
        Transform fist = mTransform.Find("Fist");
        fist.Translate(-0.8f, 0, 0, Space.Self);
        yield return new WaitForSeconds(GameMode.Instance.GameTickInterval * 0.8f);
        fist.Translate(0.8f, 0, 0, Space.Self);
    }

    void Explode()
    {
        Destroy(this.gameObject);
    }

    void Update()
    {
        Collider[] overlaps = Physics.OverlapBox(mTransform.position, new Vector3(0.2f, 0.2f, 0.2f), Quaternion.identity);
        for (int i = 0; i < overlaps.Length; i++) {
            Collider overlap = overlaps[i];
            BasicCharacterController otherCharacter = overlap.GetComponent<BasicCharacterController>();
            if (otherCharacter != null && otherCharacter != this) {
                this.Explode();
                otherCharacter.Explode();
            }
        }

        if (bUserInput && mNextMovementAction == MovementAction.NONE)
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