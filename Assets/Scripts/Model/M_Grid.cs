using Assets.Scripts.Enum;
using Assets.Scripts.Game.Foods;
using UnityEngine;

namespace Assets.Scripts.Model
{
    public class M_Grid
    {
        private Vector2 point;
        private Vector2 position;
        private GridStatu statu;

        public Vector2 Point { get => point; set => point = value; }
        public Vector2 Position { get => position; set => position = value; }
        public GridStatu Statu { get => statu; set => statu = value; }
        public GameObject Food;

        public M_Grid(Vector2 point, Vector2 position, GridStatu statu)
        {
            this.Point = point;
            this.Position = position;
            this.Statu = statu;
        }

        /// <summary>
        /// 获取食物
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public FoodBase GetFood<T>() where T : FoodBase
            => Food.GetComponent<T>();
    }
}
