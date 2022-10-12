using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineMatchUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick100Chips()
    {
        PoolSceneManager.Instance.MyLoadScene("Game_Net");
    }
    public void OnClick200Chips()
    {
        PoolSceneManager.Instance.MyLoadScene("Game_Net");
    }
    public void OnClick300Chips()
    {
        PoolSceneManager.Instance.MyLoadScene("Game_Net");

    }
    public void OnClick400Chips()
    {
        PoolSceneManager.Instance.MyLoadScene("Game_Net");
    }
    public void OnClick500Chips()
    {
        PoolSceneManager.Instance.MyLoadScene("Game_Net");
    }
    public void OnClick600Chips()
    {
        PoolSceneManager.Instance.MyLoadScene("Game_Net");
    }
    public void OnClick700Chips()
    {
        PoolSceneManager.Instance.MyLoadScene("Game_Net");
    }

    public void OnClicBackBtn()
    {
        PoolSceneManager.Instance.MyLoadScene("MainMenu");
    }
}
