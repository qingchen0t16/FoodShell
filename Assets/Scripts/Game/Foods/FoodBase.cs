using Assets.Scripts.Enum;
using Assets.Scripts.Model;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Foods
{
    public abstract class FoodBase : MonoBehaviour
    {
        protected Animator an;
        protected SpriteRenderer sr;
        protected M_Grid grid;
        protected float health; // 食物生命
        protected bool isAttack;    // 是否攻击

        public float Health { get => health; }

        public abstract float MaxHealth { get; }
        public abstract float AttackSpeed { get; }

        private void Awake()
        {
            an = GetComponentInChildren<Animator>();
            sr = GetComponentInChildren<SpriteRenderer>();

            health = MaxHealth;
        }

        /// <summary>
        /// 创建时
        /// </summary>
        public virtual GameObject InitCreate()
        {
            an.speed = 0;
            sr.color = new Color(1, 1, 1, 1);
            sr.sortingOrder = 999;
            return gameObject;
        }

        /// <summary>
        /// 在网格中的半透明
        /// </summary>
        public virtual GameObject InitGrid()
        {
            an.speed = 0;
            sr.color = new Color(1, 1, 1, 0.5F);
            sr.sortingOrder = -1;
            return gameObject;
        }

        /// <summary>
        /// 在网格中初始化(正常的)
        /// </summary>
        public virtual void InitFood(M_Grid grid, int level = 1)
        {
            this.grid = grid;
            an.speed = 1;
            sr.color = new Color(1, 1, 1, 1);

            isAttack = true;
        }

        /// <summary>
        /// 受到伤害
        /// </summary>
        public virtual void Damage(float hurt)
        {
            ColorEF(0.2F, new Color(0.5F, 0.5F, 0.5F), 0.01F);
            health -= hurt;
            if (health <= 0)
                Death();
        }

        /// <summary>
        /// 死亡
        /// </summary>
        public virtual void Death()
        {
            grid.Statu = GridStatu.main;
            grid.Food = null;
            Destroy(gameObject);
        }

        /// <summary>
        /// 颜色效果
        /// </summary>
        /// <returns></returns>
        protected IEnumerator ColorEF(float wTime, Color tColor, float dTime, UnityAction action = null)
        {
            float currTime = 0;
            float lerp;

            while (currTime < wTime)
            {
                yield return new WaitForSeconds(0.05F);
                lerp = currTime / wTime;
                currTime -= dTime;
                sr.color = Color.Lerp(Color.white, tColor, lerp);
            }
            sr.color = Color.white;
            if (action != null)
                action();
        }
    }
}
