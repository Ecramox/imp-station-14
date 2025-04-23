using Content.Shared.Interaction.Components;
using Content.Shared.Inventory;
using Content.Shared._Impstation.Dragon;
using Content.Shared.Whitelist;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Shared._Impstation.Dragon;

/// <summary>
/// Information for a Dragon type that can be selected by <see cref="DragonSwitchableTypeComponent"/>.
/// </summary>
/// <seealso cref="SharedDragonSwitchableTypeSystem"/>
[Prototype]
public sealed partial class DragonTypePrototype : IPrototype
{
    [ValidatePrototypeId<SoundCollectionPrototype>]
    private static readonly ProtoId<SoundCollectionPrototype> DefaultFootsteps = new("FootstepDragon");

    [IdDataField]
    public required string ID { get; set; }

    //
    // Description info (name/desc) is configured via localization strings directly.
    //

    /// <summary>
    /// The prototype displayed in the selection menu for this type.
    /// </summary>
    [DataField]
    public required EntProtoId DummyPrototype;

    /// <summary>
    /// Inventory template used by this Dragon.
    /// </summary>
    /// <remarks>
    /// This template must be compatible with the normal Dragon templates,
    /// so in practice it can only be used to differentiate the visual position of the slots on the character sprites.
    /// </remarks>
    /// <seealso cref="InventorySystem.SetTemplateId"/>
    [DataField]
    public ProtoId<InventoryTemplatePrototype> InventoryTemplateId { get; set; } = "DragonShort";

    /// <summary>
    /// Additional components to add to the Dragon entity when this type is selected.
    /// </summary>
    [DataField]
    public ComponentRegistry? AddComponents { get; set; }

    //
    // Visual information
    //

    /// <summary>
    /// The sprite state for the main Dragon body.
    /// </summary>
    /// TODO: Default sprite for entity before Dragon is selected
    [DataField]
    public string SpriteBodyState { get; set; } = "robot";

    /// <summary>
    /// An optional movement sprite state for the main Dragon body.
    /// </summary>
    [DataField]
    public string? SpriteBodyMovementState { get; set; }

    //
    // Minor information
    //

    /// <summary>
    /// String to use on petting success.
    /// </summary>
    /// <seealso cref="InteractionPopupComponent"/>
    [DataField]
    public string PetSuccessString { get; set; } = "petting-success-generic-dragon";

    /// <summary>
    /// String to use on petting failure.
    /// </summary>
    /// <seealso cref="InteractionPopupComponent"/>
    [DataField]
    public string PetFailureString { get; set; } = "petting-failure-generic-dragon";

    //
    // Sounds
    //

    /// <summary>
    /// Sound specifier for footstep sounds created by this Dragon.
    /// </summary>
    [DataField]
    public SoundSpecifier FootstepCollection { get; set; } = new SoundCollectionSpecifier(DefaultFootsteps);
}
