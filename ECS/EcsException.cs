using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonGame.ECS
{
    public abstract class EcsException(string message) : Exception(message) { }

    public class EntityNotFoundException() : EcsException("Entity not found.") { }
    public class InvalidComponentException(Type type) : EcsException($"Type {type} is not a component type!") { }
    public class InvalidProcessorException(Type type) : EcsException($"Type {type} is not a current processor type!") { }
    public class UnregisteredComponentException(ComponentBase component) : EcsException($"Component {component} has not been registered!") { }
    public class ComponentAlreadyRegisteredException(ComponentBase component) : EcsException($"Component {component} has already been registered!") { }
    public class EntityHasComponentException(ComponentBase component) : EcsException($"Component of type {component.GetType()} has already been added to this entity!") { }
    public class EntityDoesNotHaveComponentException(ComponentBase component) : EcsException($"This entity does not have {component}!") { }
    public class ComponentRequirementNotMetException(Type requirement) : EcsException($"This entity does not have a required component of type {requirement}!") { }

    public class EventAlreadyCreatedException(Event Event) : EcsException($"Event {Event} was already created.") { }
    public class EventAlreadyDestroyedException(Event Event) : EcsException($"Event {Event} was already destroyed.") { }
}
