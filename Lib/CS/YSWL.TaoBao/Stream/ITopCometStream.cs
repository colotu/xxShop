using YSWL.TaoBao.Stream.Connect;
using YSWL.TaoBao.Stream.Message;

namespace YSWL.TaoBao.Stream
{
    public interface ITopCometStream
    {
        void SetConnectionListener(IConnectionLifeCycleListener connectionLifeCycleListener);
        void SetMessageListener(ITopCometMessageListener cometMessageListener);
        void Start();
        void Stop();
    }
}
