using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceChange : MonoBehaviour
{
    // Start is called before the first frame update
        private ScreenOrientation lastOrientation;
        public GameObject Game_H;
        public GameObject Game_V;
        public GameObject Game_PC;
    void Start()
    {
        //Check is PC o Simulator
         if (Screen.dpi == 96)
        {
            //if is PC Active pcUiManager
            Debug.Log("PC");
            Game_PC.SetActive(true);
            Game_V.SetActive(false);
            Game_H.SetActive(false);
        }
        else
        {
            //if is Simulator Active Simulator UiManager
            Debug.Log("Simulator");
            lastOrientation = Screen.orientation;
            CheckScreenOrientation();

        }
    }



    void CheckScreenOrientation()
    {
        //For inici
        if (Screen.width > Screen.height)
        {
            //if is landscape 
            Game_H.SetActive(true);
            Game_V.SetActive(false);
        }
        else
        {
            //if is portrait  
            Game_H.SetActive(false);
            Game_V.SetActive(true);
        }
    }

    void HandleOrientationChange(ScreenOrientation orientation)
    {
        //for in game
        switch (orientation)
        {
        case ScreenOrientation.Portrait:
        case ScreenOrientation.PortraitUpsideDown:
 
        Game_H.SetActive(false);
        Game_V.SetActive(true);

        break;
        case ScreenOrientation.LandscapeLeft:
        case ScreenOrientation.LandscapeRight:
        Game_H.SetActive(true);
        Game_V.SetActive(false);
        break;
        default:
        break;
        }
    }
    void Update()
    {
        // in game, not PC mode
         if (Screen.dpi != 96)
        {
        if (Screen.orientation != lastOrientation)
        {
                // in game, check is Landscape or Portrait
                lastOrientation = Screen.orientation;
            HandleOrientationChange(lastOrientation);
        }
        }
    }
}
