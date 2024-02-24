using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BakudanController : MonoBehaviour
{
    [SerializeField] MissionManager missionManager;
    [SerializeField] PlayerC player;
    int bakudanCount = 0;
    public int BAKUDACOUNT {
        set {
            this.bakudanCount = value;
        }
        get {
            return this.bakudanCount;
        }
    }

    bool bakudan = false;
  
    // Update is called once per frame
    void Update()
    {
        if(bakudanCount >= 3 && !bakudan) {
            if(player.KEYCOUNT != 3) { 
            missionManager.MISSIONVALUE[missionManager.RADOMMISSIONCOUNT] = 3;
            missionManager.KeyActive(missionManager.RADOMMISSIONCOUNT);
                player.MISSIO = true;
            //missionManager.MiSSIONCOUNT++;
            //missionManager.STATMISSION = false;
            //missionManager.YBUTTON = false;
            bakudan = true;
            }
        }

        
    }
}
