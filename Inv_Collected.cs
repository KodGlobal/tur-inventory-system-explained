using UnityEngine;

public class Inv_Collected : MonoBehaviour
{
    // The name of the object 
    public string name;
    // The image(sprite) that will be shown in the inventory
    public Sprite image;
    // A reference to the inventory script
    private Inv_Inventory inventory;

    private void Start()
    {
        // Looking for an object with the inventory script and storing it in a variable
        inventory = FindObjectOfType<Inv_Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {                 
        // Whenever the object is picked, we call the AddItem method from the inventory script, passing
        // the object's sprite, name, and the object collected by the player
        inventory.AddItem(image, name, gameObject);
    }
}
