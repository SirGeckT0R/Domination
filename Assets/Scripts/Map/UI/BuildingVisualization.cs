using System.Collections.Generic;
using UnityEngine;

public class BuildingVisualization : MonoBehaviour
{
    [field: SerializeField] public List<GameObject> EconomicBuildingLevels { get; private set; }
    [field: SerializeField] public List<GameObject> MilitaryBuildingLevels { get; private set; }

    private byte? _currentEconomic;
    private byte? _currentMilitary;

    public void SetEconomicBuildingLevel(byte level)
    {
        if(_currentEconomic == null)
        {
            EconomicBuildingLevels[level].SetActive(true);
            _currentEconomic = level;

            return;
        }

        EconomicBuildingLevels[(int)_currentEconomic].SetActive(false);
        EconomicBuildingLevels[level].SetActive(true);
        _currentEconomic = level;
    }

    public void SetMilitaryBuildingLevel(byte level)
    {
        if (_currentMilitary == null)
        {
            _currentMilitary = level;
        }

        MilitaryBuildingLevels[(int)_currentMilitary].SetActive(false);
        MilitaryBuildingLevels[level].SetActive(true);
        _currentMilitary = level;
    }
}
