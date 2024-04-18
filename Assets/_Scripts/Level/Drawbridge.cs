using System;
using UnityEngine;

public class Drawbridge : Gate
{
    public LineRenderer chainLeft, chainRight;
    public Transform chainEndLeft, chainEndRight;

    private bool done;

    private void Update()
    {
        if (!done)
        {
            UpdateChain(chainLeft, chainEndLeft);
            UpdateChain(chainRight, chainEndRight);
        }
    }

    public override void Open()
    {
        base.Open();
        done = false;
    }
    public override void Close()
    {
        base.Close();
        done = false;
    }

    private void UpdateChain(LineRenderer chain, Transform endPos)
    {
        chain.SetPosition(0, chain.transform.position);
        chain.SetPosition(1, endPos.position);
    }

    public void Done()
    {
        done = true;
    }
}
