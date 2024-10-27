using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEntrance : MonoBehaviour
{
    [SerializeField] GameObject playerAndRoot;

    // Start is called before the first frame update
    void Start()
    {
        if (MainManager.Player)
        {
            MainManager.Player.player.GetComponent<PlayerMovement>().ResetPositionTo(transform.position);
            MainManager.Player.player.transform.rotation = transform.rotation;
        }
        else InitializePlayer();
    }

    private void InitializePlayer()
    {
        GameObject player = Instantiate(playerAndRoot, transform.position, transform.rotation);
        player.transform.Rotate(Vector3.up, 180);
    }

}
