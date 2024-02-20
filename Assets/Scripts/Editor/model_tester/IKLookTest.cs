using UnityEngine;


public sealed class IKLookTest : MonoBehaviour
{
	public float HeadWeight = 1.0f;
	public float BodyWeight = 1.0f;
	public float Weight = 1.0f;

	private Animator m_Anim;

	[SerializeField]
	public Transform LookAtTarget;


	private void Awake()
	{
		m_Anim = GetComponent<Animator>();
	}

	public void SetTarget(Transform target)
	{
		LookAtTarget = target;
	}

	private void OnAnimatorIK(int layerIndex)
	{
		if (layerIndex == 0)
		{
			m_Anim.SetLookAtWeight(Weight, BodyWeight, HeadWeight);
			m_Anim.SetLookAtPosition(LookAtTarget.position);
		}
	}
}
