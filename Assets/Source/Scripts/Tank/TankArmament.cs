using System.Collections.Generic;
using UnityEngine;

public class TankArmament : MonoBehaviour
{
    [SerializeField] private List<Shell> _shellPrefabs;
    [SerializeField] private Transform _shellsStorage;

    private List<Shell> _shells;
    private GameObject _mainTankObject;

    public GameObject MainTankObject => _mainTankObject;

    public void Construct(GameObject mainTankObject)
    {
        _shells = new List<Shell>();
        _mainTankObject = mainTankObject;

        foreach (var prefab in _shellPrefabs)
        {
            Shell shell = Instantiate(prefab, _shellsStorage);

            shell.Construct(this);
            shell.gameObject.SetActive(false);

            _shells.Add(shell);
        }
    }

    public Shell GetSelectedShell()
    {
        if (_shells.Count == 0)
        {
            Debug.LogError("The shells list cannot be empty");
            return null;
        }

        Shell shell = _shells[0];
        shell.gameObject.SetActive(true);

        return shell;
    }

    public void ReturnShell(Shell shell)
    {
        if (_shells.Contains(shell) == false)
        {
            Debug.LogError("Only owned shells can be returned");
            return;
        }

        shell.ResetValues();
        shell.gameObject.SetActive(false);
    }
}
