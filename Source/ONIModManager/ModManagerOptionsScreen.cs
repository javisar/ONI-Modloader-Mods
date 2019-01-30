// AudioOptionsScreen
using FMODUnity;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModManagerOptionsScreen : KModalScreen
{
	[SerializeField]
	private KButton closeButton;

	[SerializeField]
	private KButton doneButton;

	[SerializeField]
	private SliderContainer sliderPrefab;

	[SerializeField]
	private GameObject sliderGroup;

	[SerializeField]
	private Image jambell;

	[SerializeField]
	private GameObject alwaysPlayMusicButton;

	[SerializeField]
	private GameObject alwaysPlayAutomationButton;

	[SerializeField]
	private Dropdown deviceDropdown;

	private UIPool<SliderContainer> sliderPool;

	private Dictionary<KSlider, string> sliderBusMap = new Dictionary<KSlider, string>();

	public static readonly string AlwaysPlayMusicKey = "AlwaysPlayMusic";

	public static readonly string AlwaysPlayAutomation = "AlwaysPlayAutomation";

	private Dictionary<string, object> alwaysPlayMusicMetric = new Dictionary<string, object>
	{
		{
			ModManagerOptionsScreen.AlwaysPlayMusicKey,
			(object)null
		}
	};

	private List<KFMOD.AudioDevice> audioDevices = new List<KFMOD.AudioDevice>();

	private List<Dropdown.OptionData> audioDeviceOptions = new List<Dropdown.OptionData>();

	internal static GameObject Load()
	{
		try
		{
			return new GameObject(typeof(ModManagerOptionsScreen).FullName, typeof(ModManagerOptionsScreen));

			//return true;
		}
		catch (Exception e)
		{
			Debug.LogException(e);
		}

		return null;
	}


	protected override void OnSpawn()
	{
		base.OnSpawn();
		
		this.closeButton = new KButton();
		this.doneButton = new KButton();
		this.sliderPrefab = new SliderContainer();
		this.sliderGroup = new GameObject();
		this.alwaysPlayMusicButton = new GameObject();
		this.alwaysPlayAutomationButton = new GameObject();

		this.closeButton.onClick += delegate
		{
			this.OnClose(base.gameObject);
		};		
		this.doneButton.onClick += delegate
		{
			this.OnClose(base.gameObject);
		};		
		this.sliderPool = new UIPool<SliderContainer>(this.sliderPrefab);
		Dictionary<string, AudioMixer.UserVolumeBus> userVolumeSettings = AudioMixer.instance.userVolumeSettings;
		Debug.Log("1");
		foreach (KeyValuePair<string, AudioMixer.UserVolumeBus> item in userVolumeSettings)
		{
			SliderContainer newSlider = this.sliderPool.GetFreeElement(this.sliderGroup, true);
			this.sliderBusMap.Add(newSlider.slider, item.Key);
			newSlider.slider.value = item.Value.busLevel;
			newSlider.nameLabel.text = item.Value.labelString;
			newSlider.UpdateSliderLabel(item.Value.busLevel);
			newSlider.slider.ClearReleaseHandleEvent();
			newSlider.slider.onValueChanged.AddListener(delegate
			{
				this.OnReleaseHandle(newSlider.slider);
			});
			if (item.Key == "Master")
			{
				newSlider.transform.SetSiblingIndex(1);
				newSlider.slider.onValueChanged.AddListener(this.CheckMasterValue);
				this.CheckMasterValue(item.Value.busLevel);
			}
		}
		Debug.Log("2");
		HierarchyReferences component = this.alwaysPlayMusicButton.GetComponent<HierarchyReferences>();
		Debug.Log("2a");
		GameObject gameObject = component.GetReference("Button").gameObject;
		Debug.Log("2b");
		gameObject.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUSIC_EVERY_CYCLE_TOOLTIP);
		Debug.Log("2c");
		component.GetReference("CheckMark").gameObject.SetActive(MusicManager.instance.alwaysPlayMusic);
		Debug.Log("2d");
		gameObject.GetComponent<KButton>().onClick += delegate
		{
			this.ToggleAlwaysPlayMusic();
		};
		Debug.Log("3");
		LocText reference = component.GetReference<LocText>("Label");
		reference.SetText(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.MUSIC_EVERY_CYCLE);
		if (!KPlayerPrefs.HasKey(ModManagerOptionsScreen.AlwaysPlayAutomation))
		{
			KPlayerPrefs.SetInt(ModManagerOptionsScreen.AlwaysPlayAutomation, 1);
		}
		Debug.Log("4");
		HierarchyReferences component2 = this.alwaysPlayAutomationButton.GetComponent<HierarchyReferences>();
		GameObject gameObject2 = component2.GetReference("Button").gameObject;
		gameObject2.GetComponent<ToolTip>().SetSimpleTooltip(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.AUTOMATION_SOUNDS_ALWAYS_TOOLTIP);
		component2.GetReference("CheckMark").gameObject.SetActive(MusicManager.instance.alwaysPlayMusic);
		gameObject2.GetComponent<KButton>().onClick += delegate
		{
			this.ToggleAlwaysPlayAutomation();
		};
		Debug.Log("5");
		LocText reference2 = component2.GetReference<LocText>("Label");
		reference2.SetText(UI.FRONTEND.AUDIO_OPTIONS_SCREEN.AUTOMATION_SOUNDS_ALWAYS);
		this.alwaysPlayAutomationButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive((byte)((KPlayerPrefs.GetInt(ModManagerOptionsScreen.AlwaysPlayAutomation) == 1) ? 1 : 0) != 0);
		Debug.Log("6");
	}

	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
		{
			this.Deactivate();
		}
		else
		{
			base.OnKeyDown(e);
		}
	}

	private void CheckMasterValue(float value)
	{
		//this.jambell.enabled = (value == 0f);
	}

	private void OnReleaseHandle(KSlider slider)
	{
		AudioMixer.instance.SetUserVolume(this.sliderBusMap[slider], slider.value);
	}

	private void ToggleAlwaysPlayMusic()
	{
		MusicManager.instance.alwaysPlayMusic = !MusicManager.instance.alwaysPlayMusic;
		this.alwaysPlayMusicButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive(MusicManager.instance.alwaysPlayMusic);
		KPlayerPrefs.SetInt(ModManagerOptionsScreen.AlwaysPlayMusicKey, MusicManager.instance.alwaysPlayMusic ? 1 : 0);
	}

	private void ToggleAlwaysPlayAutomation()
	{
		KPlayerPrefs.SetInt(ModManagerOptionsScreen.AlwaysPlayAutomation, (KPlayerPrefs.GetInt(ModManagerOptionsScreen.AlwaysPlayAutomation) != 1) ? 1 : 0);
		this.alwaysPlayAutomationButton.GetComponent<HierarchyReferences>().GetReference("CheckMark").gameObject.SetActive((byte)((KPlayerPrefs.GetInt(ModManagerOptionsScreen.AlwaysPlayAutomation) == 1) ? 1 : 0) != 0);
	}

	private void BuildAudioDeviceList()
	{
		this.audioDevices.Clear();
		this.audioDeviceOptions.Clear();
		int num = default(int);
		RuntimeManager.LowlevelSystem.getNumDrivers(out num);
		for (int i = 0; i < num; i++)
		{
			KFMOD.AudioDevice item = default(KFMOD.AudioDevice);
			string name = default(string);
			RuntimeManager.LowlevelSystem.getDriverInfo(i, out name, 64, out item.guid, out item.systemRate, out item.speakerMode, out item.speakerModeChannels);
			item.name = name;
			item.fmod_id = i;
			this.audioDevices.Add(item);
			this.audioDeviceOptions.Add(new Dropdown.OptionData(item.name));
		}
	}

	private void OnAudioDeviceChanged(int idx)
	{
		RuntimeManager.LowlevelSystem.setDriver(idx);
		int num = 0;
		while (true)
		{
			if (num < this.audioDevices.Count)
			{
				KFMOD.AudioDevice audioDevice = this.audioDevices[num];
				if (idx != audioDevice.fmod_id)
				{
					num++;
					continue;
				}
				break;
			}
			return;
		}
		KFMOD.currentDevice = this.audioDevices[num];
		KPlayerPrefs.SetString("AudioDeviceGuid", KFMOD.currentDevice.guid.ToString());
	}

	private void OnClose(GameObject go)
	{
		this.alwaysPlayMusicMetric[ModManagerOptionsScreen.AlwaysPlayMusicKey] = MusicManager.instance.alwaysPlayMusic;
		ThreadedHttps<KleiMetrics>.Instance.SendEvent(this.alwaysPlayMusicMetric);
		UnityEngine.Object.Destroy(go);
	}
}
