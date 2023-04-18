using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonControl : MonoBehaviour
{
    [SerializeField]
    private Button buttonA;
    [SerializeField]
    private Button buttonS;
    [SerializeField]
    private Button buttonD;
    [SerializeField]
    private Button buttonF;
    [SerializeField]
    private Button buttonR;
    [SerializeField]
    private Button buttonW;
    [SerializeField]
    private Button buttonESC;
    [SerializeField]
    private Button buttonNUm1;
    [SerializeField]
    private Button buttonNUm2;
    [SerializeField]
    private Button buttonNUm3;
    [SerializeField]
    private Button buttonNUm4;
    [SerializeField]
    private Button buttonJump;    
    [SerializeField]
    private Button buttonShift;


    [SerializeField]
    private Button mouseUP;
    [SerializeField]
    private Button mouseDown;
    [SerializeField]
    private Button mouseLeft;
    [SerializeField]
    private Button mouseRight;
    [SerializeField]
    private Button mouseWheelUP;
    [SerializeField]
    private Button mouseWheelDown;
    [SerializeField]
    private Button mouseLeftClick;
    [SerializeField]
    private Button mouseRightClick;

    [SerializeField]
    private Image backGround;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            buttonA.GetComponent<Image>().color = Color.red;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            buttonA.GetComponent<Image>().color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            buttonS.GetComponent<Image>().color = Color.red;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            buttonS.GetComponent<Image>().color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            buttonD.GetComponent<Image>().color = Color.red;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            buttonD.GetComponent<Image>().color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            buttonW.GetComponent<Image>().color = Color.red;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            buttonW.GetComponent<Image>().color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            buttonF.GetComponent<Image>().color = Color.red;
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            buttonF.GetComponent<Image>().color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            buttonR.GetComponent<Image>().color = Color.red;
            backGround.GetComponent<Image>().color = Color.white;
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            buttonR.GetComponent<Image>().color = Color.white;
            backGround.GetComponent<Image>().color = Color.gray;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            buttonNUm1.GetComponent<Image>().color = Color.red;
            backGround.GetComponent<Image>().color = Color.white;
        }
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            buttonNUm1.GetComponent<Image>().color = Color.white;
            backGround.GetComponent<Image>().color = Color.gray;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            buttonNUm2.GetComponent<Image>().color = Color.red;
            backGround.GetComponent<Image>().color = Color.white;
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            buttonNUm2.GetComponent<Image>().color = Color.white;
            backGround.GetComponent<Image>().color = Color.gray;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            buttonNUm3.GetComponent<Image>().color = Color.red;
            backGround.GetComponent<Image>().color = Color.white;
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            buttonNUm3.GetComponent<Image>().color = Color.white;
            backGround.GetComponent<Image>().color = Color.gray;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            buttonNUm4.GetComponent<Image>().color = Color.red;
            backGround.GetComponent<Image>().color = Color.white;
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            buttonNUm4.GetComponent<Image>().color = Color.white;
            backGround.GetComponent<Image>().color = Color.gray;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            buttonESC.GetComponent<Image>().color = Color.red;
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            buttonESC.GetComponent<Image>().color = Color.white;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            buttonJump.GetComponent<Image>().color = Color.red;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            buttonJump.GetComponent<Image>().color = Color.white;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            buttonShift.GetComponent<Image>().color = Color.red;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            buttonShift.GetComponent<Image>().color = Color.white;
        }

        if(Input.GetMouseButtonDown(0))
        {
            mouseLeftClick.GetComponent<Image>().color = Color.red;
        }
        if (Input.GetMouseButtonUp(0))
        {
            mouseLeftClick.GetComponent<Image>().color = Color.white;
        }

        if (Input.GetMouseButtonDown(1))
        {
            mouseRightClick.GetComponent<Image>().color = Color.red;
        }
        if (Input.GetMouseButtonUp(1))
        {
            mouseRightClick.GetComponent<Image>().color = Color.white;
        }

        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (wheelInput > 0)
        {
            mouseWheelUP.GetComponent<Image>().color = Color.red;
            mouseWheelDown.GetComponent<Image>().color = Color.white;
        }
        else if (wheelInput < 0)
        {
            mouseWheelDown.GetComponent<Image>().color = Color.red;
            mouseWheelUP.GetComponent<Image>().color = Color.white;
        }

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // 마우스가 오른쪽으로 움직일 때 버튼의 색상을 변경합니다.
        if (mouseX > 0)
        {
            mouseRight.GetComponent<Image>().color = Color.red;
            mouseLeft.GetComponent<Image>().color = Color.white;
        }
        else if(mouseX < 0)
        {
            mouseLeft.GetComponent<Image>().color = Color.red;
            mouseRight.GetComponent<Image>().color = Color.white;
        }
        else
        {
            mouseLeft.GetComponent<Image>().color = Color.white;
            mouseRight.GetComponent<Image>().color = Color.white;
        }
        if (mouseY > 0)
        {
            mouseUP.GetComponent<Image>().color = Color.red;
            mouseDown.GetComponent<Image>().color = Color.white;
        }
        else if (mouseY < 0)
        {
            mouseDown.GetComponent<Image>().color = Color.red;
            mouseUP.GetComponent<Image>().color = Color.white;
        }
        else
        {
            mouseDown.GetComponent<Image>().color = Color.white;
            mouseUP.GetComponent<Image>().color = Color.white;
        }
    }


}
