#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion


class SkipTimer
{
    private float elapsedTime, skipTime;

    public SkipTimer(float skipTime)
    {
        this.skipTime = skipTime;
    }

    public bool Check(float deltaTime)
    {
        elapsedTime += deltaTime;
        if (elapsedTime >= skipTime)
        {
            elapsedTime -= skipTime;
            return true;
        }
        return false;
    }

    public void Reset()
    {
        elapsedTime = 0;
    }
}
