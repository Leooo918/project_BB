using System;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Sabotage")]
public class SabotageSO : ScriptableObject
{
    [field:SerializeField] public string className { get; private set; }
    [field:SerializeReference] public Sabotage sabotage { get; private set; }

    [field: SerializeField] public Sprite icon { get; private set; }
    [field: SerializeField] public string sabotageName { get; private set; }
    [field: SerializeField] public string sabotageExplain { get; private set; }




    private void OnValidate()
    {
        if(sabotage == null)
        {
            try
            {
                Type t = Type.GetType(className);
                sabotage = Activator.CreateInstance(t) as Sabotage;
            }
            catch
            {
                Debug.Log($"Can not find Sabotage class name: {className}");
            }
        }
        else
        {
            if (sabotage.GetType().ToString() != className) 
            {
                sabotage = null;
            }
        }
    }
}
