public class GameState
{
	public enum MoneySpentType { Tower, Update }

	public uint currentMoney { get; private set; }
	public uint totalMoneySpent { get; private set; }
	public uint moneySpentOnTowers { get; private set; }
	public uint moneySpentOnUpdates { get; private set; }
	public uint timeSaved { get; private set; }
	public uint currentLife { get; private set; }

	public void AddMoney(uint money)
	{
		currentMoney += money;
	}

	public void RemoveMoney(uint money, MoneySpentType type)
	{
		currentMoney -= money;
		totalMoneySpent += money;
		switch (type)
		{
			case MoneySpentType.Tower:
				moneySpentOnTowers += money;
				break;
			case MoneySpentType.Update:
				moneySpentOnUpdates += money;
				break;
		}
	}

	public void AddLife(uint life) => currentLife += life;
	public void RemoveLife(uint life) => currentLife -= life;
}
