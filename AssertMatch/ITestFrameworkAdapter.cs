namespace AssertMatch
{
    public interface ITestFrameworkAdapter
    {
        void Fail(string message);
        void Ok();
    }
}