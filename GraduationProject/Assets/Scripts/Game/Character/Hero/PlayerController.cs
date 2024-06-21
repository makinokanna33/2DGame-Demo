using GameFrameWork;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : SingletonMono<PlayerController>
{
    public GameObject DefualtHero;
    [HideInInspector]
    public List<EquipmentData> equipmentDatas  = new List<EquipmentData>();
    [HideInInspector]
    public HeroController currentCharacter;
    [HideInInspector]
    public HeroController character1;
    [HideInInspector]
    public HeroController character2;

    [HideInInspector]
    public bool isInvincible;
    private float timeSpentInvincible = -1f;
    protected override void Awake()
    {
        base.Awake();
        if(Instance != this)
        {
            return;
        }
        if (DefualtHero == null)
        {
            currentCharacter = character1 = 
                GameUtility.LoadGameObject("Prefabs/Character/Player/Skul").GetComponent<HeroController>();
        }
        else
        {
            currentCharacter = character1 = Instantiate(DefualtHero).GetComponent<HeroController>();
        }
        currentCharacter.transform.SetParent(transform);
        GameObject obj = GameObject.FindGameObjectWithTag("PlayerBorn");
        if (obj != null)
        {
            currentCharacter.gameObject.SetActive(true);
            InputManager.Instance.CharacterControlLock = false;
            currentCharacter.transform.position = obj.transform.position;
        }
    }

    private void Start()
    {
        EventManager.StartListening(EventName.EnmeyAttackPlayer, OnEnemyAttackPlayer);
        EventManager.StartListening(EventName.OnLevelLoaded, OnLevelLoaded);
        EventManager.StartListening(EventName.PlayerInput_Switch, SwitchCurrentCharacter);
        isInvincible = false;
        timeSpentInvincible = 0f;
    }

    private void Update()
    {
        if (isInvincible)
        {
            timeSpentInvincible += Time.deltaTime;

            if (timeSpentInvincible < 1f)
            {
                float remainder = timeSpentInvincible % 0.3f;
                currentCharacter.GetComponent<SpriteRenderer>().enabled = remainder > 0.15f;
            }
            else
            {
                timeSpentInvincible = 0;
                currentCharacter.GetComponent<SpriteRenderer>().enabled = true;
                isInvincible = false;
            }
        }
    }

    private void OnLevelLoaded()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("PlayerBorn");
        currentCharacter.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if(obj != null)
        {
            currentCharacter.gameObject.SetActive(true);
            InputManager.Instance.CharacterControlLock = false;
            currentCharacter.transform.position = obj.transform.position;
        }
        else
        {
            InputManager.Instance.CharacterControlLock = true;
            currentCharacter.gameObject.SetActive(false);
        }

        if(SceneManager.GetActiveScene().name == "HomeScene")
        {
            currentCharacter.RefreshHp();
            equipmentDatas.Clear();
            if(character2 != null)
                DestroyImmediate(character2.gameObject);
            if (character1 != null)
                DestroyImmediate(character1.gameObject);
            character1 = character2 = null;
            Awake();
            EventManager.EmitEvent(EventName.PlayerSwitchCharacter);
        }
    }

    /// <summary>
    /// 角色交换功能
    /// </summary>
    public void SwitchCurrentCharacter()
    {
        if(character1 == null || character2 == null)
        {
            return;
        }
        character1.SetPlayerHp(currentCharacter.GetCurrentHp());
        character2.SetPlayerHp(currentCharacter.GetCurrentHp());

        character1.transform.position = character2.transform.position = currentCharacter.transform.position;
        character1.transform.rotation = character2.transform.rotation = currentCharacter.transform.rotation;
        currentCharacter.gameObject.SetActive(false);
        currentCharacter = currentCharacter == character1 ? character2 : character1;
        currentCharacter.gameObject.SetActive(true);
        CameraController.instance.SetFollowTarget(currentCharacter.transform);
        EventManager.SetData(EventName.PlayerSwitchCharacter, currentCharacter);
        EventManager.EmitEvent(EventName.PlayerSwitchCharacter);
    }

    public void OnEnemyAttackPlayer()
    {
        if(isInvincible)
        {
            return;
        }
        isInvincible = true;
        timeSpentInvincible = 0f;

        Transform enmey = EventManager.GetSender(EventName.EnmeyAttackPlayer) as Transform;
        //TODO:怪物攻击玩家
        EnemyController enemyController = enmey.GetComponent<EnemyController>();

        //生成特效
        GameObject vfx = GameUtility.LoadGameObject("Prefabs/VFX/PlayerBeHit");
        vfx.transform.position = currentCharacter.transform.position;
        currentCharacter.TakeDamage(enemyController.GetRandomAttackDamage());
    }

    public int GetCurrentDirection()
    {
        if (currentCharacter.transform.rotation.y == 0)
            return 1;
        else
            return -1;
    }

    public void GetNewHero(HeroController hero)
    {
        if(character2 == null)
        {
            character2 = hero;
            SwitchCurrentCharacter();
        }
        else
        {
            HeroController tmp = null;
            if(currentCharacter == character1)
            {
                tmp = character1;
                character1 = hero;
                currentCharacter = character2;
            }
            else
            {
                tmp = character2;
                character2 = hero;
                currentCharacter = character1;
            }
            currentCharacter.transform.position = tmp.transform.position;
            currentCharacter.transform.rotation = tmp.transform.rotation;
            SwitchCurrentCharacter();

            Head head = GameUtility.LoadGameObject("Prefabs/Tools/Head").GetComponent<Head>();
            head.transform.position = currentCharacter.transform.position;
            head.InitHead(tmp.GetHeroData().HeroName, tmp.GetHeroData().Scene_HeadIcon);
            Destroy(tmp.gameObject);
        }
    }

    #region 装备
    public void AddEquip(EquipmentData data)
    {
        equipmentDatas.Add(data);
        if(data.type == EquipmentType.Hp)
        {
            currentCharacter.AddCurrentHp(currentCharacter.GetBasicMaxHp() * data.point);
        }
    }

    public void RemoveEquip(EquipmentData data)
    {
        equipmentDatas.Remove(data);
        currentCharacter.AddCurrentHp(0);
    }

    public float GetEquipmentEffect(EquipmentType type)
    {
        float sum = 0;
        foreach (var value in equipmentDatas)
        {
            if(value.type == type)
            {
                sum += value.point;
            }
        }
        return sum;
    }
    #endregion

}
