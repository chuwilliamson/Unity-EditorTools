using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Data/BankData")]
    public class BankData : ScriptableObject
    {
        public List<string> Bank;
    }
}