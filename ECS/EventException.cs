using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    public abstract class EventException(string message) : Exception(message) { }

    public class EventAlreadyCreatedException(Event Event) : EventException($"Event {Event} was already created.") { }
    public class EventAlreadyDestroyedException(Event Event) : EventException($"Event {Event} was already destroyed.") { }
}
