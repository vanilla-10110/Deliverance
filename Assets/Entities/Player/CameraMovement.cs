using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] [Range (1f, 10f)] float smoothSpeed = 0.125f;
    [SerializeField] Vector2 offset;

    void LateUpdate() {

        if (
            player != null &&
            GameManager.Instance.gameStats.currentGameState != EnumBus.GAME_STATE.START_MENU
        ){
            Vector3 finalPosition = player.transform.position;

            finalPosition.x += offset.x;
            finalPosition.y += offset.y;

            finalPosition.z = -10;

            transform.position = Vector3.Lerp(transform.position, finalPosition, smoothSpeed * Time.deltaTime);
        }
    }

}
