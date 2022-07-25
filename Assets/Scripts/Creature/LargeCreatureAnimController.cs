using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeCreatureAnimController : MonoBehaviour
{
    [SerializeField] private Animator animController;

    public void SetMoving()
    {
        animController.SetBool("isMoving",true);
    }

    public void StopMoving()
    {
        animController.SetBool("isMoving",false);
    }
}
