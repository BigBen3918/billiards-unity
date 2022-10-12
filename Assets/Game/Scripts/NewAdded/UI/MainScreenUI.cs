using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreenUI : MonoBehaviour
{
    public GameObject warning_holder;
    // Start is called before the first frame update
    void Start()
    {
        //warnig_holder.GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClickFacebookBtn_Click()
    {
        //warnig_holder.GetComponent<Animator>().enabled = true;
        warning_holder.GetComponent<Animator>().SetTrigger("Show");
        // StartCoroutine(iShow());
    }
    public void OnClickGoogleBtn_Click()
    {
        warning_holder.GetComponent<Animator>().SetTrigger("Show");

    }
    public void OnClickInvitadoBtn_Click()
    {
        PoolSceneManager.Instance.MyLoadScene("SimpleTu");
    }

    public void OnClickUugorBtn_Click() {
        PoolSceneManager.Instance.MyLoadScene("SimpleTu");
    }
    IEnumerator iShow()
    {
        yield return new WaitForSeconds(2f);
        warning_holder.GetComponent<Animator>().enabled = false ;

    }
}
