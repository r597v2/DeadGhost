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
    public GameObject flashlightLight;
    [Space]
    [Header("View Models")]
    public GameObject oldLanternItem;
    public GameObject boxLanternItem;
    public GameObject lighterItem;
    public GameObject flashlightItem;
    [Space]
    [Header("Prefabs")]
    public GameObject oldLanternPrefab;
    public GameObject boxLanternPrefab;
    public GameObject lighterPrefab;
    public GameObject flashlightPrefab;

    private Dictionary<ItemType, GameObject> itemSetActive = new Dictionary<ItemType, GameObject>() { };
    private Dictionary<ItemType, GameObject> itemInstantiate = new Dictionary<ItemType, GameObject>() { };

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine) return;

        itemSetActive.Add(ItemType.OldLantern, oldLanternItem);
        itemSetActive.Add(ItemType.BoxLantern, boxLanternItem);
        itemSetActive.Add(ItemType.Lighter, lighterItem);
        itemSetActive.Add(ItemType.Flashlight, flashlightItem);

        itemInstantiate.Add(ItemType.BoxLantern, boxLanternPrefab);
        itemInstantiate.Add(ItemType.OldLantern, oldLanternPrefab);
        itemInstantiate.Add(ItemType.Lighter, lighterPrefab);
        itemInstantiate.Add(ItemType.Flashlight, flashlightPrefab);

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
                itemInstantiate[inventoryList[selectedItem]].GetComponentInChildren<Light>().gameObject.SetActive(true);
            }
            else if (itemSetActive[inventoryList[selectedItem]].GetComponent<SimpleToggle>() != null &&
            itemInstantiate[inventoryList[selectedItem]].GetComponentInChildren<Light>() != null)
            {
                itemInstantiate[inventoryList[selectedItem]].GetComponentInChildren<Light>().gameObject.SetActive(false);
            }

            GameObject droppedObject = PhotonNetwork.Instantiate(itemInstantiate[inventoryList[selectedItem]].name, position: dropItemPoint.position, transform.rotation);
            droppedObject.GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.Impulse);
            inventoryList[selectedItem] = ItemType.Empty;
            NewItemSelected();
        }

        if (Input.GetKeyDown(KeyCode.R) && selectedItem != 3)
        {
            selectedItem++;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.R) && selectedItem == 3)
        {
            selectedItem = 0;
            NewItemSelected();
        }

        if (Input.GetMouseButtonDown(1) && inventoryList[selectedItem] != ItemType.Empty &&
        itemSetActive[inventoryList[selectedItem]].GetComponent<IUsable>() != null)
        {
            itemSetActive[inventoryList[selectedItem]].GetComponent<IUsable>().UseItem();
        }

        if (Input.GetKeyDown(KeyCode.T) && HasItem(ItemType.Flashlight) && inventoryList[selectedItem] != ItemType.Flashlight)
        {
            if (flashlightLight.activeInHierarchy)
            {
                flashlightItem.GetComponentInChildren<AudioSource>().Play();
                flashlightLight.SetActive(false);
            }
            else
            {
                flashlightLight.SetActive(true);
                flashlightItem.GetComponentInChildren<AudioSource>().Play();
            }
        }
        else if (inventoryList[selectedItem] == ItemType.Flashlight) flashlightLight.SetActive(false);

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
        flashlightItem.SetActive(false);

        if (inventoryList[selectedItem] != ItemType.Empty)
        {
            GameObject selectedItemGameObject = itemSetActive[inventoryList[selectedItem]];
            if (selectedItemGameObject != null) selectedItemGameObject.SetActive(true);

        }
    }

    private bool HasItem(ItemType item)
    {
        foreach (ItemType i in inventoryList)
        {
            if (i == item) return true;
        }
        return false;
    }
}

public interface IUsable
{
    void UseItem()
    {
    }
}