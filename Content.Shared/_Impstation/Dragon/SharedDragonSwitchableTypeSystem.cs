using Content.Shared.Actions;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Components;
using Content.Shared.Movement.Components;
using Content.Shared._Impstation.Dragon;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Shared._Impstation.Dragon;

/// <summary>
/// Implements Dragon type switching.
/// </summary>
/// <seealso cref="DragonSwitchableTypeComponent"/>
public abstract class SharedDragonSwitchableTypeSystem : EntitySystem
{

    [Dependency] private readonly SharedActionsSystem _actionsSystem = default!;
    [Dependency] private readonly SharedUserInterfaceSystem _userInterface = default!;
    [Dependency] protected readonly IPrototypeManager Prototypes = default!;
    [Dependency] private readonly InteractionPopupSystem _interactionPopup = default!;

    [ValidatePrototypeId<EntityPrototype>]
    public const string ActionId = "ActionSelectDragonType";

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DragonSwitchableTypeComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<DragonSwitchableTypeComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<DragonSwitchableTypeComponent, DragonToggleSelectTypeEvent>(OnSelectDragonTypeAction);

        Subs.BuiEvents<DragonSwitchableTypeComponent>(DragonSwitchableTypeUiKey.SelectDragonType,
            sub =>
            {
                sub.Event<DragonSelectTypeMessage>(SelectTypeMessageHandler);
            });
    }

    //
    // UI-adjacent code
    //

    private void OnMapInit(Entity<DragonSwitchableTypeComponent> ent, ref MapInitEvent args)
    {
        _actionsSystem.AddAction(ent, ref ent.Comp.SelectTypeAction, ActionId);
        Dirty(ent);

        if (ent.Comp.SelectedDragonType != null)
        {
            SelectDragonModule(ent, ent.Comp.SelectedDragonType.Value);
        }
    }

    private void OnShutdown(Entity<DragonSwitchableTypeComponent> ent, ref ComponentShutdown args)
    {
        _actionsSystem.RemoveAction(ent, ent.Comp.SelectTypeAction);
    }

    private void OnSelectDragonTypeAction(Entity<DragonSwitchableTypeComponent> ent, ref DragonToggleSelectTypeEvent args)
    {
        if (args.Handled || !TryComp<ActorComponent>(ent, out var actor))
            return;

        args.Handled = true;

        _userInterface.TryToggleUi((ent.Owner, null), DragonSwitchableTypeUiKey.SelectDragonType, actor.PlayerSession);
    }

    private void SelectTypeMessageHandler(Entity<DragonSwitchableTypeComponent> ent, ref DragonSelectTypeMessage args)
    {
        if (ent.Comp.SelectedDragonType != null)
            return;

        if (!Prototypes.HasIndex(args.Prototype))
            return;

        SelectDragonModule(ent, args.Prototype);
    }

    //
    // Implementation
    //

    protected virtual void SelectDragonModule(
        Entity<DragonSwitchableTypeComponent> ent,
        ProtoId<DragonTypePrototype> DragonType)
    {
        ent.Comp.SelectedDragonType = DragonType;

        _actionsSystem.RemoveAction(ent, ent.Comp.SelectTypeAction);
        ent.Comp.SelectTypeAction = null;
        Dirty(ent);

        _userInterface.CloseUi((ent.Owner, null), DragonSwitchableTypeUiKey.SelectDragonType);

        UpdateEntityAppearance(ent);
    }

    protected void UpdateEntityAppearance(Entity<DragonSwitchableTypeComponent> entity)
    {
        if (!Prototypes.TryIndex(entity.Comp.SelectedDragonType, out var proto))
            return;

        UpdateEntityAppearance(entity, proto);
    }

    protected virtual void UpdateEntityAppearance(
        Entity<DragonSwitchableTypeComponent> entity,
        DragonTypePrototype prototype)
    {
        if (TryComp(entity, out InteractionPopupComponent? popup))
        {
            _interactionPopup.SetInteractSuccessString((entity.Owner, popup), prototype.PetSuccessString);
            _interactionPopup.SetInteractFailureString((entity.Owner, popup), prototype.PetFailureString);
        }

        if (TryComp(entity, out FootstepModifierComponent? footstepModifier))
        {
            footstepModifier.FootstepSoundCollection = prototype.FootstepCollection;
        }
    }
}
