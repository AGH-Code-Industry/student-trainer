using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour, IDamageable, IInputConsumer
{
    [Inject] readonly PlayerService playerService;
    [Inject] readonly ResourceReader reader;
    [Inject] readonly EventBus eventBus;
    [Inject] readonly InputService inputService;

    PlayerAnimationController animationController;

    ComboList playerComboList;
    ComboSystem comboSystem;

    [SerializeField] Transform attackOrigin;
    RaycastHit hit;

    [SerializeField] Renderer[] renderers;
    Dictionary<Material, Color> _materialColors = new();

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
        IDamageable damageComponent;

        // If the attackOrigin object is inside an enemy, this will ensure that it still deals damage
        Collider[] overlappingColliders = Physics.OverlapSphere(attackOrigin.position, 0.25f);
        foreach (Collider col in overlappingColliders)
        {
            // Avoid dealing damage to self
            if (col.transform.root == transform.root)
                continue;

            damageComponent = col.transform.root.GetComponent<IDamageable>();
            if (damageComponent != null)
            {
                damageComponent.TakeDamage(damage);
                return;
            }
        }

        Ray ray = new Ray(attackOrigin.position, attackOrigin.forward);
        bool inRange = Physics.Raycast(ray, out hit, range);
        if (!inRange)
            return;

        damageComponent = hit.transform.root.GetComponent<IDamageable>();
        damageComponent?.TakeDamage(damage);
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

    public void TakeDamage(float amount)
    {
        StartCoroutine(FlashDamage());
        playerService.Health -= amount;
    }

    #endregion
}
