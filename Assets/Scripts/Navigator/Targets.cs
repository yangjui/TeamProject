using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class Targets : MonoBehaviour
{
    public delegate void TriggerDelegate(string _name);
    private TriggerDelegate triggerCallback = null;

    [Tooltip("Change this color to change the indicators color for this target")]
    [SerializeField] private Color targetColor = Color.red;

    [Tooltip("Select if box indicator is required for this target")]
    [SerializeField] private bool needBoxIndicator = true;

    [Tooltip("Select if arrow indicator is required for this target")]
    [SerializeField] private bool needArrowIndicator = true;

    [Tooltip("Select if distance text is required for this target")]
    [SerializeField] private bool needDistanceText = true;

    
    [HideInInspector] public Indicator indicator;

    public Color TargetColor
    {
        get
        {
            return targetColor;
        }
    }

    public bool NeedBoxIndicator
    {
        get
        {
            return needBoxIndicator;
        }
    }
    
    public bool NeedArrowIndicator
    {
        get
        {
            return needArrowIndicator;
        }
    }
    
    public bool NeedDistanceText
    {
        get
        {
            return needDistanceText;
        }
    }
   
    private void OnEnable()
    {
        if(OffScreenIndicator.TargetStateChanged != null)
        {
            OffScreenIndicator.TargetStateChanged.Invoke(this, true);
        }
    }

    private void OnDisable()
    {
        if(OffScreenIndicator.TargetStateChanged != null)
        {
            OffScreenIndicator.TargetStateChanged.Invoke(this, false);
        }
    }

    public float GetDistanceFromCamera(Vector3 cameraPosition)
    {
        float distanceFromCamera = Vector3.Distance(cameraPosition, transform.position);
        return distanceFromCamera;
    }

    public void OnTriggerDelegate(TriggerDelegate _triggerCallback)
    {
        triggerCallback = _triggerCallback;
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.CompareTag("Player"))
        {
            if (transform.name == "Quest1" || transform.name == "Quest3")
            {
                triggerCallback?.Invoke(transform.name);
            }
        }
    }
}