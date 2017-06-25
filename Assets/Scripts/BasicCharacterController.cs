using UnityEngine;
using System.Collections;


public class BasicCharacterController : MonoBehaviour
{
    public int MyID;

    public AudioClip SpawnAudio;
    public AudioClip PunchAudio;
    
    public enum MovementAction {
        NONE, RIGHT, UP, LEFT, DOWN, ATTACK
    }
    MovementAction mNextMovementAction;
    Transform mTransform;
    public bool bUserInput = true;
    FloorTile mLastTile;

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

    void OnDestroy()
    {
        GameMode.Instance.UnregisterGameTickMethod(OnGameTick);
    }

    public void CreateClone(Vector3 pos)
    {
        GameObject clone = Instantiate(this.gameObject, pos, mTransform.rotation);
        CloneLogic logic = clone.AddComponent<CloneLogic>();
        clone.GetComponent<BasicCharacterController>().bUserInput = false;
        logic.SetOriginalCharacter(this);
        logic.SetExplosion(this.explosion);
        clone.SetActive(false);
    }

    void OnGameTick()
    {
        float tickInterval = GameMode.Instance.GetTimeUntilNextTick();
        Vector3 nextPosition = mTransform.position;
        float wrapAroundX = 0;
        float wrapAroundX2 = 0;
        bool doWrapAround = false;
        switch(mNextMovementAction) {
            case MovementAction.LEFT:
                nextPosition += new Vector3(-1, 0, 0);
                if (mTransform.position.x <= -5) {
                    doWrapAround = true;
                    wrapAroundX = 3;
                    wrapAroundX2 = 4;
                }
                mTransform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case MovementAction.RIGHT:
                nextPosition += new Vector3(1, 0, 0);
                if (mTransform.position.x >= 3) {
                    doWrapAround = true;
                    wrapAroundX = -5;
                    wrapAroundX2 = -6;
                }
                mTransform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case MovementAction.UP:
                if (mTransform.position.z < 5) {
                    nextPosition += new Vector3(0, 0, 1);
                }
                 mTransform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case MovementAction.DOWN:
                if (mTransform.position.z > -1) {
                    nextPosition += new Vector3(0, 0, -1);
                }
                mTransform.rotation = Quaternion.Euler(0, 270, 0);
                break;
            case MovementAction.ATTACK:
                StartCoroutine(Attack(nextPosition));
                break;
        }

        bool canMove = true;

        Collider[] overlaps = Physics.OverlapBox(nextPosition, new Vector3(0.2f, 0.2f, 0.2f), Quaternion.identity);
        for (int i = 0; i < overlaps.Length; i++) {
            Collider overlap = overlaps[i];
            BasicCharacterController otherCharacter = overlap.GetComponent<BasicCharacterController>();
            if (otherCharacter != null  && otherCharacter.bUserInput && otherCharacter != this) {
                canMove = false;
            }
        }

        if (canMove || !bUserInput) {
            if (mNextMovementAction != MovementAction.NONE && !doWrapAround) {
                LeanTween.move(this.gameObject, nextPosition, tickInterval).setEase(LeanTweenType.easeOutCubic);
            } else if (doWrapAround) {
                mTransform.position = nextPosition;
                LeanTween.move(this.gameObject, nextPosition, tickInterval * 0.5f)
                    .setOnComplete(()=>{
                        nextPosition.x = wrapAroundX2;
                        mTransform.position = nextPosition;
                        nextPosition.x = wrapAroundX;
                        LeanTween.move(this.gameObject, nextPosition, tickInterval * 0.5f)
                            .setEase(LeanTweenType.easeOutCubic);
                    });
            }
        }

        bool didLandOnConveyor = false;

        RaycastHit hit;
        if (Physics.Raycast(nextPosition, Vector3.down, out hit)) {
            FloorTile tile = hit.collider.GetComponent<FloorTile>();

            if (tile != mLastTile) {
                LeanTween.moveY(tile.gameObject, -0.2f, 0.2f)
                    .setEase(LeanTweenType.easeOutElastic)
                    .setOnComplete(() => {
                        LeanTween.moveY(tile.gameObject, 0.0f, 0.2f);
                    });
            }

            mLastTile = tile;

            if (tile != null) {
                switch(tile.MyType) {
                    case FloorTileType.TIMEPAD:
                        if (bUserInput && tile.MyCooldown == 0) {
                            CreateClone(nextPosition);
                            tile.MyCooldown = GameMode.Instance.CloneDelayTicks;
                            GetComponent<AudioSource>().PlayOneShot(SpawnAudio);
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

    IEnumerator Attack(Vector3 pos)
    {
        GetComponent<AudioSource>().PlayOneShot(PunchAudio);
        float animTime = GameMode.Instance.GameTickInterval * 0.5f;
        Transform fist = mTransform.Find("Fist");
        LeanTween.moveLocalX(fist.gameObject, fist.localPosition.x - 0.8f, animTime)
            .setEase(LeanTweenType.easeOutExpo);
        yield return new WaitForSeconds(animTime);
        LeanTween.moveLocalX(fist.gameObject, fist.localPosition.x + 0.8f, animTime)
            .setEase(LeanTweenType.easeOutExpo);

        RaycastHit hit;
        Debug.DrawRay(pos, mTransform.right * -1, Color.red, 1.0f);
        if (Physics.Raycast(pos, mTransform.right * -1, out hit, 1.0f)) {
            Debug.Log(hit.collider.name);
            BasicCharacterController otherCharacter = hit.collider.GetComponent<BasicCharacterController>();
            if (otherCharacter != null && otherCharacter != this) {
                otherCharacter.Explode();
            }
        }
    }

    public GameObject explosion;
    void Explode()
    {
        GameObject drop = Instantiate(explosion, transform.position, Quaternion.identity);
        if (bUserInput) {
            GameMode.Instance.OnPlayerDie(this);
            this.gameObject.SetActive(false);
            Invoke("Respawn", 3);
        }
        else
        {
            Destroy(this.gameObject);
        }
        Destroy(drop.gameObject, 1);
    }

    void Respawn()
    {
        this.gameObject.SetActive(true);
    }

    void Update()
    {
        Collider[] overlaps = Physics.OverlapBox(mTransform.position, new Vector3(0.2f, 0.2f, 0.2f), Quaternion.identity);
        for (int i = 0; i < overlaps.Length; i++) {
            Collider overlap = overlaps[i];
            CloneLogic otherCharacter = overlap.GetComponent<CloneLogic>();
            if (otherCharacter != null && otherCharacter != this.GetComponent<CloneLogic>()) {
                this.Explode();
                otherCharacter.GetComponent<BasicCharacterController>().Explode();
            }
        }

        if (bUserInput && mNextMovementAction == MovementAction.NONE)
        {
            if (cInput.GetKey("attack" + MyID)) {
                mNextMovementAction = MovementAction.ATTACK;
            } else if (cInput.GetKeyDown("left" + MyID)) {
                mNextMovementAction = MovementAction.LEFT;
            } else if (cInput.GetKeyDown("right" + MyID)) {
                mNextMovementAction = MovementAction.RIGHT;
            } else if (cInput.GetKeyDown("down" + MyID)) {
                mNextMovementAction = MovementAction.DOWN;
            } else if (cInput.GetKeyDown("up" + MyID)) {
                mNextMovementAction = MovementAction.UP;
            }            
        }
    }
}