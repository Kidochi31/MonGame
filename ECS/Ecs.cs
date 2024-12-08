using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace MonGame.ECS
{
    public class Ecs
    {
        readonly GameManager GameManager;

        readonly Dictionary<Guid, UnderlyingEntity> Entities;
        readonly List<(Type Type, List<ComponentBase> Components)> Components;
        readonly List<(Type Type, Processor Processor)> UpdateProcessors;
        readonly List<(Type Type, Processor Processor)> DrawProcessors;
        readonly LinkedList<Event> EventQueue;
                
        public Ecs(GameManager gameManager)
        {
            GameManager = gameManager;
            Entities = [];
            EventQueue = [];

            // components contains a list of lists of components, with all sealed descendants of Component
            Components = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                          from type in  domainAssembly.GetTypes()
                where type.IsSealed && type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(ComponentBase))
                select (type, new List<ComponentBase>())).ToList();

            // Update processors contain all descendants of Processor that have the [UpdateProcessor] attribute
            var updateProcessors = from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                                from type in domainAssembly.GetTypes()
                                where type.GetCustomAttribute<UpdateProcessorAttribute>() is not null
                                && type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Processor))
                                select (type, (Processor)Activator.CreateInstance(type));
            UpdateProcessors = [];
            Processor.InsertProcessorsIntoList(updateProcessors, UpdateProcessors);

            // Update processors contain all descendants of Processor that have the [DrawProcessor] attribute
            var drawProcessors = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                              from type in domainAssembly.GetTypes()
                              where type.GetCustomAttribute<DrawProcessorAttribute>() is not null
                              && type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Processor))
                              select (type, (Processor)Activator.CreateInstance(type))).ToList();
            
            DrawProcessors = [];
            Processor.InsertProcessorsIntoList(drawProcessors, DrawProcessors);
        }

        #region Entities

        public Entity CreateEntity()
        {
            UnderlyingEntity entity = new UnderlyingEntity(this);
            Entities.Add(entity.Guid, entity);
            return new Entity(entity);
        }

        // Destroys the entity and all of its components, the entity reference becomes invalid
        public void DestroyEntity(Entity entity)
        {
            if (!Entities.ContainsKey(entity.Guid))
                throw new EntityNotFoundException();
            UnderlyingEntity underlying = Entities[entity.Guid];
            underlying.Destroy();
            Entities.Remove(entity.Guid);
        }

        internal UnderlyingEntity GetEntity(Entity entity)
        {
            if (!Entities.ContainsKey(entity.Guid))
                throw new EntityNotFoundException();
            return Entities[entity.Guid];
        }

        public bool EntityExists(Entity entity) => Entities.ContainsKey(entity.Guid);

        #endregion

        #region Components

        #region GetComponents
        public List<ComponentBase> GetComponents(Type type)
        {
            // type must be either a subclass of component or an interface
            if (!type.IsAssignableTo(typeof(ComponentBase)) && !type.IsInterface)
                throw new InvalidComponentException(type);
            List<ComponentBase> componentList = (from components in Components where components.Type.IsAssignableTo(type) select components.Components).SelectMany(x => x).ToList();
            return componentList;
        }

        // Returns list of components T
        public List<T> GetComponents<T>()
            => GetComponents(typeof(T)).Cast<T>().ToList();

        // Returns list of components T1 and T2 on the same entity
        public List<(T1, T2)> GetComponents<T1, T2>() where T1 : ComponentBase where T2 : ComponentBase
            => (from component in GetComponents<T1>()
                where component.Entity.HasComponent<T2>()
                select (component, component.Entity.GetComponent<T2>())).ToList();

        // Returns list of components T1, T2, and T3 on the same entity
        public List<(T1, T2, T3)> GetComponents<T1, T2, T3>() where T1 : ComponentBase where T2 : ComponentBase where T3 : ComponentBase
            => (from components in GetComponents<T1, T2>()
                where components.Item1.Entity.HasComponent<T3>()
                select (components.Item1, components.Item2, components.Item1.Entity.GetComponent<T3>())).ToList();

        #endregion

        //Registers the component in the component list here
        internal void RegisterComponent(ComponentBase component)
        {
            Type componentType = component.GetType();
            List<ComponentBase>? componentList = (from components in Components where components.Type == componentType select components.Components).FirstOrDefault();
            if (componentList is null)
                throw new InvalidComponentException(componentType);
            if (componentList.Any(c => ReferenceEquals(component, c)))
                throw new ComponentAlreadyRegisteredException(component);

            componentList.Add(component);
        }

        //Unregisters the component from the component list here, but not from its entity
        internal void UnregisterComponent(ComponentBase component)
        {
            Type componentType = component.GetType();
            List<ComponentBase>? componentList = (from components in Components where components.Type == componentType select components.Components).FirstOrDefault();
            if (componentList is null)
                throw new InvalidComponentException(componentType);
            if (!componentList.Contains(component))
                throw new UnregisteredComponentException(component);

            componentList.Remove(component);
        }

        #endregion

        #region Processors


        public void RemoveProcessorsOfType(Type processorType)
        {
            UpdateProcessors.RemoveAll(processor => processor.Type == processorType);
            DrawProcessors.RemoveAll(processor => processor.Type == processorType);
        }

        public void AddUpdateProcessor(Processor processor)
        {
            Processor.InsertProcessorsIntoList([(processor.GetType(), processor)], UpdateProcessors);
        }

        public void AddDrawProcessor(Processor processor)
        {
            Processor.InsertProcessorsIntoList([(processor.GetType(), processor)], DrawProcessors);
        }

        public Processor? GetProcessor(Type type)
        {
            if (!type.IsAssignableTo(typeof(Processor)))
                return null;
            Processor? processor = (from pro in UpdateProcessors.Concat(DrawProcessors) where pro.Type == type select pro.Processor).FirstOrDefault();
            return processor;
        }

        public T? GetProcessor<T>() where T : Processor
            => (T?)GetProcessor(typeof(T));

        public void Initialize()
        {
            RunProcessors(UpdateProcessors, p => p.Initialize(this, GameManager), "initialising process");
            RunProcessors(DrawProcessors, p => p.Initialize(this, GameManager), "initialising process");
        }

        public void Update(GameTime gameTime)
        {
            RunProcessors(UpdateProcessors, p => p.OnUpdate(gameTime, this, GameManager), "update process");
            ExecuteEvents();
        }

        public void DrawUpdate(GameTime gameTime)
        {
            RunProcessors(DrawProcessors, p => p.OnUpdate(gameTime, this, GameManager), "draw process");
        }

        public void RunProcessors(List<(Type type, Processor Processor)> Processors, Action<Processor> action, string info)
        {
            List<Type> removeTypes = [];
            foreach ((Type type, Processor processor) in Processors)
            {
                try
                {
                    if (processor.IsActive)
                        action(processor);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"FATAL EXCEPTION on {info} {processor}: {e}");
                    if (processor.StopOnError)
                        removeTypes.Add(type);
                }
            }
            removeTypes.ForEach(RemoveProcessorsOfType);
        }

        public void RunProcessors(List<(Type type, Processor Processor)> Processors, Func<Processor, bool> shouldStop, string info)
        {
            List<Type> removeTypes = [];
            foreach ((Type type, Processor processor) in Processors)
            {
                try
                {
                    bool stopping = false;
                    if (processor.IsActive)
                        stopping = shouldStop(processor);

                    if (stopping)
                        break;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"FATAL EXCEPTION on {info} {processor}: {e}");
                    if (processor.StopOnError)
                        removeTypes.Add(type);
                }
            }
            removeTypes.ForEach(RemoveProcessorsOfType);
        }
        #endregion

        #region Events

        internal void RegisterEvent(Event Event)
        {
            if (EventQueue.Any(e => ReferenceEquals(e, Event)))
                throw new EventAlreadyCreatedException(Event);
            EventQueue.AddLast(Event);
        }

        public void DestroyEvent(Event Event)
        {
            if (EventQueue.All(e => !ReferenceEquals(e, Event)))
                throw new EventAlreadyDestroyedException(Event);
            EventQueue.Remove(Event);
        }

        public List<T> GetAllEvents<T>() where T : Event
        {
            List<T> events = new List<T>();
            foreach(Event e in EventQueue)
            {
                if(e is T t)
                    events.Add(t);
            }
            return events;
        }

        private void ExecuteEvents()
        {
            while(EventQueue.Count > 0)
            {
                Event nextEvent = EventQueue.First();
                Type eventType = nextEvent.GetType();
                EventQueue.RemoveFirst();
                // go through all update processors in order, and execute their event managers
                RunProcessors(UpdateProcessors,
                    processor => processor.Events.ContainsKey(eventType)
                    // run the event and stop if it consumes the event
                    ? processor.Events[eventType].Invoke(nextEvent) == EventAction.Consume
                    // if it doesn't run the event, don't stuck
                    : false,
                    $"event {nextEvent} executed in process");
            }
        }

        #endregion
    }
}
