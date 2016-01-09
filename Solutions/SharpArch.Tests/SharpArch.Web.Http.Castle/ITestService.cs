// ReSharper disable InternalMembersMustHaveComments
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global
namespace Tests.SharpArch.Web.Http.Castle
{
    internal interface ITestService
    {
        void Do();
    }

    
    class TestService : ITestService
    {
        public void Do()
        {
        }
    }
}