using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInventory : MonoBehaviourPunCallbacks
{
    public ItemType[] inventoryList = new ItemType[] { };
    public int selectedItem;
    public Transform dropItemPoint;
    public GameObject itemViewModels;
    public Camera _camera;
    [Space]
    public GameObject oldLanternItem;
    public GameObject boxLanternItem;
    public GameObject lighterItem;
    [Space]
    public GameObject oldLanternPrefab;
    public GameObject boxLanternPrefab;
    public GameObject lighterPrefab;

    private Dictionary<ItemType, GameObject> itemSetActive = new Dictionary<ItemType, GameObject>() {};
    private Dictionary<ItemType, GameObject> itemInstantiate = new Dictionary<ItemType, GameObject>() {};

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine) return;

        itemSetActive.Add(ItemType.OldLantern, oldLanternItem);
        itemSetActive.Add(ItemType.BoxLantern, boxLanternItem);
        itemSetActive.Add(ItemType.Lighter, lighterItem);

        itemInstantiate.Add(ItemType.BoxLantern, boxLanternPrefab);
        itemInstantiate.Add(ItemType.OldLantern, oldLanternPrefab);
        itemInstantiate.Add(ItemType.Lighter, lighterPrefab);

        selectedItem = 0;

        NewItemSelected();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        itemViewModels.transform.rotation = new Quaternion(_camera.transform.rotation.x, _camera.transform.rotation.y, _camera.transform.rotation.z, _camera.transform.rotation.w);

        if (Input.GetKeyDown(KeyCode.Q) && inventoryList[selectedItem] != ItemType.Empty) //drop items
        {
            if (itemSetActive[inventoryList[selectedItem]].GetComponent<SimpleToggle>() != null &&
            itemSetActive[inventoryList[selectedItem]].GetComponent<SimpleToggle>().itemOn &&
            itemInstantiate[inventoryList[selectedItem]].GetComponentInChildren<Light>() != null)
            {
                itemInstantiate[inventoryList[selectedItem]].GetComponentInChildren<Light>().enabled = true;
            }
            else if (itemSetActive[inventoryList[selectedItem]].GetComponent<SimpleToggle>() != null &&
            itemInstantiate[inventoryList[selectedItem]].GetComponentInChildren<Light>() != null)
            {
                itemInstantiate[inventoryList[selectedItem]].GetComponentInChildren<Light>().enabled = false;
            }

            Instantiate(itemInstantiate[inventoryList[selectedItem]], position: dropItemPoint.position, new Quaternion());
            inventoryList[selectedItem] = ItemType.Empty;
            NewItemSelected();
        }

        if (Input.GetMouseButtonDown(1) && inventoryList[selectedItem] != ItemType.Empty &&
        itemSetActive[inventoryList[selectedItem]].GetComponent<IUsable>() != null)
        {
            itemSetActive[inventoryList[selectedItem]].GetComponent<IUsable>().UseItem();
        }

        // change selected slot
        if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryList.Length > 0)
        {
            selectedItem = 0;
            NewItemSelected();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && inventoryList.Length > 1)
        {
            selectedItem = 1;
            NewItemSelected();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && inventoryList.Length > 2)
        {
            selectedItem = 2;
            NewItemSelected();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && inventoryList.Length > 3)
        {
            selectedItem = 3;
            NewItemSelected();
        }
    }

    public void NewItemSelected()
    {
        oldLanternItem.SetActive(false);
        boxLanternItem.SetActive(false);
        lighterItem.SetActive(false);

        if (inventoryList[selectedItem] != ItemType.Empty)
        {
            GameObject selectedItemGameObject = itemSetActive[inventoryList[selectedItem]];
            if (selectedItemGameObject != null) selectedItemGameObject.SetActive(true);

        }
    }
}

public interface IUsable
{
    void UseItem()
    {
    }
}