using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class InputManager : MonoBehaviour
{
    public RoundManager rm;
    public GameObject Pause_Menu;
    public UnityEvent InputQKeyEvent,InputEKeyEvent,
        InputSpaceKeyUPEvent,InputSpacePressedKeyEvent,InputSpaceKeyDownEvent,
        InputJumpKeyEvent,
        InputAKeyEvent,InputDKeyEvent, InputAKeyUPEvent, InputDKeyUPEvent,
        InputArrowUPkey,InputArrowUPkeyDown, InputArrowDOWNkey, InputArrowDOWNkeyDown;

  
    public void Resume()
	{
        Pause_Menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (Pause_Menu.activeSelf) Pause_Menu.SetActive(false);
            else Pause_Menu.SetActive(true);
        }

        if (!rm.ismyTurn) return;

        if (Input.GetKeyUp(KeyCode.Q))
		{
            InputQKeyEvent?.Invoke();
        }

        if(Input.GetKeyUp(KeyCode.E))
		{
            InputEKeyEvent?.Invoke();
        }

        if(Input.GetKeyUp(KeyCode.Space))
		{
            InputSpaceKeyUPEvent?.Invoke();
        }

        if(Input.GetKey(KeyCode.Space))
		{
            InputSpacePressedKeyEvent?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            InputSpaceKeyDownEvent?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
		{
            InputArrowUPkeyDown?.Invoke();
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            InputArrowUPkey?.Invoke();
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            InputArrowDOWNkey?.Invoke();
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            InputArrowDOWNkeyDown?.Invoke();
        }

        if (Input.GetKey(KeyCode.A))
		{
            InputAKeyEvent?.Invoke();
        }
        
        if(Input.GetKey(KeyCode.D))
		{
            InputDKeyEvent?.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            InputAKeyUPEvent?.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            InputDKeyUPEvent?.Invoke();
        }

        if(Input.GetKeyUp(KeyCode.W))
		{
            InputJumpKeyEvent?.Invoke();
		}


       
    }

    public void KeySimulateClient(string key)
	{
        if (rm.ismyTurn) return;
        switch(key)
		{
            case "D":
				{
                    InputDKeyEvent?.Invoke();
                    break;
				}  
            case "D_up":
				{
                    InputDKeyUPEvent?.Invoke();
                    break;
				}
            case "A":
			{
                    InputAKeyEvent?.Invoke();
                    break;
			}
            case "A_up":
				{
                    InputAKeyUPEvent?.Invoke();
                    break;
				}
            case "ArrowUP":
				{
                    InputArrowUPkey?.Invoke();
                    break;
				}

            case "ArrowUP_down":
				{
                    InputArrowUPkeyDown?.Invoke();
                    break;
				}
            case "ArrowDOWN_down":
				{
                    InputArrowDOWNkeyDown?.Invoke();
                    break;
				}
            case "ArrowDOWN":
				{
                    InputArrowDOWNkey?.Invoke();
                    break;
				}
            case "E":
				{
                    InputEKeyEvent?.Invoke();
                    break;
				}
            case "W":
                {
                    InputJumpKeyEvent?.Invoke();
                    break;
                }

            case "Space_up":
				{
                    InputSpaceKeyUPEvent?.Invoke();
                    break;
				}
            case "Space":
				{
                    InputSpacePressedKeyEvent?.Invoke();
                    break;
				}
            case "Space_down":
				{
                    InputSpaceKeyDownEvent?.Invoke();
                    break;
				}
          
            default: break;
		}
	}
 
}
