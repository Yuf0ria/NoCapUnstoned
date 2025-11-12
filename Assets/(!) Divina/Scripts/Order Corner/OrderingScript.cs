using UnityEngine;using UnityEngine.UI;using System.Collections.Generic;using TMPro;using DG.Tweening;

public class OrderingScript : MonoBehaviour
{
    [System.Serializable]
    public class foodList
    {
        public string FoodID;
        public string FoodVendor;
        public Image foodPic;
        public string foodName;
        public float price;
        public int quantity;
        public TextMeshProUGUI quantityDisplay;
        public Button addFood;
        public Button removeFood;
    }

    [System.Serializable]
    public struct foodPick
    {
        public string FoodID;
        public float price;
        public int quantity;
    }

    public List<foodList> menuItems = new List<foodList>();
    public List<foodPick> orderItems = new List<foodPick>();

    [SerializeField] private GameObject[] receipt;
    [SerializeField] private float totalAmountValue;
    [SerializeField] private int totalQuantity;

    [Header("UI stuff - Text")]
    //TextMeshProUGUI
    [SerializeField] private TextMeshProUGUI displayTotalQuantity;
    [SerializeField] private TextMeshProUGUI totalAmountDisplay;
    [SerializeField] private TextMeshProUGUI buttonTotalAmountDisplay;
    [Header("UI stuff - Buttons")]
    // Buttons
    [SerializeField] private Button cartButton;
    [SerializeField] private Button purschaseButton;

