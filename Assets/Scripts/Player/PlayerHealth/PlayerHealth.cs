using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public interface IObserver
{
    void NotifyPlayerHit(int currentLives);
}
public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void NotifyObservers(int currentLives);
}

public class PlayerHealth : MonoBehaviour, IDataPersistence, ISubject
{ 
    #region Singleton
    private static PlayerHealth instance;
    public static PlayerHealth Instance
    {
        get
        { 
            return instance;
        }
    }
    #endregion
    //Observers list.
    private List<IObserver> _observers = new List<IObserver>();

    //DataLives.
    private GameData_SO _playerLives;

    //Player light 
    private HardLight2D _playerLight;

    //Functional variables.
    private bool hitPlayer = false;
    private GameObject _playerManager;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        _playerManager = this.gameObject;
        _playerLight = GameObject.Find("HeadLight").GetComponent<HardLight2D>();
        Attach(GetComponent<HitFeedback>());
        Attach(GameObject.FindObjectOfType<UIManager>());
        _playerLives = Resources.Load("PlayerLives") as GameData_SO;
        _playerLight.Intensity = ReturnLivesIntensity(_playerLives.CurrentLives);
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            SetLives(_playerLives.livesMax);
        }
      //  Debug.Log("Player vida atual: " + _playerLives.CurrentLives);
    }

    private void Update()
    {
        //Debug.Log(hitPlayer);
        Debug.Log("Player vida atual: " + _playerLives.CurrentLives);
    }
    #region ModifyLives
    public void Hit(int damage)
    {
        if (hitPlayer == false)
        {
            _playerLives.CurrentLives -= damage;
            _playerLight.Intensity = ReturnLivesIntensity(_playerLives.CurrentLives);
            
            hitPlayer = true;
            Invoke("HitFalse", 1);
            if (_playerLives.CurrentLives <= 0)
            {
                Die();
            }
            NotifyObservers(_playerLives.CurrentLives);
        }
    }
    public void AddLives(int addAmount)
    {
        _playerLives.CurrentLives += addAmount;
    }
    public void SetLives(int liveNewValue)
    {
        _playerLives.CurrentLives = liveNewValue;
    }
    private void Die()
    {
        _playerManager.SetActive(false);
    }
    public int GetLives()
    {
        return _playerLives.livesMax;
    }
    private float ReturnLivesIntensity(int currentLives)
    {
        switch (currentLives)
        {
            case 6:
                return 1.4f;
            case 5:
                return 1.3f;
            case 4:
                return 1.24f;
            case 3:
                return 1.08f;
            case 2:
                return 0.92f;
            case 1:
                return 0.6f;
        }
        return 0;
    }

    private void HitFalse()
    {
        hitPlayer = false;
    }
    #endregion

    #region Observer
    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyObservers(int damage)
    {
        foreach (IObserver observer in _observers)
        {
            observer.NotifyPlayerHit(damage);
        }
    }
    #endregion

    //Data Save and load.
    public void LoadData(GameData data)
    {
        _playerLives.CurrentLives = data.lives;
    }
    public void SaveData(ref GameData data)
    {
        _playerLives.CurrentLives = data.lives;
    }
}