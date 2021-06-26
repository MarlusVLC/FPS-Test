using System.Collections;

namespace Auxiliary
{
    public interface LateStarter
    {
        public IEnumerator LateStart(float waitTime);

    }
}