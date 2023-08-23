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
    public GameObject torchItem;
    [Space]
    [Header("Prefabs")]
    public GameObject oldLanternPrefab;
    public GameObject boxLanternPrefab;
    public GameObject lighterPrefab;
    public GameObject flashlightPrefab;
    public GameObject torchPrefab;

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
        itemSetActive.Add(ItemType.Torch, torchItem);

        itemInstantiate.Add(ItemType.BoxLantern, boxLanternPrefab);
        itemInstantiate.Add(ItemType.OldLantern, oldLanternPrefab);
        itemInstantiate.Add(ItemType.Lighter, lighterPrefab);
        itemInstantiate.Add(ItemType.Flashlight, flashlightPrefab);
        itemInstantiate.Add(ItemType.Torch, torchPrefab);

        selectedItem = 0;

        NewItemSelected();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        itemViewModels.transform.rotation = new Quaternion(_camera.transform.rotation.x, _camera.transform.rotation.y, _camera.transform.rotation.z, _camera.transform.rotation.w);

        //drop items
        if (Input.GetKeyDown(KeyCode.Q) && inventoryList[selectedItem] != ItemType.Empty)
        {
            GameObject droppedObject = PhotonNetwork.Instantiate(itemInstantiate[inventoryList[selectedItem]].name, position: dropItemPoint.position, _camera.transform.rotation);
            if (itemSetActive[inventoryList[selectedItem]].GetComponent<SimpleToggle>() != null && itemInstantiate[inventoryList[selectedItem]].GetComponentInChildren<Light>(true) != null)
            {
                droppedObject.GetComponentInChildren<Light>(true).gameObject.SetActive(itemSetActive[inventoryList[selectedItem]].GetComponent<SimpleToggle>().itemOn);
            }
            droppedObject.GetComponent<Rigidbody>().AddForce(_camera.transform.forward, ForceMode.Impulse);
            inventoryList[selectedItem] = ItemType.Empty;
            NewItemSelected();
        }

        //rotate slots
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

        //use items
        if (Input.GetMouseButtonDown(1) && inventoryList[selectedItem] != ItemType.Empty &&
        itemSetActive[inventoryList[selectedItem]].GetComponent<IUsable>() != null)
        {
            itemSetActive[inventoryList[selectedItem]].GetComponent<IUsable>().UseItem();
        }
        
        //turn off flashlight light when drop flashlight
        if (!HasItem(ItemType.Flashlight)) flashlightLight.SetActive(false);

        //toggle flashlight while not equipped
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
        //flashlight
        else if (inventoryList[selectedItem] == ItemType.Flashlight) flashlightLight.SetActive(flashlightItem.GetComponent<SimpleToggle>().itemOn);

        //change selected slot
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
        torchItem.SetActive(false);

        if (inventoryList[selectedItem] != ItemType.Empty)
        {
            GameObject selectedItemGameObject = itemSetActive[inventoryList[selectedItem]];
            if (selectedItemGameObject != null) selectedItemGameObject.SetActive(true);

        }
    }

    public bool HasItem(ItemType item)
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