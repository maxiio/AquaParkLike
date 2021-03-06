using System;
using UnityEngine;

/// <summary>
/// Attach to : Player prefab
/// This component manages all the inputs sent by the player to Movements and every exceptions of being a player
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField] private float releaseHardness = 1;
    [SerializeField] private float dragControl = 1;
    [SerializeField] private Transform cameraPlaceHolder;
    [SerializeField] private float flyingRotationControl;
    
    private Movements _movements;
    InGameUI _inGameUi;

    private void Start()
    {
        _movements = GetComponent<Movements>();
        _inGameUi = FindObjectOfType<InGameUI>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Finish")) //Finish is the terrain's tag, when the player flies and falls on the floor
        {
            _movements.enabled = false;
            _inGameUi.End();
        }
    }
    
    public void InitializeCamera() //makes the camera follow the character
    {
        //Setting up the camera
        CameraFollow cameraFollow = FindObjectOfType<CameraFollow>();
        cameraFollow.target = transform;
        cameraFollow.placeHolder = cameraPlaceHolder;
    }
    
    void Update()
    {
        
        if (Input.touchCount > 0 || Input.GetMouseButton(0)) //The player drags the character on the sides
        {
            float mouseX = InputManager.MouseX;
            if (_movements.onPath && _movements.deviationModifAuthorization)
            {
                _movements.deviation = Mathf.Clamp(_movements.deviation + mouseX * dragControl, -1f, 1f);
            }
            else if(!_movements.onPath)
            {
                transform.RotateAround(transform.position, transform.up, flyingRotationControl * mouseX * Time.deltaTime);
            }        
        }
        else if(_movements.deviationModifAuthorization) //When the character is released, he returns to the center of the toboggan by itself
            _movements.deviation = Mathf.Lerp(_movements.deviation, 0, Time.deltaTime * releaseHardness);

    }

}