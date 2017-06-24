using UnityEngine;

public enum FloorTileType
{
    NONE, TIMEPAD, CONVEYOR, ELEVATOR, TRAPDOOR, TOGGLE
}

public class FloorTile : MonoBehaviour
{
    public FloorTileType MyType;
    public bool MyState;
    public FloorTile MyTarget;
    public int MyCooldown;

    void Start()
    {
        GameMode.Instance.RegisterGameTickMethod(OnGameTick);
    }

    void OnGameTick()
    {
        if (MyType == FloorTileType.ELEVATOR) {
            this.transform.Translate(0, MyState ? -1 : 1, 0, Space.World);
            MyState = !MyState;
        }
        if (MyCooldown > 0) {
            MyCooldown--;
        }
    }
}