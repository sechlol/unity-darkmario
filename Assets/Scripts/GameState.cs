using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Timers;

public delegate void Callback();

public class GameState : MonoBehaviour {
	
	private int timeLeft = 400;
	private float _reloadTimer = 0.002f;
	private bool _marioDied = false;
	private float _timeAfterDeath;
	private float endTime;
	private Mario _mario;
	private bool _isUiShown = false;

	public Text score;
	public Text coins;
	public Text timer;
	public Text FinalScoreText;
	public static bool gameEnded = false;

	void Awake(){
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"),LayerMask.NameToLayer("Player"),true);
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Items"),LayerMask.NameToLayer("Items"));
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemies"),LayerMask.NameToLayer("Enemies"));
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemies"),LayerMask.NameToLayer("Items"));
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("RunningEnemies"),LayerMask.NameToLayer("Items"));
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("RunningEnemies"),LayerMask.NameToLayer("RunningEnemies"));
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("CameraWall"),LayerMask.NameToLayer("Items"));
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("CameraWall"),LayerMask.NameToLayer("Enemies"));
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("CameraWall"),LayerMask.NameToLayer("RunningEnemies"));
	}

	void Start () {
		_mario = FindObjectOfType<Mario>();
		_mario.OnMarioDie += MarioDied;
		endTime = Time.time + 400;
		_timeAfterDeath = -1;
	}

	private void ShowFinalText(){
		if(_isUiShown)
			return;
		_isUiShown = true;
		GameObject FinalLayerText = GameObject.Find ("EndLevelText");
		if (FinalLayerText != null) {
			for(int i=0;i<FinalLayerText.transform.childCount;i++){
				FinalLayerText.transform.GetChild(i).gameObject.SetActive(true);
			}
			FinalScoreText.text = "Your Score: "+(_mario.Score + timeLeft*10);
		}
	}

	private void HideFinalText(){
		if(!_isUiShown)
			return;
		_isUiShown = false;
		GameObject FinalLayerText = GameObject.Find ("EndLevelText");
		if (FinalLayerText != null) 
			for(int i=0;i<FinalLayerText.transform.childCount;i++)
				FinalLayerText.transform.GetChild(i).gameObject.SetActive(false);
	}
	
	void Update () {

		if (gameEnded) {
			ShowFinalText();
			if(UserInput.JumpDown()){
				HideFinalText();
				gameEnded = false;
				Application.LoadLevel("StartScreen");
			}
				
			return;
		}
		if(timeLeft > 0){
			timeLeft = (int)(endTime - Time.time);
			score.text = _mario.Score.ToString();
			coins.text = _mario.Coins.ToString();
			timer.text = timeLeft.ToString();
		}
		else if(timeLeft == 0)
		{
			_mario.Die();
			timeLeft--;
		}
		if(_marioDied){
			if(_timeAfterDeath == -1)
				_timeAfterDeath = Time.time;
			if(Time.time - _timeAfterDeath >= _reloadTimer){
				Time.timeScale = 1;
				Application.LoadLevel(Application.loadedLevel);
			}
		}
	}

	private void MarioDied(){
		Time.timeScale = 0.001f;
		_marioDied = true;
		Destroy(Camera.main.GetComponent<CameraFollow>());
	}
}
