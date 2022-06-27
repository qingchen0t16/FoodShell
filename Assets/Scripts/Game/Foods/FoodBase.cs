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
        public GameObject InitCreate()
        {
            an.speed = 0;
            sr.color = new Color(1, 1, 1, 1);
            sr.sortingOrder = 999;
            return gameObject;
        }

        /// <summary>
        /// 在网格中的半透明
        /// </summary>
        public GameObject InitGrid()
        {
            an.speed = 0;
            sr.color = new Color(1, 1, 1, 0.5F);
            sr.sortingOrder = -1;
            return gameObject;
        }

        /// <summary>
        /// 在网格中初始化(正常的)
        /// </summary>
        public void InitFood(M_Grid grid,int level = 1)
        {
            this.grid = grid;
            an.speed = 1;
            sr.color = new Color(1, 1, 1, 1);
            _initFood(level);
        }
        protected abstract void _initFood(int level);

        /// <summary>
        /// 受到伤害
        /// </summary>
        public void Damage(float hurt)
        {
            ColorEF(0.2F, new Color(0.5F, 0.5F, 0.5F), 0.01F);
            health -= hurt;
            if (health <= 0)
                Death();
        }

        /// <summary>
        /// 死亡
        /// </summary>
        public void Death()
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
            float cTime = 0;
            float lerp;

            while (cTime < wTime)
            {
                yield return new WaitForSeconds(dTime);
                lerp = cTime / wTime;
                cTime += dTime;
                sr.color = Color.Lerp(Color.white, tColor, lerp);
            }

            sr.color = Color.white;
            if (action != null)
                action();
        }
    }
}
