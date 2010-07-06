namespace Tests.SharpArchContrib.Castle.Logging {
    public interface ILogTestClass {
        int Method(string name, int val);
        int VirtualMethod(string name, int val);
        int NotLogged(string name, int val);
        void ThrowException();
    }
}