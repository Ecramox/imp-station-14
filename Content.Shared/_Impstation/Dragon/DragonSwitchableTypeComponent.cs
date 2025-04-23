using Content.Shared.Actions;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Impstation.Dragon;

/// <summary>
/// Look this is mostly just copied and editted from the Borg prototypes of the same name, just go to that
/// </summary>
/// <remarks>
/// <para>
/// This is used by all naturally spawning Dragons.
/// </para>
/// <para>
/// Available types are specified in <see cref="DragonTypePrototype"/>s.
/// </para>
/// </remarks>
/// <seealso cref="SharedDragonSwitchableTypeSystem"/>
[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState(raiseAfterAutoHandleState: true)]
[Access(typeof(SharedDragonSwitchableTypeSystem))]
public sealed partial class DragonSwitchableTypeComponent : Component
{
    /// <summary>
    /// Action entity used by players to select their type.
    /// </summary>
    [DataField, AutoNetworkedField]
    public EntityUid? SelectTypeAction;

    /// <summary>
    /// The currently selected Dragon type, if any.
    /// </summary>
    /// <remarks>
    /// This can be set in a prototype to immediately apply a Dragon type, and not have switching support.
    /// </remarks>
    [DataField, AutoNetworkedField]
    public ProtoId<DragonTypePrototype>? SelectedDragonType;
}

/// <summary>
/// Action event used to open the selection menu of a <see cref="DragonSwitchableTypeComponent"/>.
/// </summary>
public sealed partial class DragonToggleSelectTypeEvent : InstantActionEvent;

/// <summary>
/// UI message used by a Dragon to select their type with <see cref="DragonSwitchableTypeComponent"/>.
/// </summary>
/// <param name="prototype">The Dragon type prototype that the user selected.</param>
[Serializable, NetSerializable]
public sealed class DragonSelectTypeMessage(ProtoId<DragonTypePrototype> prototype) : BoundUserInterfaceMessage
{
    public ProtoId<DragonTypePrototype> Prototype = prototype;
}

/// <summary>
/// UI key used by the selection menu for <see cref="DragonSwitchableTypeComponent"/>.
/// </summary>
[NetSerializable, Serializable]
public enum DragonSwitchableTypeUiKey : byte
{
    SelectDragonType,
}
