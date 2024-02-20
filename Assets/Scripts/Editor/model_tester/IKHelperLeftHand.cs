using UnityEngine;


public sealed class IKHelperLeftHand : MonoBehaviour
{
	public float weight = 0.0f;

	private Animator m_Anim;
	private Transform m_Target;


	public void SetTarget(Transform tr)
	{
		m_Target = tr;
		enabled = tr != null;
	}

	private void Awake()
	{
		m_Anim = GetComponent<Animator>();
		enabled = false;
	}

	private void OnAnimatorIK(int layerIndex)
	{
		if (layerIndex == 1)
		{
			m_Anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
			m_Anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, weight);
			m_Anim.SetIKPosition(AvatarIKGoal.LeftHand, m_Target.position);
			m_Anim.SetIKRotation(AvatarIKGoal.LeftHand, m_Target.rotation);
		}
	}
}
