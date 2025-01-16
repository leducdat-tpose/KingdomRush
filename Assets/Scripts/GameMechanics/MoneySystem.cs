using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySystem : MonoBehaviour
{
    private int _totalMoney=1000;
    public int TotalMoney => _totalMoney;
    public bool AddMoney(int amount)
    {
        if (_totalMoney < amount) return false;
        _totalMoney -= amount;
        return true;
    }
}
