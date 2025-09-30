using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerObjectsAndInteractions : MonoBehaviour
{
    public LayerMask layers;
    public LayerMask defaultLayer;
    [Header("Pick Up")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform objectHolder;
    public float maxDistance = 2f;
    private GameObject heldObject = null;
    private GameUI ui;
    
    [Header("Usage")]
    [SerializeField] private GameObject campfire;
    [SerializeField] private Material buildingMaterial;
    public bool isBuilding = false;
    private GameObject buildingObject = null; 
    public float maxBuildingDistance = 4;
    private Material prevBuildingMaterial = null;
    
    private StarterAssetsInputs inputs;

    private void Awake()
    {
        inputs = GetComponent<StarterAssetsInputs>();
    }

    private void Start()
    {
        ui = FindFirstObjectByType<GameUI>();
    }

    void Update()
    {
        ManageUI();

        if (isBuilding && buildingObject is not null)
        {
            UpdateBuildPosition();
        }
    }

    private void UpdateBuildPosition()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        
        if (Physics.Raycast(ray, out var hit, maxBuildingDistance, defaultLayer))
        {
            buildingObject.transform.position = hit.point;
            
            Quaternion targetRot = Quaternion.FromToRotation(Vector3.up, hit.normal);
            buildingObject.transform.rotation = targetRot;
        }
    }
    
    private void OnInteract()
    {
        if (heldObject is null && !isBuilding)
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            if (Physics.Raycast(ray, out var hit, maxDistance, layers))
            {
                var obj = hit.collider.gameObject;
                
                var objRb = obj.GetComponent<Rigidbody>();
                
                objRb.isKinematic = true;
                objRb.useGravity = false;
                
                heldObject = obj;
                obj.layer = LayerMask.NameToLayer("HeldObject");
                obj.transform.parent = objectHolder;
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = Quaternion.identity;
                
            }
        }
    }

    private void OnThrow()
    {
        if (heldObject is not null && !isBuilding)
        {
            var objRb = heldObject.GetComponent<Rigidbody>();
            objRb.isKinematic = false;
            objRb.useGravity = true;
            
            Vector3 throwDirection = playerCamera.transform.forward;
            heldObject.transform.parent = null;
            heldObject.transform.position = objectHolder.position + new Vector3(0, 1, 0);
            objRb.AddForce(throwDirection * 8, ForceMode.Impulse);
            heldObject.layer = LayerMask.NameToLayer("Objects");
            heldObject = null;
        }
    }

    private void OnUse()
    {
        if (heldObject is not null && !isBuilding)
        {
            if (heldObject.CompareTag("Wood"))
            {
                Build(campfire);
            }
        }
    }
    
    private void ManageUI()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out var hit, maxDistance, layers))
        {
            ui.ShowItemText(hit.collider.gameObject.tag);
            if (heldObject is null) ui.ShowInputActionsText("E - Pick up");
        }
        else
        {
            ui.ShowItemText(string.Empty);
            ui.ShowInputActionsText(string.Empty);
        }

        if (heldObject is not null)
        {
            if (heldObject.CompareTag("Wood"))
            {
                ui.ShowInputActionsText("G - Throw");
            }
        }
    }

    private void Build(GameObject obj)
    {
        isBuilding = true;
        buildingObject = Instantiate(obj);
        prevBuildingMaterial = buildingObject.GetComponentInChildren<MeshRenderer>().material;
        buildingObject.GetComponentInChildren<MeshRenderer>().material = buildingMaterial;
    }
}