using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script
{
    public class QueryExamples : MonoBehaviour
    {
        public Npc[] npcs = new Npc[2];

        public Npc myNPC;


        private void Update()
        {
        }

        bool HasAnElf()
        {
            foreach (var npc in npcs)
            {
                if (npc.Faction == Faction.Elf)
                {
                    return true;
                }
            }

            return false;
        }
        
        bool LINQ_AnyExamples_HasAnElf()
        {
            // var hasAtLeastOneElf = npcs.Any(npc => npc.Faction == Faction.Elf);
            // return hasAtLeastOneElf;
            return npcs.Any
                (npc => npc.Faction == Faction.Elf 
                        && npc.Stats.AttackRange > 10);
        }

        float AverageStrength()
        {
            var avgStrength = 0.0f;
            var count = 0;
            foreach (var npc in npcs)
            {
                avgStrength += npc.Stats.Strength;
                count++;
            }

            return avgStrength / count;
        }

        float LINQ_AverageExample_AverageStrength()
        {
            var avgStrength = npcs.Average(npc => npc.Stats.Strength);
            return (float) avgStrength;
        }

        float AverageStrength_OrcsOnly()
        {
            var avgStrength = 0.0f;
            var count = 0;
            foreach (var npc in npcs)
            {
                if (npc.Faction != Faction.Orc) continue;
                avgStrength += npc.Stats.Strength;
                count++;
            }

            return avgStrength / count;
        }

        float LINQ_AverageExample_AverageStrength_OrcsOnly()
        {
            var avgOrcStrength = npcs
                .Where(npc => npc.Faction == Faction.Orc)
                .Average(npc => npc.Stats.Strength);
            return (float) avgOrcStrength;
        }


        int NumberOfHumans()
        {
            var count = 0;
            foreach (var npc in npcs)
            {
                if (npc.Faction == Faction.Human)
                    count++;
            }

            return count;
        }
        
        
        int LINQ_CountExample_NumberOfHumans()
        {
            return npcs.Count(npc => npc.Faction == Faction.Human);
        }

        Npc[] UniqueNPCs()
        {
            
            List<Npc> npcList = new List<Npc>();
            var npc = new GameObject("npc").AddComponent<Npc>();
            npcList.Add(npc);
            npcList.Add(npc);
            
            var npcArray = new Npc[npcList.Count];

            var exists = false;
            var i = 0;
            foreach (var bot in npcList)
            {
                foreach (var bott in npcArray)
                {
                    exists = bot == bott;
                }
                if (exists)
                    continue;
                npcArray[i] = bot;
                i++;
            }

            return npcArray;
        }

        Npc[] LINQ_DistinctExample_UniqueNPCs()
        {
            List<Npc> npcList = new List<Npc>();
            var npc = new GameObject("npc").AddComponent<Npc>();
            npcList.Add(npc);
            npcList.Add(npc);

            return npcList.Distinct().ToArray();
        }

        void LINQ_FirstOrDefaultExample()
        {
            var firstDeadNpc = npcs.FirstOrDefault(npc => npc.Health <= 0);
            if (!firstDeadNpc)
            {
                Debug.Log("No npcs are dead");
                return;
            }
            Debug.Log($"{firstDeadNpc} is the first entry in the Npcs list that's dead!");;
        }

        void LINQ_FirstExample()
        {
            var firstDeadNpc = npcs.First(npc => npc.Health <= 0);
            Debug.Log($"{firstDeadNpc} is the first entry in the Npcs list that's dead!");
        }

        IOrderedEnumerable<Npc> LINQ_OrderByExample()
        {
            var npcsByName = npcs.OrderBy(npc => npc.name);
            return npcsByName;
        }

        IOrderedEnumerable<Npc> LINQ_OrderByExample_HealthDescending()
        {
            return npcs.OrderByDescending(npc => npc.Health);
        }

        IOrderedEnumerable<Npc> LINQ_OrderByExample_Random()
        {
            return npcs.OrderBy(npc => Random.Range(0, 1000));
        }

        void LINQ_GroupByExample()
        {
            var npcsByFaction = npcs.GroupBy(npc => npc.Faction);
            foreach (var grouping in npcsByFaction)
            {
                Faction faction = grouping.Key;
                Debug.Log($"Looking at {faction} npcs");
                foreach (var npc in grouping)
                {
                    Debug.Log(npc);
                }
            }
        }

        void LINQ_Intersect()
        {
            var npcsWhoAreElves = npcs.Where(npc => npc.Faction == Faction.Elf);
            var npcsWhoHaveNoHealth = npcs.Where(npc => npc.Health <= 0);
            var npcsWhoAreInBothCollections = npcsWhoAreElves.Intersect(npcsWhoHaveNoHealth);
        }

        void LINQ_MinMaxExample()
        {
            var minStrengthAnNpcHas = npcs.Min(npc => npc.Stats.Strength);
            var maxStrengthAnNpcHas = npcs.Max(npc => npc.Stats.Strength);
        }

        IEnumerable<Faction> LINQ_SelectExample_GetActiveFactions()
        {
            return npcs.Where(npc => npc.Health > 0).Select(npc => npc.Faction);
            //Inclui duplicatas
        }

        IEnumerable<Faction> LINQ_SelectExample_GetActiveFactions_Distinct()
        {
            var uniqueActiveFactions = npcs.Where(npc => npc.Health > 0).Select(npc => npc.Faction).Distinct();
            return uniqueActiveFactions;
        }

        // IEnumerable<Faction> LINQ_SelectManyExample()
        // {
        //     IEnumerable<Faction> query1 = npcs.SelectMany(npc => npc.Faction);
        //     return npcs.Where(npc => npc.Stats.Damage > 0).SelectMany(npc => npc.Faction);
        // }

        private int pageNumber = 0; //contagem do zero

        IEnumerable<Npc> LINQ_SkipExample_GetNextPageOfNpcs()
        {
            const int pageSize = 10;
            var nextPageOfNpcs = npcs.Skip(pageSize * pageNumber).Take(pageSize);
            pageNumber++;
            return nextPageOfNpcs;
        }

        void LINQ_ToListAndBackExample()
        {
            List<Npc> npcsList = npcs.ToList();
            npcs = npcsList.ToArray();
        }

        Dictionary<Faction, List<Npc>> LINQ_ToDictionaryExample()
        {
            Dictionary<string, Npc> npcsByName = npcs.ToDictionary(k => k.name, v => v);

            Dictionary<Faction, List<Npc>> npcsByFaction =
                npcs.GroupBy(k => k.Faction, v => v).
                    ToDictionary(k => k.Key, v => v.ToList());

            return npcsByFaction;
        }



        void LINQ_LookupExample()
        {
            //Tipo um dicionário, mas imutável
            var npcsLookup = npcs.ToLookup(k => k.Faction, v => v);

            var elves = npcsLookup[Faction.Elf];
            var orcs = npcsLookup[Faction.Orc];
        }















    }
    
}