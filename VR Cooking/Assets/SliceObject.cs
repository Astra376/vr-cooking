using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class SliceObject : MonoBehaviour
{
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public VelocityEstimator velocityEstimator;
    public LayerMask sliceableLayer;
    
    public Material crossSectionalMaterial;

    float lastCutTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lastCutTime += Time.fixedDeltaTime;

        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer);
        if (hasHit && lastCutTime > .8f)
        {
            GameObject target = hit.transform.gameObject;

            lastCutTime = 0;

            Slice(target);
        }
    }

    public void Slice(GameObject target)
    {
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();

        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);

        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(target, crossSectionalMaterial);
            SetupSlicedComponent(upperHull, target);

            GameObject lowerHull = hull.CreateLowerHull(target, crossSectionalMaterial);
            SetupSlicedComponent(lowerHull, target);

            Destroy(target);
        }
    }

    public void SetupSlicedComponent(GameObject slicedObject, GameObject target)
    {
        Rigidbody rb = slicedObject.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;

        slicedObject.layer = target.layer;
        XRGrabInteractable interactable = slicedObject.AddComponent<XRGrabInteractable>();
        interactable.useDynamicAttach = true;
    }
}
