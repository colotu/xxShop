using System;

namespace YSWL.TaoBao.Stream
{
    public class TopCometException : TopException
    {
        public TopCometException()
            : base()
        {
        }

        public TopCometException(string message, Exception cause)
            : base(message, cause)
        {
        }

        public TopCometException(string message)
            : base(message)
        {
        }
    }
}
