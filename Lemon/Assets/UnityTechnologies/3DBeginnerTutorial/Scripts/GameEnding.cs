using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

namespace UnityTechnologies._3DBeginnerTutorial.Scripts {
	public class GameEnding : MonoBehaviour
	{
		public float fadeDuration = 1f;
		public float displayImageDuration = 1f;
		public GameObject player;
		public CanvasGroup exitBackgroundImageCanvasGroup;
		public AudioSource exitAudio;
		public CanvasGroup caughtBackgroundImageCanvasGroup;
		public AudioSource caughtAudio;

		bool m_IsPlayerAtExit;
		bool m_IsPlayerCaught;
		float m_Timer;
		bool m_HasAudioPlayed;

		// TIMER ADDITION VARIABLES //
		public TextMeshProUGUI timerText;
		public Slider sliderValue;
		private const float GameDuration = 90f;
		private float _gameTimer = GameDuration;
		//////////////////////////////
		
		private void Start() {
			_gameTimer = GameDuration;
		}

		void OnTriggerEnter (Collider other) {
			if (other.gameObject == player) {
				m_IsPlayerAtExit = true;
			}
		}

		public void CaughtPlayer () {
			m_IsPlayerCaught = true;
		}
		
		void Update () {
			if (m_IsPlayerAtExit) {
				EndLevel(exitBackgroundImageCanvasGroup, false, exitAudio);
				return;
			}
			
			if (m_IsPlayerCaught) {
				EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio);
				return;
			}
			
			// MAJOR ADDITION GAME TIMER : ESCAPE WITHING 1:30 OR LOSE
			_gameTimer -= Time.deltaTime;
			_gameTimer = Mathf.Clamp(_gameTimer, 0f, GameDuration);
			
			sliderValue.value = Mathf.Clamp(_gameTimer/GameDuration, 0f, 1f);
			
			int minutes = Mathf.FloorToInt(_gameTimer / 60f);
			int seconds = Mathf.FloorToInt(_gameTimer % 60f);
			timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
			
			if (_gameTimer <= 0f) {
				CaughtPlayer();
			}
			//////////////////////////////////////////////////////////
		}

		void EndLevel (CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource) {
			if (!m_HasAudioPlayed) {
				audioSource.Play();
				m_HasAudioPlayed = true;
			}
            
			m_Timer += Time.deltaTime;
			imageCanvasGroup.alpha = m_Timer / fadeDuration;

			if (m_Timer > fadeDuration + displayImageDuration) {
				if (doRestart) {
					SceneManager.LoadScene(0);
				}else {
					Application.Quit ();
				}
			}
		}
	}
}