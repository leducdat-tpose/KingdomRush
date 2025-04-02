using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Role
{
    None,
    RoleRanger,
    RoleMeele,
}

public abstract class RoleType : MonoBehaviour
{
    protected Role mainRole = Role.None;
    public abstract float CalCauseDamage();
    public void SetRoleType(Role role)
    {
        mainRole = role;
    }
}
