using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeBBoxBehaviour : MonoBehaviour, ISelectable
{
    ProcessSystem Sys;
    Renderer rend;
    public int Index;
    void Start()
    {
        Sys = ProcessSystem.Instance;
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMouseDrag()
    {
        if (Sys.GetSelectObj() != this.gameObject)
        {
            return;
        }
        Vector3 CameraPoint = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 ObjPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraPoint.z));
        this.transform.position = ObjPoint;
    }

    public void OnSelected()
    {
        rend.material.color = new Color(1f,0,0,0.2f);
    }

    public void OnDeSelected()
    {
        rend.material.color = new Color(1f, 0, 0, 0);
    }
}
