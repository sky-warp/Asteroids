using R3;

namespace _Project.Scripts.Score.Model
{
    public class ScoreModel
    {
        public readonly ReactiveProperty<int> CurrentScore = new();
    }
}