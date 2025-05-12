// Auto-generated code. Reference: "Packages/com.tss.cms/Editor/CMSGenerator.cs"

// ReSharper disable RedundantUsingDirective
#pragma warning disable CS1998

using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using FMODUnity;
using UnityEngine;
using UnityEngine.AddressableAssets;
using R3;
using TSS.Core;

namespace TSS.Audio
{
    [UsedImplicitly]
    [RuntimeOrder(ERuntimeOrder.SystemRegistration)]
    public class AudioSystem : IRuntimeLoader
    {
	    public static class Volumes
	    {
		    public static float MasterVolume
		    {
			    get
			    {
				    _masterBus.getVolume(out var volume);
				    return volume;
			    }
			    set
			    {
				    _masterBus.setVolume(value);
				    PlayerPrefs.SetFloat("master_volume", value);
			    }
		    }

		    public static float GetVolume(int index)
		    {
			    _buses[index].getVolume(out var volume);
			    return volume;
		    }

		    public static void SetVolume(int index, float volume)
		    {
			    _buses[index].setVolume(volume);
			    _buses[index].getID(out var id);
			    PlayerPrefs.SetFloat("volume_of_" + id, volume);
		    }
	    }
	    
		public static class Global
		{
			private static readonly FMOD.Studio.PARAMETER_ID SlowmoId = new FMOD.Studio.PARAMETER_ID() { data1 = 806771966, data2 = 2412655200 };

			public static void SetSlowmo(float value) => RuntimeManager.StudioSystem.setParameterByID(SlowmoId, value);
			public static float GetSlowmo()
			{
				RuntimeManager.StudioSystem.getParameterByID(SlowmoId, out var value);
				return value;
			}

		}

		public static SoundEvent_UI_Hover UI_Hover { get; } = new();
		public static SoundEvent_UI_Slider UI_Slider { get; } = new();
		public static SoundEvent_UI_Click UI_Click { get; } = new();
		public static SoundEvent_UI_AchievementAppear UI_AchievementAppear { get; } = new();
		public static SoundEvent_Game_Death Game_Death { get; } = new();
		public static SoundEvent_Game_StepForest Game_StepForest { get; } = new();
		public static SoundEvent_Game_StepWood Game_StepWood { get; } = new();
		public static SoundEvent_OST_OSTMenu OST_OSTMenu { get; } = new();
		public static SoundEvent_OST_OST1 OST_OST1 { get; } = new();
		public static SoundEvent_Game_PortalOut Game_PortalOut { get; } = new();
		public static SoundEvent_Game_Attack Game_Attack { get; } = new();
		public static SoundEvent_Game_PortalIn Game_PortalIn { get; } = new();
		public static SoundEvent_Game_Cut Game_Cut { get; } = new();
		public static SoundEvent_Game_SpawnEnemy Game_SpawnEnemy { get; } = new();
		public static SoundEvent_Game_Reflect Game_Reflect { get; } = new();
		public static SoundEvent_Game_Shot Game_Shot { get; } = new();
		public static SoundEvent_Game_FlyEnemyIdle Game_FlyEnemyIdle { get; } = new();
		public static SoundEvent_Game_BossAttack1 Game_BossAttack1 { get; } = new();
		public static SoundEvent_Game_Beam Game_Beam { get; } = new();
		public static SoundEvent_Game_BossAttack2 Game_BossAttack2 { get; } = new();
		public static SoundEvent_Game_PhaseChange Game_PhaseChange { get; } = new();
		public static SoundEvent_Game_BossDeath Game_BossDeath { get; } = new();
		public static SoundEvent_Game_MeleeEnemyAttack Game_MeleeEnemyAttack { get; } = new();
		public static SoundEvent_Game_MeleeEnemyMove Game_MeleeEnemyMove { get; } = new();
		public static SoundEvent_Game_BossLaugh Game_BossLaugh { get; } = new();
    
		private System.IDisposable _focusDisposable;
		
		private static FMOD.Studio.Bus _masterBus;
		private static FMOD.Studio.Bus[] _buses;
		
        public async UniTask Initialize(CancellationToken cancellationToken)
        {
			RuntimeManager.LoadBank("Master.strings", true);
			RuntimeManager.LoadBank("Master", true);

            await UniTask.WaitUntil(() => FMODUnity.RuntimeManager.HaveAllBanksLoaded);
            await UniTask.WaitWhile(FMODUnity.RuntimeManager.AnySampleDataLoading);
            
            var volumesSettings = await Addressables.LoadAssetAsync<AudioVolumesSettings>("Audio Volumes")
	            .ToUniTask(cancellationToken: cancellationToken);

            _masterBus = FMODUnity.RuntimeManager.GetBus(volumesSettings.MasterBusPath);
            _masterBus.setVolume(PlayerPrefs.GetFloat("master_volume", volumesSettings.DefaultMasterVolume));

            _buses = new FMOD.Studio.Bus[volumesSettings.BusesPaths.Length];
            for (int i = 0; i < _buses.Length; i++)
            {
	            _buses[i] = FMODUnity.RuntimeManager.GetBus(volumesSettings.BusesPaths[i]);
	            _buses[i].getID(out var busId);
	            _buses[i].setVolume(PlayerPrefs.GetFloat("volume_of_" + busId, volumesSettings.DefaultVolume));
            }
            
            Addressables.Release(volumesSettings);
            
            _focusDisposable = Runtime.ObserveFocus().Subscribe(focus =>
            {
	            if (RuntimeManager.StudioSystem.isValid())
	            {
		            RuntimeManager.PauseAllEvents(!focus);

		            if (!focus)
			            RuntimeManager.CoreSystem.mixerSuspend();
		            else
			            RuntimeManager.CoreSystem.mixerResume();
	            }
            });
        }

