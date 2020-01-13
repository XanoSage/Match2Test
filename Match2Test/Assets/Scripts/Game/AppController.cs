using Game;
using UnityEngine;

public class AppController : MonoBehaviour
{
	[SerializeField] private GameFieldViewController _gameFieldViewController = null;
	private IGameController _gameController;

    void Start()
    {
		_gameController = Game.Factory.GameFactory.CreateGameController(_gameFieldViewController);
		_gameController.Init();
    }
}
