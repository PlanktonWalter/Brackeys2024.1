using UnityEngine;

public sealed class Player : MonoBehaviour
{

    [SerializeField] private Pawn _defaultPawn;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _hud;

    private Pawn _currentPawn;

    private void Start()
    {
        Possess(_defaultPawn);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            if (_currentPawn != _defaultPawn)
            {
                Unpossess();
            }
        }

        if (_currentPawn != null)
        {
            if (_currentPawn.WantsUnpossess == false)
            {
                _currentPawn.PossessedTick();
                _mainCamera.transform.SetPositionAndRotation(
                    _currentPawn.GetCameraPosition(), 
                    _currentPawn.GetCameraRotation());
            }
            else
            {
                if (_currentPawn != _defaultPawn)
                {
                    Unpossess();
                }
            }
        }
    }

    public void Possess(Pawn pawn)
    {
        if (_currentPawn == pawn)
            return;

        _currentPawn = pawn;
        _currentPawn.OnPossessed(this);

        _hud.SetActive(pawn == _defaultPawn);
    }

    public void Unpossess()
    {
        if (_currentPawn != null && _currentPawn != _defaultPawn)
        {
            _currentPawn.OnUnpossessed();
            Possess(_defaultPawn);
        }
    }

}