        public void Dispose() => _focusDisposable.Dispose();
    }

	public class SoundEvent_UI_Hover : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 0;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = 896916501, Data2 = 1233405146, Data3 = -1268089204, Data4 = 273580992 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_UI_Slider : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 0;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = 1108191865, Data2 = 1075715523, Data3 = -641858942, Data4 = 139831584 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_UI_Click : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 0;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -73757971, Data2 = 1092300725, Data3 = 530329010, Data4 = 2045083333 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_UI_AchievementAppear : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 0;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -1117165947, Data2 = 1076614636, Data3 = -967558497, Data4 = 1705713923 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_Game_Death : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 1400;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = 870002202, Data2 = 1120332501, Data3 = 1317432707, Data4 = 703449289 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_Game_StepForest : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 0;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = 1283986310, Data2 = 1332459210, Data3 = 1117376145, Data4 = 874156131 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_Game_StepWood : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 0;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -2043027316, Data2 = 1107539914, Data3 = -382578245, Data4 = 2007029359 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_OST_OSTMenu : ISoundEvent
	{
		public bool IsOneShot => false;
		public float Length => 132716;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -988305474, Data2 = 1192618021, Data3 = -1417118821, Data4 = 1323137793 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_OST_OST1 : ISoundEvent
	{
		public bool IsOneShot => false;
		public float Length => 287357;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -1591420219, Data2 = 1107691609, Data3 = -1628929105, Data4 = -1748314166 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			private static readonly FMOD.Studio.PARAMETER_ID TrackId = new FMOD.Studio.PARAMETER_ID() { data1 = 3531871485, data2 = 364292095 };

			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

			public void SetTrack(int value) => this.Instance.setParameterByID(TrackId, value);
			public int GetTrack()
			{
				this.Instance.getParameterByID(TrackId, out var value);
				return (int) value;
			}

		}
	}

	public class SoundEvent_Game_PortalOut : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 0;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = 1729894602, Data2 = 1281055264, Data3 = 220348820, Data4 = 1343284601 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_Game_Attack : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 0;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -644694830, Data2 = 1252829600, Data3 = -852884338, Data4 = -1026355759 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_Game_PortalIn : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 0;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -612544044, Data2 = 1192485007, Data3 = 1889473459, Data4 = 1543792493 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_Game_Cut : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 0;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = 254455273, Data2 = 1209414177, Data3 = 1406841737, Data4 = 1661628402 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_Game_SpawnEnemy : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 0;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = 238131555, Data2 = 1298660259, Data3 = -1552592741, Data4 = 1427297354 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_Game_Reflect : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 0;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -382338101, Data2 = 1208439130, Data3 = -1796777600, Data4 = 1373888254 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_Game_Shot : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 0;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -32681228, Data2 = 1223151010, Data3 = 1404607403, Data4 = -215606143 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_Game_FlyEnemyIdle : ISoundEvent
	{
		public bool IsOneShot => false;
		public float Length => 10176;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -1917266682, Data2 = 1302811211, Data3 = -872749950, Data4 = 1304060123 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_Game_BossAttack1 : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 0;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = 1098088479, Data2 = 1168846996, Data3 = 1836396436, Data4 = 669628032 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_Game_Beam : ISoundEvent
	{
		public bool IsOneShot => false;
		public float Length => 10070;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = 1765789230, Data2 = 1238978566, Data3 = -2107645813, Data4 = 1350091121 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			private static readonly FMOD.Studio.PARAMETER_ID BeamId = new FMOD.Studio.PARAMETER_ID() { data1 = 1210248230, data2 = 2082045266 };

			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

			public void SetBeam(int value) => this.Instance.setParameterByID(BeamId, value);
			public int GetBeam()
			{
				this.Instance.getParameterByID(BeamId, out var value);
				return (int) value;
			}

		}
	}

	public class SoundEvent_Game_BossAttack2 : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 0;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -1199017895, Data2 = 1165953288, Data3 = 1997810591, Data4 = 1840782224 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_Game_PhaseChange : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 0;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -853141370, Data2 = 1272933156, Data3 = 2117108891, Data4 = -1387677879 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_Game_BossDeath : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 7515;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -1907711088, Data2 = 1301778313, Data3 = 470096776, Data4 = -1022168409 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_Game_MeleeEnemyAttack : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 0;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -1466784356, Data2 = 1321182549, Data3 = 1130972581, Data4 = 92854286 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_Game_MeleeEnemyMove : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 0;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -906372097, Data2 = 1332760761, Data3 = 1987473030, Data4 = 44967090 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

	public class SoundEvent_Game_BossLaugh : ISoundEvent
	{
		public bool IsOneShot => true;
		public float Length => 0;

		private static readonly FMOD.GUID _guid = new FMOD.GUID() { Data1 = -1360194740, Data2 = 1091972172, Data3 = -1910793053, Data4 = -285833583 };

		public void PlayOneShot() => RuntimeManager.PlayOneShot(_guid);
		public void PlayOneShotAttached(GameObject attachTo) => RuntimeManager.PlayOneShotAttached(_guid, attachTo);
		public void PlayOneShotInPoint(Vector3 point) => RuntimeManager.PlayOneShot(_guid, point);

		public Instance CreateInstance() => new Instance(RuntimeManager.CreateInstance(_guid));
		ISoundEventInstance ISoundEvent.CreateInstance() => CreateInstance();

		public class Instance : SoundEventInstance
		{
			public Instance(FMOD.Studio.EventInstance eventInstance) : base(eventInstance) { }

		}
	}

}