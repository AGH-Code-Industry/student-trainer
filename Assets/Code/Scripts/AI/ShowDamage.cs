using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShowDamage : MonoBehaviour {

    [SerializeField] Renderer[] renderers;
    Dictionary<Material, Color> _materialColors = new();


    void Start()
    {
        renderers.SelectMany(r => r.materials).ToList().ForEach(m => _materialColors.Add(m, m.color));

    }

    public void Show()
    {
        StartCoroutine("FlashDamage");
    }

    IEnumerator FlashDamage()
    {
        renderers.SelectMany(r => r.materials).ToList().ForEach(m => m.color = Color.red);
        yield return new WaitForSeconds(0.1f);
        renderers.SelectMany(r => r.materials).ToList().ForEach(m => m.color = _materialColors[m]);
    }

}