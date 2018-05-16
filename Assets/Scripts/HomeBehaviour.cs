using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.UI;

public class HomeBehaviour : MonoBehaviour
{
    public Text BankAmount;
    protected BankData BankData => Resources.Load<BankData>("BankData");
    // Use this for initialization
    void Start()
    {
        BankData.Bank = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        BankAmount.text = "Bank: " + BankData.Bank.Count;
    }
}
