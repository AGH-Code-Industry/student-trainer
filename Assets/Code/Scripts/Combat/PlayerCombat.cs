using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;
using Combat;

public class PlayerCombat : MonoBehaviour, IDamageable, IInputConsumer
{
    [Inject] readonly PlayerService playerService;
    [Inject] readonly ResourceReader reader;
    [Inject] readonly EventBus eventBus;
    [Inject] readonly InputService inputService;

    PlayerAnimationController animationController;

    ComboList playerComboList;
    CombatSystem combat;
    ComboSystem combo;

    [SerializeField] Transform attackOrigin;
    RaycastHit hit;

    [SerializeField] Renderer[] renderers;
    Dictionary<Material, Color> _materialColors = new();

    public SystemFreezer freezer = new SystemFreezer();

    void Start()
    {
        playerComboList = reader.ReadSettings<ComboList>();

        // Initialize the ComboSystem class
        string firstComboName = playerComboList.combos[0].name;
        comboSystem = new ComboSystem(playerComboList, firstComboName, this);

        // Subscribe to ComboSystem's events
        comboSystem.onAttackStart += AttackStarted;
        comboSystem.onAttackPerformed += AttackPerformed;
        comboSystem.onRecoveryEnd += RecoveryEnded;

        animationController = GetComponent<PlayerAnimationController>();

        //eventBus.Subscribe<MouseClickUncaught>(OnAttack);
        List<string> wantedActions = new List<string>();
        wantedActions.Add("MouseClick");
        inputService.RegisterConsumer(this, wantedActions);

        // Save renderer materials
        renderers.SelectMany(r => r.materials).ToList().ForEach(m => _materialColors.Add(m, m.color));
    }

    void Update()
    {

    }
    /*
    private void OnAttack(MouseClickUncaught click)
    {
        bool isValid = click.ctx.performed && click.button == MouseClickEvent.MouseButton.Left;
        if (isValid) comboSystem.Attack();
    }
    */

    public int priority { get; } = 1;

    public bool ConsumeInput(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return false;

        if (freezer.Frozen)
            return false;

        InputHelper.MouseClickData click = new InputHelper.MouseClickData(context);
        if (click.button == InputHelper.MouseClickData.MouseButton.Left)
        {
            comboSystem.Attack();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDestroy()
    {
        //eventBus.Unsubscribe<MouseClickUncaught>(OnAttack);
        inputService.RemoveConsumer(this);
    }

    #region Player_Attacks

    void AttackStarted(string animationName)
    {
        playerService.freezer.Freeze("combat");
        animationController.PlayAnimation(animationName);
    }

    void AttackPerformed(float damage, float range)
    {

    }

    void RecoveryEnded()
    {
        playerService.freezer.Unfreeze("combat");
    }

    #endregion

    #region Player_Damage

    IEnumerator FlashDamage()
    {
        renderers.SelectMany(r => r.materials).ToList().ForEach(m => m.color = Color.red);
        yield return new WaitForSeconds(0.1f);
        renderers.SelectMany(r => r.materials).ToList().ForEach(m => m.color = _materialColors[m]);
    }

    public void TakeDamage(DamageInfo damage)
    {
        StartCoroutine(FlashDamage());
        playerService.Health -= damage.amount;
    }

    #endregion
}
