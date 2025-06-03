using UnityEngine;

public class Container
{
    public string name;
    public Slot[] slots;

    public Container(int slotsAmount, string _name)
    {
        name = _name;

        slots = new Slot[slotsAmount];

        for (int i = 0; i < slotsAmount; i++)
        {
            slots[i] = new Slot();
        }
    }
}
