using UnityEngine;

public class Inv_ItemPosition : MonoBehaviour
{
    //Selecting the body part we want the object to appear at
    public enum ItemPos
    {
        Head,
        Spine,
        RightArm
    }
    public ItemPos positon;
}
