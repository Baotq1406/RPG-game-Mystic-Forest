using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveInventory : Singleton<ActiveInventory>
{
    private int activeSlotIndexNum = 0;

    private PlayerControls playerControls;

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlot((int)ctx.ReadValue<float>());
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }

    public void EquipStartingWeapon()
    {
        ToggleActiveHighlight(0);
    }

    private void ToggleActiveSlot(int numValue)
    {
        ToggleActiveHighlight(numValue - 1);
    }

    private void ToggleActiveHighlight(int indexNum)
    {
        activeSlotIndexNum = indexNum;  

        foreach (Transform inventorySlot in this.transform)
        {
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }
        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon()
    {
        //Debug.Log(transform.GetChild(activeSlotIndexNum).GetComponent<InventorySlot>().GetWeaponInfo().weaponPrefab.name);

        //Check if there's a currently active weapon and destroy it
        if (ActiveWeapon.Instance.CurrentActiveWeapon!= null) {
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        Transform childTransform = transform.GetChild(activeSlotIndexNum);
        InventorySlot inventorySlot = childTransform.GetComponentInChildren<InventorySlot>();
        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();
        GameObject weaponToSpawn = weaponInfo.weaponPrefab;

        if (weaponInfo == null) {
            ActiveWeapon.Instance.WeaponNull();
            return;
        }
        //Check if the selected slot has a weapon
        //if (transform.GetChild(activeSlotIndexNum).GetComponentInChildren<InventorySlot>().GetWeaponInfo() == null)
        //{
        //    ActiveWeapon.Instance.WeaponNull();
        //    return;
        //}

        //Retrieve the weapon prefab
        //GameObject weaponToSpawm = transform.GetChild(activeSlotIndexNum).
        //GetComponentInChildren<InventorySlot>().GetWeaponInfo().weaponPrefab;

        //Spawn the new weapon
        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform);

        //Setting the rotation of the ActiveWeapon object to a specific orientation using Euler angles
        //ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0f, 0f, 0f);    

        //Set the new weapon as a child of the ActiveWeapon object
        //newWeapon.transform.parent = ActiveWeapon.Instance.transform;

        //Update the active weapon reference
        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }
}
