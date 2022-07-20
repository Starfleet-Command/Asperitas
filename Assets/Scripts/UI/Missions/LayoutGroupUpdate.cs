using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LayoutGroupUpdate : MonoBehaviour
{
    public VerticalLayoutGroup layoutGroup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ForceLayoutUpdate()
    {
        StartCoroutine("UpdateLayoutGroup");
    }


    public IEnumerator UpdateLayoutGroup()
    {
    layoutGroup.childScaleHeight = true;
    yield return new WaitForSeconds(0.01f);
    layoutGroup.childScaleHeight = false;
    }
}
