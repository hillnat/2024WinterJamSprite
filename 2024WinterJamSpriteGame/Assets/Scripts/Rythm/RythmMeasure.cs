using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RythmMeasure", menuName = "Rythm Measure", order = 100)]
// A MEASURE IN THIS CONTEXT IS A LIST OF NOTES WITH UNSPECIFIED LENGTH. THANKS
public class RythmMeasure : ScriptableObject
{
	public List<RythmNote> noteSet
	{
		get { return _noteSet; }
		set { _noteSet = value; CalibrateMeasure(); }
	}
	public List<RythmNote> _noteSet = new List<RythmNote>();
	[HideInInspector]public List<Vector2> noteTimes = new List<Vector2>(); // Represents the time where a note is playing. Each vector2 represents the start and end of a note
	public float measureEndTime => noteTimes[noteTimes.Count - 1].y;//Return the end of the last note
	public void CalibrateMeasure()//Create list of times where notes are playing
	{
		//Build note times list for evaluating in the future
		noteTimes = new List<Vector2>();
		float timeOffset = 0f;
		for(int i=0; i<noteSet.Count; i++)
		{
			RythmNote note = noteSet[i];
			float begin = timeOffset + note.preDelay;
			float end = begin + note.duration;
			timeOffset = end + note.postDelay;//Total note time offset = pre + dur + post
			noteTimes.Add(new Vector2(begin, end));
		}
	}
	public EvaluationResults Evaluate(float t) //Returns whether or not a given time has a note playing
	{
		EvaluationResults eR = new EvaluationResults();

		eR.isPlaying = false;
        for (int i=0; i < noteTimes.Count; i++)
		{
			if (t >= noteTimes[i].x && t <= noteTimes[i].y)//If time is between the x and y, a note is playing during that time
			{
                eR.isPlaying= true;
				eR.note = noteSet[i];
			}
		}
		return eR;
    }
}
public struct EvaluationResults // Return type of evlatiion call. Tell us if we clicked while a note is palying, and if so which note
{
	public bool isPlaying;
    public RythmNote note;
}