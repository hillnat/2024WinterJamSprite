using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RythmMeasure", menuName = "Rythm Measure", order = 100)]
// A MEASURE IN THIS CONTEXT IS A LIST OF NOTES WITH UNSPECIFIED LENGTH. THANKS
public class RythmMeasure : ScriptableObject
{
	public float tolerance=0.25f;//Margin for error when checking if a sound is playing at a certain time
	public float endOffset = 0f;
	public List<Note> noteSet
	{
		get { return _noteSet; }
		set { _noteSet = value; GenerateNoteTimes(); }
	}
	public List<Note> _noteSet = new List<Note>();
	public List<float> noteTimes = new List<float>(); // Represents the time where a note is playing. Each vector2 represents the start and end of when a note is playing
	public float measureEndTime;
	public void GenerateNoteTimes()//Create list of times where notes are playing
	{
		//Build note times list for evaluating in the future
		noteTimes = new List<float>();
        float timeOffset = 0f;
		for(int i=0; i<noteSet.Count; i++)
		{
            timeOffset += noteSet[i].delay;
            noteTimes.Add(timeOffset);
        }
		measureEndTime = timeOffset+ endOffset;
    }
	public EvaluationResults EvaluateNoteTimes(float timer) //Returns whether or not a given time has a note playing
	{
		EvaluationResults eR = new EvaluationResults();

		for (int i = noteTimes.Count-1; i >= 0 ; i--)
		{
			eR.isPlayingWithoutTolerance = false;
			eR.isPlayingWithTolerance = false;
			//The notes show up on time regardless of the tolerance, but we need to know if we hit within the tolerance also for player input
			if (timer >= noteTimes[i])//If time is between the x and y, a note is playing during that time
			{
				eR.isPlayingWithoutTolerance = true;
				eR.note = noteSet[i].note;
				eR.index = i;
			}
			if (timer >= noteTimes[i] - tolerance && timer <= noteTimes[i] + tolerance)//If time is between the x and y, a note is playing during that time
			{
				eR.isPlayingWithTolerance = true;
				eR.note = noteSet[i].note;
				eR.index = i;
			}
			if (eR.isPlayingWithTolerance || eR.isPlayingWithoutTolerance){return eR;}
        }
		return eR;
    }
}
public struct EvaluationResults // Return type of evlatiion call. Tell us if we clicked while a note is palying, and if so which note
{
	public int index;
	public bool isPlayingWithoutTolerance;
	public bool isPlayingWithTolerance;
    public RythmNote note;
}
[Serializable]
public struct Note
{
	public RythmNote note;
	public float delay;
}