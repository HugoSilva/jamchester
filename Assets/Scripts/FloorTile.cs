using UnityEngine;
using UnityEngine.UI;

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
    public Image MyUIImage;

    void Start()
    {
        GameMode.Instance.RegisterGameTickMethod(OnGameTick);
        this.transform.localRotation = Quaternion.Euler(90, Random.Range(0, 3) * 90, 0);
        if (MyType == FloorTileType.NONE) {
            Material mat = transform.Find("default").GetComponent<MeshRenderer>().material;
            Color c = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1));
            mat.color = c;
        }
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
        if (MyUIImage) {
            MyUIImage.fillAmount = (float)MyCooldown / (float)GameMode.Instance.CloneDelayTicks;
        }
    }
}