using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RythmMeasure", menuName = "Rythm Measure", order = 100)]

public class RythmMeasure : ScriptableObject
{
	public List<RythmNote> noteSet
	{
		get { return _noteSet; }
		set { _noteSet = value; CalibrateNoteTimes(); }
	}
	public List<RythmNote> _noteSet = new List<RythmNote>();
	private List<Vector2> noteTimes = new List<Vector2>(); // Represents the time where a note is playing. Each vector2 represents the start and end of a note

	private void CalibrateNoteTimes()//Create list of times where notes are playing
	{
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
    #region Unity Callbacks
    void Start()
	{
		CalibrateNoteTimes();
	}

	void Update()
	{
		
	}
    #endregion
}
