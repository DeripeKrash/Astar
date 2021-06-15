using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerInputManager
{
    public static bool PS4Input = false;
    public static bool qwertyInput = false;

    public static bool JumpInput()
    {
        if (PS4Input)
        {
            return (Input.GetButton("JumpPS4") || Input.GetButton("Jump"));
        }
        return (Input.GetButton("JumpXbox") || Input.GetButton("Jump"));
    }
    public static bool JumpInputDown()
    {
        if (PS4Input)
        {
            return (Input.GetButtonDown("JumpPS4") || Input.GetButtonDown("Jump"));
        }
        return (Input.GetButtonDown("JumpXbox") || Input.GetButtonDown("Jump"));
    }

    
    public static bool AttackInput()
    {
        if (PS4Input)
        {
            return (Input.GetButton("AttackPS4") || Input.GetButton("Attack"));
        }
        return (Input.GetButton("AttackXbox") || Input.GetButton("Attack"));
    }
    public static bool AttackInputDown()
    {
        if (PS4Input)
        {
            return (Input.GetButtonDown("AttackPS4") || Input.GetButtonDown("Attack"));
        }
        return (Input.GetButtonDown("AttackXbox") || Input.GetButtonDown("Attack"));
    }
}
