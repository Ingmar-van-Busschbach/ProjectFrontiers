using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class TankPartsTracker : MonoBehaviour
{
    [SerializeField] private List<GameObject> partsList = new List<GameObject>();
    [SerializeField] private UnityEvent deathEvent;

    private void Update()
    {
        for(int i = 0; i < partsList.Count; i++)
        {
            if(partsList[i] == null)
            {
                partsList.RemoveAt(i);
            }
        }
        if(partsList.Count == 0)
        {
            deathEvent?.Invoke();
        }
    }
}
