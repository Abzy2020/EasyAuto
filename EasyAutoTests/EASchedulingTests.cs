using EasyAuto;
using EasyAuto.Scheduling;
using System.Security.AccessControl;

namespace EasyAuto.Scheduling
{
    [TestClass]
    public class EASchedulingTests
    {
        [TestMethod]
        public void EnquedTask_WithArgs_IsGenerated()
        {
            // arrange
            string executable = "";
            string arguments = "";
            bool useShellExecute = true;
            ScheduledAction action = new(executable, arguments, useShellExecute);

            // act
            ScheduleManager.GenerateEnqueuedTask(action);

            // assert

        }
    }
}