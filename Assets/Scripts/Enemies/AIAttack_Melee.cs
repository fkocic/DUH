using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttack_Melee : AIAttack_Base
{
    public float stabSoundDelay;

    public override void PlayEffects()
    {
        anim?.SetTrigger("Shoot");
        particleMuzzle.Play();
        StartCoroutine(stabDelay());
    }

    private IEnumerator stabDelay()
    {
        yield return new WaitForSeconds(stabSoundDelay);

        scriptMain.PlaySound(gun.soundShooting[Random.Range(0, gun.soundShooting.Length)]);
    }
}
