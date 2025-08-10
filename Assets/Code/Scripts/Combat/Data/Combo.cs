using UnityEngine;

namespace Combat
{

    [System.Serializable]
    public struct Combo
    {
        public string name;
        public Attack[] attacks;

        public Combo(string _name, Attack[] _attacks)
        {
            name = _name;
            attacks = _attacks;
        }
    }

}