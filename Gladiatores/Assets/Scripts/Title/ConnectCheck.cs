using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GamepadInput.GamePad;
//For Debug
using UnityEngine.UI;

public class ConnectCheck : MonoBehaviour {

    private bool singleFlag;//

    private bool multiFlag;//

    [SerializeField]
    private Text OneState;//state of 1P

    [SerializeField]
    private Text TwoState;//state of 2P

    [SerializeField]
    private Text playButton;//Play Button

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Input.ResetInputAxes();
        // 接続されているコントローラの名前を調べる
        var controllerNames = Input.GetJoystickNames();

        if (controllerNames.Length == 1)
        {
            Debug.Log("SinglePlay");
            if (Input.anyKeyDown)
            {
                OneState.color = new Color(OneState.color.r, OneState.color.g, OneState.color.b, 255f);
                playButton.color = new Color(playButton.color.r, playButton.color.g, playButton.color.b, 255f);
            }

            if (GamepadInput.GamePad.GetButtonDown(GamepadInput.GamePad.Button.A, GamepadInput.GamePad.Index.One))
            {
                //開始
                Debug.Log("start");
            }
        }
        else if (controllerNames.Length==2)
        {
            //Debug.Log("MultiPlay");
            //if (GamepadInput.GamePad.GetButtonDown(GamepadInput.GamePad.Button.A, GamepadInput.GamePad.Index.One))
            if (GamepadInput.GamePad.GetButtonDown(GamepadInput.GamePad.Button.A, GamepadInput.GamePad.Index.One))
            {
                OneState.color = new Color(OneState.color.r, OneState.color.g, OneState.color.b, 255f);
                singleFlag = true;
            }
            if(GamepadInput.GamePad.GetButtonDown(GamepadInput.GamePad.Button.A, GamepadInput.GamePad.Index.Two))
            {
                multiFlag = true;
                TwoState.color = new Color(TwoState.color.r, TwoState.color.g, TwoState.color.b, 255f);
            }
            if (singleFlag == true)
            {
                playButton.color = new Color(playButton.color.r, playButton.color.g, playButton.color.b, 255f);
                if(GamepadInput.GamePad.GetButtonDown(GamepadInput.GamePad.Button.A, GamepadInput.GamePad.Index.One)||
                   GamepadInput.GamePad.GetButtonDown(GamepadInput.GamePad.Button.A, GamepadInput.GamePad.Index.Two))
                {
                    Debug.Log("start");
                    if(multiFlag)
                    {
                        Debug.Log("MULTI");
                    }
                    else
                    {
                        Debug.Log("SINGLE");
                    }
                }
            }
        }
        Debug.Log(controllerNames.Length);     
    }
}
