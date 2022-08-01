using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls the animator for the large creature, since it has less animations than the rest
/// </summary>
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
