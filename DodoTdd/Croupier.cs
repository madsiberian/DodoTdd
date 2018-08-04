namespace DodoTdd
{
    public class Croupier
    {
        public Game CreateTwoDiceGame(Casino casino)
        {
            return casino.CreateGame(new Die(), 2);
        }
    }
}