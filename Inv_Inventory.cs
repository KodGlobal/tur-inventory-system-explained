using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inv_Inventory : MonoBehaviour
{       
    // Envanter butonlarının olduğu bir liste
    [SerializeField] List<Button> buttons = new List<Button>();   
    // Resources klasöründeki bütün objeler
    [SerializeField] List<GameObject> resourceItems = new List<GameObject>();
    [SerializeField] GameObject buttonsPath;
    // Topladığımız objelerin isimleri
    [SerializeField] List<string> inventoryItems = new List<string>();
    // Şu an kuşanılan objenin adı
    GameObject itemInArm;
    // Objenin oyuncu üzerinde ekleneceği konum
    [SerializeField] Transform itemPoint;
    [SerializeField] Transform[] itemPositions;
    // Envanter uyarı mesajları için bir yazı alanı (Text)
    [SerializeField] TMP_Text warning;
    // Oyuncu tarafından toplanan objelerin listesi
    [SerializeField] List<GameObject> playerItems = new List<GameObject>();
    GameObject itemPosition;


    private void Start()
    {
        // Resources klasöründeki bütün envanter objelerini yükleyelim
        GameObject[] objArr = Resources.LoadAll<GameObject>("Items");
        
        // Olası envanter itemleri listesini doldurmak
        resourceItems.AddRange(objArr);
        // Bütün envanter butonlarına bakıp bir listede tutmak
        foreach(Transform child in buttonsPath.transform)
        {
            buttons.Add(child.GetComponent<Button>());
        }
    }
    private void Update()
    {
        // Oyuncu Q tuşuna bastığında imleci aktive/deaktive etme
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
        // Eğer envanter doluysa, fonksiyonu durdurup bir uyarı mesajı gösteriyoruz 
        if (inventoryItems.Count >= buttons.Count)
        {
            warning.text = "Full Inventory!";
            Invoke("WarningUpdate", 1f);
            return;
        }
        // Eğer oyuncuda bu item zaten varsa fonksiyonu durdurup bir uyarı mesajı gösteriyoruz 
        if (inventoryItems.Contains(itemName))
        {
            warning.text = "You already have " + itemName;
            Invoke("WarningUpdate", 1f);
            return;
        }
        // İtemin adını envantere ekleme
        inventoryItems.Add(itemName);
        // Bir sonraki boş butonu ve Image bileşenini bulmak
        var buttonImage = buttons[inventoryItems.Count - 1].GetComponent<Image>();
        // Butonun görselini topladığımız objenin görseli yapma
        buttonImage.sprite = img;
        // Topladığımız itemi kaldırma
        Destroy(obj);
    }
    // Bütün uyarı mesajlarını silen bir fonksiyon
    void WarningUpdate()
    {
        warning.text = "";
    }
    // Bu fonksiyon bir butona tıklanıldığında çalışacak
    public void UseItem(int itemPos)
    {           
        // Eğer item olamayan bir butona basarsak fonksiyonu durduruyoruz 
        if (inventoryItems.Count <= itemPos) return;
        // Butona ekli itemin adını bir değişkende tutma
        string item = inventoryItems[itemPos];
        // Envanterdeki itemi kuşanmayı sağlayacak fonksiyonu çağırmak, itemin adını yollamak
        GetItemFromInventory(item);
    }
    // Bu fonksiyon itemin kuşanılmasını sağlıyor. UseItem fonksiyonunda çağrılıyor 
    public void GetItemFromInventory(string itemName)
    {
        // Objeler listesinde yollanan isimde bir obje arama
        var resourceItem = resourceItems.Find(x => x.name == itemName);
        // Eğer bu isimde bir obje yoksa fonksiyonu durduruyoruz
        if (resourceItem == null) return;

        // Oyuncudaki objeler listesinde yollanan isimde bir obje arama 
        var putFind = playerItems.Find(x => x.name == itemName);

        // Eğer yoksa
        if (putFind == null)
        {
            // Eğer zaten varsa, deaktive ediyoruz
            if (itemInArm != null)
            {
                itemInArm.SetActive(false);
            }
            // Objenin hangi vücut parçasına eklenmesi gerektiğini kontrol
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
            // Belirlenen doğma noktasına objeyi eklemek
            var newItem = Instantiate(resourceItem, itemPoint);
            // Objeyi oyuncunun hierarchysine eklemek 
            newItem.transform.parent = itemPosition.transform;
            // Yeni objeyi isimlendirmek
            newItem.name = itemName;
            // Objeyi oyuncunun envanterindeki objeler listesine eklemek
            playerItems.Add(newItem);
            //Unity'e itemInArm'ın yeni kuşanılan objeye eşit olduğunu söylüyoruz (yani, şu an kuşandığımız objeyi aklımızda tutuyoruz)
            itemInArm = newItem;
        }
        // Eğer item sahnede mevcutsa
        else
        {
            // Eğer bu obje zaten kuşanılmış durumdaysa, durumunu değiştiriyoruz
            {
                putFind.SetActive(!putFind.activeSelf);
            }
            // Eğer başka bir objeyse, şu an kuşanılan objeyi deaktive ediyoruz ve yenisini kuşanıyoruz
            else
            {
                itemInArm.SetActive(false);
                putFind.SetActive(true);
                itemInArm = putFind;
            }
        }
    }
}
