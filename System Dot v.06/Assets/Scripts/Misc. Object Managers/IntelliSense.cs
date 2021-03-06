﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntelliSense : Dialogue {

    public float y0;
    public float amplitude = .2f;
    public float speed = 1.5f;
    public float scaleX = .25f;
    public float scaleY = .25f;

    protected bool allowZooming = true; // in cases where zooming is not allowed
    protected bool startDifferent;

    public GameObject intelliLocation;

    public void Start()
    {
        base.Start();

        if (!startDifferent)
        {
            this.transform.parent = player.transform;
            this.transform.localPosition = new Vector2(0, 0);
            this.transform.localScale = new Vector2(0, 0);
        }
    }

    public void Update()
    {
        base.Update();

        if (allowZooming)
        {
            if (talking)
            {
                ZoomOutPlayer();
            }
            else
            {
                ZoomIntoPlayer();
            }
        }

        // THIS PART IMMOBOLIZES THE PLAYER
        if (player.GetComponentInParent<PlayerController>())
            player.GetComponentInParent<PlayerController>().IntelliSenseTalking(talking);
    }

    public void ZoomIntoPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 10 * Time.deltaTime);
        if (transform.localScale.x > 0)
            transform.localScale += Vector3.one * -.01f;
    }

    public void ZoomOutPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, intelliLocation.transform.position, 3 * Time.deltaTime);
        if (transform.localScale.x < scaleX)
            transform.localScale += Vector3.one * .01f;
        else
        {
            y0 = this.transform.position.y;
            transform.localScale = new Vector3(scaleX, scaleY, 1);
        }
    }
}
