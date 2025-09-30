using System;
using UnityEngine;

public class BuildedObject : MonoBehaviour
{
    public Material validMaterial;
    public Material invalidMaterial;
    public Renderer renderer;
    public LayerMask layerMask;

    private int collisionCount = 0;
    public bool canBeBuilt = true;

    private void Start()
    {
        SetMaterial(true);
    }

    private void Update()
    {
        canBeBuilt = IsValid();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerMask)
        {
            collisionCount++;
            SetMaterial(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerMask)
        {
            collisionCount = Mathf.Max(0, collisionCount - 1);
            if (collisionCount == 0) SetMaterial(true);
        }
    }

    private void SetMaterial(bool isValid)
    {
        renderer.material = isValid ? validMaterial : invalidMaterial;
    }

    public bool IsValid()
    {
        return collisionCount == 0;
    }
}
