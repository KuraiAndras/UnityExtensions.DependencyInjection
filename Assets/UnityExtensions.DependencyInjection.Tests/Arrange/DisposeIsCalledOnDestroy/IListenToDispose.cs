namespace UnityExtensions.DependencyInjection.Tests.Arrange.DisposeIsCalledOnDestroy
{
    public interface IListenToDispose
    {
        bool DisposeCalled { get; set; }
    }
}