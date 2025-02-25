using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOrderable
{
    public bool HaveOrder{get;}
    public void OrderAction();

    public void SetOrder(bool order);
}
