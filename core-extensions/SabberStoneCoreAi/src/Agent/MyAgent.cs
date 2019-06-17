using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCore.Tasks;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.POGame;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCore.Enums;
using SabberStoneCoreAi.Score;


namespace SabberStoneCoreAi.Agent
{
	class MyAgent : AbstractAgent
	{
		private Random Rnd = new Random();

		public override void FinalizeAgent()
		{
		}

		public override void FinalizeGame()
		{
		}

		public override PlayerTask GetMove(SabberStoneCoreAi.POGame.POGame poGame)
		{
			List<PlayerTask> simulatedactions = new List<PlayerTask>();
			simulatedactions.AddRange(poGame.CurrentPlayer.Options());

			try
			{
				Dictionary<PlayerTask, SabberStoneCoreAi.POGame.POGame> sim = poGame.Simulate(simulatedactions);


				Dictionary<PlayerTask, SabberStoneCoreAi.POGame.POGame>.KeyCollection keyColl = sim.Keys;

				//required for score keeping .. creates acnd cleares a dictionary
				Dictionary<int, PlayerTask> scoresKeyPair = new Dictionary<int, PlayerTask>();
				scoresKeyPair.Clear();


				//Console.WriteLine(poGame.PartialPrint());

				foreach (PlayerTask p in simulatedactions)
				{//Console.WriteLine(p);
				}
				//Console.WriteLine("*********** after simulation ***********");

				int maxScore = Int32.MinValue;
				int _score = 0;

				foreach (PlayerTask key in keyColl)
				{
					//Console.WriteLine(key);
					//Console.WriteLine("Player num -->>>>"+poGame.CurrentPlayer.PlayerId);
					//Console.WriteLine("SIM  -->>>>"+sim[key]);
					if (sim[key] == null)
						continue;
					_score = Score(sim[key], poGame.CurrentPlayer.PlayerId);
					//Console.WriteLine(_score);
					if (!scoresKeyPair.ContainsKey(_score))
						scoresKeyPair.Add(_score, key);
					if (_score > maxScore)
						maxScore = _score;

				}

				//Console.WriteLine(maxScore);

				return scoresKeyPair[maxScore];

				/**
				foreach (PlayerTask key in keyColl)
				{
					
					if (key.PlayerTaskType == PlayerTaskType.MINION_ATTACK)
					{
						//Console.WriteLine("minion!!");
						return key;
					}
						//do something with simulated actions
					//in case an EndTurn was simulated you need to set your own cards
					//see POGame.prepareOpponent() for an example
				}

				foreach (PlayerTask key in keyColl)
				{
					if (key.PlayerTaskType == PlayerTaskType.PLAY_CARD)
					{
						//Console.WriteLine("Play!!");
						return key;
					}
					//do something with simulated actions
					//in case an EndTurn was simulated you need to set your own cards
					//see POGame.prepareOpponent() for an example
				}


		*/

				foreach (PlayerTask key in keyColl)
				{

				}
			}
			catch
			{
				return poGame.CurrentPlayer.Options()[Rnd.Next(poGame.CurrentPlayer.Options().Count)];
			}
		}


		private static int Score(POGame.POGame state, int playerId)
		{
			//Console.WriteLine("Passed Player -->>>>"+state.CurrentPlayer.PlayerId);
			var p = state.CurrentPlayer.PlayerId == playerId ? state.CurrentPlayer : state.CurrentOpponent;
			switch (state.CurrentPlayer.HeroClass)
			{
				case CardClass.WARRIOR: return new AggroScore { Controller = p }.Rate();
				case CardClass.MAGE: return new MyScore { Controller = p }.Rate();
				default: return new MidRangeScore { Controller = p }.Rate();
			}
		}

		public override void InitializeAgent()
		{
			Rnd = new Random();
		}

		public override void InitializeGame()
		{
		}
	}
}
