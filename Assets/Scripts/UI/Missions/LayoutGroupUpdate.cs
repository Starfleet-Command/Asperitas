using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class forces the layout groups of all mission holders to actively reload when opened. <br/>
/// This prevents a bug where the dynamic resizing is delayed by a frame.
/// </summary>
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
