using UnityEngine;

using WMSDK.Assets;


public sealed class WeaponAnimTester : MonoBehaviour
{
    [SerializeField]
    private WeaponModelDescription[] weapons;

    [SerializeField]
    private uint slot;

	[SerializeField]
	private Transform rightHandAttachment;

	private WeaponModelDescription m_Desc;
	private WeaponModelDescription m_DescPrev;


	private enum InternalAct
	{
		NONE, HOLSTER, SWITCH, DRAW
	}

	private Transform m_RightHand;
	private Animator m_Anim;
	private IKHelperLeftHand m_LeftHandHelper;
	private InternalAct m_Act;


	private void Awake()
	{
		m_Anim = GetComponent<Animator>();
		m_RightHand = rightHandAttachment == null ?
			m_Anim.GetBoneTransform(HumanBodyBones.RightHand) : rightHandAttachment;

		m_LeftHandHelper = gameObject.AddComponent<IKHelperLeftHand>();
	}

	private void Update()
	{
		if (m_Desc != null)
		{
			m_LeftHandHelper.weight = 1.0f;

			Vector3 vOffset = m_Desc.rhand.localPosition;
			Quaternion qOffset = m_Desc.rhand.localRotation;
			m_Desc.transform.SetLocalPositionAndRotation(vOffset, qOffset);

			m_LeftHandHelper.SetTarget(m_Desc.lhand);
		}
		else
		{
			m_LeftHandHelper.weight = 0.0f;
		}

		switch (m_Act)
		{
			case InternalAct.SWITCH:
				m_Anim.SetLayerWeight(1, 1.0f);
				SetWeaponAnimType(m_Desc != null ? m_Desc.type : 0);
				m_Anim.CrossFade(DRAW, 0.2f, 1);
				m_Act = InternalAct.NONE;

				SetWeaponVisible();
				break;
			case InternalAct.HOLSTER:
				m_Anim.SetLayerWeight(1, 0.0f);
				m_Act = InternalAct.NONE;

				SetWeaponVisible();
				break;
			case InternalAct.DRAW:
				m_Anim.SetLayerWeight(1, 1.0f);
				m_Act = InternalAct.NONE;
				break;
		}
	}

	public void OnWeaponChange(WeaponModelDescription desc)
	{
		SetWeaponAnimDescriptor(desc);

		if (m_DescPrev == null)
		{
			SetWeaponAnimType(m_Desc != null ? m_Desc.type : 0);
			m_Anim.CrossFade(DRAW, 0.2f, 1);
			m_Act = InternalAct.DRAW;

			SetWeaponVisible();
		}
		else
		{
			m_Anim.CrossFade(HOLSTER, 0.1f, 1);
			m_Act = InternalAct.SWITCH;
		}
	}

	private void SetWeaponAnimDescriptor(WeaponModelDescription desc)
	{
		m_DescPrev = m_Desc;
		m_Desc = desc;
	}

	[ContextMenu("Switch to 'slot'")]
	public void SwitchWeapon()
	{
		OnWeaponChange(weapons[slot]);
	}

	[ContextMenu("Holster")]
	public void OnWeaponHolster()
	{
		SetWeaponAnimDescriptor(null);
		m_Anim.CrossFade(HOLSTER, 0.1f, 1);
		m_Act = InternalAct.HOLSTER;
	}

	private void SetWeaponVisible()
	{
		if (m_DescPrev != null)
		{
			m_DescPrev.mesh.gameObject.SetActive(false);
			m_DescPrev.transform.SetParent(null);
		}

		if (m_Desc != null)
		{
			m_Desc.mesh.gameObject.SetActive(true);
			ApplyHandTarget(m_Desc);
		}
	}

	private void ApplyHandTarget(WeaponModelDescription desc)
	{
		desc.transform.SetParent(m_RightHand);

		Vector3 vOffset = desc.rhand.localPosition;
		Quaternion qOffset = desc.rhand.localRotation;
		desc.transform.SetLocalPositionAndRotation(vOffset, qOffset);

		m_LeftHandHelper.SetTarget(desc.lhand);
	}

	// Animations hash
	//
	public static void SetWeaponAnimType(WeaponAnimType t)
	{
		switch (t)
		{
			case WeaponAnimType.RIFLE:
				DRAW = AnimRifle.DRAW;
				HOLSTER = AnimRifle.HOLSTER;
				break;
			case WeaponAnimType.PISTOL:
				DRAW = AnimPistol.DRAW;
				HOLSTER = AnimPistol.HOLSTER;
				break;
			case WeaponAnimType.SHOTGUN:
				DRAW = AnimShotgun.DRAW;
				HOLSTER = AnimShotgun.HOLSTER;
				break;
			default:
				DRAW = AnimRifle.DRAW;
				HOLSTER = AnimRifle.HOLSTER;
				break;
		}
	}

	private static int DRAW;
	private static int HOLSTER;

	private static class AnimRifle
	{
		public static readonly int DRAW = Animator.StringToHash("Rifle_draw");
		public static readonly int HOLSTER = Animator.StringToHash("Rifle_holster");
	}

	private static class AnimPistol
	{
		public static readonly int DRAW = Animator.StringToHash("Pistol_draw");
		public static readonly int HOLSTER = Animator.StringToHash("Pistol_holster");
	}

	private static class AnimShotgun
	{
		public static readonly int DRAW = Animator.StringToHash("Shotgun_draw");
		public static readonly int HOLSTER = Animator.StringToHash("Shotgun_holster");
	}
}
