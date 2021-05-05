using System;
using UnityEngine;

namespace Scripts
{
    public class PlayerA : MonoBehaviour
    {

        // #region Singleton
        // private static PlayerA INSTANCE;
        //
        // public static PlayerA GetInstance
        // {
        //     get => INSTANCE;
        // }
        //
        // private void Awake()
        // {
        //     //SINGLETON check
        //     if (INSTANCE != null && INSTANCE != this)
        //     {
        //         Destroy(gameObject);
        //     }
        //     //SINGLETON instantiate
        //     else
        //     {
        //         INSTANCE = this;
        //     }
        // }
        // #endregion

        public delegate void ChangeEnemyColor(Color color);
        public static event ChangeEnemyColor OnEnemyHit;
        
        public delegate void MyDelegate();

        private MyDelegate _myDelegate;


        // private void Start()
        // {
        //     _myDelegate += Funcao;
        //     _myDelegate();
        // }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Danificar inimigo
                if (OnEnemyHit != null)
                {
                    OnEnemyHit(Color.black);
                }
            }
            
            
        }

        private void Funcao()
        {
            print("Delegate chamado rapá");
        }
        

        
    }
}