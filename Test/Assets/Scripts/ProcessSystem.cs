using Parabox.CSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessSystem : MonoBehaviour
{
    public static ProcessSystem Instance;
    [SerializeField]List<TypeABoxBehaviour> TypeAList;
    [SerializeField] List<GameObject> ProtoTypeAList;
    [SerializeField] List<GameObject> TypeAObjList;
    [SerializeField]List<TypeBBoxBehaviour> TypeBList;
    [SerializeField]List<GameObject> TypeBObjList;
    [SerializeField]GameObject SelectedObj;
    Camera MainCamera;
    GameObject OnMouseDownObj;
    bool CameraMove =true;
    int CallSubtractNum;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        MainCamera = Camera.main;
        SelectedObj = this.gameObject;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(MainCamera.ScreenPointToRay(Input.mousePosition),out hit))
            {
                if(hit.collider.gameObject!= SelectedObj)
                {
                    ISelectable T;
                    if(SelectedObj.TryGetComponent<ISelectable>(out T))
                    {
                        T.OnDeSelected();
                    }
                    SelectedObj = this.gameObject;
                }
                OnMouseDownObj = hit.collider.gameObject;
            }
            else
            {
                ISelectable T;
                if (SelectedObj.TryGetComponent<ISelectable>(out T))
                {
                    T.OnDeSelected();
                }
                SelectedObj = this.gameObject;
                OnMouseDownObj = null;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(MainCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.gameObject == OnMouseDownObj)
                {
                    SelectedObj = OnMouseDownObj;
                    ISelectable T;
                    if (SelectedObj.TryGetComponent<ISelectable>(out T))
                    {
                        T.OnSelected();
                    }
                }
                else
                {
                    ISelectable T;
                    if (SelectedObj.TryGetComponent<ISelectable>(out T))
                    {
                        T.OnDeSelected();
                    }
                    SelectedObj = this.gameObject;
                }
            }
            else
            {
                SelectedObj = this.gameObject;
            }
        }
    }
    private void FixedUpdate()
    {
        CallSubtractNum = 0;
    }
    public GameObject GetSelectObj()
    {
        return SelectedObj;
    }
    public void MoveTypeAObj(int _Index,Vector3 _MoveV3)
    {
        TypeAObjList[_Index].transform.position -= _MoveV3;
    }
    public void MoveProtoAObj(int _Index, Vector3 _MoveV3)
    {
        ProtoTypeAList[_Index].transform.position -= _MoveV3;
    }
    public bool GetCameraMove()
    {
        return CameraMove;
    }
    public GameObject GetTypeAObj(int _Index)
    {
        return TypeAObjList[_Index];
    }
    public void RecoverProto(int _A)
    {
        GameObject ProtoA = ProtoTypeAList[_A];
        GameObject A = TypeAObjList[_A];
        Mesh ProtoMesh = ProtoA.GetComponent<MeshFilter>().sharedMesh;
        A.GetComponent<MeshFilter>().sharedMesh = ProtoMesh;
        A.GetComponent<MeshRenderer>().sharedMaterial = ProtoA.GetComponent<MeshRenderer>().sharedMaterial;
        A.transform.position = ProtoA.transform.position;
        A.transform.rotation = ProtoA.transform.rotation;
        A.transform.localScale = ProtoA.transform.localScale;

        TypeAList[_A].SetMeshCollider(ProtoMesh);
        if (SelectedObj == TypeAList[_A].gameObject)
        {
            TypeAList[_A].OnSelected();
        }
    }
    public void CallSubtract(int _A, int _B,int _CallSubtractNum= 0)
    {
        GameObject ProtoA = ProtoTypeAList[_A];
        GameObject A = TypeAObjList[_A];
        GameObject B = TypeBObjList[_B];
        //ProtoA.transform.position = A.transform.position;
        //ProtoA.transform.rotation = A.transform.rotation;
        //ProtoA.transform.localScale = A.transform.localScale;

        Model result;
        if (_CallSubtractNum > 0)
        {
            result = CSG.Subtract(A, B);
        }
        else
        {
            result = CSG.Subtract(ProtoA, B);
        }
        A.GetComponent<MeshFilter>().sharedMesh = result.mesh;
        A.GetComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
        A.transform.position = Vector3.zero;
        A.transform.rotation = Quaternion.identity;
        A.transform.localScale = Vector3.one;
        CallSubtractNum++;

        TypeAList[_A].SetMeshCollider(result.mesh);
        if(SelectedObj == TypeAList[_A].gameObject)
        {
            TypeAList[_A].OnSelected();
        }
    }
    void Test1()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position += new Vector3(1f, 1.5f, 1f);
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = Vector3.one * 1.3f;
        sphere.transform.position += new Vector3(1f, 1f, 1f);
        Model result = CSG.Subtract(cube, sphere);

        var composite = new GameObject();
        composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
        composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
        cube.SetActive(false);
        sphere.SetActive(false);
    }
    void Test2()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position += new Vector3(1f, 1.5f, 1f);
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = Vector3.one * 1.3f;
        sphere.transform.position += new Vector3(1f, 1f, 1f);
        Model result = CSG.Union(cube, sphere);

        var composite = new GameObject();
        composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
        composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
        cube.SetActive(false);
        sphere.SetActive(false);
    }
}
