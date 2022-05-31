using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CameraSC : MonoBehaviour
{
    [SerializeField] Camera cam;
    public Action CameraBehaviour;
    public WormUI wUI;
    float z,plusZ;
    public GameObject SelectedObject;
    Vector3 DragOrigin,distance,TESTCAMERA;
    public bool drag = false;
    public InputManager inp;
	private void Start()
	{
        z = cam.transform.position.z;
        plusZ = Mathf.Abs(z);
        CameraBehaviour += CameraZoom;
        inp.InputQKeyEvent.AddListener( OnQPressed);

    }
   

    public void OnQPressed()
    {
        if (drag)
            StartFollow();
        else
            DragAndStopFollow();

        wUI.DragIconShow(drag);
    }
    // Update is called once per frame
    void Update()
    {
        CameraBehaviour();
    }


    void CameraZoom()
	{
      cam.fieldOfView = Mathf.Clamp( cam.fieldOfView -= Input.mouseScrollDelta.y * Time.fixedDeltaTime,0.3f,1.44f);
        
	}

    void CameraDrag()
	{

        if (Input.GetMouseButtonDown(0))
            DragOrigin = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y, plusZ));
        
        if(Input.GetMouseButton(0))
		{
            //Debug.Log("origin " + DragOrigin + " difference " + distance);
            //DragOrigin.z = 0;
            distance =  DragOrigin - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, plusZ));
            distance.z = 0;
            cam.transform.position += distance;
		}

	}

    public void FollowObject()
	{
        gameObject.transform.position = new Vector3(SelectedObject.transform.position.x,SelectedObject.transform.position.y,z);

    }

  

    public void StartFollow()
	{
        CameraBehaviour += FollowObject;
        CameraBehaviour -= CameraDrag;
        drag = false;
	}

    public void DragAndStopFollow()
	{
        CameraBehaviour -= FollowObject;
        CameraBehaviour += CameraDrag;
        drag = true;
	}

    public void StopFollow_Drag()
	{
        CameraBehaviour -= FollowObject;
        CameraBehaviour -= CameraDrag;
        drag = false;
	}
}
