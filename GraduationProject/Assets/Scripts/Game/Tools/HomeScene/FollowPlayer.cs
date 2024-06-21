using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject player;
    public bool AutoOffset;
    public Vector2 offset;

    private ProCamera2DNumericBoundaries cameraBoundaries;
    private void Start()
    {
        player = PlayerController.Instance.currentCharacter.gameObject;
        if(AutoOffset == true)
        {
            offset = transform.position - player.transform.position;
        }
        cameraBoundaries = Camera.main.GetComponent<ProCamera2DNumericBoundaries>();
    }

    private void LateUpdate()
    {
        if(!cameraBoundaries.IsCameraPositionHorizontallyBounded)
        {
            transform.position = new Vector3(cameraBoundaries.transform.position.x, cameraBoundaries.transform.position.y) 
                + new Vector3(offset.x, offset.y);
        }
    }
}
