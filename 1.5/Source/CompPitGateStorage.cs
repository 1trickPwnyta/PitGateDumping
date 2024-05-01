using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace PitGateDumping
{
    public class CompPitGateStorage : ThingComp, IStoreSettingsParent
    {
        private static readonly int dumpIntervalTicks = 300;

        private StorageSettings storageSettings;
        private bool autoDump = true;

        private void InitializeStorageSettings()
        {
            storageSettings = new StorageSettings(this);
            if (parent.def.building.defaultStorageSettings != null)
            {
                storageSettings.CopyFrom(parent.def.building.defaultStorageSettings);
            }
        }

        private bool IsValidItemToDump(Thing item)
        {
            return !item.IsForbidden(Faction.OfPlayer) && storageSettings.filter.Allows(item);
        }

        private void AddAndRemoveItems()
        {
            if (autoDump)
            {
                MapPortal portal = parent as MapPortal;
                if (portal.Map != null)
                {
                    Lord lord = portal.Map.lordManager.lords.Find(l => l.LordJob is LordJob_LoadAndEnterPortal && ((LordJob_LoadAndEnterPortal)l.LordJob).portal == portal);
                    if (lord == null || !lord.ownedPawns.Any())
                    {
                        foreach (Thing thing in portal.Map.listerThings.AllThings.Where(t => IsValidItemToDump(t)))
                        {
                            TransferableOneWay transferable = new TransferableOneWay();
                            transferable.things.Add(thing);
                            portal.AddToTheToLoadList(transferable, 1);
                        }

                        if (portal.leftToLoad != null)
                        {
                            portal.leftToLoad.RemoveAll(t => t.HasAnyThing && !IsValidItemToDump(t.AnyThing));
                        }
                    }
                }
            }
        }

        public override void CompTick()
        {
            base.CompTick();
            if (Find.TickManager.TicksGame % dumpIntervalTicks == 0) 
            {
                AddAndRemoveItems();
            }
        }

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            InitializeStorageSettings();
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Deep.Look(ref storageSettings, "storageSettings");
            if (storageSettings == null)
            {
                InitializeStorageSettings();
            }
            Scribe_Values.Look(ref autoDump, "autoDump");
        }

        public bool StorageTabVisible
        {
            get
            {
                return true;
            }
        }

        public StorageSettings GetParentStoreSettings()
        {
            return parent.def.building.fixedStorageSettings;
        }

        public StorageSettings GetStoreSettings()
        {
            return storageSettings;
        }

        public void Notify_SettingsChanged()
        {
            
        }

        public override IEnumerable<Gizmo> CompGetGizmosExtra()
        {
            yield return new Command_Toggle
            {
                defaultLabel = "PitGateDumping_CommandAutoDump".Translate(),
                defaultDesc = "PitGateDumping_CommandAutoDumpDesc".Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/AutoDump", true),
                toggleAction = delegate ()
                {
                    autoDump = !autoDump;
                    if (!autoDump)
                    {
                        MapPortal portal = parent as MapPortal;
                        if (portal.leftToLoad != null)
                        {
                            portal.leftToLoad.Clear();
                        }
                    }
                },
                isActive = (() => autoDump)
            };
        }
    }
}
