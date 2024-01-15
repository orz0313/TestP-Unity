using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Parabox.CSG;

public class TypeABoxBehaviour : MonoBehaviour, ISelectable
{
    ProcessSystem Sys;
    public int Index;
    int CallSubtractNum;
    int TypeBStackNum;
    Vector3 m_Temp;
    Vector3 m_Temp2;
    MeshFilter m_MeshFilter;
    MeshRenderer m_MeshRenderer;
    MeshCollider m_MeshCollider;
    Renderer rend;

    void Start()
    {
        Sys = ProcessSystem.Instance;
        m_MeshFilter = GetComponent<MeshFilter>();
        m_MeshRenderer = GetComponent<MeshRenderer>();
        m_MeshCollider = GetComponent<MeshCollider>();
        m_MeshCollider.sharedMesh = Sys.GetTypeAObj(Index).GetComponent<MeshFilter>().sharedMesh;
        rend = Sys.GetTypeAObj(Index).GetComponent<Renderer>();
    }
    void Update()
    {

    }
    void FixedUpdate()
    {
        CallSubtractNum = 0;
    }
    public void SetMeshCollider(Mesh _Mesh)
    {
        m_MeshCollider.sharedMesh = _Mesh;
    }
    public void OnMouseDrag()
    {
        if(Sys.GetSelectObj()!=this.gameObject)
        {
            return;
        }
        Vector3 CameraPoint = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 ObjPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, CameraPoint.z));
        Vector3 Offset = this.transform.position - ObjPoint;
        this.transform.position = ObjPoint;
        Sys.MoveTypeAObj(Index,Offset);
        Sys.MoveProtoAObj(Index, Offset);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            TypeBStackNum++;
        }
        if (other.gameObject.layer == 8)
        {

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            Sys.CallSubtract(Index, other.GetComponent<TypeBBoxBehaviour>().Index, CallSubtractNum);
            CallSubtractNum++;
        }
        if(other.gameObject.layer == 8)
        {

        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            TypeBStackNum--;
            if(TypeBStackNum == 0)
            {
                Sys.RecoverProto(Index);
            }
        }
    }

    public void OnSelected()
    {
        rend.material.color = new Color(1f, 0, 0, 0.2f);
    }

    public void OnDeSelected()
    {
        rend.material.color = new Color(0, 0.5f, 1f, 1f);
    }
}
