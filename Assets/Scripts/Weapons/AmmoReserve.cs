using Unity.Collections;
using UnityEngine;

namespace Weapons
{
    public enum AmmoType
    {
        AK47,
        SMG,
        GRENADE,
        SHOTGUN,
    }
    public class AmmoReserve : MonoBehaviour
    {
        [Header("Current Ammo")]
        [SerializeField] private int ak47 = 90;
        [SerializeField] private int smg = 120;
        [SerializeField] private int grenades = 3;
        [SerializeField] private int shotgun = 40;
        [Header("Maximum Ammo")]
        public readonly int maxGrenades = 3;
        [SerializeField] private int maxAk47 = 90;
        [SerializeField] private int maxShotgun = 90;
        [SerializeField] private int maxSMG = 90;
        

        public void ClampAmmo(AmmoType ammoType, int missingAmmo = 0)
        {
            switch (ammoType)
            {
                case AmmoType.AK47:
                    ak47 = Mathf.Clamp(ak47 - missingAmmo, 0, int.MaxValue);
                    break;
                case AmmoType.SMG:
                    smg = Mathf.Clamp(smg - missingAmmo, 0, int.MaxValue);
                    break;
                case AmmoType.SHOTGUN:
                    shotgun = Mathf.Clamp(shotgun - missingAmmo, 0, int.MaxValue);
                    break;
                case AmmoType.GRENADE:
                    grenades = Mathf.Clamp(grenades, 0, maxGrenades);
                    break;
            }
        }

        public int GetAmmo(AmmoType ammoType)
        {
            switch (ammoType)
            {
                case AmmoType.AK47:
                    return ak47;
                case AmmoType.SMG:
                    return smg;
                case AmmoType.SHOTGUN:
                    return shotgun;
                default:
                    return 0;
            }
        }

        public void AddAmmo(AmmoType ammoType, int newAmmo)
        {
            switch (ammoType)
            {
                case AmmoType.AK47:
                    ak47 += newAmmo;
                    break;
                case AmmoType.SMG:
                    smg += newAmmo;
                    break;
                case AmmoType.SHOTGUN:
                    shotgun += newAmmo;
                    break;
                case AmmoType.GRENADE:
                    grenades += newAmmo;
                    break;
            }
            ClampAmmo(ammoType);
            
        }
        

        public int Grenades
        {
            get => grenades;
            set => grenades = value;
        }
    }
    
    
    
}