namespace DodoTdd.Test.DSL
{
    public class PlayerBuilder
    {
        public PlayerBuilder InSomeGame()
        {
            _game = _casino.CreateGame(new Die());
            return this;
        }

        public PlayerBuilder InGame(Game game)
        {
            _game = game;
            return this;
        }

        public PlayerBuilder WithChips(int amount)
        {
            _chipsAmount = amount;
            return this;
        }

        public Player Please()
        {
            var player = new Player();
            if (_game != null)
                player.Join(_game);
            if (_chipsAmount > 0)
                player.BuyFromCasino(_chipsAmount, _casino);

            return player;
        }

        public PlayerBuilder InCasino(Casino casino)
        {
            _casino = casino;
            return this;
        }

        Casino _casino = new Casino();
        Game _game;
        int _chipsAmount;
    }
}