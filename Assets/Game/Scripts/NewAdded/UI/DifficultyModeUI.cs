using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyModeUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickEasyMode()
    {
        PoolSceneManager.Instance.MyLoadScene("Game_Local");
    }
    public void OnClickNormalMode()
    {
        PoolSceneManager.Instance.MyLoadScene("Game_Local");
    }
    public void OnClickHardMode()
    {
        PoolSceneManager.Instance.MyLoadScene("Game_Local");

    }
    public void OnClicBackBtn()
    {
        PoolSceneManager.Instance.MyLoadScene("MainMenu");
    }
  
 
}
