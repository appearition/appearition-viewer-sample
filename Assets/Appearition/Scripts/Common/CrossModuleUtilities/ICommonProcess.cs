using System.Collections;

namespace Appearition.Common
{
    /// <summary>
    /// Base instructions for a cross-module process from the SDK.
    /// Those processes are likely to hit multiple modules in order to achieve what they require,
    /// and may occur in multiple steps, asking for the user to provide some input in mid-process.
    /// Make sure to check the requirements and constructors of each of those processes for more information.
    /// </summary>
    public interface ICommonProcess 
    {
        /// <summary>
        /// The main process to begin. Make sure the process is correctly setup before starting this coroutine.
        /// </summary>
        /// <returns></returns>
        IEnumerator ExecuteMainProcess();
    }
}