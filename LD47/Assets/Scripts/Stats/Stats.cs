using TMPro;

namespace Stats {

    [System.Serializable]
    public class Stats<T> {

        public T roundsSurvived;
        public T allMoneyMade;
        public T allMoneyUsed;
        public T moneyUsedForTurret;
        public T moneyUsedForIncome;
        public T enemiesKilled;
        public T meleeEnemiesKilled;
        public T rangedEnemiesKilled;
        public T suicideEnemiesKilled;
        public T flyingEnemiesKilled;

    }
    [System.Serializable]
    public class CountStats : Stats<int> { }
    [System.Serializable]
    public class UIStats : Stats<TextMeshProUGUI> { }
}