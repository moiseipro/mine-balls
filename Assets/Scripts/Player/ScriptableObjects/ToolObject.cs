using System.Collections.Generic;
using Level.Model;
using UnityEngine;

namespace Player.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ToolObject", menuName = "ToolObject", order = 0)]
    public class ToolObject : ScriptableObject
    {
        public float DropChance = 0.1f;
        public ToolType ToolType;
        public int Durability;
        public int Strength;
        [SerializeField] public MoneyDictionary PricesDictionary;
        public List<MoneyType> MoneyTypes;
        public bool IsBuy = false;
    }
}