using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameButtonFunctions : MonoBehaviour{
    //public GameObject reference;

    //PlayAgain(); Resetting the game;
    public void PlayAgain() {
        GameManagerType game = GameManagerType.Instance;

        if (!game.playPressed) {
            game.isReady = false;
            game.gameStart = false;
            game.gameEnd = false;
            game.transitionEnd = false;
            game.canLoadResultsScreen = false;
            game.abruptEnd = false;
            Instantiate(game.transition, game.canvas);
            game.playPressed = true;
        }
        
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame(int scoresAmount) {
        GameManagerType game = GameManagerType.Instance;
        
        /*if(!game.abruptEnd && game.resultsScreen.scores.Count <= scoresAmount) {
            if(game.isReady) {
                print("Triggered!");
                Instantiate(game.transition, game.canvas);
                game.abruptEnd = true;
                game.transitionEnd = false;
                EndScreenMinigame.ProcessScores();
            }
        }*/
    }

    public void HowToPlay() {
        HowToPlayScreen entity = HowToPlayScreen.Instance;
        GameManagerType game = GameManagerType.Instance;

        if(!game.playPressed) {
            entity.parentObject.SetActive(true);
            entity.gameObject.SetActive(true);
        }
    }

    public void CloseGame() {
        Application.Quit();
    }

    public void Mute(GameObject reference) {
        AudioSource song = GameManagerType.Instance.soundtrack, sfx = GameManagerType.Instance.sfx;

        if (song.mute) {
            song.mute = false;
            sfx.mute = false;
            reference.SetActive(false);
        } else {
            song.mute = true;
            sfx.mute = true;
            reference.SetActive(true);
        }
    }
    
    public void StartMinigame() {
        GameManagerType game = GameManagerType.Instance;
        MinigameSO so = new MinigameSO();
        
        /*if (!game.playPressed) {
            game.resultsScreen.scores = new List<int>();
            Instantiate(game.transition, game.canvas);
            game.playPressed = true;
        }*/
    }

    public void PlaySound(AudioClip soundToPlay) {
        try {
            GameManagerType.Instance.sfx.clip = soundToPlay;
            GameManagerType.Instance.sfx.Play();
        } catch { }
        
    }
}
