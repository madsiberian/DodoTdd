using Moq;

namespace DodoTdd.Test.DSL
{
    public class Create
    {
        public static PlayerBuilder Player => new PlayerBuilder();
        public static DieBuilder Die => new DieBuilder();
    }

    public class DieBuilder
    {
        public DieBuilder Rolling(int score)
        {
            _score = score;
            return this;
        }

        public Die Please()
        {
            var die = new Mock<Die>();
            die.Setup(x => x.Roll()).Returns(_score);
            return die.Object;
        }

        int _score;
    }
}