// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI;
using Molecule.Model;
using System.Diagnostics;

public class TransparentFocus : MonoBehaviour {
	
	public Vector3 target;
	public Shader shaderDifuse;
	public Shader shaderTransparent;
	public float targetAlpha = 180;
	public float time = 0.5f;
	public GameObject o;
	public bool mustFadeBack = false;
	public ParticleSystem pSystem;
	public static int pNumber;
	public GameObject parentObj;
	private Particle[] sourceParticles;
	private ParticleSystem.Particle[] particles;
	private RaycastHit[] backupHits;
	private bool hitting = true;
	private int count = 0;
	private GameObject[] AtomsHit = new GameObject[1000];
	private bool empty = true;
	
	// Use this for initialization
	void Start ()
	{      
		shaderDifuse      = Shader.Find("Diffuse");
		shaderTransparent = Shader.Find("Transparent/Diffuse");
	}
	/*
	void MakeTransparent(int c)
	{
		Stopwatch stopWatch = new Stopwatch();
		stopWatch.Start();
		UnityEngine.Debug.LogWarning("Make transparent");
		
		for(int i=0; i<c; i++)
		{
			//Color k = Color.white;
			o = AtomsHit[i];
			o.renderer.material.shader = shaderTransparent;
			Color k = o.renderer.material.color;
			k.a = 0.1f;
			o.renderer.material.color = k;
		}
		AtomsHit = new GameObject[1000];
		empty = true;
		stopWatch.Stop();
		TimeSpan ts = stopWatch.Elapsed;
		
		UnityEngine.Debug.LogWarning("RunTime: " + ts.Seconds + "s "+ts.Milliseconds/10);
	}
	
	// Update is called once per frame
	void Update ()
	{
		RaycastHit[] hits;
		
		if((UIData.Instance.atomtype == UIData.AtomType.sphere) && (UIData.Instance.optim_view))
		{
			target = maxCamera.optim_target;
			double distance = Vector3.Distance(transform.position, target) * 0.50;
			UnityEngine.Debug.Log ("Distance: "+distance);
			hits = Physics.SphereCastAll(transform.position, (float) distance / 2, target - transform.position, (float) distance, 1);
			if(hits.Length > 0)
			{
				UnityEngine.Debug.Log("Number of hits: "+hits.Length);
				for(int j = 0; j<hits.Length; j++)
				{
					o = hits[j].collider.gameObject;
					if(o.GetComponent<iTween>() == null || o.renderer.material.color.a != 0.1f)
					{
						//UnityEngine.Debug.Log("Fade UP");

						//FadeDown(o);
						if(o.renderer.material.shader != shaderTransparent || o.renderer.material.color.a != 0.1f)
						{
							o.layer = 1;
							AtomsHit[count] = o;
							//UnityEngine.Debug.Log("Setting transparent shader" );
							count = count +1;
						
							//count = count +1;
						}
					}
				}
				empty = false;
				hitting = false;
				UnityEngine.Debug.LogWarning("COUNT: "+count);
				if(count > 0){
					MakeTransparent(count);
				}
				else
				{
					empty = true;
				}
				count = 0;		
			}
			else
			{
				empty = false;
				hitting = false;
				UnityEngine.Debug.LogWarning("COUNT: "+count);
				if(count > 0){
					MakeTransparent(count);
				}
				else
				{
					empty = true;
				}
				count = 0;
				//UnityEngine.Debug.LogWarning("LENGTH: "+AtomsHit.Length);
			}		
		}
	}
	
	void FadeUp(GameObject f)
	{
		//iTween.Stop(f);
		iTween.FadeTo(f, iTween.Hash("alpha", 1, "time", time, "oncomplete", "SetDifuseShading", "oncompletetarget", this.gameObject, "oncompleteparams", f));
	}
	
	void FadeDown(GameObject f)
	{
		//iTween.Stop(f);
		iTween.FadeTo(f, iTween.Hash("alpha", targetAlpha, "time", time));
	}
	
	void SetDifuseShading(GameObject f)
	{
		if(f.renderer.material.color.a == 1)
		{
			f.renderer.material.shader = shaderDifuse;
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, target - transform.position);
	}
	*/
}

