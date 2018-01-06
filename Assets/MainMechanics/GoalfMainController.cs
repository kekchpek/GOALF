

using UnityEngine;

public class GoalfMainController : MainController {

    public AudioClip gameoverSound, lockedSound, spawnGolferSound, hitSound, boogerS, parS, birdyS, holyS, aimSound, playS;
    public GameObject ball;

    protected override void Start()
    {
        base.Start();
        SoundController.AddSound(gameoverSound, "gameover");
        SoundController.AddSound(lockedSound, "locked");
        SoundController.AddSound(spawnGolferSound, "spawn");
        SoundController.AddSound(hitSound, "hit");
        SoundController.AddSound(boogerS, "booger");
        SoundController.AddSound(parS, "par");
        SoundController.AddSound(birdyS, "birdy");
        SoundController.AddSound(holyS, "holy");
        SoundController.AddSound(aimSound, "aimr");
        SoundController.AddSound(playS, "play");
    }

}
