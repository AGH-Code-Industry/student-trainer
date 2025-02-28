using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerCombat : MonoBehaviour, IDamageable
{
    [Inject] readonly PlayerService playerService;
    [Inject] readonly ResourceReader reader;
    [Inject] readonly EventBus eventBus;

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
        comboSystem.onAttackEnd += AttackEnded;

        animationController = GetComponent<PlayerAnimationController>();

        eventBus.Subscribe<PlayerAttack>(OnAttack);

        // Save renderer materials
        foreach (Renderer r in renderers)
        {
            foreach (Material material in r.materials)
            {
                _materialColors.Add(material, material.color);
            }
        }
    }

    void Update()
    {
        
    }

    private void OnAttack(PlayerAttack playerAttack)
    {
        if (playerAttack.ctx.performed) comboSystem.Attack();
    }

    private void OnDestroy()
    {
        eventBus.Unsubscribe<PlayerAttack>(OnAttack);
    }

    #region Player_Attacks

    void AttackStarted(string animationName)
    {
        playerService.Freeze();
        animationController.PlayAnimation(animationName);
    }

    void AttackPerformed(float damage, float range)
    {
        Ray ray = new Ray(attackOrigin.position, attackOrigin.forward);
        bool inRange = Physics.Raycast(ray, out hit, range);
        if (!inRange)
            return;

        IDamageable damageComponent = hit.transform.root.GetComponent<IDamageable>();
        if (damageComponent != null)
            damageComponent.TakeDamage(damage);
    }

    void AttackEnded()
    {
        playerService.Unfreeze();
    }

    #endregion

    #region Player_Damage

    IEnumerator FlashDamage()
    {
        foreach (Renderer r in renderers)
        {
            foreach (Material material in r.materials)
            {
                material.color = Color.red;
            }
        }

        yield return new WaitForSeconds(0.1f);

        foreach (Renderer r in renderers)
        {
            foreach (Material material in r.materials)
            {
                material.color = _materialColors[material];
            }
        }
    }

    public void TakeDamage(float amount)
    {
        StartCoroutine(FlashDamage());
        playerService.Health -= amount;
    }

    #endregion
}
