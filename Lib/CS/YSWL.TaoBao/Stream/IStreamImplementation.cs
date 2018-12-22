using System;

namespace YSWL.TaoBao.Stream
{
    public interface IStreamImplementation
    {
        bool IsAlive();
        void NextMsg();
        string ParseLine(string msg);
        void OnException(Exception ex);
        void Close();
    }
}
