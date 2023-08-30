using UnityEngine;

public class Inv_Collected : MonoBehaviour
{
    // Objenin adı
    public string name;
    // Objenin envanterde gözükecek görseli
    public Sprite image;
    // Envanter koduna bir referans
    private Inv_Inventory inventory;

    private void Start()
    {
        // İçinde envanter kodu olan bir obje arayıp bir değişkende tutmak
        inventory = FindObjectOfType<Inv_Inventory>();
    }

    private void OnTriggerEnter(Collider other)
    {                 
        // Obje toplandığında, envanter kodundan AddItem fonksiyonunu çağırıyoruz
        // objenin görseli, adı ve oyuncu tarafından toplanan obje bilgilerini aktarıyoruz
        inventory.AddItem(image, name, gameObject);
    }
}
