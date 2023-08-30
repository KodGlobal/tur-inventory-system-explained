using UnityEngine;

public class Inv_ItemPosition : MonoBehaviour
{
    //Objenin eklenmesini istediğimiz vücut parçasının seçimi
    public enum ItemPos
    {
        Head,
        Spine,
        RightArm
    }
    public ItemPos positon;
}