    [Header("Food Preparation")]
    [SerializeField] private FoodPreparationSlider foodPreparationSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < menuItems.Count; i++)
        {
            int index = i;
            menuItems[i].addFood.onClick.AddListener(() => AddQuantity(index));
            menuItems[i].removeFood.onClick.AddListener(() => RemoveQuantity(index));
        }

        // Deactivate all receipt GameObjects
        for (int i = 0; i < receipt.Length; i++)
        {
            receipt[i].SetActive(false);
        }

        cartButton.interactable = false;

        purschaseButton.onClick.AddListener(PurchaseFood);
    }

    void AddQuantity(int index)
    {
        if (totalQuantity >= 4)
        {
            Debug.Log("I add too much for myself now...");
            return;
        }
        menuItems[index].quantity += 1;
        if (menuItems[index].quantityDisplay != null)
        {
            menuItems[index].quantityDisplay.text = menuItems[index].quantity.ToString();
        }

        // Check if the food is already in orderItems
        bool found = false;
        int orderIndex = -1;
        for (int i = 0; i < orderItems.Count; i++)
        {
            if (orderItems[i].FoodID == menuItems[index].FoodID)
            {
                // Update existing
                orderItems[i] = new foodPick
                {
                    FoodID = menuItems[index].FoodID,
                    price = menuItems[index].price,
                    quantity = menuItems[index].quantity
                };
                found = true;
                orderIndex = i;
                // Update receipt for existing item
                UpdateReceipt(orderIndex);
                break;
            }
        }
        if (!found)
        {
            // Add new
            orderItems.Add(new foodPick
            {
                FoodID = menuItems[index].FoodID,
                price = menuItems[index].price,
                quantity = menuItems[index].quantity
            });

            // Update receipt for new item
            UpdateReceipt(orderItems.Count - 1);
        }

        // Recalculate totals
        totalAmountValue = 0f;
        totalQuantity = 0;
        for (int i = 0; i < orderItems.Count; i++)
        {
            totalAmountValue += orderItems[i].price * orderItems[i].quantity;
            totalQuantity += orderItems[i].quantity;
        }

        // Updating Text

        if (displayTotalQuantity != null)
        {
            displayTotalQuantity.text = "Basket: " + totalQuantity + " item/s";
        }
        if (totalAmountDisplay != null)
        { totalAmountDisplay.text = totalAmountValue.ToString("F2"); }
        if (buttonTotalAmountDisplay != null)
        { buttonTotalAmountDisplay.text = totalAmountValue.ToString("F2"); }
    }

    void RemoveQuantity(int index)
    {
        if (menuItems[index].quantity > 0)
        {
            menuItems[index].quantity -= 1;
            if (menuItems[index].quantityDisplay != null)
            {
                menuItems[index].quantityDisplay.text = menuItems[index].quantity.ToString();
            }

            // Update orderItems
            for (int i = 0; i < orderItems.Count; i++)
            {
                if (orderItems[i].FoodID == menuItems[index].FoodID)
                {
                    if (menuItems[index].quantity > 0)
                    {
                        orderItems[i] = new foodPick
                        {
                            FoodID = menuItems[index].FoodID,
                            price = menuItems[index].price,
                            quantity = menuItems[index].quantity
                        };
                        // Update receipt for reduced quantity
                        UpdateReceipt(i);
                    }
                    else
                    {
                        orderItems.RemoveAt(i);
                        // Deactivate receipt for removed item
                        if (i < receipt.Length)
                        {
                            receipt[i].SetActive(false);
                        }
                    }
                    break;
                }
            }

            // Recalculate totals
            totalAmountValue = 0f;
            totalQuantity = 0;
            for (int i = 0; i < orderItems.Count; i++)
            {
                totalAmountValue += orderItems[i].price * orderItems[i].quantity;
                totalQuantity += orderItems[i].quantity;
            }

            // Updating Text
            if (displayTotalQuantity != null)
            {
                displayTotalQuantity.text = "Basket: " + totalQuantity + " item/s";
            }
            if (totalAmountDisplay != null)
            { totalAmountDisplay.text = totalAmountValue.ToString("F2"); }
            if (buttonTotalAmountDisplay != null)
            { buttonTotalAmountDisplay.text = totalAmountValue.ToString("F2"); }
        }
    }

    void UpdateReceipt(int orderIndex)
    {
        if (orderIndex < receipt.Length)
        {
            receipt[orderIndex].SetActive(true);

            // Get the food item from menuItems based on FoodID
            foodList item = null;
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (menuItems[i].FoodID == orderItems[orderIndex].FoodID)
                {
                    item = menuItems[i];
                    break;
                }
            }

            if (item != null)
            {
                // Set Food Image
                Transform foodImageTransform = receipt[orderIndex].transform.Find("Food Image");
                if (foodImageTransform != null)
                {
                    Image foodImage = foodImageTransform.GetComponent<Image>();
                    if (foodImage != null && item.foodPic != null)
                    {
                        foodImage.sprite = item.foodPic.sprite;
                    }
                }

                // Set Food Name
                Transform foodNameTransform = receipt[orderIndex].transform.Find("Food Name");
                if (foodNameTransform != null)
                {
                    TextMeshProUGUI foodNameText = foodNameTransform.GetComponent<TextMeshProUGUI>();
                    if (foodNameText != null)
                    {
                        foodNameText.text = item.foodName + " x" + orderItems[orderIndex].quantity;
                    }
                }

                // Set Food Vendor Name
                Transform foodVendorTransform = receipt[orderIndex].transform.Find("Food Vendor Name");
                if (foodVendorTransform != null)
                {
                    TextMeshProUGUI foodVendorText = foodVendorTransform.GetComponent<TextMeshProUGUI>();
                    if (foodVendorText != null)
                    {
                        foodVendorText.text = item.FoodVendor;
                    }
                }

                // Set Price (total for this food: price * quantity)
                Transform priceTransform = receipt[orderIndex].transform.Find("Price");
                if (priceTransform != null)
                {
                    TextMeshProUGUI priceText = priceTransform.GetComponent<TextMeshProUGUI>();
                    if (priceText != null)
                    {
                        float totalPriceForItem = item.price * orderItems[orderIndex].quantity;
                        priceText.text = totalPriceForItem.ToString("F2");
                    }
                }
            }
        }
    }

    void PurchaseFood()
    {
        GameObject notif = GameObject.FindWithTag("Notification");
        if (notif != null)
        {
            TextMeshProUGUI nameNotifTMP = notif.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI descNotifTMP = notif.transform.Find("Desc").GetComponent<TextMeshProUGUI>();

            if (nameNotifTMP != null && descNotifTMP != null)
            {
                nameNotifTMP.text = "Preparing food!";
                descNotifTMP.text = "Your $" + totalAmountValue.ToString("F2") + " is on hold as your food is being prepared.";
            }

            //NOTIF POP UP THANGGG
            Transform showPos = GameObject.Find("NOTIF SHOW POSITION").transform;
            Transform hidePos = GameObject.Find("NOTIF HIDE POSITION").transform;

            if (showPos != null && hidePos != null)
            {
                notif.transform.DOMove(showPos.position, 0.5f).OnComplete(() =>
                {
                    //Just waiting for a while
                    DOVirtual.DelayedCall(3f, () => notif.transform.DOMove(hidePos.position, 0.5f));
                });
            }
        }

        // Start food preparation slider
        if (foodPreparationSlider != null)
        {
            foodPreparationSlider.gameObject.SetActive(true);
            foodPreparationSlider.StartPreparation();
        }

        // Clear order data
        orderItems.Clear();
        totalQuantity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (totalQuantity != 0)
        {
            cartButton.interactable = true;
        }
        if (totalQuantity == 0)
        {
            cartButton.interactable = false;
        }
    }
}

/*


██╗░░██╗███████╗██╗░░░██╗
██║░░██║██╔════╝╚██╗░██╔╝
███████║█████╗░░░╚████╔╝░
██╔══██║██╔══╝░░░░╚██╔╝░░
██║░░██║███████╗░░░██║░░░
╚═╝░░╚═╝╚══════╝░░░╚═╝░░░

*/