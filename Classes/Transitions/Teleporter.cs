using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : Interactable
{
    public Teleporter otherTeleporter;
    public GameObject[] ToggleActives = new GameObject[2];
    public GameObject player;
    public bool canTeleport;
    public Animator screenFader;

    private InternalTimer timer;


    private void Start()
    {
        timer = GetComponent <InternalTimer>();
        
    }

    public override void Interaction()
    {
        if (player && canTeleport && timer.currentInternalTime > 1f) {
            
            StartCoroutine(Fade());
        }
    }

    IEnumerator Fade()
    {
        screenFader.SetInteger("FadeState", 1); //fadein

        yield return new WaitForSeconds(.3f);

        player.transform.position = otherTeleporter.transform.position;
        Camera.main.transform.position = new Vector3(otherTeleporter.transform.position.x, otherTeleporter.transform.position.y, Camera.main.transform.position.z);

        otherTeleporter.Cooldown();

        if (ToggleActives[0] && ToggleActives[1]) // activate/deactivate parts of the level.
        {
            ToggleActives[0].SetActive(false);
            ToggleActives[1].SetActive(true);
        }

        screenFader.SetInteger("FadeState", 2); // fadeout
        yield return new WaitForSeconds(1f);
        screenFader.SetInteger("FadeState", 0); // default

    }

    public void Cooldown()
    {
        timer.ResetInternalTime();
    }
}
