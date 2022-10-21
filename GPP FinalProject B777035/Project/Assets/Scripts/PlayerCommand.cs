using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCommand : MonoBehaviour
{
    protected float timeStamp;
    protected Ray mouseHoverPos;

    protected GameObject player;
    protected Player script;

    public PlayerCommand()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        script = player.GetComponent<Player>();

        this.timeStamp = script.GetTime();
        this.mouseHoverPos = script.getPos();
    }
    public float GetTimeTable()
    {
        return this.timeStamp;
    }

    public abstract void Execute();
    public virtual void Do() { }

    protected void push()
    {
        Player.oldCommands.Add(this);
    }
}

public class Move : PlayerCommand
{
    public Move() : base() { }
    public override void Execute()
    {
        Do();
        push();
    }

    public override void Do()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Ground");
        RaycastHit hitInfo;
        if (Physics.Raycast(mouseHoverPos, out hitInfo, 100f, layerMask))
        {
            script.navAgent.SetDestination(hitInfo.point);
        }
    }
}

public class PLACEMENT : PlayerCommand
{
    public PLACEMENT() : base() { }
    public override void Execute()
    {
        Do();
        push();
    }

    public override void Do()
    {
        Instantiate(script.tower, player.transform.localPosition, player.transform.localRotation);
    }
}

public class REPLAY : PlayerCommand
{
    public REPLAY() : base() { }
    public override void Execute()
    {
        script.init_Replay();
    }
}

