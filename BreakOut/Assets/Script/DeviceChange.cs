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
        
    
        Debug.Log("Android" + Screen.dpi);
    
        

        
         if (Screen.dpi == 96)
        {
            Debug.Log("PC");
            Game_PC.SetActive(true);
            Game_V.SetActive(false);
            Game_H.SetActive(false);
        }
        else
        {
            // 这是在移动设备上运行
            Debug.Log("mobil");
            lastOrientation = Screen.orientation;
            CheckScreenOrientation();

        }
    }

    // Update is called once per frame

    void CheckScreenOrientation()
    {

        if (Screen.width > Screen.height)
        {
            Game_H.SetActive(true);
            Game_V.SetActive(false);
        }
        else
        {
       
            Game_H.SetActive(false);
            Game_V.SetActive(true);
        }
    }

    void HandleOrientationChange(ScreenOrientation orientation)
    {
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
         if (Screen.dpi != 96)
        {
        if (Screen.orientation != lastOrientation)
        {
            lastOrientation = Screen.orientation;
            HandleOrientationChange(lastOrientation);
        }
        }
    }
}
