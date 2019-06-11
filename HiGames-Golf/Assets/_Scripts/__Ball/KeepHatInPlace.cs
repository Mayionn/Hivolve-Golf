using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepHatInPlace : MonoBehaviour
{
    public Player Player;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Player.SelectedBall.transform.position + ((Vector3.up * (Player.SelectedBall.SphereCollider.radius * 0.75f)) * Player.SelectedBall.transform.localScale.y);
    }
}
