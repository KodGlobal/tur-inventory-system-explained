using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inv_Inventory : MonoBehaviour
{       
    // A list with all the inventory buttons
    [SerializeField] List<Button> buttons = new List<Button>();   
    // All the objects from the Resources folder
    [SerializeField] List<GameObject> resourceItems = new List<GameObject>();
    [SerializeField] GameObject buttonsPath;
    // The names of objects that we collected
    [SerializeField] List<string> inventoryItems = new List<string>();
    // The object that is currently equipped
    GameObject itemInArm;
    // The spot at which the object is going to spawn spawn
    [SerializeField] Transform itemPoint;
    [SerializeField] Transform[] itemPositions;
    // A text field for inventory warning messages (Text)
    [SerializeField] TMP_Text warning;
    // The list of items collected by the player
    [SerializeField] List<GameObject> playerItems = new List<GameObject>();
    GameObject itemPosition;


    private void Start()
    {
        // Loading all the inventory items located in the Resources folder
        GameObject[] objArr = Resources.LoadAll<GameObject>("Items");
        
        // Filling out the list of possible inventory items
        resourceItems.AddRange(objArr);
        // Going through all the inventory buttons and storing them in the list
        foreach(Transform child in buttonsPath.transform)
        {
            buttons.Add(child.GetComponent<Button>());
        }
    }
    private void Update()
    {
        // Enabling/disabling the mouse cursor whenever the player presses Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public void AddItem(Sprite img, string itemName, GameObject obj)
    {        
        // If the inventory is full, we output a warning message and abort the method
        if (inventoryItems.Count >= buttons.Count)
        {
            warning.text = "Full Inventory!";
            Invoke("WarningUpdate", 1f);
            return;
        }
        // If the player already has a copy of the item, we output a warning message and abort the method
        if (inventoryItems.Contains(itemName))
        {
            warning.text = "You already have " + itemName;
            Invoke("WarningUpdate", 1f);
            return;
        }
        // Adding the item's name to the inventory
        inventoryItems.Add(itemName);
        // Getting the next free button and its Image component
        var buttonImage = buttons[inventoryItems.Count - 1].GetComponent<Image>();
        // Setting the button's image to the image of the item we collected
        buttonImage.sprite = img;
        // Destroying the item we collected
        Destroy(obj);
    }
    // A method that erases all the warning messages
    void WarningUpdate()
    {
        warning.text = "";
    }
    // This method is called whenever a button is pressed
    public void UseItem(int itemPos)
    {           
        // If we pressed a button with no item attached to it, we abort the function
        if (inventoryItems.Count <= itemPos) return;
        // Storing the name of the object attached to this button in a variable
        string item = inventoryItems[itemPos];
        // Calling the method that equips the item from the inventory, and passing the name of the item as an inventory
        GetItemFromInventory(item);
    }
    // This method equips the item. It is called from the UseItem method 
    public void GetItemFromInventory(string itemName)
    {
        // Searching for the object with the specified name in the list of all objects
        var resourceItem = resourceItems.Find(x => x.name == itemName);
        // If the object with such name does not exist in the resource folder, we abort the function
        if (resourceItem == null) return;

        // Looking for the object with the specified name in the list of player objects
        var putFind = playerItems.Find(x => x.name == itemName);

        // If such an item does not exist, then
        if (putFind == null)
        {
            // If the player already has an item equipped, we disable it
            if (itemInArm != null)
            {
                itemInArm.SetActive(false);
            }
            // Checking out which body part the object should be attached to
            var pos = resourceItem.GetComponent<Inv_ItemPosition>().positon;
            if (pos == Inv_ItemPosition.ItemPos.Head)
            {
                itemPoint.position = itemPositions[0].position;
                itemPosition = itemPositions[0].gameObject;
            }
            else if (pos == Inv_ItemPosition.ItemPos.Spine)
            {
                itemPoint.position = itemPositions[1].position;
                itemPosition = itemPositions[1].gameObject;
            }
            else
            {
                itemPoint.position = itemPositions[2].position;
                itemPosition = itemPositions[2].gameObject;
            }
            // Spawning the object at the previously defined point
            var newItem = Instantiate(resourceItem, itemPoint);
            // Moving this object to the player's Hierarchy 
            newItem.transform.parent = itemPosition.transform;
            // Naming the new object
            newItem.name = itemName;
            // Adding the object to the list of objects in player's inventory
            playerItems.Add(newItem);
            //Telling unity that the itemInArm inventory equals the newly-equipped itemТ (in other words, we remember the item that is currently equipped)
            itemInArm = newItem;
        }
        // If this item already exists in the scene, then
        else
        {
            // If this object is the object that is already equipped, we simply change its state
            {
                putFind.SetActive(!putFind.activeSelf);
            }
            // If this is another object, we simply disabling the currently equipped item and enable the new one
            else
            {
                itemInArm.SetActive(false);
                putFind.SetActive(true);
                itemInArm = putFind;
            }
        }
    }
}
