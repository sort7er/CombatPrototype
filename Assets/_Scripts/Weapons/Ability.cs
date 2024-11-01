using System.Collections.Generic;
using UnityEngine;
using EnemyAI;

public abstract class Ability
{
    public float range = 10f;
    public float idealDotProduct = 0.85f;
    public float acceptedDotProduct = 0.75f;


    protected Player player;
    protected Rigidbody rb;
    protected Transform playerTrans;
    protected CameraController camController;
    protected List<Enemy> enemies;
    protected Transform[] abilityTransforms;

    protected Transform enemyTrans;
    protected Vector3 targetPosition;

    public virtual void InitializeAbility()
    {

    }
    public virtual void UpdateAbility()
    {

    }
    public virtual void LateUpdateAbility()
    {

    }

    public virtual void ExecuteAbility(Player player, List<Enemy> enemies)
    {
        SetPlayer(player);
        SetEnemies(enemies);
    }
    public virtual void ExecuteAbilityNoTarget(Player player)
    {
        SetPlayer(player);
        NoEnemies();
    }
    private void SetPlayer(Player player)
    {
        if (this.player == null)
        {
            this.player = player;
            playerTrans = player.transform;
            camController = player.cameraController;
            rb = player.rb;
            abilityTransforms = player.abilityTransforms;
        }
    }
    private void SetEnemies(List<Enemy> enemies)
    {
        this.enemies = enemies;
    }
    private void NoEnemies()
    {
        if(enemies != null)
        {
            enemies.Clear();
        }
    }
    protected void LookAtTarget(float duration)
    {
        Vector3 compensatedCamLookAt = new Vector3(targetPosition.x, targetPosition.y + 1.3f, targetPosition.z);
        camController.LookAt(compensatedCamLookAt, duration);
    }
    protected void ReleaseCurrentWeapon()
    {
        Weapon weapon = player.currentWeapon;

        for (int i = 0; i < weapon.weaponModel.Length; i++)
        {
            abilityTransforms[i].position = weapon.weaponModel[i].Position();
            abilityTransforms[i].rotation = weapon.weaponModel[i].Rotation();
            abilityTransforms[i].parent = ParentManager.instance.abilities;
        }
        weapon.SetParentForModels(0, abilityTransforms);
    }
    protected void ReturnCurrentWeapon()
    {
        player.currentWeapon.SetParentForModels(player.currentWeapon.startLocalEulerY, player.weaponTransform);

        for (int i = 0; i < abilityTransforms.Length; i++)
        {
            abilityTransforms[i].parent = player.transform;
            abilityTransforms[i].localPosition = Vector3.zero;
        }
    }
}