using User;

namespace Game {
    public interface IGameController
	{
		ILevelController Level { get; }
		IUser User { get; }

		void Init();
		void StartNextLevel();
	}
}
