using Unity.Entities;
using Unity.Jobs;
using Unity.Physics.Systems;
using Unity.Collections;
using Unity.Burst;

namespace Unity.Physics.Stateful
{
    // This system converts stream of TriggerEvents to StatefulTriggerEvents that can be stored in a Dynamic Buffer.
    // In order for this conversion, it is required to:
    //    1) Use the 'Raise Trigger Events' option of the 'Collision Response' property on a PhysicsShapeAuthoring component, and
    //    2) Add a StatefulTriggerEventBufferAuthoring component to that entity
    // or, if this is desired on a Character Controller:
    //    1) Tick the 'Raise Trigger Events' flag on the CharacterControllerAuthoring component.
    //       Note: the Character Controller will not become a trigger, it will raise events when overlapping with one
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateAfter(typeof(PhysicsSimulationGroup))]
    public partial struct StatefulTriggerEventBufferSystem : ISystem
    {
        private StatefulSimulationEventBuffers<StatefulTriggerEvent> _mStateFulEventBuffers;
        private ComponentHandles _mComponentHandles;
        private EntityQuery _mTriggerEventQuery;

        struct ComponentHandles
        {
            public ComponentLookup<StatefulTriggerEventExclude> EventExcludes;
            public BufferLookup<StatefulTriggerEvent> EventBuffers;

            public ComponentHandles(ref SystemState systemState)
            {
                EventExcludes = systemState.GetComponentLookup<StatefulTriggerEventExclude>(true);
                EventBuffers = systemState.GetBufferLookup<StatefulTriggerEvent>();
            }

            public void Update(ref SystemState systemState)
            {
                EventExcludes.Update(ref systemState);
                EventBuffers.Update(ref systemState);
            }
        }

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            var builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAllRW<StatefulTriggerEvent>()
                .WithNone<StatefulTriggerEventExclude>();

            _mStateFulEventBuffers = new StatefulSimulationEventBuffers<StatefulTriggerEvent>();
            _mStateFulEventBuffers.AllocateBuffers();

            _mTriggerEventQuery = state.GetEntityQuery(builder);
            state.RequireForUpdate(_mTriggerEventQuery);

            _mComponentHandles = new ComponentHandles(ref state);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            _mStateFulEventBuffers.Dispose();
        }

        [BurstCompile]
        public partial struct ClearTriggerEventDynamicBufferJob : IJobEntity
        {
            public void Execute(ref DynamicBuffer<StatefulTriggerEvent> eventBuffer) => eventBuffer.Clear();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _mComponentHandles.Update(ref state);

            state.Dependency = new ClearTriggerEventDynamicBufferJob()
                .ScheduleParallel(_mTriggerEventQuery, state.Dependency);

            _mStateFulEventBuffers.SwapBuffers();

            var currentEvents = _mStateFulEventBuffers.Current;
            var previousEvents = _mStateFulEventBuffers.Previous;

            state.Dependency = new StatefulEventCollectionJobs.CollectTriggerEvents
            {
                TriggerEvents = currentEvents
            }.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);

            state.Dependency = new StatefulEventCollectionJobs
                .ConvertEventStreamToDynamicBufferJob<StatefulTriggerEvent, StatefulTriggerEventExclude>
            {
                CurrentEvents = currentEvents,
                PreviousEvents = previousEvents,
                EventLookup = _mComponentHandles.EventBuffers,

                UseExcludeComponent = true,
                EventExcludeLookup = _mComponentHandles.EventExcludes
            }.Schedule(state.Dependency);
        }
    }
}
