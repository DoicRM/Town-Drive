using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip playerHitSound, playerWinSound, playerGetScoreSound, playerGetNotSound, playerGetBoostSound;
    static AudioSource audioSrc;

    void Start()
    {
        playerHitSound = Resources.Load<AudioClip> ("playerHit");
        playerWinSound = Resources.Load<AudioClip> ("playerWin");
        playerGetScoreSound = Resources.Load<AudioClip>("playerGetScore");
        playerGetNotSound = Resources.Load<AudioClip>("playerNot");
        playerGetBoostSound = Resources.Load<AudioClip>("playerBoost");
        audioSrc = GetComponent<AudioSource> ();
    }

    void Update()
    {
        
    }

    public static void PlaySound(string clip)
    {
        switch(clip) {
        case "playerHit":
            audioSrc.PlayOneShot(playerHitSound);
            break;
        case "playerWin":
            audioSrc.PlayOneShot(playerWinSound);
            break;
        case "playerGetScore":
            audioSrc.PlayOneShot(playerGetScoreSound);
            break;
        case "playerGetNot":
            audioSrc.PlayOneShot(playerGetNotSound);
            break;
        case "playerGetBoost":
            audioSrc.PlayOneShot(playerGetBoostSound);
        break;
    }
    }
}
